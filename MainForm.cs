using Viadivy.Tools.VyCapture.Data;
using Viadivy.Tools.VyCapture.Models;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Viadivy.Tools.VyCapture
{
    public sealed class MainForm : Form
    {
        private readonly CaptureRepository _repository;

        private readonly TextBox _txtSearch =
    new TextBox();

        private readonly DataGridView _gridResults =
    new DataGridView();

        private readonly TextBox _txtPreview =
    new TextBox();

        private readonly GroupBox _previewGroup =
    new GroupBox();

        private List<CaptureItem> _allCaptures =
    new List<CaptureItem>();

        private readonly TextBox _txtCapture =
            new TextBox();

        private readonly Button _btnSave =
            new Button();

        private readonly Button _btnSaveTxt =
    new Button();

        private readonly Button _btnPaste =
    new Button();

        private readonly Button _btnDelete =
    new Button();

        private readonly ToolStripStatusLabel _statusLabel =
            new ToolStripStatusLabel();

        private readonly Button _btnCopy =
    new Button();




        public MainForm(
            CaptureRepository repository)
        {
            _repository =
                repository;


            Version? version =
     typeof(MainForm)
         .Assembly
         .GetName()
         .Version;


            if (version != null)
            {
                Text =
                    "VyCapture " +
                    version.Major.ToString() +
                    "." +
                    version.Minor.ToString() +
                    "." +
                    version.Build.ToString();
            }
            else
            {
                Text =
                    "VyCapture";
            }

            StartPosition =
                FormStartPosition.CenterScreen;

            SetInitialWindowSize();

            MinimumSize =
                new Size(
                    700,
                    450);


            Icon =
                System.Drawing.Icon
                    .ExtractAssociatedIcon(
                        Application.ExecutablePath);


            BuildUi();

            BuildStatusBar();

            LoadCaptures();

            ApplySearch();


            _btnSave.Click +=
       Save_Click;

            _btnCopy.Click +=
                Copy_Click;

            _btnSaveTxt.Click +=
    SaveTxt_Click;

            _btnPaste.Click +=
    Paste_Click;

            _btnDelete.Click +=
                Delete_Click;

            _txtCapture.KeyDown +=
                Capture_KeyDown;

            _txtSearch.TextChanged +=
    Search_TextChanged;

          

            _gridResults.CellFormatting +=
    GridResults_CellFormatting;

            _gridResults.SelectionChanged +=
                GridResults_SelectionChanged;
        }

        private void SetInitialWindowSize()
        {
            Screen? screen =
                Screen.PrimaryScreen;


            if (screen == null)
            {
                Width =
                    900;

                Height =
                    700;

                return;
            }


            Rectangle workingArea =
                screen.WorkingArea;


            int targetWidth =
                workingArea.Width * 75 / 100;

            int targetHeight =
                workingArea.Height * 80 / 100;


            Width =
                Math.Min(
                    targetWidth,
                    1100);

            Height =
                Math.Min(
                    targetHeight,
                    800);
        }

        private void Delete_Click(
    object? sender,
    EventArgs e)
        {
            if (_gridResults.SelectedRows.Count == 0)
            {
                _statusLabel.Text =
                    "Nothing selected";

                return;
            }


            DataGridViewRow selectedRow =
                _gridResults.SelectedRows[0];


            CaptureItem? item =
                selectedRow.DataBoundItem
                as CaptureItem;


            if (item == null)
            {
                _statusLabel.Text =
                    "Nothing selected";

                return;
            }


            DialogResult result =
                MessageBox.Show(
                    this,
                    "Are you sure you want to delete this capture？\r\n\r\n" +
                    "This action cannot be undone.",
                    "Delete Capture",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);


            if (result != DialogResult.Yes)
            {
                return;
            }


            DeleteCapture(
                item);
        }

        private void DeleteCapture(
    CaptureItem item)
        {
            try
            {
                bool deleted =
                    _repository.Delete(
                        item.Id);


                if (!deleted)
                {
                    MessageBox.Show(
                        this,
                        "The selected capture could not be found.",
                        "VyCapture",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }


                _allCaptures.Remove(
                    item);


                _txtPreview.Clear();


                ApplySearch();


                _statusLabel.Text =
                    "Deleted | Id = " +
                    item.Id.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    ex.ToString());


                MessageBox.Show(
                    this,
                    "Unable to delete the capture. Please try again.",
                    "VyCapture",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);


                _statusLabel.Text =
                    "Delete failed";
            }
        }

        private Control BuildCapturePanel()
        {
            TableLayoutPanel captureLayout =
                new TableLayoutPanel();

            captureLayout.Dock =
                DockStyle.Fill;

            captureLayout.ColumnCount =
                1;

            captureLayout.RowCount =
                2;


            captureLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100));

            captureLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    42));


            _btnSave.Text =
     "Save  (Ctrl+Enter)";

            _btnSave.Width =
       155;

            _btnSave.Height =
                30;

            _btnSave.Anchor =
                AnchorStyles.Right;

            _btnSave.Text =
     "Save  (Ctrl+Enter)";

            _btnSave.Width =
                155;

            _btnSave.Height =
                30;


            _btnPaste.Text =
                "Paste";

            _btnPaste.Width =
                100;

            _btnPaste.Height =
                30;


            FlowLayoutPanel buttonPanel =
                new FlowLayoutPanel();

            buttonPanel.Dock =
                DockStyle.Fill;

            buttonPanel.FlowDirection =
                FlowDirection.RightToLeft;

            buttonPanel.WrapContents =
                false;


            buttonPanel.Controls.Add(
                _btnSave);

            buttonPanel.Controls.Add(
                _btnPaste);



            captureLayout.Controls.Add(
         _txtCapture,
         0,
         0);

            captureLayout.Controls.Add(
                buttonPanel,
                0,
                1);


            return captureLayout;
        }

        private void Paste_Click(
    object? sender,
    EventArgs e)
        {
            try
            {
                if (!Clipboard.ContainsText())
                {
                    _statusLabel.Text =
                        "Clipboard is empty";

                    return;
                }


                _txtCapture.Paste();

                _txtCapture.Focus();


                _statusLabel.Text =
                    "Pasted";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    ex.ToString());


                MessageBox.Show(
                    this,
                    "Unable to paste text from the clipboard.",
                    "VyCapture",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);


                _statusLabel.Text =
                    "Paste failed";
            }
        }

        private void Copy_Click(
    object? sender,
    EventArgs e)
        {
            string content =
                _txtPreview.Text;


            if (string.IsNullOrWhiteSpace(
                    content))
            {
                _statusLabel.Text =
                    "Nothing to copy";

                return;
            }


            try
            {
                Clipboard.SetText(
                    content);


                _statusLabel.Text =
                    "Copied";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    ex.ToString());


                MessageBox.Show(
                    this,
                    "Unable to copy the text to the clipboard.",
                    "VyCapture",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);


                _statusLabel.Text =
                    "Copy failed";
            }
        }


        private Control BuildPreviewPanel()
        {
            TableLayoutPanel previewLayout =
                new TableLayoutPanel();

            previewLayout.Dock =
                DockStyle.Fill;

            previewLayout.ColumnCount =
                1;

            previewLayout.RowCount =
                2;


            previewLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100));

            previewLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    42));


            FlowLayoutPanel buttonPanel =
                new FlowLayoutPanel();

            buttonPanel.Dock =
                DockStyle.Fill;

            buttonPanel.FlowDirection =
                FlowDirection.RightToLeft;

            buttonPanel.WrapContents =
                false;


            _btnCopy.Text =
                "Copy";

            _btnCopy.Width =
                100;

            _btnCopy.Height =
                30;

            _btnSaveTxt.Text =
    "Save TXT";

            _btnSaveTxt.Width =
                100;

            _btnSaveTxt.Height =
                30;

            _btnDelete.Text =
                "Delete";

            _btnDelete.Width =
                100;

            _btnDelete.Height =
                30;


            buttonPanel.Controls.Add(
                _btnCopy);

            buttonPanel.Controls.Add(
    _btnSaveTxt);

            buttonPanel.Controls.Add(
                _btnDelete);


            previewLayout.Controls.Add(
                _txtPreview,
                0,
                0);

            previewLayout.Controls.Add(
                buttonPanel,
                0,
                1);


            return previewLayout;
        }



        private void SaveTxt_Click(
    object? sender,
    EventArgs e)
        {
            string content =
                _txtPreview.Text;


            if (string.IsNullOrWhiteSpace(
                    content))
            {
                _statusLabel.Text =
                    "Nothing to save";

                return;
            }


            try
            {
                string desktopPath =
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.DesktopDirectory);


                string fileName =
                    "VyCapture_" +
                    DateTime.Now.ToString(
                        "yyyyMMdd_HHmmss") +
                    ".txt";


                string filePath =
                    Path.Combine(
                        desktopPath,
                        fileName);


                UTF8Encoding utf8Encoding =
                    new UTF8Encoding(
                        false);


                File.WriteAllText(
                    filePath,
                    content,
                    utf8Encoding);


                _statusLabel.Text =
                    "Saved to Desktop | " +
                    fileName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    ex.ToString());


                MessageBox.Show(
                    this,
                    "文字無法儲存到桌面。",
                    "VyCapture",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);


                _statusLabel.Text =
                    "Save TXT failed";
            }
        }





        private void GridResults_SelectionChanged(
    object? sender,
    EventArgs e)
        {
            ShowSelectedCapture();
        }

        private void ShowSelectedCapture()
        {
            if (_gridResults.SelectedRows.Count == 0)
            {
                _txtPreview.Clear();

                _previewGroup.Text =
                    "Preview";

                return;
            }


            DataGridViewRow selectedRow =
                _gridResults.SelectedRows[0];


            CaptureItem? item =
                selectedRow.DataBoundItem
                as CaptureItem;


            if (item == null)
            {
                _txtPreview.Clear();

                _previewGroup.Text =
                    "Preview";

                return;
            }


            _txtPreview.Text =
                item.Content;


            _previewGroup.Text =
                "Preview (" +
                item.Content.Length.ToString("N0") +
                " chars)";
        }

        private void GridResults_CellFormatting(
    object? sender,
    DataGridViewCellFormattingEventArgs e)
        {
            if (_gridResults.Columns[e.ColumnIndex]
                    .DataPropertyName != "Content")
            {
                return;
            }


            if (e.Value == null)
            {
                return;
            }


            string content =
                e.Value.ToString() ?? string.Empty;


            content =
                content.Replace(
                    "\r",
                    " ");

            content =
                content.Replace(
                    "\n",
                    " ");


            if (content.Length > 120)
            {
                content =
                    content.Substring(
                        0,
                        120);

                content +=
                    "...";
            }


            e.Value =
                content;

            e.FormattingApplied =
                true;
        }

        private void BuildResultGrid()
        {
            _gridResults.Dock =
                DockStyle.Fill;

            _gridResults.ReadOnly =
                true;

            _gridResults.AllowUserToAddRows =
                false;

            _gridResults.AllowUserToDeleteRows =
                false;

            _gridResults.AllowUserToResizeRows =
                false;

            _gridResults.MultiSelect =
                false;

            _gridResults.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            _gridResults.AutoGenerateColumns =
                false;

            _gridResults.RowHeadersVisible =
                false;


            DataGridViewTextBoxColumn createdColumn =
                new DataGridViewTextBoxColumn();

            createdColumn.HeaderText =
                "Created";

            createdColumn.DataPropertyName =
                "CreatedAt";

            createdColumn.Width =
                150;

            createdColumn.DefaultCellStyle.Format =
                "yyyy-MM-dd HH:mm";


            DataGridViewTextBoxColumn previewColumn =
                new DataGridViewTextBoxColumn();

            previewColumn.HeaderText =
                "Preview";

            previewColumn.DataPropertyName =
                "Content";

            previewColumn.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;


            _gridResults.Columns.Add(
                createdColumn);

            _gridResults.Columns.Add(
                previewColumn);
        }


        private void Search_TextChanged(
    object? sender,
    EventArgs e)
        {
            ApplySearch();
        }


        private void ApplySearch()
        {
            string keyword =
                _txtSearch.Text.Trim();


            List<CaptureItem> resultItems =
                new List<CaptureItem>();


            int matchCount =
                0;


            if (string.IsNullOrWhiteSpace(
                    keyword))
            {
                matchCount =
                    _allCaptures.Count;


                for (int index = 0;
                     index < _allCaptures.Count;
                     index++)
                {
                    if (resultItems.Count >= 50)
                    {
                        break;
                    }


                    resultItems.Add(
                        _allCaptures[index]);
                }
            }
            else
            {
                foreach (CaptureItem item in
                    _allCaptures)
                {
                    bool matched =
                        false;


                    if (item.Content.IndexOf(
                            keyword,
                            StringComparison.OrdinalIgnoreCase)
                        >= 0)
                    {
                        matched =
                            true;
                    }


                    if (!matched &&
                        !string.IsNullOrWhiteSpace(
                            item.Title))
                    {
                        if (item.Title.IndexOf(
                                keyword,
                                StringComparison.OrdinalIgnoreCase)
                            >= 0)
                        {
                            matched =
                                true;
                        }
                    }


                    if (matched)
                    {
                        matchCount++;


                        if (resultItems.Count < 50)
                        {
                            resultItems.Add(
                                item);
                        }
                    }
                }
            }


            _gridResults.DataSource =
                null;

            _gridResults.DataSource =
                resultItems;


            _gridResults.ClearSelection();


            if (_gridResults.Rows.Count > 0)
            {
                _gridResults.Rows[0].Selected =
                    true;
            }
            else
            {
                _txtPreview.Clear();
            }


            if (matchCount > 50)
            {
                _statusLabel.Text =
                    "Showing 50 of " +
                    matchCount.ToString() +
                    " matches";
            }
            else
            {
                _statusLabel.Text =
                    matchCount.ToString() +
                    " matches";
            }
        }



        private void BuildUi()
        {
            TableLayoutPanel mainLayout =
                new TableLayoutPanel();

            mainLayout.Dock =
                DockStyle.Fill;

            mainLayout.ColumnCount =
                1;

            mainLayout.RowCount =
                4;

            mainLayout.Padding =
                new Padding(
                    24);


            //
            // Search
            //
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    48));

            //
            // Search Results
            //
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    35));

            //
            // Preview
            //
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    30));

            //
            // New Capture
            //
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    35));


            //
            // Search Panel
            //
            TableLayoutPanel searchLayout =
                new TableLayoutPanel();

            searchLayout.Dock =
                DockStyle.Fill;

            searchLayout.ColumnCount =
                2;

            searchLayout.RowCount =
                1;


            searchLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    70));

            searchLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    100));


            Label searchLabel =
                new Label();

            searchLabel.Text =
                "Search";

            searchLabel.Dock =
                DockStyle.Fill;

            searchLabel.TextAlign =
                ContentAlignment.MiddleLeft;


            _txtSearch.Dock =
                DockStyle.Fill;

            _txtSearch.PlaceholderText =
                "Search saved text...";


            searchLayout.Controls.Add(
                searchLabel,
                0,
                0);

            searchLayout.Controls.Add(
                _txtSearch,
                1,
                0);


            //
            // Search Results
            //
            BuildResultGrid();


            GroupBox resultGroup =
                new GroupBox();

            resultGroup.Text =
                "Search Results";

            resultGroup.Dock =
                DockStyle.Fill;

            resultGroup.Padding =
                new Padding(
                    8);

            resultGroup.Margin =
                new Padding(
                    0,
                    4,
                    0,
                    4);

            resultGroup.Controls.Add(
                _gridResults);


            //
            // Preview
            //
            _txtPreview.Dock =
                DockStyle.Fill;

            _txtPreview.Multiline =
                true;

            _txtPreview.ScrollBars =
                ScrollBars.Vertical;

            _txtPreview.ReadOnly =
                true;

            _txtPreview.Font =
                new Font(
                    "Segoe UI",
                    10);


            Control previewPanel =
                BuildPreviewPanel();


            _previewGroup.Text =
        "Preview";

            _previewGroup.Dock =
                DockStyle.Fill;

            _previewGroup.Padding =
                new Padding(
                    8);

            _previewGroup.Margin =
                new Padding(
                    0,
                    4,
                    0,
                    4);

            _previewGroup.Controls.Add(
                previewPanel);


            //
            // New Capture
            //
            _txtCapture.Dock =
                DockStyle.Fill;

            _txtCapture.Multiline =
                true;

            _txtCapture.ScrollBars =
                ScrollBars.Vertical;

            _txtCapture.AcceptsReturn =
                true;

            _txtCapture.AcceptsTab =
                true;

            _txtCapture.Font =
                new Font(
                    "Segoe UI",
                    11);


            Control capturePanel =
                BuildCapturePanel();


            GroupBox captureGroup =
                new GroupBox();

            captureGroup.Text =
                "New Capture";

            captureGroup.Dock =
                DockStyle.Fill;

            captureGroup.Padding =
                new Padding(
                    8);

            captureGroup.Margin =
                new Padding(
                    0,
                    4,
                    0,
                    0);

            captureGroup.Controls.Add(
                capturePanel);


            //
            // Main Layout
            //
            mainLayout.Controls.Add(
                searchLayout,
                0,
                0);

            mainLayout.Controls.Add(
                resultGroup,
                0,
                1);

            mainLayout.Controls.Add(
     _previewGroup,
     0,
     2);

            mainLayout.Controls.Add(
                captureGroup,
                0,
                3);


            Controls.Add(
                mainLayout);
        }



        private void BuildStatusBar()
        {
            StatusStrip statusStrip =
                new StatusStrip();

            statusStrip.Dock =
                DockStyle.Bottom;

            statusStrip.SizingGrip =
                false;


            _statusLabel.Text =
                "Ready";


            statusStrip.Items.Add(
                _statusLabel);


            Controls.Add(
                statusStrip);

            statusStrip.BringToFront();
        }


        private void Save_Click(
      object? sender,
      EventArgs e)
        {
            SaveCapture();
        }


        private void Capture_KeyDown(
    object? sender,
    KeyEventArgs e)
        {
            if (e.Control &&
                e.KeyCode == Keys.Enter)
            {
                SaveCapture();

                e.SuppressKeyPress =
                    true;

                e.Handled =
                    true;
            }
        }


        private void SaveCapture()
        {
            string content =
                _txtCapture.Text.Trim();


            if (string.IsNullOrWhiteSpace(
                    content))
            {
                _statusLabel.Text =
                    "Nothing to save";

                return;
            }


            try
            {
                CaptureItem newItem =
    _repository.Insert(
        content);


                _allCaptures.Insert(
                    0,
                    newItem);

                ApplySearch();

                _txtCapture.Clear();

                _txtCapture.Focus();

                _statusLabel.Text =
                    "Saved | Id = " +
                    newItem.Id.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    ex.ToString());


                MessageBox.Show(
                    this,
                    "Unable to save the capture. Please try again.",
                    "VyCapture",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);


                _statusLabel.Text =
                    "Save failed";
            }
        }


        private void LoadCaptures()
        {
            try
            {
                _allCaptures =
                    _repository.GetAll();


                _statusLabel.Text =
                    "Ready | " +
                    _allCaptures.Count.ToString() +
                    " items loaded";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    ex.ToString());


                MessageBox.Show(
                    this,
                    "Unable to load saved captures.",
                    "VyCapture",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);


                _statusLabel.Text =
                    "Load failed";
            }
        }

    }
}