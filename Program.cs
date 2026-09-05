using Viadivy.Tools.VyCapture.Data;

using System;
using System.Windows.Forms;

namespace Viadivy.Tools.VyCapture
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();


            DatabaseManager databaseManager =
                new DatabaseManager();

            databaseManager.InitializeDatabase();


            CaptureRepository repository =
                new CaptureRepository(
                    databaseManager);


            MainForm mainForm =
                new MainForm(
                    repository);


            Application.Run(
                mainForm);
        }
    }
}