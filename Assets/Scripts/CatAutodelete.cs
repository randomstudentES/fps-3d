using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private float timeToSelfDestruct = 5f;

    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private System.Collections.IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(timeToSelfDestruct);

        Destroy(gameObject);
    }
}
