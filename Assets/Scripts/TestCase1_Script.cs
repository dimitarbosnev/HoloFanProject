using System.Collections;
using CMGT;
using TMPro;
using UnityEngine;

public class TestCase1_Script : MonoBehaviour
{
    public TMP_Text text;
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
        CMGTFanManager.transmissionDelay = delay;
        text.text = "Transmission Delay: " + CMGTFanManager.transmissionDelay;
        yield return new WaitForSecondsRealtime(2);
        SwapBackgroundcolor(Color.red);
        yield return new WaitForSecondsRealtime(2);
        SwapBackgroundcolor(Color.green);
        yield return new WaitForSecondsRealtime(2);
        SwapBackgroundcolor(Color.red);
    }
    
    void SwapBackgroundcolor(Color color)
    {
        CMGTFanManager.targetCamera.backgroundColor = color;
        Camera.main.backgroundColor = color;
    }
}
