using System.Collections;
using TMPro;
using UnityEngine;

public class TestCase1_Script : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        CMGTFanManager.Instance.transmissionDelay = 0;
    }

    public void StartTest()
    {
        StartCoroutine("TestCase1Func");
    }

    IEnumerator TestCase1Func()
    {
        text.text = "Warning flashing screen!\nTest is about to begin...";
        yield return new WaitForSecondsRealtime(2);

        yield return StartCoroutine("SequanceRoutine", 0);
        yield return StartCoroutine("SequanceRoutine", 5);
        yield return StartCoroutine("SequanceRoutine", 10);
        yield return StartCoroutine("SequanceRoutine", 15);
        yield return StartCoroutine("SequanceRoutine", 20);
        yield return StartCoroutine("SequanceRoutine", 25);
        yield return StartCoroutine("SequanceRoutine", 30);
        yield return StartCoroutine("SequanceRoutine", 35);
        yield return StartCoroutine("SequanceRoutine", 40);
        yield return StartCoroutine("SequanceRoutine", 45);
        yield return StartCoroutine("SequanceRoutine", 50);
        yield return StartCoroutine("SequanceRoutine", 55);
        yield return StartCoroutine("SequanceRoutine", 60);
        yield return StartCoroutine("SequanceRoutine", 65);
        yield return StartCoroutine("SequanceRoutine", 70);
        yield return StartCoroutine("SequanceRoutine", 75);
        yield return StartCoroutine("SequanceRoutine", 80);
        yield return StartCoroutine("SequanceRoutine", 85);
        yield return StartCoroutine("SequanceRoutine", 90);
        yield return StartCoroutine("SequanceRoutine", 95);
        yield return StartCoroutine("SequanceRoutine", 100);
        yield return StartCoroutine("SequanceRoutine", 105);
        yield return StartCoroutine("SequanceRoutine", 110);
        yield return StartCoroutine("SequanceRoutine", 115);
        yield return StartCoroutine("SequanceRoutine", 120);
    }

    IEnumerator SequanceRoutine(int delay)
    {
        CMGTFanManager.Instance.transmissionDelay = delay;
        text.text = "Transmission Delay: " + CMGTFanManager.Instance.transmissionDelay;
        SwapBackgroundcolor(new Color(0.1921569f,0.3019608f,0.4745098f));
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
