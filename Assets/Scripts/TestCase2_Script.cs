using System.Collections;
using System.Collections.Generic;
using CMGT;
using TMPro;
using UnityEngine;

public class TestCase2_Script : MonoBehaviour
{
    public TMP_Text text;

    public void StartTest()
    {
        StartCoroutine("TestCase2Func");
    }

    IEnumerator TestCase2Func()
    {
        text.text = "Warning flashing screen!\nTest is about to begin...";
        yield return new WaitForSecondsRealtime(2);
        
        for(int i = 0; i <= 80; i += 5)
        yield return StartCoroutine("SequanceRoutine", i);

        CMGTFanManager.PostTransmition -= ColorSwapOnTransmition;
    }

    IEnumerator SequanceRoutine(int delay)
    {
        CMGTFanManager.transmissionDelay = delay;
        text.text = "Transmission Delay: " + CMGTFanManager.transmissionDelay;
        index = 0;
        CMGTFanManager.PostTransmition -= ColorSwapOnTransmition;
        SwapBackgroundcolor(new Color(0.1921569f,0.3019608f,0.4745098f));
        yield return new WaitForSecondsRealtime(2);
        CMGTFanManager.PostTransmition += ColorSwapOnTransmition;
        yield return new WaitForSecondsRealtime(2);
    }

    static float duration = 3f;
    Color[] colors = new Color[] { Color.red, Color.blue, Color.cyan, Color.green, Color.white, Color.magenta, Color.yellow};
    static int index = 0;

    void ColorSwapOnTransmition()
    {
        SwapBackgroundcolor(colors[index++]);
        if(index >= colors.Length) index = 0;
    }
    
    void SwapBackgroundcolor(Color color)
    {
        CMGTFanManager.targetCamera.backgroundColor = color;
        Camera.main.backgroundColor = color;
    }

    void OnDestroy()
    {
        CMGTFanManager.PostTransmition -= ColorSwapOnTransmition;
    }
}
