using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Viadivy.Tools.VyCapture
{
    public sealed class AboutForm : Form
    {
        private readonly LinkLabel _linkGitHub =
            new LinkLabel();

        private readonly LinkLabel _linkFeedback =
            new LinkLabel();

        private readonly Button _btnClose =
            new Button();


        public AboutForm()
        {
            Text =
                "About VyCapture";

            StartPosition =
                FormStartPosition.CenterParent;

            Width =
                520;

            Height =
                430;

            MinimumSize =
                new Size(
                    500,
                    400);

            MaximizeBox =
                false;

            MinimizeBox =
                false;

            ShowInTaskbar =
                false;


            BuildUi();


            _linkGitHub.LinkClicked +=
                GitHub_LinkClicked;

            _linkFeedback.LinkClicked +=
                Feedback_LinkClicked;

            _btnClose.Click +=
                Close_Click;
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
                7;

            mainLayout.Padding =
                new Padding(
                    24);


            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    48));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    32));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    42));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    32));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    32));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    48));


            Label lblProductName =
                new Label();

            lblProductName.Text =
                "VyCapture";

            lblProductName.Font =
                new Font(
                    Font.FontFamily,
                    20,
                    FontStyle.Bold);

            lblProductName.Dock =
                DockStyle.Fill;

            lblProductName.TextAlign =
                ContentAlignment.MiddleLeft;


            Label lblVersion =
                new Label();

            lblVersion.Text =
                GetVersionText();

            lblVersion.Dock =
                DockStyle.Fill;

            lblVersion.TextAlign =
                ContentAlignment.MiddleLeft;


            Label lblTagline =
                new Label();

            lblTagline.Text =
                "Capture now. Find later.";

            lblTagline.Font =
                new Font(
                    Font.FontFamily,
                    11,
                    FontStyle.Bold);

            lblTagline.Dock =
                DockStyle.Fill;

            lblTagline.TextAlign =
                ContentAlignment.MiddleLeft;


            Label lblDescription =
                new Label();

            lblDescription.Text =
                "VyCapture is a lightweight offline Windows tool for " +
                "quickly capturing, searching, previewing, and reusing text.\r\n\r\n" +
                "Your captures are stored locally on your computer. " +
                "No account, cloud service, analytics, or telemetry is required.\r\n\r\n" +
                "VyCapture is open source and released under the MIT License.";

            lblDescription.Dock =
                DockStyle.Fill;

            lblDescription.AutoSize =
                false;


            _linkGitHub.Text =
                "GitHub - VyCapture";

            _linkGitHub.Dock =
                DockStyle.Fill;

            _linkGitHub.TextAlign =
                ContentAlignment.MiddleLeft;


            _linkFeedback.Text =
                "Feedback: services@aiwitheveryone.com";

            _linkFeedback.Dock =
                DockStyle.Fill;

            _linkFeedback.TextAlign =
                ContentAlignment.MiddleLeft;


            FlowLayoutPanel buttonPanel =
                new FlowLayoutPanel();

            buttonPanel.Dock =
                DockStyle.Fill;

            buttonPanel.FlowDirection =
                FlowDirection.RightToLeft;

            buttonPanel.WrapContents =
                false;


            _btnClose.Text =
                "Close";

            _btnClose.Width =
                100;

            _btnClose.Height =
                30;


            buttonPanel.Controls.Add(
                _btnClose);


            mainLayout.Controls.Add(
                lblProductName,
                0,
                0);

            mainLayout.Controls.Add(
                lblVersion,
                0,
                1);

            mainLayout.Controls.Add(
                lblTagline,
                0,
                2);

            mainLayout.Controls.Add(
                lblDescription,
                0,
                3);

            mainLayout.Controls.Add(
                _linkGitHub,
                0,
                4);

            mainLayout.Controls.Add(
                _linkFeedback,
                0,
                5);

            mainLayout.Controls.Add(
                buttonPanel,
                0,
                6);


            Controls.Add(
                mainLayout);
        }


        private string GetVersionText()
        {
            Version? version =
                typeof(AboutForm)
                    .Assembly
                    .GetName()
                    .Version;


            if (version == null)
            {
                return "Version unknown";
            }


            string versionText =
                version.Major.ToString() +
                "." +
                version.Minor.ToString() +
                "." +
                version.Build.ToString();


            return "Version " +
                versionText;
        }


        private void GitHub_LinkClicked(
            object? sender,
            LinkLabelLinkClickedEventArgs e)
        {
            OpenExternalLink(
                "https://github.com/myadhoc/VyCapture");
        }


        private void Feedback_LinkClicked(
            object? sender,
            LinkLabelLinkClickedEventArgs e)
        {
            OpenExternalLink(
                "mailto:services@aiwitheveryone.com?subject=VyCapture%20Feedback");
        }


        private void Close_Click(
            object? sender,
            EventArgs e)
        {
            Close();
        }


        private void OpenExternalLink(
            string target)
        {
            try
            {
                ProcessStartInfo startInfo =
                    new ProcessStartInfo();

                startInfo.FileName =
                    target;

                startInfo.UseShellExecute =
                    true;


                Process.Start(
                    startInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    ex.ToString());


                MessageBox.Show(
                    this,
                    "Unable to open the requested link.",
                    "VyCapture",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }
}