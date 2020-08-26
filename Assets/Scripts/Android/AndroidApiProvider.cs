using UnityEngine;
using System;

public static class AndroidApiProvider
{
    public static AndroidJavaClass unityPlayerInstance = null;

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        #if UNITY_ANDROID && !UNITY_EDITOR
        unityPlayerInstance = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        new AndroidJavaClass("com.unity.dialog.UnityBridge").CallStatic("registerMessageHandler");
        #endif
    }

    public static void RunAndroidThread(Action<AndroidJavaObject> action)
    {
        AndroidJavaObject activity = AndroidApiProvider.unityPlayerInstance.GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                action(activity);
            }
        ));

    }

}
