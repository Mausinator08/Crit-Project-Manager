#!/bin/sh
while true; do
    echo "Running backup..."
    pg_dump -h postgres -U ${POSTGRES_USER} ${POSTGRES_DB} > /backup/db_$(date +%Y%m%d%H%M%S).sql
    echo "Deleting old backups..."
    ls -1t /backup/*.sql | tail -n +31 | xargs -r rm --
    sleep 86400
done
