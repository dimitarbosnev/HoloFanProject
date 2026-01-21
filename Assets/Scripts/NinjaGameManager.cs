using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;

public class NinjaGameManager : MonoBehaviour
{
    public TMP_Text timer;
    public TMP_Text counter;
    public TMP_Text gameOver;

    public float time = 180f;
    [HideInInspector] public int count = 0;
    [HideInInspector] public bool isRunning = false;

    private static NinjaGameManager instance = null;
    public static NinjaGameManager Instance => instance;
    void Awake()
    {
        if(instance == null) instance = this;          
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = CMGTFanManager.targetCamera;

        UpdateTimerText();
        count = 0;
        counter.text = $"{count}";
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
        StartCoroutine(nameof(EndMiniGame));
    }

    IEnumerator EndMiniGame()
    {
        gameOver.text = "GAME OVER";
        gameOver.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        //High scores
        HighScoreData data = SaveSystem<HighScoreData>.Read();
        List<int> scores = new List<int>(5);
        for(int i = 0; i < 5; i++)
        {
            scores.Add(data[i]);
        }
        scores.Sort();
        int index = scores.BinarySearch(count);
        if (index < 0)
            index = ~index; // bitwise complement gives insertion position

        scores.Insert(index, count);
        
        string highscore = "HIGH SCORES\n\n";
        highscore = string.Concat(highscore, "1st ",   index == 5? "YOU " : "", scores[5].ToString());
        highscore = string.Concat(highscore, "\n2nd ", index == 4? "YOU " : "", scores[4].ToString());
        highscore = string.Concat(highscore, "\n3rd ", index == 3? "YOU " : "", scores[3].ToString());
        highscore = string.Concat(highscore, "\n4th ", index == 2? "YOU " : "", scores[2].ToString());
        highscore = string.Concat(highscore, "\n5th ", index == 1? "YOU " : "", scores[1].ToString());
        highscore = string.Concat(highscore, "\nYOU ", count);

        gameOver.text = highscore;
        for(int i = 5; i > 0; i--)
        {
            data[i-1] = scores[i];
        }

        yield return new WaitForSeconds(2f);
        SaveSystem<HighScoreData>.Save(data);

        //Go back to scene
        SceneManager.LoadScene("TestScene");
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
