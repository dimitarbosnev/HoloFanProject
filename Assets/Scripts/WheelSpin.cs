using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class WheelSpin : MonoBehaviour
{
    [SerializeField, Range(0,1)] private float spin_friction = 0.99f;
    [SerializeField] private float spin_force_min = 10000;
    [SerializeField] private float spin_force_max = 20000;
    [SerializeField] private float spin_threshold = 1;

    [SerializeField] private int num_of_value = 6;
    [SerializeField, ReadOnly(true)] private int value = 0;
    [SerializeField, ReadOnly(true)] private Vector3 torque;
    private bool isRunning = false;
    private Rigidbody rb;
    private BoxCollider collider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
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
            collider.enabled = false;
        }
        else if (Mathf.Abs(rb.angularVelocity.z) < spin_threshold && isRunning)
        {
            float sction_degrees = 360 / num_of_value;
            value = (int)(transform.eulerAngles.z / sction_degrees);
            rb.angularVelocity = Vector3.zero;
            isRunning = false;
            collider.enabled = true;
            Debug.Log("Stop");
        }


        torque = rb.angularVelocity;


        //if (Input.GetKeyDown(KeyCode.S) && !isRunning)
        //{
        //    float force = Random.Range(spin_force_min, spin_force_max);
        //    isRunning = true;
        //    StartCoroutine("WheelSpinProc", force);
        //}
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            rb.angularVelocity = Vector3.forward * rb.angularVelocity.z * spin_friction;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.layer == LayerMask.NameToLayer("PhysicalHands") && !isRunning)
        //{
        //    float force = Random.Range(spin_force_min, spin_force_max);
        //    isRunning = true;
        //    StartCoroutine("WheelSpinProc", force);
        //}
    }
}
