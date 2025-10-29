using UnityEngine;
using CMGT;
using System.Collections;
using UnityEngine.Experimental.Rendering;
using System.Threading;
using System.Threading.Tasks;
public class CMGTFanManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public byte[][] imageBuffer = new byte[2]{new byte[], new byte[]};
    public RenderTexture fanTexture;
    public Camera targetCamera;
    public bool isRunning;
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
        StartCoroutine("UpdateFan");
    }

    
    IEnumerator UpdateFan()
    {
        //Start
        Task task_start = Task.Run(() =>
        {
            Fan.Connect();
            Thread.Sleep(100);
            Fan.PowerOn();
            Thread.Sleep(100);
            Fan.StartProjection();
            Debug.Log("Connected");
        });
        yield return new WaitUntil(() => task_start.IsCompleted);

        //Update
        while (isRunning)
        {
            if (Fan.isProjecting)
            {
                RenderTexture.active = fanTexture;
                Texture2D tex = new Texture2D(fanTexture.width, fanTexture.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, fanTexture.width, fanTexture.height), 0, 0);
                tex.Apply();

                byte[] bytes = tex.GetRawTextureData();
                RenderTexture.active = null;

                byte[] array = Jpeg.bytesToJpeg(bytes, fanTexture.width, fanTexture.height, 30);
                Fan.ProjectOnDisplay(in array);
            }
            yield return new WaitForSecondsRealtime(0.08f);
        }
    }

    // Update is called once per frame
    void OnDestroy()
    {
        isRunning = false;
        //End
        Task task_end = Task.Run(() =>
        {
            Fan.EndProjection();
            Thread.Sleep(200);
            Fan.PowerOff();
            Thread.Sleep(200);
            Fan.Disconnect();
            Debug.Log("Disconnected");
        });
    }
}
