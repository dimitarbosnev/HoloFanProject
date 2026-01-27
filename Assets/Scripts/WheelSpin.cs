using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WheelSpin : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float spin_friction = 0.99f;
    [SerializeField] private float spin_force_min = 0.5f;
    [SerializeField] private float spin_threshold = 1;
    [SerializeField] private List<UnityEvent> events;

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

        if (Mathf.Abs(rb.angularVelocity.z) > spin_force_min && !isRunning)
        {
            Debug.Log("Running");
            isRunning = true;
            Collider.enabled = false;
        }
        else if (Mathf.Abs(rb.angularVelocity.z) < spin_threshold && isRunning)
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
        Invoke(nameof(LoadScene), 1f);

    }
    private void LoadScene()
    {
        SceneManager.LoadSceneAsync("SlashNinja");
    }

    public void Fact()
    {
        Invoke(nameof(ShowFact), 1f);
    }

    private void ShowFact()
    {
        
    }

    public void Sticker()
    {
        Invoke(nameof(ShowSticker), 1f);
    }

    private void ShowSticker()
    {
        
    }

    public void Nothing()
    {
        Invoke(nameof(ShowNothing), 1f);
    }

    private void ShowNothing()
    {
        
    }
}
