# VyCapture

**Capture now. Find later.**

VyCapture is a lightweight, offline Windows desktop app for quickly capturing, searching, previewing, and reusing useful text.

When you find something worth keeping, you do not need to decide on a file name, folder, tag, or category first.

Just paste it, save it, and find it again later with a keyword.

> **Capture now. Find later.**

看到一段值得留下來的文字，不需要先想檔名、資料夾、標籤或分類。

先貼上、先存下來，需要的時候再用關鍵字找回來。

> **先存下來，以後再找。**

---

## Features

- Quick text capture
- One-click Paste from the Windows clipboard
- `Ctrl + Enter` to save
- Fast keyword search
- Preview saved content with character count
- One-click Copy back to the clipboard
- Save selected content directly as a UTF-8 TXT file
- Local SQLite storage
- Fully offline core functionality
- No account required
- No cloud synchronization
- No ads, analytics, or telemetry

---

## Typical Use Cases

VyCapture is useful for text that you want to keep now and process later.

Examples include:

- AI responses
- Prompts
- SQL queries
- Code snippets
- PowerShell commands
- JSON
- Markdown
- Email drafts
- Work notes
- SOP content
- Error messages and troubleshooting notes
- Useful web text
- Temporary ideas

It works especially well as a lightweight staging area for text that is not ready for a formal system yet, but should not be lost.

---

## How It Works

The workflow is intentionally simple:

1. Copy useful text from anywhere
2. Paste it into **New Capture**
3. Save it or press `Ctrl + Enter`
4. Search by keyword when needed
5. Select a result
6. Preview the full content
7. Copy it back to your work or save it as a TXT file

**Paste → Save → Search → Preview → Reuse**

VyCapture is intentionally not a full note-taking or knowledge-management platform.

The goal is to reduce the friction between:

> “This may be useful later.”

and:

> “I found it again.”

---

## 中文說明

VyCapture 是一款簡單、快速、以本機為核心的 Windows 文字保存與搜尋工具。

它適合處理一種很常見的資訊：

> **現在還不值得整理進正式系統，但又不想把它忘掉。**

例如 AI 回覆、Prompt、SQL、程式碼、Email 草稿、工作筆記、SOP、錯誤訊息，或任何之後可能還會再用到的文字。

不需要先建立文件、不需要取檔名，也不需要先想好分類。

基本流程就是：

**貼上 → 儲存 → 搜尋 → 預覽 → 再利用**

需要時可以直接複製回剪貼簿，也可以將完整內容輸出成 TXT 檔案，方便交給其他工具或 AI 繼續處理。

---

## Privacy

VyCapture is designed as a local-first application.

Captured content is stored locally in a SQLite database on the user's Windows device.

VyCapture does not require an account and does not intentionally upload captured text to any cloud service.

There is no advertising, analytics, tracking, or telemetry.

The database is stored under:

`%LOCALAPPDATA%\Viadivy\VyCapture\VyCapture.db`

For more details, see the [Privacy Policy](PRIVACY.md).

---

## Data Safety

VyCapture does not directly overwrite existing captures.

If content needs to be changed, the recommended workflow is:

1. Copy the original capture
2. Modify the content
3. Save it as a new capture
4. Delete the old capture if it is no longer needed

When a capture is deleted from the active list, VyCapture first stores a copy in a local deletion archive before removing it from the active data.

The current version does not provide a user interface for restoring deleted captures.

Application removal does not automatically remove the local VyCapture database. Users who want to completely remove local data can delete the VyCapture data folder under `%LOCALAPPDATA%\Viadivy\VyCapture` after closing the application.

---

## Technology

- .NET 10
- Windows Forms
- C#
- Microsoft.Data.Sqlite
- SQLite

---

## Build from Source

Requirements:

- Windows
- .NET 10 SDK
- Visual Studio with the **.NET Desktop Development** workload

Clone the repository and open:

`VyCapture.csproj`

Build and run the project normally in Visual Studio.

---

## Project Philosophy

VyCapture is intentionally kept small.

The goal is not to become another large note-taking, document-management, or knowledge-management platform.

Its purpose is simple:

> **Save useful text with minimal friction and find it again when needed.**

Or, in Traditional Chinese:

> **先存下來，以後再找。**

---

## Releases

Published versions and release notes are available in [GitHub Releases](../../releases).

---

## License

VyCapture is open source software licensed under the [MIT License](LICENSE).
