using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace HexEditor.Core.Services
{
    public class FileSelectDialogService : IFileSelectDialogService
    {
        private OpenFileDialog openFileDialog;
        private string filter = "All files (*.*)|*.*";
        private const int MAX_FILEPATH_LENGTH = 259;
        private const int MAX_DIRECTORY_LENGTH = 247;
        private bool pathLengthTooLong = false;
        private string defaultFilter;
        private IFileService fileService;
        private IMessageBoxService messageBoxService;

        public FileSelectDialogService(IFileService fileService, IMessageBoxService messageBoxService)
        {
            defaultFilter = filter;
            this.fileService = fileService;
            this.messageBoxService = messageBoxService;
        }
        public List<string> FileNames
        {
            get
            {
                if (openFileDialog != null && openFileDialog.FileNames != null)
                    return openFileDialog.FileNames.ToList();
                return new List<string>();
            }
        }

        public string FileName
        {
            get
            {
                if (openFileDialog != null && openFileDialog.FileNames != null)
                    return openFileDialog.FileNames.Count() > 0 ? openFileDialog.FileNames[0] : openFileDialog.FileName;
                return openFileDialog.FileName;
            }
        }

        public IFileSelectDialogService Filter(string filter = "")
        {
            this.filter = filter == "" ? defaultFilter : filter;
            return this;
        }

        public bool? ShowDialog()
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = filter;
            openFileDialog.FileOk += fileSelected;
            var dialogResult = openFileDialog.ShowDialog();
            bool? result = dialogResult.HasValue && dialogResult.Value && !pathLengthTooLong;
            return result;
        }

        private void fileSelected(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var pathLength = openFileDialog.FileName.Length;
            var directoryLength = fileService.GetDirectoryName(openFileDialog.FileName).Length;
            notifyPathsTooLong(pathLength, directoryLength);
        }

        public bool? ShowMultiSelectDialog()
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = filter;
            openFileDialog.FileOk += filesSelected;
            var dialogResult = openFileDialog.ShowDialog();
            bool? result = dialogResult.HasValue && dialogResult.Value && !pathLengthTooLong;
            return result;
        }

        private void filesSelected(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var pathLength = openFileDialog.FileNames.Max(fileName => fileName.Length);
            var directoryLength = openFileDialog.FileNames.Max(fileName => fileService.GetDirectoryName(fileName).Length);
            notifyPathsTooLong(pathLength, directoryLength);
        }

        private void notifyPathsTooLong(int pathLength, int directoryLength)
        {
            pathLengthTooLong = pathLength > MAX_FILEPATH_LENGTH || directoryLength > MAX_DIRECTORY_LENGTH;
            if (pathLength > MAX_FILEPATH_LENGTH)
                messageBoxService.Show("Full file path is too long. It is greater than the maximum 259 characters.", "Warning");
            else if (directoryLength > MAX_DIRECTORY_LENGTH)
                messageBoxService.Show("Full file directory path is too long. It is greater than the maximum 247 characters.", "Warning");
        }
    }

    public interface IFileSelectDialogService
    {
        List<string> FileNames { get; }
        string FileName { get; }
        bool? ShowDialog();
        bool? ShowMultiSelectDialog();
        IFileSelectDialogService Filter(string filter = "");
    }
}
