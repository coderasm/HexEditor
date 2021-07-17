using System.Threading.Tasks;
using System.Windows;

namespace HexEditor.Core.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public async Task<DialogServiceResult> Show(string message, string caption = "")
        {
            return await Application.Current.Dispatcher.InvokeAsync(() => {
                var result = MessageBox.Show(message, caption);
                return mapToDialogServiceResult(result);
            });
        }

        public async Task<DialogServiceResult> Show(string message, string caption, MessageBoxServiceButtons buttons)
        {
            var messageBoxButtons = MessageBoxButton.OK;
            switch (buttons)
            {
                case MessageBoxServiceButtons.YesNo:
                    messageBoxButtons = MessageBoxButton.YesNo;
                    break;
                default:
                    break;
            }
            return await Application.Current.Dispatcher.InvokeAsync(() => {
                var result = MessageBox.Show(message, caption, messageBoxButtons);
                return mapToDialogServiceResult(result);
            });
        }

        private DialogServiceResult mapToDialogServiceResult(MessageBoxResult MessageBoxResult)
        {
            DialogServiceResult mappedResult = DialogServiceResult.OK;
            switch (MessageBoxResult)
            {
                case MessageBoxResult.None:
                    mappedResult = DialogServiceResult.NONE;
                    break;
                case MessageBoxResult.Cancel:
                    mappedResult = DialogServiceResult.CANCEL;
                    break;
                case MessageBoxResult.Yes:
                    mappedResult = DialogServiceResult.YES;
                    break;
                case MessageBoxResult.No:
                    mappedResult = DialogServiceResult.NO;
                    break;
                case MessageBoxResult.OK:
                    mappedResult = DialogServiceResult.OK;
                    break;
                default:
                    break;
            }
            return mappedResult;
        }
    }

    public interface IMessageBoxService
    {
        Task<DialogServiceResult> Show(string message, string caption = "");
        Task<DialogServiceResult> Show(string message, string caption, MessageBoxServiceButtons buttons);
    }

    public enum MessageBoxServiceButtons
    {
        YesNo
    }
}
