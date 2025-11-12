using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Tiempo en segundos antes de eliminarse
    private float timeToSelfDestruct = 5f;

    void Start()
    {
        // Iniciar la coroutine para eliminar el objeto
        StartCoroutine(DestroyAfterTime());
    }

    private System.Collections.IEnumerator DestroyAfterTime()
    {
        // Esperar 5 segundos
        yield return new WaitForSeconds(timeToSelfDestruct);
        
        // Eliminar el objeto
        Destroy(gameObject);
    }
}
