# EmbeddedPowerBI
- Embedded Power BI reports
- Uses Identity Core Authentication and Authorization
- Centralized user registration
- Users are organized by organization and role
- Each organization may have 0 or more administrators
- Organization admin users may add other users in their organization as either admin or general user roles
- System Administrators may add users, organizations and roles.

### appsettings.json
- set the sql server connection string.
- smtp settings must be set before running the application.
- To view a pbi report the pbi settings must be completed.
