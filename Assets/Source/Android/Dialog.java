package com.unity.dialog;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;

public final class Dialog {

    interface Button {
        CharSequence getTextButton();

        void onClick();
    }

    public static AlertDialog createDialog(
            final Context context,
            final CharSequence title,
            final CharSequence message,
            final Button positiveButton,
            final Button negativeButton) {
        AlertDialog.Builder builder = new AlertDialog.Builder(context);
        builder.setTitle(title);
        builder.setMessage(message);
        if (positiveButton != null) {
            builder.setPositiveButton(positiveButton.getTextButton(), new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    UnityBridge.runOnUnityThread(new Runnable() {
                        @Override
                        public void run() {
                            positiveButton.onClick();
                        }
                    });
                }
            });
        }
        if (negativeButton != null) {
            builder.setNegativeButton(negativeButton.getTextButton(), new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    UnityBridge.runOnUnityThread(new Runnable() {
                        @Override
                        public void run() {
                            negativeButton.onClick();
                        }
                    });
                }
            });
        }
        builder.setCancelable(false);
        AlertDialog dialog = builder.create();
        dialog.show();
        return dialog;
    }

    public static AlertDialog createDialog(
            final Context context,
            final CharSequence title,
            final CharSequence message,
            final Button positiveButton) {
        AlertDialog.Builder builder = new AlertDialog.Builder(context);
        builder.setTitle(title);
        builder.setMessage(message);
        if (positiveButton != null) {
            builder.setPositiveButton(positiveButton.getTextButton(), new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    UnityBridge.runOnUnityThread(new Runnable() {
                        @Override
                        public void run() {
                            positiveButton.onClick();
                        }
                    });
                }
            });
        }
        builder.setCancelable(false);
        AlertDialog dialog = builder.create();
        dialog.show();
        return dialog;
    }

    public static ProgressDialog createDialog(
            final Context context,
            final CharSequence title,
            final CharSequence message,
            final int max
    ) {
        return new ProgressDialog(context, title, message, max);
    }
}
