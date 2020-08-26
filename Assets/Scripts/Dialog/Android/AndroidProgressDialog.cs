using System;
using UnityEngine;

public class AndroidProgressDialog : AndroidMessageDialog, IProgressDialog
{
    private int _progress = 0;

    public int Progress
    {
        get
        {
            return _progress;
        }
        set
        {
            _progress = value;
            if (_dialog != null)
            {
                AndroidApiProvider.RunAndroidThread((activity) =>
                {
                    _dialog.Call("setProgress", _progress);
                });
            }
        }
    }

    public override void SetDialog(AndroidJavaObject dialog)
    {
        base.SetDialog(dialog);
        Progress = _progress;
    }

}