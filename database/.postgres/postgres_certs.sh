#!/bin/sh
mkdir -p /certs
chmod 755 /certs
if [ ! -f /certs/server.key ]; then
    openssl req -x509 -nodes -newkey rsa:2048 -keyout /certs/server.key -out /certs/server.crt -days 365 -subj "/CN=localhost"
    chmod 600 /certs/server.key
    chmod 644 /certs/server.crt
    chown 70:70 /certs/server.key /certs/server.crt
    echo 'Self-signed cert and key generated.'
else
    echo 'Using existing cert and key.'
fi
ls -la /certs

# will NOT exit until both files exist and match UID=70
TRIES=0
while [ ! -f /certs/server.key ] || [ ! -f /certs/server.crt ]; do
  echo "⏳ Waiting for certs to appear in volume..."
  sleep 1
  TRIES=$((TRIES + 1))
  if [ "$TRIES" -gt 30 ]; then
    echo "❌ Timeout waiting for certs!"
    exit 1
  fi
done

# Confirm ownership
OWNER_UID=$(stat -c '%u' /certs/server.key)
if [ "$OWNER_UID" -ne "70" ]; then
  echo "❌ server.key has wrong UID ($OWNER_UID), fixing..."
  chown 70:70 /certs/server.key
fi

echo "✅ Certgen done — exiting cleanly."