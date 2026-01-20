using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class NinjaGameManager : MonoBehaviour
{
    public TMP_Text timer;
    public TMP_Text counter;
    public TMP_Text gameOver;

    public float time = 180f;
    [HideInInspector] public int count = -1;
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
        StartCoroutine(nameof(EndMiniGame));
    }

    IEnumerator EndMiniGame()
    {
        gameOver.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        //High scores
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
