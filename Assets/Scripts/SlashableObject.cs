using UnityEngine;

public class SlashableObject : MonoBehaviour
{
    private int slashedLayer;
    // Start is called before the first frame update
    void Start()
    {
        slashedLayer = LayerMask.NameToLayer("Slashed");
    }
    
    void SlashObject(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.red;
        if(obj.layer != slashedLayer)
        {
            obj.layer = slashedLayer;
            NinjaGameManager.Instance.UpdateCounter();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");
        if(other.gameObject.layer == LayerMask.NameToLayer("PhysicalHands"))
        {
            SlashObject(gameObject);
        }
    }
}
