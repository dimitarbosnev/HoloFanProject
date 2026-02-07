using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WheelSpin : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float spin_friction = 0.99f;
    [SerializeField] private float spinForceMin = 0.5f;
    [SerializeField] private float spinThreshold = 1;
    [SerializeField] private float waitBeforeSeconds = 1f;
    [SerializeField] private float waitAfterSeconds = 10f;
    [SerializeField] private List<UnityEvent> events;

    [SerializeField] private GameObject factCanvas;
    [SerializeField] private GameObject stickerCanvas;
    [SerializeField] private GameObject nothingCanvas;

    private bool isRunning = false;
    private Rigidbody rb;
    private BoxCollider Collider;
    private AudioSource source;
    private int section;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.angularVelocity = new Vector3(0,0,5);
        }

        if (Mathf.Abs(rb.angularVelocity.z) > spinForceMin && !isRunning)
        {
            Debug.Log("Running");
            isRunning = true;
            Collider.enabled = false;
        }
        else if (Mathf.Abs(rb.angularVelocity.z) < spinThreshold && isRunning)
        {
            float sction_degrees = 360 / events.Count;
            int value = (int)(transform.eulerAngles.z / sction_degrees);
            rb.angularVelocity = Vector3.zero;
            isRunning = false;
            Collider.enabled = true;
            Debug.Log("Stop");
            events[value]?.Invoke();
        }

        if (isRunning)
        {
            float sction_degrees = 360 / events.Count;
            int value = (int)(transform.eulerAngles.z / sction_degrees);
            if(section != value)
            {
                source.Play();
                section = value;
            }
        }
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            rb.angularVelocity = Vector3.forward * rb.angularVelocity.z * spin_friction;
        }
    }

    public void MiniGame()
    {
        Invoke(nameof(LoadScene), waitBeforeSeconds);

    }
    private void LoadScene()
    {
        SceneManager.LoadSceneAsync("SlashNinja");
    }

    public void Fact()
    {
        StartCoroutine(nameof(ShowFact));
    }

    private IEnumerator ShowFact()
    {
        yield return new WaitForSeconds(waitBeforeSeconds);
        factCanvas.SetActive(true);
        yield return new WaitForSeconds(waitAfterSeconds);
        factCanvas.SetActive(false);
    }

    public void Sticker()
    {
        StartCoroutine(nameof(ShowSticker));
    }

    private IEnumerator ShowSticker()
    {
        yield return new WaitForSeconds(waitBeforeSeconds);
        stickerCanvas.SetActive(true);
        yield return new WaitForSeconds(waitAfterSeconds);
        stickerCanvas.SetActive(false);
    }

    public void Nothing()
    {
        StartCoroutine(nameof(ShowNothing));
    }

    private IEnumerator ShowNothing()
    {
        yield return new WaitForSeconds(waitBeforeSeconds);
        nothingCanvas.SetActive(true);
        yield return new WaitForSeconds(waitAfterSeconds);
        nothingCanvas.SetActive(false);
    }
}
