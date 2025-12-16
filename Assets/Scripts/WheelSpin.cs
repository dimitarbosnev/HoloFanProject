using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class WheelSpin : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float spin_friction = 0.99f;
    [SerializeField] private float spin_force_min = 0.5f;
    [SerializeField] private float spin_threshold = 1;
    [SerializeField] private int num_of_value = 6;
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
    
    IEnumerator WheelSpinProc(float spin_force)
    {
        while (spin_force > spin_threshold && isRunning)
        {
            transform.eulerAngles = transform.eulerAngles + Vector3.forward * spin_force * Time.deltaTime;
            spin_force *= spin_friction;
            yield return null;
        }
        float sction_degrees = 360 / num_of_value;
        value = (int)(transform.eulerAngles.z / sction_degrees);
        isRunning = false;
    }

    void Update()
    {
        if (Mathf.Abs(rb.angularVelocity.z) > spin_force_min && !isRunning)
        {
            Debug.Log("Running");
            isRunning = true;
            Collider.enabled = false;
        }
        else if (Mathf.Abs(rb.angularVelocity.z) < spin_threshold && isRunning)
        {
            float sction_degrees = 360 / num_of_value;
            value = (int)(transform.eulerAngles.z / sction_degrees);
            rb.angularVelocity = Vector3.zero;
            isRunning = false;
            Collider.enabled = true;
            Debug.Log("Stop");
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
}
