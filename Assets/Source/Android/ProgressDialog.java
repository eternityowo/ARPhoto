package com.unity.dialog;

import android.app.AlertDialog;
import android.content.Context;
import android.graphics.Color;
import android.view.Gravity;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;

public final class ProgressDialog {

    private final AlertDialog mDialog;
    private final TextView mText;
    private final int mMax;

    public ProgressDialog(
            final Context context,
            final CharSequence title,
            final CharSequence message,
            final int max
    ) {
        mMax = max;
        int padding = (int) (24 * context.getResources().getDisplayMetrics().density);
        LinearLayout layout = new LinearLayout(context);
        layout.setOrientation(LinearLayout.HORIZONTAL);
        layout.setPadding(padding, padding, padding, padding);
        layout.setGravity(Gravity.CENTER);
        LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.WRAP_CONTENT,
                LinearLayout.LayoutParams.WRAP_CONTENT);
        params.gravity = Gravity.CENTER_VERTICAL;
        layout.setLayoutParams(params);

        ProgressBar progressBar = new ProgressBar(context);
        progressBar.setIndeterminate(true);
        progressBar.setPadding(0, 0, padding, 0);
        layout.addView(progressBar, params);

        params = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.WRAP_CONTENT);
        params.gravity = Gravity.CENTER_VERTICAL;
        mText = new TextView(context);
        mText.setText(String.format("%d/%d", 0, mMax));
        mText.setTextColor(Color.BLACK);
        mText.setTextSize(20);
        mText.setLayoutParams(params);
        layout.addView(mText);

        AlertDialog.Builder builder = new AlertDialog.Builder(context);
        builder.setTitle(title);
        builder.setMessage(message);
        builder.setCancelable(false);
        builder.setView(layout);

        mDialog = builder.create();
        mDialog.show();/*
        Window window = mDialog.getWindow();
        if (window != null) {
            WindowManager.LayoutParams layoutParams = new WindowManager.LayoutParams();
            layoutParams.copyFrom(mDialog.getWindow().getAttributes());
            layoutParams.width = LinearLayout.LayoutParams.WRAP_CONTENT;
            layoutParams.height = LinearLayout.LayoutParams.WRAP_CONTENT;
            mDialog.getWindow().setAttributes(layoutParams);
        }*/
    }

    public void setProgress(int progress) {
        mText.setText(String.format("%d/%d", progress, mMax));
    }

    public void dismiss() {
        mDialog.dismiss();
    }
}
