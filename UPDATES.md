# Updates

## 2024/08/27 20:35 CST

### Enhancements

* Added initial app layout with working title bar and nav menu.
* Added a bare home page.
* Added theme toggling.
* Added bare API with no controllers.

## 2024/11/13 13:56 CST

### Enhancements

* Added Projects and Project page to house the tasks.

## 2024/12/27 06:38 CST

### Enhancements

* Refactored links and routes to be separate and began adding task components.

## 2025/01/01 21:09 CST

### Enhancements

* Added projects and tasks in front-end. Refactored code.
* Moved task details component to Tasks folder in components.
* Moved Projects Component to Project folder with the Project Options Component.

## 2025/01/02 17:38 CST

### Enhancements

* Updated README.md
* Temporarily removed Authorize attribute from Project Controller for testing.
* Fixed styling/coloring issues.

## 2025/03/08 20:04 CST

### Enhancements

* Restored Authorize attribute for Controllers that needed it.
* Implemented API endpoints for Login/Logout, Organizations, Projects, and Tasks.

## 2025/03/13 21:00 CST

### Enhancements

* Registration and Login works and API endpoints are authorizing correctly

## 2025/03/14 00:08 CST

### Fixes

* Fixed nav menu not updating after login/logout.

## 2025/03/16 20:48 CST

### Enhancements

* Projects are pulling, creating, and saving in API via UI.

## 2025/03/19 13:08 CST

### Fixes

* Fixed issue where navbar collapsible items would not revert to collapsed state when the navbar is condensed.
* Fixed some visual issues with icons and fixed the Lock Projects checkbox to save in the local storage.
* Removed unnecessary crypto.randomUUID() calls for setting id field in models since the API already handles it.
* Fixed string[] | undefined errors.

## 2025/03/28 20:40 CST

### Fixes

* Fixed css for icons in projects, and started cleaning up tasks.
* Moved project service to the app page component and fixed nav bar to refresh when new projects are added or projects are deleted.

## 2025/04/04 21:32 CST

### Fixes

* Renamed function(s) that lists task, and removed a redundant function for task rows.
* Fixed a few issues in the User Controller in API and cleaned up some of the task objects and names in the front-end.
* Fixed login controller's register endpoint to detect if an organization already exists.
* If organization exists, user is added to User role, and if organization is new, the user is added to the organization owner role.
* Fixed CreatedAtAction in controller endpoints for resource creation.

### Enhancements

* Implemented the tasks table.

## 2025/04/07 23:06 CST

### Fixes

* Fixed Authorize roles to all use commas instead of semi colons.
* Fixed Tasks Repository to restrict what can be viewed/edited.
* Fixed Projects and Project Tasks pages.
* Removed collaborator user ids from Task. Too granular.
* Fixed GetOrganization return value to not be nullable.
* Fixed the other User Service methods to use the ApiResult\<T> response model and fixed project tasks page accordingly.
* Project Options will now retrieve the logged in user id and added USER_ROLES constants.

### Enhancements

* Created scrollable panels for projects' tasks and details split panel.
* Added new API endpoints for logged in user and user in role.
* Added the two new endpoint calls to the User Service and added the ApiResult response model.

## 2025/04/10 03:22 CST

### Fixes

* Removed unused import.
* Fixed error in list tasks.

### Enhancements

* Enhanced Organization API.
* Added organization service.
* Added User and Organization fields.

## 2025/04/12 17:57 CST

### Fixes

* Changed Ok() to NoContent() for API endpoints that return nothing successfully.
* Added remaining AsNoTracking() to getters in API.

### Enhancements

* Added status controller and repository.
* Added priority controller and repository.
* Added Status and Priority services and implemented status and priority selection in project options.
* Added custom field type controller and repository.

## 2025/09/09 23:49 CST

### Fixes

* Applied fixes project-options.page.tsx.

### Enhancements

* Cleaned up scss for projects and custom. 
* Applied enhancements to project-options.page.tsx.

## 2025/09/19 20:51 CST

### Enhancements

* Containerized postgreSQL database, and stopped using Mongo DB.
* Stopped using Mongo DB in favor of postgreSQL due to the nature of entity framework core, and adapted API to it.
* Updated models and their usages throughout to accommodate the API after switching to PostgreSQL.

## 2025/09/20 21:01 CST

### Fixes

* Slightly modified the project options panel and added ability to close it again.

### Enhancements

* Shrunk the borders of panels for visual appeal.

## 2025/11/27 14:24 CST

### Fixes

* Fixed a bug in boostrap-database-wsl.ps1

### Enhancements

* Updated readme.md.

## 2025/12/05 23:50 CST

### Enhancements

* Completely did away with custom scripts for bootstrapping in lue of devcontainers.

## 2025/12/07 19:01 CST

### Fixes

* Normalized line endings and fixed settings in dev container for vs solutions.

## 2025/12/08 01:10 CST

### Fixes

* Fixed dev containers, updated README.md
* Updated tsconfig.json for moduleResolution to use bundler instead of node for newer typescript.
* Removed unnecessary global.json file.
* Fixed column names and created a migration for it.
* Fixed phone numbers to save the UserId associated with it.