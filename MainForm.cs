using Viadivy.Tools.VyCapture.Data;
using Viadivy.Tools.VyCapture.Models;

using System;
using System.Collections.Generic;
using System.Drawing;
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


        private List<CaptureItem> _allCaptures =
    new List<CaptureItem>();

        private readonly TextBox _txtCapture =
            new TextBox();

        private readonly Button _btnSave =
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


            Text =
                "VyCapture";

            StartPosition =
                FormStartPosition.CenterScreen;

            Width =
                900;

            Height =
                700;

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
                    "確定要刪除這筆資料嗎？\r\n\r\n" +
                    "刪除後無法復原。",
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
                        "找不到要刪除的資料。",
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
                    "資料未能刪除，請重新嘗試。",
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


            captureLayout.Controls.Add(
                _txtCapture,
                0,
                0);

            captureLayout.Controls.Add(
                _btnSave,
                0,
                1);


            return captureLayout;
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
                    "文字無法複製到剪貼簿。",
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


            _btnDelete.Text =
                "Delete";

            _btnDelete.Width =
                100;

            _btnDelete.Height =
                30;


            buttonPanel.Controls.Add(
                _btnCopy);

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

                return;
            }


            _txtPreview.Text =
                item.Content;
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


            GroupBox previewGroup =
                new GroupBox();

            previewGroup.Text =
                "Preview";

            previewGroup.Dock =
                DockStyle.Fill;

            previewGroup.Padding =
                new Padding(
                    8);

            previewGroup.Margin =
                new Padding(
                    0,
                    4,
                    0,
                    4);

            previewGroup.Controls.Add(
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
                previewGroup,
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
                    "資料未能儲存，請重新嘗試。",
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
                    "無法載入已儲存的資料。",
                    "VyCapture",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);


                _statusLabel.Text =
                    "Load failed";
            }
        }

    }
}