# Crit

- Author: John Patrick Maus
- Date Created: 08/25/2024
- Purpose: Crit is a project management tool for developers and testers to track requirements in tasks and to track test cases as well as its results.

## Description

This repository contains the Crit web application written in React and Typescript in `./crit-web`, and a C# .NET 8 Entity Framework Core based Web API which utilizes a SQL Server database for data hosting and for CRUD operations in `./crit-api`.

## Table of Contents

- [Crit](#crit)
  - [Description](#description)
  - [Table of Contents](#table-of-contents)
  - [Prerequisites](#prerequisites)
    - [Required](#required)
    - [Optional](#optional)
  - [Installation](#installation)
  - [Usage](#usage)
  - [VSCode Launch and Task Examples](#vscode-launch-and-task-examples)
    - [launch.json (API)](#launchjson-api)
    - [tasks.json (API)](#tasksjson-api)
    - [launch.json (web)](#launchjson-web)

## Prerequisites

### Required

- NodeJS >= v20.17.0 and NodeJS <= LTS
- Npm >= 10.8.2
- wsl2 (for hosting postgreSQL container)

### Optional

- Chrome Extension: [React Dev Tools](https://chromewebstore.google.com/detail/react-developer-tools/fmkadmapgofadopljbjfkapdkoienihi?hl=en)
  - Other browsers and device instructions such as mobile device can be found [here](https://react.dev/learn/react-developer-tools).

## Installation

1. Open the cloned repository in VS Code or your favorite IDE (such as Visual Studio 2022/Rider) or text editor.
2. Open a terminal/command prompt/powershell instance at `./crit-web` for the React website.
3. Run the following command to get the `node_modules`:

```Powershell
npm install
```

4. Set the following environment variables:
   1. POSTGRES_USER (admin)
   2. POSTGRES_PASSWORD (admin)
   3. POSTGRES_DB
   4. POSTGRESQL_APP_CONTEXT_USER
   5. POSTGRESQL_APP_CONTEXT_PASSWORD
   6. PGADMIN_DEFAULT_EMAIL
   7. PGADMIN_DEFAULT_PASSWORD
5. Run the following powershell script as administrator: `./database/bootstrap-database-wsl.ps1` and follow the prompts.
6. Set appsettings.Development.json variable for `PostgreSQL:ServerName` to your PostgreSQL server instance. (eg. localhost)
7. Set appsettings.Development.json variable for `PostgreSQL:Database` to the database that will house Crit's tables. (eg. crit)
8. Set appsettings.Development.json variable for `PostgreSQL:Port` to the port used for your server instance. (eg. default is `5433`)
9. Set appsettings.Development.json variable for `PostgreSQL:UserName` to the `POSTGRESQL_APP_CONTEXT_USER` environment variable name.
10. Set appsettings.Development.json variable for `PostgreSQL:Password` to the `POSTGRESQL_APP_CONTEXT_PASSWORD` environment variable name.

## Usage

1. In VS Code, just press `F5` to run the server on your local machine. (ensure the correct launch configuration has been created and is selected in VS Code. The `/.vscode` folder is not included in the git repository since every system's environment is different.)
   1. If in another IDE, refer to your IDE documentation to setup a launch or debug configuration.
   2. Remember to run the `CritApi` project in `./crit-api/CritApi` first and to configure it's port in the React project's `.env.development.local` file. (or which ever environment you intend to run this.) This can all be done in your own `/.vscode/launch.json` file if using VS Code. See example at [vscode launch and task examples](#vscode-launch-and-task-examples). I have a `compounds` configuration in `launch.json` example that does this for you.
   3. If in a text editor without launch or debug configurations, simply run the following command in `./crit-web` and then launch a web browser at `http://localhost:3000/`. (replace the `3000` port with whatever port you configure in the `.env` files):

```
npm run start
```

2. Navigate the web app/site.

## VSCode Launch and Task Examples

### launch.json (API)

```json
{
    "configurations": [
        {
            "name": ".NET Core Launch (CritApi) Debug",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "Build Crit Api Debug",
            "program": "${workspaceFolder}/CritApi/bin/Debug/net8.0/CritApi.dll",
            "args": [],
            "cwd": "${workspaceFolder}/CritApi",
            "stopAtEntry": false,
            "launchSettingsFilePath": "${workspaceFolder}/CritApi/Properties/launchSettings.json",
            "launchSettingsProfile": "https",
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            }
        }
    ]
}
```

### tasks.json (API)

```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "Build Crit Api Debug",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/CritApi.sln",
                "/property:GenerateFullPaths=true",
                "/consoleloggerparameters:NoSummary"
            ],
            "problemMatcher": "$msCompile",
            "presentation": {
                "close": true
            }
        },
        {
            "label": "Publish Crit Api Debug",
            "command": "dotnet",
            "type": "process",
            "args": [
                "publish",
                "${workspaceFolder}/CritApi.sln",
                "/property:GenerateFullPaths=true",
                "/consoleloggerparameters:NoSummary"
            ],
            "problemMatcher": "$msCompile",
            "presentation": {
                "close": true
            }
        },
        {
            "label": "Watch Crit Api Debug",
            "command": "dotnet",
            "type": "process",
            "args": [
                "watch",
                "run",
                "--project",
                "${workspaceFolder}/CritApi.sln"
            ],
            "problemMatcher": "$msCompile",
            "dependsOn": [
                "Build Crit Api Debug"
            ],
            "presentation": {
                "close": true
            }
        }
    ]
}
```

### launch.json (web)

```json
{
    "configurations": [
        {
            "name": "Launch Crit Server",
            "request": "launch",
            "type": "node-terminal",
            "command": "npm start",
            "cwd": "${workspaceFolder}"
        }
    ]
}
```
