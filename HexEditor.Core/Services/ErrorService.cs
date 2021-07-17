using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HexEditor.Core.Services
{
    public class ErrorService : IErrorService
    {
        private string errorMessage = $"An error has occurred. Do you want to create an error\n" + $"report to send to the developers?";
        private string errorTitle = "Error";
        private string combinedErrorMessage = "";

        private string folderDialogTitle = "Report Folder";
        private string reportPath = $@"C:\";
        private string timeStampFormat = "MM-dd-yyyy_HH-mm-ss";
        private string reportFileName = "eMASSTER_error_report_";
        private string timeStamp = "";
        private StackTrace stackTrace;

        private IFolderBrowserDialogService folderBrowserDialogService;
        private IMessageBoxService messageBoxService;
        private IFileService fileService;
        private IEnvironmentService environmentService;

        public ErrorService(
          IFolderBrowserDialogService folderBrowserDialogService,
          IMessageBoxService messageBoxService,
          IFileService fileService,
          IEnvironmentService environmentService
          )
        {
            this.folderBrowserDialogService = folderBrowserDialogService;
            this.folderBrowserDialogService.Description = folderDialogTitle;
            this.messageBoxService = messageBoxService;
            this.fileService = fileService;
            this.environmentService = environmentService;
        }

        private void promptForSaveDirectory()
        {
            var result = folderBrowserDialogService.ShowDialog();
            if (result.HasValue && result.Value)
            {
                if (!string.IsNullOrEmpty(folderBrowserDialogService.SelectedPath))
                {
                    reportPath = folderBrowserDialogService.SelectedPath;
                }
            }
        }

        public async Task<string> NotifyUser(Exception exception, string customMessage = "")
        {
            stackTrace = new StackTrace(exception, true);
            combinedErrorMessage = errorMessage;
            if (customMessage != "")
                combinedErrorMessage = $"" +
                                $"{customMessage}\n" +
                                $"{errorMessage}";
            var errorDialogResult = await messageBoxService.Show(combinedErrorMessage, errorTitle, MessageBoxServiceButtons.YesNo);
            if (errorDialogResult == DialogServiceResult.YES)
            {
                promptForSaveDirectory();
                var reportContents = createReportContents(exception);
                fileService.WriteAll($@"{reportPath}\{reportFileName}.txt", reportContents);
                await messageBoxService.Show($"An error report has been created in {reportPath}", "Information");
                return reportContents;
            }
            return "";
        }

        private int getLineNumber()
        {
            var frame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
            return frame.GetFileLineNumber();
        }

        private string getClassName()
        {
            var frame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
            return frame.GetMethod().ReflectedType.FullName;
        }

        private string createReportContents(Exception exception)
        {
            timeStamp = DateTime.Now.ToString(timeStampFormat);
            reportFileName += timeStamp;
            var reportContents = $"eMASTER Error Report\n" +
                                $"{timeStamp}\n" +
                                $"MachineName: {environmentService.MachineName}\n" +
                                $"UserDomainName: {environmentService.UserDomainName}\n" +
                                $"UserName: {environmentService.UserName}\n" +
                                $"OS: {environmentService.OperatingSystem}\n" +
                                $"CLR Version: {environmentService.CLRVersion}\n" +
                                $"{exception}\n" +
                                $"Class::Line number: {getClassName()}::{getLineNumber()}\n" +
                                $"Exception Message: {exception.Message}\n" +
                                $"Stacktrace: {exception.StackTrace}\n";
            if (exception.InnerException != null)
                reportContents += $"InnerException {exception.InnerException}\n" +
                                  $"InnerException Message: {exception.InnerException.Message}\n" +
                                  $"InnerException Stacktrace: {exception.InnerException.StackTrace}";
            return reportContents;
        }
    }

    public interface IErrorService
    {
        Task<string> NotifyUser(Exception exception, string customMessage = "");
    }
}
