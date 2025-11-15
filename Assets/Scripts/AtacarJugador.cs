using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtacarJugador : MonoBehaviour
{

    public int vidas;
    public int daño;

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
