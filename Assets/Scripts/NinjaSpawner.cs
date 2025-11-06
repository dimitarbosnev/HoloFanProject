
using System.Collections;
using UnityEngine;

public class NinjaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 timeMinMax;
    [SerializeField] private Vector3 size;
    [SerializeField] private Vector2 forceMin;
    [SerializeField] private Vector2 forceMax;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnObjects");
    }
    
    IEnumerator SpawnObjects()
    {
        yield return new WaitForSeconds(2.0f);
        while (true)
        {
            float min_max = size.x / 2;
            float PosX = Random.Range(-min_max, min_max);
            GameObject spawned = Instantiate(prefab, transform.position + new Vector3(PosX, 0, 0), Quaternion.identity, transform);
            float forceX = Random.Range(forceMin.x, forceMax.x);
            float forceY = Random.Range(forceMin.y, forceMax.y);
            spawned.GetComponent<Rigidbody>().AddForce(new Vector3(forceX, forceY, 0), ForceMode.Impulse);
            float time = Random.Range(timeMinMax.x, timeMinMax.y);
            yield return new WaitForSeconds(time);
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
