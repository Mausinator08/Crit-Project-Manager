FROM mcr.microsoft.com/devcontainers/dotnet:1-10.0

# Always operate as root inside the container for Windows bind mounts
USER root

ENV PATH="${PATH}:/root/.dotnet/tools"

# Install OS packages + Node in one clean apt run
RUN apt-get update && \
    apt-get install -y \
        wget \
        curl \
        git \
        build-essential \
        postgresql-client \
        unzip \
        bash \
        ca-certificates \
        dos2unix \
        docker.io && \
    \
    # Install Node 20
    curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs && \
    \
    # Clean apt cache
    rm -rf /var/lib/apt/lists/*
