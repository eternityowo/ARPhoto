using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public UnityDialogManager unityManager;

    private IDialogManager _managerCurrentPlatform = null;

    private IMessageDialog _openDialog = null;

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        _managerCurrentPlatform = new AndroidDialogManager();
#else
        _managerCurrentPlatform = unityManager;
#endif

    }

    private void CloseDialog()
    {
        if (_openDialog != null)
        {
            _openDialog.CloseDialog();
            _openDialog = null;
        }
    }

    public void ShowStratupDialog(Action positiveClick = null)
    {
        //CloseDialog();
        _openDialog = _managerCurrentPlatform.ShowInformationDialog(
            "Использование",
            "Чтобы оживить фотографии в альбоме:\n" +
            "1. Наведите камеру на QR-код, расположенный на альбоме\n" +
            "2. Скачайте дополнительные материалы (необходимо наличие интернета)\n" +
            "3. Наведите камеру на фотографии альбома\n",
            new Button("ОК", CloseDialog + positiveClick)
        );
    }

    public void ShowConfirmDownloadDialog(Action positiveClick, Action negativeClick = null)
    {
        CloseDialog();
        _openDialog = _managerCurrentPlatform.ShowConfirmDialog(
            "Подтвердите действие",
            "Был считан QR-код с альбома. Вы действительно хотите скачать дополнительные материалы? Рекомендуется скачивание альбома при наличии Wi-Fi соединения.",
            new Button("Скачать", CloseDialog + positiveClick),
            new Button("Отмена", CloseDialog + negativeClick)
        );
    }

    public IProgressDialog ShowProgressDownloadDialog()
    {
        CloseDialog();
        return _managerCurrentPlatform.ShowProgressDialog(
            "Загрузка альбома",
            "Идёт загрузка материалов. Пожалуйста, подождите. Не выключайте приложение.",
            100
        );
    }

    public void ShowDownloadCompleteDialog(Action positiveClick = null)
    {
        CloseDialog();
        _openDialog = _managerCurrentPlatform.ShowInformationDialog(
            "Загрузка завершена",
            "Загрузка для альбома завершена! Теперь вы можете навести камеру на фотографии альбома, чтобы они ожили.",
            new Button("ОК", CloseDialog + positiveClick)
        );
    }

    public void ShowDownloadErrorDialog(Action positiveClick = null)
    {
        CloseDialog();
        _openDialog = _managerCurrentPlatform.ShowInformationDialog(
            "Ошибка загрузки",
            "Произошла ошибка при загрузке. Повторите попытку позже. Возможно сервер недоступен или QR-код считался с ошибкой.",
            new Button("ОК", CloseDialog + positiveClick)
        );
    }

    public void ShowGalleryExistDialog(Action positiveClick = null)
    {
        CloseDialog();
        _openDialog = _managerCurrentPlatform.ShowInformationDialog(
            "Альбом уже загружен",
            "Данный альбом уже загружен в ваше устройство. Если фотографии не оживают, убедитесь что их хорошо видно.",
            new Button("ОК", CloseDialog + positiveClick)
        );
    }
}
