using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLogic : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float speed = 5f; // Velocidad de movimiento

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player != null)
        {
            // Calcular dirección hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;

            // Mover el enemigo
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        }
    }
}
