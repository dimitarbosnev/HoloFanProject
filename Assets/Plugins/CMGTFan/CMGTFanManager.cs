using UnityEngine;
using CMGT;
using UnityEngine.Experimental.Rendering;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections;
[RequireComponent(typeof(Camera))]
public class CMGTFanManager : MonoBehaviour
{
    private bool disconnectOnDestroy = true;

    private static CMGTFanManager instance = null;
    public static CMGTFanManager Instance => instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }     
        else 
        {
            disconnectOnDestroy = false;
            Destroy(this);
        }
    }

    public static RenderTexture fanTexture;
    public static byte[] textureBytes = new byte[24 * Fan.TextureSize * Fan.TextureSize];
    public static Camera targetCamera;
    public static bool isRunning;
    public static int transmissionDelay = 40;
    public static event Action PostTransmition;
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

    static void Transmit()
    {
        byte[] array = Jpeg.bytesToJpeg(textureBytes, Fan.TextureSize, Fan.TextureSize, 30);
        Fan.ProjectOnDisplay(in array);
    }

    
    IEnumerator UpdateFan()
    {
        while (isRunning)
        {
            if (Fan.isProjecting)
            {    
                Task.Run(Transmit);
            }
            PostTransmition?.Invoke();
            float delay = transmissionDelay/1000.0f;
            yield return new WaitForSecondsRealtime(delay);
        }
    }

    void OnPostRender()
    {
        RenderTexture flippedRT = new RenderTexture(fanTexture.width, fanTexture.height, 0);
        Graphics.Blit(fanTexture, flippedRT, new Vector2(1, -1), new Vector2(0, 1));
        RenderTexture.active = flippedRT;
        Texture2D tex = new Texture2D(fanTexture.width, fanTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, fanTexture.width, fanTexture.height), 0, 0, false);
        tex.Apply();
        tex.GetRawTextureData().CopyTo(textureBytes,0);
        RenderTexture.active = null;
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
        if(disconnectOnDestroy)
            DisconnecFan();

        if(instance == this) instance = null;
    }
}
