using System.Collections;
using CMGT;
using TMPro;
using UnityEngine;

public class TestCase1_Script : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        //CMGTFanManager.Instance.transmissionDelay = 0;
    }

    public void StartTest()
    {
        StartCoroutine("TestCase1Func");
    }

    IEnumerator TestCase1Func()
    {
        text.text = "Warning flashing screen!\nTest is about to begin...";
        yield return new WaitForSecondsRealtime(2);
        for(int i = 0; i <= 80; i += 5)
        yield return StartCoroutine("SequanceRoutine", i);

    }

    IEnumerator SequanceRoutine(int delay)
    {
        SwapBackgroundcolor(new Color(0.1921569f,0.3019608f,0.4745098f));
        CMGTFanManager.Instance.transmissionDelay = delay;
        text.text = "Transmission Delay: " + CMGTFanManager.Instance.transmissionDelay;
        yield return new WaitForSecondsRealtime(2);
        SwapBackgroundcolor(Color.red);
        yield return new WaitForSecondsRealtime(2);
        SwapBackgroundcolor(Color.green);
        yield return new WaitForSecondsRealtime(2);
        SwapBackgroundcolor(Color.red);
    }
    
    void SwapBackgroundcolor(Color color)
    {
        CMGTFanManager.Instance.targetCamera.backgroundColor = color;
        Camera.main.backgroundColor = color;
    }
}
