using Ookii.Dialogs.Wpf;

namespace HexEditor.Core.Services
{
    public class FolderBrowserDialogService : IFolderBrowserDialogService
    {
        private VistaFolderBrowserDialog folderBrowserDialog;
        public string SelectedPath
        {
            get
            {
                if (folderBrowserDialog != null)
                    return folderBrowserDialog.SelectedPath;
                return "";
            }
        }
        public string Description
        {
            set
            {
                if (folderBrowserDialog != null)
                    folderBrowserDialog.Description = value;
            }
        }

        public void Reset()
        {
            if (folderBrowserDialog != null)
                folderBrowserDialog.Reset();
        }

        public bool? ShowDialog(string description = "")
        {
            folderBrowserDialog = new VistaFolderBrowserDialog();
            folderBrowserDialog.Description = description;
            return folderBrowserDialog.ShowDialog();
          ;
        }
    }

    public interface IFolderBrowserDialogService
    {
        string SelectedPath { get; }
        string Description { set; }
        void Reset();
        bool? ShowDialog(string description = "");
    }

    public enum DialogServiceResult
    {
        OK,
        YES,
        CANCEL,
        NONE,
        ABORT,
        RETRY,
        IGNORE,
        NO
    }
}
