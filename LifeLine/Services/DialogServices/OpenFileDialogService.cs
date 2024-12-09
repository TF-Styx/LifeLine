using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeLine.Services.DialogServices
{
    public class OpenFileDialogService : IOpenFileDialogService
    {
        public string[] OpenDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Multiselect = true;
            dialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileNames;
            }
            else
            {
                return null;
            }
        }
    }
}
