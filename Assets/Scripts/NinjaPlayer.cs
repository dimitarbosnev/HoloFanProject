using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaPlayer : MonoBehaviour
{
    private int raycastLayers;
    private int slashedLayer;
    // Start is called before the first frame update
    void Start()
    {
        raycastLayers = LayerMask.GetMask("Slashable");
        slashedLayer = LayerMask.NameToLayer("Slashed");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f, raycastLayers);
            foreach (RaycastHit hit in hits)
            {
                Debug.Log("Hit object: " + hit.collider.name);
                SlashObject(in hit);
                Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
            }
        }
    }
    
    void SlashObject(in RaycastHit hit)
    {
        hit.collider.GetComponent<Renderer>().material.color = Color.red;
        hit.collider.gameObject.layer = slashedLayer;
    }
}
