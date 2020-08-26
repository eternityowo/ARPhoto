using UnityEngine;
using UnityEngine.UI;

public class UnityProgressDialog : MonoBehaviour, IProgressDialog
{
    public Text titleField;
    public Text messageField;

    public Image progressBar;
    public Text progressHint;

    public int max = 0;

    private int _currentProgress = 0;

    public int Progress
    {
        get
        {
            return _currentProgress;
        }
        set
        {
            _currentProgress = value;
            progressBar.fillAmount = ((float)_currentProgress) / max;
            progressHint.text = string.Format("{0}/{1}", _currentProgress, max);
        }
    }

    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }


    public void CloseDialog()
    {
        Destroy(gameObject);
    }
}