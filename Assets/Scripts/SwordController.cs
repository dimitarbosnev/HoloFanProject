using Leap;
using UnityEngine;

[RequireComponent(typeof(GrabDetector))]
public class SwordController : MonoBehaviour
{
    private GrabDetector detector;
    private GameObject swordChild;
    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent(out GrabDetector det))
        {
            detector = det;
        }
        detector.onActionStart += OnGrabStart;
        detector.onActionEnd += OnGrabEnd;

        swordChild = transform.GetChild(0).gameObject;
        if(swordChild)
            swordChild.SetActive(false);
    }

    void Update()
    {
        if (!detector.IsGrabbing && detector.TryGetHand(out Hand hand))
        {
        Pose pose;
        pose.position = hand.PalmPosition + (Vector3.right *0.01f);
        pose.rotation = Quaternion.LookRotation(hand.PalmNormal, Vector3.up);
        transform.SetWorldPose(pose);
        }
    }

    void OnDisable()
    {
        detector.onActionStart -= OnGrabStart;
        detector.onActionEnd -= OnGrabEnd;
    }

    void OnGrabStart(Hand hand)
    {
        if(swordChild)
            swordChild.SetActive(false);
    }

    void OnGrabEnd(Hand hand)
    {
        if(swordChild)
            swordChild.SetActive(true);
    }
}
