FROM mcr.microsoft.com/devcontainers/dotnet:1-8.0

# Always operate as root inside the container for Windows bind mounts
USER root

# Install specific .NET SDK required by global.json
RUN wget https://dotnetcli.azureedge.net/dotnet/Sdk/8.0.416/dotnet-sdk-8.0.416-linux-x64.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar zxf dotnet-sdk-8.0.416-linux-x64.tar.gz -C /usr/share/dotnet \
    && rm dotnet-sdk-8.0.416-linux-x64.tar.gz

ENV DOTNET_ROOT=/usr/share/dotnet
ENV PATH="$PATH:/usr/share/dotnet"

# Install dotnet-ef tool globally
RUN dotnet tool install -g dotnet-ef
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
