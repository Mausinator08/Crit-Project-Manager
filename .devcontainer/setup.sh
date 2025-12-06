#!/bin/bash
set -e

echo "Running initial project setup..."

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
