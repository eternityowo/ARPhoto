public interface IDialogManager
{
    IMessageDialog ShowConfirmDialog(string title, string text, Button positiveButton, Button negativeButton);
    IMessageDialog ShowInformationDialog(string title, string text, Button positiveButton);
    IProgressDialog ShowProgressDialog(string title, string text, int max);
}
