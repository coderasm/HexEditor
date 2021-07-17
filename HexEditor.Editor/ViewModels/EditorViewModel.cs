using HexEditor.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace HexEditor.EditorModule.ViewModels
{
    class EditorViewModel : BindableBase
    {
        private readonly IFileSelectDialogService fileSelectDialogService;
        private readonly IFileService fileService;
        private readonly IMessageBoxService messageBoxService;
        public EditorViewModel(
            IFileSelectDialogService fileSelectDialogService,
            IFileService fileService,
            IMessageBoxService messageBoxService)
        {
            this.fileSelectDialogService = fileSelectDialogService;
            this.fileService = fileService;
            this.messageBoxService = messageBoxService;
        }

        private string uploadedFile = "";
        public string UploadedFile
        {
            get { return uploadedFile; }
            set { SetProperty(ref uploadedFile, value); }
        }

        private string fileAsHex = "";
        public string FileAsHex
        {
            get { return fileAsHex; }
            set { SetProperty(ref fileAsHex, value); }
        }

        private DelegateCommand convertToHex;
        public DelegateCommand ConvertToHex =>
            convertToHex ?? (convertToHex = new DelegateCommand(ExecuteConvertToHex));

        async void ExecuteConvertToHex()
        {
            var selected = fileSelectDialogService.ShowDialog();
            if (selected.HasValue && selected.Value && fileSelectDialogService.FileName != "")
            {
                //UploadedFile = fileSelectDialogService.FileName;
                await Task.Run(() => { fileService.ConvertToHex(fileSelectDialogService.FileName); });
                messageBoxService.Show($"File converted to hex");
            }
        }

        private DelegateCommand convertToBinary;
        public DelegateCommand ConvertToBinary =>
            convertToBinary ?? (convertToBinary = new DelegateCommand(ExecuteConvertToBinary));

        async void ExecuteConvertToBinary()
        {
            var selected = fileSelectDialogService.ShowDialog();
            if (selected.HasValue && selected.Value && fileSelectDialogService.FileName != "")
            {
                //UploadedFile = fileSelectDialogService.FileName;
                await Task.Run(() => { fileService.ConvertToBinary(fileSelectDialogService.FileName); });
                messageBoxService.Show($"File converted to Binary");
            }
        }
    }
}
