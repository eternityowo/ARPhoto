using UnityEngine;
using UnityEngine.UI;

public class UnityMessageDialog : MonoBehaviour, IMessageDialog
{
    public Text titleField;
    public Text messageField;

    public UnityEngine.UI.Button negativeButton;
    public UnityEngine.UI.Button positiveButton;

    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;      
    }


    public void CloseDialog()
    {
        Destroy(gameObject);
    }
}
