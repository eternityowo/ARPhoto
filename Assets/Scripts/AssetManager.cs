using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Networking;
using EasyAR;

[RequireComponent(typeof(DialogManager), typeof(QRCodeReader))]
public class AssetManager : MonoBehaviour
{
    public AudioSource defaultAudioSource;

    private DialogManager _dialogManager;
    private CameraDeviceBaseBehaviour _cameraDevice;

    private List<string> _loadedDirectories;

    private string _path
    {
        get { return Path.Combine(Application.persistentDataPath, "albums"); }
    }

    void Awake()
    {
        _dialogManager = GetComponent<DialogManager>();
        _cameraDevice = GetComponentInChildren<CameraDeviceBaseBehaviour>();
        GetComponent<QRCodeReader>().detectQRCode += LoadNewGallery;

        Directory.CreateDirectory(_path);
        _loadedDirectories = new List<string>(Directory.GetDirectories(_path));
    }

    void Start()
    {
        if (EasyAR.ARBuilder.Instance.ImageTrackerBehaviours.Count == 0)
        {
            Debug.Log("Not found ImageTracker");
            return;
        }
        if (_loadedDirectories.Count == 0) _dialogManager.ShowStratupDialog();
        else LoadExistGalleries();
    }

    private void LoadExistGalleries()
    {
        foreach (var name in _loadedDirectories)
        {
            if (!LoadGallery(name))
            {
                //TODO: add dialog refresh download
                //LoadNewGallery(name);
            }
        }
    }

    private bool LoadGallery(string id)
    {
        var currentPath = Path.Combine(_path, id);
        var assetBundle = AssetBundle.LoadFromFile(Path.Combine(currentPath, "content"));

        if (assetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return false;
        }

        var info = assetBundle.LoadAsset<TextAsset>("info.txt").text;
        Vector2[] objectSize = Utils.ParseStringToSize(info);
        var filesCount = Directory.GetFiles(currentPath).Length;

        //TODO:check filename, but no check count
        if (filesCount - 1 != objectSize.Length)
        {
            Debug.Log("Not all images!");
            return false;
        }

        for (int i = 1; i < filesCount; i++)
        {
            CreateImageTarget(
                i.ToString(),
                Path.Combine(currentPath, String.Format("{0}.jpg", i)),
                objectSize[i - 1],
                assetBundle.LoadAsset<GameObject>(i.ToString())
            );
        }
        return true;
    }

    private void CreateImageTarget(string name, string imagePath, Vector2 size, GameObject contents)
    {
        var gameObject = new GameObject("ImageTarget");
        var imageTarget = gameObject.AddComponent<EasyAR.ImageTargetBehaviour>();
        imageTarget.SetupWithImage(imagePath, EasyAR.StorageType.Absolute, name, new Vector2());
        imageTarget.Bind(EasyAR.ARBuilder.Instance.ImageTrackerBehaviours[0]);

        var prefab = Instantiate(contents, gameObject.transform);
        prefab.transform.localScale = new Vector3(size.x, size.y, 1);

        AddRequireComponents(imageTarget, prefab);
    }

    private void AddRequireComponents(EasyAR.ImageTargetBehaviour imageTarget, GameObject prefab)
    {
        var video = prefab.GetComponent<VideoPlayer>();
        if (video != null)
        {
            var render = video.GetComponent<MeshRenderer>();
            imageTarget.TargetFound += (obj) => { video.Play(); };
            imageTarget.TargetLost += (obj) => { video.Stop(); render.enabled = false; };
            video.prepareCompleted += (source) => render.enabled = true;
            render.enabled = false;
        }
        //TODO: add Video scripts
    }

    public void LoadNewGallery(string id)
    {
        _cameraDevice.StopCapture();
        if (!IsDownloadedContent(Path.Combine(_path, id)))
        {
            _dialogManager.ShowConfirmDownloadDialog(
                () => { StartCoroutine(LoadFromWeb(id)); },
                () => { _cameraDevice.StartCapture(); }
            );
        }
        else
        {
            _dialogManager.ShowGalleryExistDialog(() => { _cameraDevice.StartCapture(); });
        }
    }

    private IEnumerator LoadFromWeb(string id)
    {
        IProgressDialog dialog = GetComponent<DialogManager>().ShowProgressDownloadDialog();
        //Add id in web url
        UnityWebRequest request = UnityWebRequest.Get("http://localhost:52250/photocards/" + id);
        //request.SetRequestHeader("Authorization", "Basic cGhvdG9jYXJkczpwYXNz");
        yield return request.SendWebRequest();

        dialog.Progress = 25;

        if (IsRequestError(request))
        {
            _cameraDevice.StartCapture();
            dialog.CloseDialog();
            _dialogManager.ShowDownloadErrorDialog();
            yield break;
        }

        Debug.Log(request.downloadHandler.text);
        GalleryData json;
        try
        {
            json = JsonUtility.FromJson<GalleryData>(request.downloadHandler.text);
        }
        catch
        {
            _cameraDevice.StartCapture();
            dialog.CloseDialog();
            _dialogManager.ShowDownloadErrorDialog();
            yield break;
        }

        string pathGallery = Path.Combine(_path, id);
        Debug.Log(pathGallery + "\n");
        Directory.CreateDirectory(pathGallery);

        request = UnityWebRequest.Get(json.assetBundle);
        request.downloadHandler = new DownloadHandlerFile(Path.Combine(pathGallery, "content"));
        //request.SetRequestHeader("Authorization", "Basic cGhvdG9jYXJkczpwYXNz");
        yield return request.SendWebRequest();
        if (IsRequestError(request))
        {
            _cameraDevice.StartCapture();
            dialog.CloseDialog();
            _dialogManager.ShowDownloadErrorDialog();
            yield break;
        }

        dialog.Progress = 50;

        float step = 50f / json.data.Length;

        for (int i = 0; i < json.data.Length; i++)
        {
            request = UnityWebRequest.Get(json.data[i]);
            request.downloadHandler = new DownloadHandlerFile(Path.Combine(pathGallery, String.Format("{0}.jpg", i + 1)));
            //request.SetRequestHeader("Authorization", "Basic cGhvdG9jYXJkczpwYXNz");
            yield return request.SendWebRequest();
            dialog.Progress = 50 + (int)(step * (i + 1));
            if (IsRequestError(request))
            {
                _cameraDevice.StartCapture();
                dialog.CloseDialog();
                _dialogManager.ShowDownloadErrorDialog();
                yield break;
            }
            Debug.Log("Complete: " + i);
        }
        dialog.CloseDialog();
        GetComponent<DialogManager>().ShowDownloadCompleteDialog(() => { _cameraDevice.StartCapture(); });
        _loadedDirectories.Add(id);
        LoadGallery(id);
    }

    private bool IsRequestError(UnityWebRequest request)
    {
        bool result = (request.isNetworkError || request.isHttpError);
        if (result) Debug.LogAssertion(request.error);
        return result;
    }

    public bool IsDownloadedContent(string id)
    {
        foreach (var directory in _loadedDirectories)
        {
            if (directory.Equals(id)) return true;
        }
        return false;
    }
}
