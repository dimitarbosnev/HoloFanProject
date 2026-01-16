using UnityEngine;
using CMGT;
using System.Collections;
using UnityEngine.Experimental.Rendering;
using System.Threading;
using System.Threading.Tasks;
using System;
[RequireComponent(typeof(Camera))]
public class CMGTFanManager : MonoBehaviour
{
    private static CMGTFanManager instance = null;
    public static CMGTFanManager Instance => instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
            
        else Destroy(this);
    }

    [HideInInspector] public RenderTexture fanTexture;
    [HideInInspector] public Camera targetCamera;
    [HideInInspector] public bool isRunning;
    public int transmissionDelay = 40;
    public event Action OnTransmition;
    void Start()
    {
        fanTexture = new RenderTexture(Fan.TextureSize, Fan.TextureSize, 0);
        fanTexture.graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;
        fanTexture.antiAliasing = 1;
        fanTexture.wrapMode = TextureWrapMode.Clamp;
        fanTexture.filterMode = FilterMode.Bilinear;

        targetCamera = GetComponent<Camera>();
        targetCamera.targetTexture = fanTexture;
        isRunning = true;
        ConnectFan();
    }

    
    IEnumerator UpdateFan()
    {
        while (isRunning)
        {
            if (Fan.isProjecting)
            {
                RenderTexture flippedRT = new RenderTexture(fanTexture.width, fanTexture.height, 0);
                Graphics.Blit(fanTexture, flippedRT, new Vector2(1, -1), new Vector2(0, 1));

                RenderTexture.active = flippedRT;
                Texture2D tex = new Texture2D(fanTexture.width, fanTexture.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, fanTexture.width, fanTexture.height), 0, 0, false);
                tex.Apply();

                byte[] bytes = tex.GetRawTextureData();
                RenderTexture.active = null;
                
                byte[] array = Jpeg.bytesToJpeg(bytes, fanTexture.width, fanTexture.height, 30);
                Fan.ProjectOnDisplay(in array);
            }
            OnTransmition?.Invoke();
            float delay = transmissionDelay/1000.0f;
            yield return new WaitForSecondsRealtime(delay);
        }
    }
    public void ConnectFan()
    {
        StopAllCoroutines();
        Task task_start = Task.Run(() =>
        {
            Fan.Connect();
            Thread.Sleep(500);
            Fan.PowerOn();
            Thread.Sleep(500);
            Fan.StartProjection();
            Debug.Log("Connected");
        });
        StartCoroutine("UpdateFan");
    }

    public void DisconnecFan()
    {
        isRunning = false;
        //End
        Task task_end = Task.Run(() =>
        {
            Fan.EndProjection();
            Thread.Sleep(300);
            Fan.PowerOff();
            Thread.Sleep(300);
            Fan.Disconnect();
            Debug.Log("Disconnected");
        });
    }

    // Update is called once per frame
    void OnDestroy()
    {
        DisconnecFan();

        if(instance == this) instance = null;
    }
}
