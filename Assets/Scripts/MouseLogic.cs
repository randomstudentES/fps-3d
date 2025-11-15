using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLogic : MonoBehaviour
{
    public Transform player;
    public float speed = 300f;
    public int vidas;
    public bool puedePerseguir = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player != null && puedePerseguir)
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