using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogGunFollower : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Movimiento")]
    public float speed = 300f;
    public float chaseRange;
    public bool puedePerseguir = false;

    [Header("Ataque")]
    public GameObject bulletPrefab;   // Prefab de la minibola
    public Transform firePointRight;       // Desde donde dispara
    public Transform firePointLeft;       // Desde donde dispara
    public float fireRate = 1f;       // Balas por segundo
    public float shootRange;    // Alcance del disparo
    private float nextFireTime = 0f;

    public int vidas;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Perseguir si está en rango
        if (distanceToPlayer < chaseRange && puedePerseguir)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 targetPosition = transform.position + direction * speed * Time.deltaTime;
            rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, 0.1f));

            Quaternion lookRotation = Quaternion.LookRotation(-direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            // Instanciamos bala
            GameObject bala1 = Instantiate(bulletPrefab, firePointRight.position, firePointRight.rotation);
            GameObject bala2 = Instantiate(bulletPrefab, firePointLeft.position, firePointLeft.rotation);

            // Damos fuerza hacia adelante
            Rigidbody rbBala1 = bala1.GetComponent<Rigidbody>();
            rbBala1.AddForce(firePointRight.forward * -500f);

            Rigidbody rbBala2 = bala2.GetComponent<Rigidbody>();
            rbBala2.AddForce(firePointLeft.forward * -500f);

            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("DAÑO");
        }
        else if (collision.gameObject.CompareTag("Bala"))
        {
            restarVida(1);
        }
    }

    private void restarVida(int danyo)
    {
        vidas -= danyo;

        if (vidas < 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        // Color del rango de persecución
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Color del rango de disparo
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, shootRange);
    }


}
