package com.unity.dialog;

import android.os.Handler;
import android.os.Looper;

public final class UnityBridge {

    private static Handler unityMainThreadHandler;

    public static void registerMessageHandler() {
        if(unityMainThreadHandler == null) {
            unityMainThreadHandler = new Handler(Looper.getMainLooper());
        }
    }

    public static void runOnUnityThread(Runnable runnable) {
        if(unityMainThreadHandler != null && runnable != null) {
            unityMainThreadHandler.post(runnable);
        }
    }
}
