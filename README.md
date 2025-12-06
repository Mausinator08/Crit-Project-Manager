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
  - [Installation (VSCode approach heavily recommended for devcontainers.)](#installation-vscode-approach-heavily-recommended-for-devcontainers)
  - [Usage](#usage)
  - [VSCode .vscode directory and files](#vscode-vscode-directory-and-files)
    - [csharp.runtimeconfig.json](#csharpruntimeconfigjson)
    - [launch.json](#launchjson)
    - [settings.json](#settingsjson)
  - [VSCode .devcontainer](#vscode-devcontainer)
    - [.env](#env)

## Prerequisites

### Required

- NodeJS >= v20.17.0 and NodeJS <= LTS (devcontainer comes with NodeJS 20 and NPM is downgraded to 9 for compatibility with react-scripts)
- wsl2 (for hosting postgreSQL container)

### Optional

- Chrome Extension: [React Dev Tools](https://chromewebstore.google.com/detail/react-developer-tools/fmkadmapgofadopljbjfkapdkoienihi?hl=en)
  - Other browsers and device instructions such as mobile device can be found [here](https://react.dev/learn/react-developer-tools).

## Installation (VSCode approach heavily recommended for devcontainers.)

1. Open the cloned repository in VS Code.
2. Create the .vscode directory and its files. (See [VSCode .vscode directory and files](#vscode-vscode-directory-and-files))
3. Create the `.env` file. (described [VSCode .devcontainer](#vscode-devcontainer))
4. Type `CTRL` + `SHIFT` + `P` to open the Command Palette.
5. Type `Dev Containers: Rebuild and Reopen in Container` and press `ENTER`.
6. The VSCode window will reload and take quite some time to create the devcontainer and setup the PostgreSQL DB.

## Usage

1. Once done, the project is ready to debug. Go to the debug menu, and select `Build and Run Server/Client Debug`.
2. Press `F5` to start both the API and the Crit Web application.
3. Open a browser to `http://localhost:3000/` and click Register to test creating a user/organization.
4. Then login with that username and password, and the app can be tested.

## VSCode .vscode directory and files

### csharp.runtimeconfig.json

```json
{
	"runtimeOptions": {
		"tfm": "net8.0",
		"framework": {
			"name": "Microsoft.NETCore.App",
			"version": "8.0.0"
		}
	}
}
```

### launch.json

```json
{
    "version": "0.2.0",
    "compounds": [
        {
            "name": "Build and Run Server/Client Debug",
            "configurations": [
                ".NET Core (CritApi) Debug with Watch",
                "Launch Crit Web",
            ],
            "stopAll": true
        }
    ],
    "configurations": [
        {
            "name": "Launch Crit Web",
            "request": "launch",
            "type": "node-terminal",
            "command": "npm run start",
            "cwd": "/workspace/crit-web/"
        },
        {
            "name": ".NET Core (CritApi) Debug with Watch",
            "type": "coreclr",
            "request": "launch",
            "program": "dotnet",
            "args": [
                "watch",
                "run",
                "--launch-profile",
                "Http Debug",
                "--project",
                "/workspace/crit-api/CritApi/CritApi.csproj"
            ],
            "cwd": "/workspace/crit-api/CritApi",
            "stopAtEntry": false,
            "justMyCode": false,
            "console": "integratedTerminal",
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            },
            "sourceFileMap": {
                "/workspace": "${workspaceFolder}"
            }
        }
    ]
}
```

### settings.json

```json
{
	"dotnetServer.buildsEnabled": false,
	"files.exclude": {
		"**/bin": true,
		"**/obj": true
	},
	"csharp.referencesCodeLens.enabled": true,
	"csharp.testsCodeLens.enabled": true,
	"editor.formatOnSave": true,
	"editor.defaultFormatter": "ms-dotnettools.csharp",
	"dotnet.server.internalOmniSharpOnlyRestoreEnabled": false,
	"dotnet.projects.useProjectToolsServer": false
}
```


## VSCode .devcontainer

### .env

```env
POSTGRES_USER=admin
POSTGRES_PASSWORD=<admin password>
POSTGRES_DB=crit

PGADMIN_DEFAULT_EMAIL=<your email>
PGADMIN_DEFAULT_PASSWORD=<password to access pgadmin>

POSTGRES_APP_CONTEXT_USER=crit_context
POSTGRES_APP_CONTEXT_PASSWORD=<crit_context password>

API_HTTP_PORT=5139
API_HTTPS_PORT=7295
API_IIS_HTTP_PORT=20171
API_IIS_HTTPS_PORT=44314

CRIT_WEB_HTTP_PORT=3000
REACT_APP_CRIT_API_URL=http://localhost:5139/api
DISABLE_ESLINT_PLUGIN=true
```
