\# VyCapture



\*\*Capture now. Find later.\*\*



VyCapture is a lightweight, offline Windows desktop tool for quickly saving and retrieving useful text.



看到一段值得留下來的文字，不需要先想檔名、資料夾或分類。



貼上、儲存，需要的時候再用關鍵字找回來。



> 先存下來，以後再找。



\---



\## Features



\- Quick text capture

\- `Ctrl + Enter` to save

\- Instant full-text search

\- Preview and one-click copy

\- Local SQLite storage

\- Fully offline

\- No account required

\- No cloud synchronization

\- No ads or analytics



\---



\## Typical Use Cases



VyCapture can be used to save:



\- AI responses

\- Prompts

\- SQL

\- Code snippets

\- PowerShell commands

\- JSON

\- Markdown

\- Email drafts

\- Work notes

\- SOP content

\- Useful web text

\- Temporary ideas



\---



\## How It Works



The basic workflow is intentionally simple:



1\. Paste text into \*\*New Capture\*\*

2\. Save it or press `Ctrl + Enter`

3\. Search by keyword when needed

4\. Select a result

5\. Preview the full content

6\. Copy it back to your work



\*\*Capture → Search → Preview → Copy\*\*



\---



\## Privacy



VyCapture is designed as a local-first application.



Captured content is stored locally in a SQLite database.



No account is required, and VyCapture does not intentionally upload captured text to any cloud service.



The database is stored under:



`%LOCALAPPDATA%\\Viadivy\\VyCapture\\VyCapture.db`



\---



\## Data Safety



VyCapture does not directly overwrite existing captures.



If content needs to be changed, copy the original capture, modify it, and save it as a new capture.



When a capture is deleted from the active list, VyCapture first stores a copy in the local deletion archive.



VyCapture 1.0 does not currently provide a user interface for restoring deleted captures.



\---



\## Technology



\- .NET 10

\- Windows Forms

\- Microsoft.Data.Sqlite

\- SQLite

\- C#



\---



\## Build



Requirements:



\- Windows

\- .NET 10 SDK

\- Visual Studio with .NET Desktop Development workload



Clone the repository and open the `VyCapture.csproj` project in Visual Studio.



Build and run normally.



\---



\## Project Status



Current version:



\*\*VyCapture 1.0\*\*



VyCapture is intentionally kept small.



The goal is not to become another large note-taking or knowledge-management system.



Its purpose is simple:



> Save useful text with minimal friction and find it again when needed.



\---

\## License



VyCapture is open source software licensed under the \[MIT License](LICENSE).



\## Brand



VyCapture is a Viadivy project.



燒腦捉怪｜J博土的時空圖書館

