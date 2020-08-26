using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Unity Dialog Manager", menuName = "Unity Dialog Manager", order = 51)]
public class UnityDialogManager : ScriptableObject, IDialogManager
{
    [SerializeField]
    private UnityMessageDialog messagePrefab = null;
    [SerializeField]
    private UnityProgressDialog progressPrefab = null;

    public IMessageDialog ShowConfirmDialog(string title, string text, Button positiveButton, Button negativeButton)
    {
        UnityMessageDialog dialog = Instantiate(messagePrefab);
        dialog.titleField.text = title;
        dialog.messageField.text = text;
        dialog.positiveButton.onClick.AddListener(delegate { positiveButton.Action.Invoke(); });
        dialog.positiveButton.transform.GetChild(0).GetComponent<Text>().text = positiveButton.Name;
        dialog.negativeButton.onClick.AddListener(delegate { negativeButton.Action.Invoke(); });
        dialog.negativeButton.transform.GetChild(0).GetComponent<Text>().text = negativeButton.Name;

        return dialog;
    }
    public IMessageDialog ShowInformationDialog(string title, string text, Button positiveButton)
    {
        UnityMessageDialog dialog = Instantiate(messagePrefab).GetComponent<UnityMessageDialog>();
        dialog.titleField.text = title;
        dialog.messageField.text = text;
        dialog.positiveButton.onClick.AddListener(delegate { positiveButton.Action.Invoke(); });
        dialog.positiveButton.transform.GetChild(0).GetComponent<Text>().text = positiveButton.Name;
        dialog.negativeButton.gameObject.SetActive(false);

        return dialog;
    }
    public IProgressDialog ShowProgressDialog(string title, string text, int max)
    {
        UnityProgressDialog dialog = Instantiate(progressPrefab);
        dialog.titleField.text = title;
        dialog.messageField.text = text;
        dialog.max = max;
        dialog.Progress = 0;
        
        return dialog;
    }
}
