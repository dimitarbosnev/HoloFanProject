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
    [SerializeField, ReadOnly(true)] private int value = 0;
    [SerializeField, ReadOnly(true)] private Vector3 torque;
    public UnityAction action;

    private bool isRunning = false;
    private Rigidbody rb;
    private BoxCollider Collider;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
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
            value = (int)(transform.eulerAngles.z / sction_degrees);
            rb.angularVelocity = Vector3.zero;
            isRunning = false;
            Collider.enabled = true;
            Debug.Log("Stop");
            events[value]?.Invoke();
        }

        torque = rb.angularVelocity;
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
        SceneManager.LoadScene("SlashNinja");
    }

    public void Fact()
    {
        
    }

    public void Sticker()
    {
        
    }

    public void Nothing()
    {
        
    }
}
