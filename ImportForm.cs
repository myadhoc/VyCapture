using Viadivy.Tools.VyCapture.Data;

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Viadivy.Tools.VyCapture
{
    public sealed class ImportForm : Form
    {
        private readonly CaptureRepository _repository;

        private readonly ListBox _listFiles =
            new ListBox();

        private readonly Label _lblSummary =
            new Label();

        private readonly Button _btnSelectFiles =
            new Button();

        private readonly Button _btnImport =
            new Button();

        private string[] _selectedFilePaths =
            Array.Empty<string>();


        public int ImportedCount
        {
            get;
            private set;
        }


        public ImportForm(
            CaptureRepository repository)
        {
            _repository =
                repository;


            Text =
                "Import TXT";

            StartPosition =
                FormStartPosition.CenterParent;

            Width =
                650;

            Height =
                450;

            MinimumSize =
                new Size(
                    500,
                    320);

            ShowInTaskbar =
                false;


            BuildUi();


            _btnSelectFiles.Click +=
         SelectFiles_Click;

            _btnImport.Click +=
                Import_Click;

            _listFiles.DragEnter +=
                Files_DragEnter;

            _listFiles.DragDrop +=
                Files_DragDrop;
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
                3;

            mainLayout.Padding =
                new Padding(
                    16);


            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    32));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    48));


            _lblSummary.Text =
                "No files selected";

            _lblSummary.Dock =
                DockStyle.Fill;

            _lblSummary.TextAlign =
                ContentAlignment.MiddleLeft;


            _listFiles.Dock =
     DockStyle.Fill;

            _listFiles.HorizontalScrollbar =
                true;

            _listFiles.AllowDrop =
                true;



            FlowLayoutPanel buttonPanel =
                new FlowLayoutPanel();

            buttonPanel.Dock =
                DockStyle.Fill;

            buttonPanel.FlowDirection =
                FlowDirection.RightToLeft;

            buttonPanel.WrapContents =
                false;


            _btnImport.Text =
                "Import";

            _btnImport.Width =
                100;

            _btnImport.Height =
                30;

            _btnImport.Enabled =
                false;


            _btnSelectFiles.Text =
                "Select TXT Files...";

            _btnSelectFiles.Width =
                150;

            _btnSelectFiles.Height =
                30;


            buttonPanel.Controls.Add(
                _btnImport);

            buttonPanel.Controls.Add(
                _btnSelectFiles);


            mainLayout.Controls.Add(
                _lblSummary,
                0,
                0);

            mainLayout.Controls.Add(
                _listFiles,
                0,
                1);

            mainLayout.Controls.Add(
                buttonPanel,
                0,
                2);


            Controls.Add(
                mainLayout);
        }


        private void SelectFiles_Click(
            object? sender,
            EventArgs e)
        {
            OpenFileDialog openFileDialog =
                new OpenFileDialog();

            try
            {
                openFileDialog.Title =
                    "Select TXT Files";

                openFileDialog.Filter =
                    "Text files (*.txt)|*.txt";

                openFileDialog.Multiselect =
                    true;

                openFileDialog.CheckFileExists =
                    true;


                DialogResult result =
                    openFileDialog.ShowDialog(
                        this);


                if (result != DialogResult.OK)
                {
                    return;
                }


                SetSelectedFiles(openFileDialog.FileNames);
            }
            finally
            {
                openFileDialog.Dispose();
            }
        }

        private void Files_DragEnter(
    object? sender,
    DragEventArgs e)
        {
            if (e.Data == null)
            {
                e.Effect =
                    DragDropEffects.None;

                return;
            }


            if (!e.Data.GetDataPresent(
                    DataFormats.FileDrop))
            {
                e.Effect =
                    DragDropEffects.None;

                return;
            }


            object? fileDropData =
                e.Data.GetData(
                    DataFormats.FileDrop);


            string[]? filePaths =
                fileDropData as string[];


            if (filePaths == null)
            {
                e.Effect =
                    DragDropEffects.None;

                return;
            }


            if (!AreValidTxtFiles(
                    filePaths))
            {
                e.Effect =
                    DragDropEffects.None;

                return;
            }


            e.Effect =
                DragDropEffects.Copy;
        }

        private void Files_DragDrop(
    object? sender,
    DragEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }


            object? fileDropData =
                e.Data.GetData(
                    DataFormats.FileDrop);


            string[]? filePaths =
                fileDropData as string[];


            if (filePaths == null)
            {
                return;
            }


            if (!AreValidTxtFiles(
                    filePaths))
            {
                return;
            }


            SetSelectedFiles(
                filePaths);
        }

        private bool AreValidTxtFiles(
    string[] filePaths)
        {
            if (filePaths.Length == 0)
            {
                return false;
            }


            for (int index = 0;
                 index < filePaths.Length;
                 index++)
            {
                string filePath =
                    filePaths[index];


                if (!File.Exists(
                        filePath))
                {
                    return false;
                }


                string extension =
                    Path.GetExtension(
                        filePath);


                bool isTxtFile =
                    string.Equals(
                        extension,
                        ".txt",
                        StringComparison.OrdinalIgnoreCase);


                if (!isTxtFile)
                {
                    return false;
                }
            }


            return true;
        }

        private void SetSelectedFiles(
    string[] filePaths)
        {
            _selectedFilePaths =
                filePaths;


            _listFiles.Items.Clear();


            for (int index = 0;
                 index < _selectedFilePaths.Length;
                 index++)
            {
                _listFiles.Items.Add(
                    _selectedFilePaths[index]);
            }


            _btnImport.Enabled =
                _selectedFilePaths.Length > 0;


            if (_selectedFilePaths.Length == 1)
            {
                _lblSummary.Text =
                    "1 file selected";
            }
            else
            {
                _lblSummary.Text =
                    _selectedFilePaths.Length.ToString() +
                    " files selected";
            }
        }

        private void Import_Click(
            object? sender,
            EventArgs e)
        {
            if (_selectedFilePaths.Length == 0)
            {
                return;
            }


            int importedCount =
                0;

            int emptyCount =
                0;

            int failedCount =
                0;


            _btnImport.Enabled =
                false;

            _btnSelectFiles.Enabled =
                false;


            _listFiles.Items.Clear();


            for (int index = 0;
                 index < _selectedFilePaths.Length;
                 index++)
            {
                string filePath =
                    _selectedFilePaths[index];


                try
                {
                    if (!File.Exists(
                            filePath))
                    {
                        failedCount++;

                        _listFiles.Items.Add(
                            "[Failed] " +
                            filePath);

                        continue;
                    }


                    string content =
       File.ReadAllText(
           filePath);


                    if (string.IsNullOrWhiteSpace(
                            content))
                    {
                        emptyCount++;

                        _listFiles.Items.Add(
                            "[Empty] " +
                            filePath);

                        continue;
                    }


                    string fileName =
                        Path.GetFileName(
                            filePath);


                    string captureContent =
                        "Source: " +
                        fileName +
                        "\r\n\r\n" +
                        content;


                    _repository.Insert(
                        captureContent);


                    importedCount++;


                    _listFiles.Items.Add(
                        "[Imported] " +
                        filePath);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        ex.ToString());


                    failedCount++;


                    _listFiles.Items.Add(
                        "[Failed] " +
                        filePath);
                }
            }


            ImportedCount =
                importedCount;


            _lblSummary.Text =
                "Imported: " +
                importedCount.ToString() +
                " | Empty: " +
                emptyCount.ToString() +
                " | Failed: " +
                failedCount.ToString();


            if (importedCount == 0)
            {
                _btnSelectFiles.Enabled =
                    true;

                _btnImport.Enabled =
                    _selectedFilePaths.Length > 0;

                return;
            }


            MessageBox.Show(
                this,
                _lblSummary.Text,
                "Import TXT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);


            DialogResult =
                DialogResult.OK;

            Close();
        }
    }
}