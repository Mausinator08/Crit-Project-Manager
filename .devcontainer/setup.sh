#!/bin/bash
set -e

echo "Running initial project setup..."

# Load env vars from .env inside workspace, if needed
if [ -f /workspace/.env ]; then
  export $(grep -v '^#' /workspace/.env | xargs)
fi

if [[ -z "$POSTGRES_APP_CONTEXT_USER" || -z "$POSTGRES_APP_CONTEXT_PASSWORD" ]]; then
  echo "❌ POSTGRES_APP_CONTEXT_USER or POSTGRES_APP_CONTEXT_PASSWORD not set — skipping user creation."
else
  CONTAINER_NAME="crit-postgres-1"

  echo "⏳ Waiting for Postgres to be healthy..."
  until docker exec $CONTAINER_NAME pg_isready -U "$POSTGRES_USER" >/dev/null 2>&1; do
    sleep 2
  done

  echo "➡️ Ensuring PostgreSQL user \"$POSTGRES_APP_CONTEXT_USER\" exists..."

  docker exec -i $CONTAINER_NAME psql -U "$POSTGRES_USER" -d "$POSTGRES_DB" <<EOF
DO \$\$
DECLARE
  user_name CONSTANT text := '${POSTGRES_APP_CONTEXT_USER}';
BEGIN
    IF NOT EXISTS (
        SELECT FROM pg_catalog.pg_roles WHERE rolname = '${POSTGRES_APP_CONTEXT_USER}'
    ) THEN
        CREATE USER ${POSTGRES_APP_CONTEXT_USER} WITH PASSWORD '${POSTGRES_APP_CONTEXT_PASSWORD}';
        GRANT ALL PRIVILEGES ON DATABASE ${POSTGRES_DB} TO ${POSTGRES_APP_CONTEXT_USER};
    END IF;
    -- Allow creation of tables, sequences, etc.
    EXECUTE format('GRANT ALL PRIVILEGES ON SCHEMA public TO %I', user_name);

    -- Give full rights on all existing objects
    EXECUTE format('GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO %I', user_name);
    EXECUTE format('GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO %I', user_name);
    EXECUTE format('GRANT ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public TO %I', user_name);

    -- Ensure new objects created in the future are accessible
    EXECUTE format('ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL PRIVILEGES ON TABLES TO %I', user_name);
    EXECUTE format('ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL PRIVILEGES ON SEQUENCES TO %I', user_name);
END
\$\$ LANGUAGE plpgsql;
EOF

  echo "✅ User ensured: $POSTGRES_APP_CONTEXT_USER"
fi

echo "Fixing line endings for .env..."
if [ -f /workspace/.env ]; then
    dos2unix /workspace/.env
fi

echo "Downgrade NPM to version 9 so that react-scripts behaves on Linux."
npm install -g npm@9

echo "Installing React Dependencies..."
cd /workspace/crit-web/
npm install

echo "Setup complete."
