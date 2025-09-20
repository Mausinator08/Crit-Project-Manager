#!/bin/sh

sudo apt-get update
sudo apt-get upgrade -y

sudo apt-get install -y curl

curl -fsSL https://get.docker.com -o ./get-docker.sh
sudo ./get-docker.sh

cd "$(dirname "$0")"

echo "📂 Working dir: $(pwd)"

echo "🗑️ Cleaning up old certs and containers..."

docker-compose down -v
sudo rm -rf /database_crit_postgres_data

echo "🔧 Building certgen Docker image..."

docker-compose build --no-cache certgen

echo "🔑 Running certgen to generate self-signed cert..."

docker-compose run --rm certgen

echo "🚀 Starting Docker containers..."
docker-compose up -d postgres pgadmin backup