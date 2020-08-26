using UnityEngine;

public class AndroidDialogManager : IDialogManager
{
    private AndroidJavaClass _dialogManager;
    public AndroidDialogManager()
    {
        _dialogManager = new AndroidJavaClass("com.unity.dialog.Dialog");
    }


    public IMessageDialog ShowConfirmDialog(string title, string text, Button positiveButton, Button negativeButton)
    {
        AndroidMessageDialog dialog = new AndroidMessageDialog();
        AndroidApiProvider.RunAndroidThread((activity) =>
        {
            dialog.SetDialog(_dialogManager.CallStatic<AndroidJavaObject>(
                "createDialog",
                activity,
                title,
                text,
                new AndroidMessageDialog.AndroidButton(positiveButton.Name, () => { dialog.SetDialog(null); positiveButton.Action(); }),
                new AndroidMessageDialog.AndroidButton(negativeButton.Name, () => { dialog.SetDialog(null); negativeButton.Action(); })
                ));
        });
        return dialog;
    }

    public IMessageDialog ShowInformationDialog(string title, string text, Button positiveButton)
    {
        AndroidMessageDialog dialog = new AndroidMessageDialog();
        AndroidApiProvider.RunAndroidThread((activity) =>
        {
            dialog.SetDialog(_dialogManager.CallStatic<AndroidJavaObject>(
                "createDialog",
                activity,
                title,
                text,
                new AndroidMessageDialog.AndroidButton(positiveButton.Name, () => { dialog.SetDialog(null); positiveButton.Action(); })
                ));
        });
        return dialog;
    }

    public IProgressDialog ShowProgressDialog(string title, string text, int max)
    {
        AndroidProgressDialog dialog = new AndroidProgressDialog();
        AndroidApiProvider.RunAndroidThread((activity) =>
        {
            dialog.SetDialog(_dialogManager.CallStatic<AndroidJavaObject>(
                "createDialog",
                activity,
                title,
                text,
                max
                ));
        });
        return dialog;
    }
}
