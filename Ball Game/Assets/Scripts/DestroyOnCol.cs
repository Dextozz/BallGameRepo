using UnityEngine;

public class DestroyOnCol : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Terrain" || other.tag == "Player")
            Destroy(transform.gameObject);
    }
}
