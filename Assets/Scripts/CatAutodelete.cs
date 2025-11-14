using UnityEngine;

public class SelfDestructExplosion : MonoBehaviour
{

    [Header("Explota")]
    public bool explota;

    [Header("Tiempo antes de autodestrucción")]
    public float timeToSelfDestruct = 5f;

    [Header("Onda expansiva")]
    public float explosionRadius = 10f;
    public float explosionForce = 70000f;
    public float upwardModifier = 1f;

    [Header("Efectos opcionales")]
    public GameObject explosionEffect;

    private bool hasExploded = false;

    void Start()
    {
        // Destruye el objeto tras un tiempo si no colisiona antes
        StartCoroutine(DestroyAfterTime());
    }

    private System.Collections.IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(timeToSelfDestruct);
        if (explota)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded && explota)
        {
            Explode();
        }
        else if (!hasExploded)
        {

            Destroy(gameObject);

        }
    }

    private void Explode()
    {
        hasExploded = true;

        // Instancia efecto visual
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Detecta todos los objetos cercanos
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (nearby.gameObject.CompareTag("1LifeHamster"))
                {
                    Destroy(nearby.gameObject);
                }
                else
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier, ForceMode.Impulse);
                }
            }
        }

        // Finalmente, destruye este objeto
        Destroy(gameObject);
    }

    // Dibuja el radio en la vista de escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.4f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
