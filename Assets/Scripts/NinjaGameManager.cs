using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NinjaGameManager : MonoBehaviour
{
    public TMP_Text timer;
    public TMP_Text counter;

    public float time = 180f;
    [HideInInspector] public int count = -1;
    [HideInInspector] public bool isRunning = false;

    private static NinjaGameManager instance = null;
    public static NinjaGameManager Instance => instance;
    void Awake()
    {
        if(instance == null) instance = this;          
        else Destroy(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateTimerText();
        UpdateCounter();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isRunning) return;

        time -= Time.deltaTime;
        time = Mathf.Max(time, 0f);

        UpdateTimerText();

        if (time <= 0f)
        {
            isRunning = false;
            OnTimerEnd();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timer.text = $"{minutes:00}:{seconds:00}";
    }

    public void OnTimerEnd()
    {
        
    }

    public void UpdateCounter()
    {
        count++;
        counter.text = $"{count}";
    }

    void OnDestroy()
    {
        if(instance == this) instance = null;
    }
}
