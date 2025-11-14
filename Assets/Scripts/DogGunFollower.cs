using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogGunFollower : MonoBehaviour
{
    public Transform player;
    public float speed = 300f;
    public int vidas;
    public float chaseRange = 10f; // Rango máximo para perseguir al jugador

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player != null)
        {
            // Calcular la distancia al jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Solo perseguir al jugador si está dentro del rango
            if (distanceToPlayer < chaseRange)
            {
                Vector3 direction = (player.position - transform.position).normalized;

                // Mover el enemigo suavemente
                Vector3 targetPosition = transform.position + direction * speed * Time.deltaTime;
                rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, 0.1f));

                // Hacer que el enemigo mire hacia el jugador
                Quaternion lookRotation = Quaternion.LookRotation(-direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
            }
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
}
