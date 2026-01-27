using UnityEngine;
using CMGT;
using UnityEngine.Experimental.Rendering;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.IO;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CMGTFanManager : MonoBehaviour
{
    private bool disconnectOnDestroy = true;
    public int transmissionDelay = 35;
    public int quality = 100;

    private static CMGTFanManager instance = null;
    public static CMGTFanManager Instance => instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 24;
        }     
        else 
        {
            disconnectOnDestroy = false;
            Destroy(gameObject);
        }
    }

    public static RenderTexture fanTexture;
    public static byte[] textureBytes = new byte[24 * Fan.TextureSize * Fan.TextureSize];
    public static byte[] jpegBytes;
    public static Camera targetCamera;
    public static bool isRunning;
    public static event Action PostTransmition;
    void Start()
    {
        fanTexture = new RenderTexture(Fan.TextureSize, Fan.TextureSize, 0)
        {
            graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm,
            antiAliasing = 1,
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear,
        };
        fanTexture.Create();

        targetCamera = GetComponent<Camera>();
        targetCamera.targetTexture = fanTexture;
        targetCamera.clearFlags = CameraClearFlags.SolidColor;
        targetCamera.backgroundColor = Color.black;
        targetCamera.rect = new Rect(0, 0, 1, 1);
        targetCamera.ResetProjectionMatrix();
        targetCamera.aspect = 1.0f;
        isRunning = true;
        ConnectFan();
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        Jpeg.BytesSaveToJpeg(textureBytes, Fan.TextureSize, Fan.TextureSize, 30, "E:/Year 4 Semester 1/HologramFanBuild/screenshotJpeg.jpeg");
    //    }
    //}

    static void Transmit()
    { 
        Fan.ProjectOnDisplay(in jpegBytes);
        PostTransmition?.Invoke();
    }


    static void Compress()
    {
        jpegBytes = Jpeg.bytesToJpeg(textureBytes, Fan.TextureSize, Fan.TextureSize, Instance.quality);
    }

    public static Task transmit = null;
    IEnumerator UpdateFan()
    {
        while (isRunning)
        {
            if (Fan.isProjecting)
            {    
                Compress();
                Transmit();
            }
            float delay = transmissionDelay/1000.0f;
            yield return new WaitForSecondsRealtime(delay);
            
        }
    }

    void OnPostRender()
    {
        RenderTexture flippedRT = new RenderTexture(Fan.TextureSize, Fan.TextureSize, 0);
        Graphics.Blit(fanTexture, flippedRT, new Vector2(1, -1), new Vector2(0, 1));
        RenderTexture.active = flippedRT;
        Texture2D tex = new Texture2D(Fan.TextureSize, Fan.TextureSize, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Fan.TextureSize, Fan.TextureSize), 0, 0, false);
        tex.Apply();
        tex.GetRawTextureData().CopyTo(textureBytes,0);
        RenderTexture.active = null;

        if (Fan.isProjecting)
        {    
            Compress();
            Transmit();
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
        //StartCoroutine(nameof(UpdateFan));
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
        if(disconnectOnDestroy) DisconnecFan();
        if(instance == this) instance = null;
    }
}