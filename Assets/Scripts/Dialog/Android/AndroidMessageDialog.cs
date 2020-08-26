using System;
using UnityEngine;

public class AndroidMessageDialog : IMessageDialog
{
    protected AndroidJavaObject _dialog = null;

    private bool _isClose = false;

    public virtual void SetDialog(AndroidJavaObject dialog)
    {
        _isClose = dialog == null;
        _dialog = dialog;
        if (_isClose) CloseDialog();
    }

    public void CloseDialog()
    {
        _isClose = true;
        if (_dialog != null)
        {
            AndroidApiProvider.RunAndroidThread((activity) =>
            {
                _dialog.Call("dismiss");
                _dialog = null;
            });
        }
    }

    public class AndroidButton : AndroidJavaProxy
    {
        private Action _action;
        private string _name;

        public AndroidButton(string name, Action click)
         : base("com.unity.dialog.Dialog$Button")
        {
            _name = name;
            _action = click;
        }

        string getTextButton()
        {
            return _name;
        }

        void onClick()
        {
            _action();
        }
    }
}