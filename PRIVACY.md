\# VyCapture Privacy Policy



Last updated: September 5, 2026



VyCapture is a local-first Windows desktop application developed under the Viadivy project.



VyCapture is designed to let users save and retrieve text locally on their own Windows device.



\## Data Collection



VyCapture does not require an account or user registration.



VyCapture itself does not include:



\- User accounts

\- Cloud synchronization

\- Advertising

\- Analytics

\- Usage tracking

\- Telemetry

\- Remote content storage



VyCapture does not intentionally collect or transmit the text that users save in the application.



\## Local Data Storage



Captured text is stored locally in a SQLite database on the user's Windows device.



The default database location is:



`%LOCALAPPDATA%\\Viadivy\\VyCapture\\VyCapture.db`



The database may contain text that the user has manually saved through VyCapture.



Users are responsible for the content they choose to store in the application.



\## Deleted Captures



When a capture is deleted from the active list, VyCapture stores a local copy in its deletion archive before removing it from the active capture list.



This deletion archive is stored in the same local SQLite database.



VyCapture 1.0 does not currently provide a user interface for restoring deleted captures.



\## Network Access



VyCapture's core capture, search, preview, copy, and delete functions are designed to operate locally without requiring an Internet connection.



VyCapture does not intentionally send captured text to Viadivy or to any cloud service.



\## Third-Party Components



VyCapture uses open-source software components, including:



\- .NET

\- Microsoft.Data.Sqlite

\- SQLite-related libraries



These components are used to provide the local desktop application and database functionality.



Relevant third-party licenses and notices are provided with the project where applicable.



\## Data Backup and Removal



Users may back up their VyCapture data by closing the application and copying the following database file:



`%LOCALAPPDATA%\\Viadivy\\VyCapture\\VyCapture.db`



To remove locally stored VyCapture data, users may delete the local database file after closing the application.



Deleting the database permanently removes the locally stored captures and deletion archive contained in that database.



\## Future Changes



If future versions of VyCapture introduce optional online services, cloud synchronization, analytics, accounts, or other features that affect user data, this Privacy Policy will be updated accordingly.



The current policy applies to VyCapture 1.0.



\## Contact



For questions about VyCapture or this Privacy Policy:



Email: services@aiwitheveryone.com



Project: Viadivy / VyCapture

