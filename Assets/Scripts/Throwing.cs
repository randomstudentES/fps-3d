using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objetoATirar;

    [Header("Settings")]
    public int maxThrows;
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    [Header("Reload")]
    public KeyCode reloadKey = KeyCode.R;
    public float reloadCooldown = 1;


    bool readyToThrow;

    [Header("Gun")]
    public GameObject gun;
    public Animator animatorGun;

    private void Start()
    {
        readyToThrow = true;
        animatorGun = gun.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0) {
            Throw();
        }

        if (Input.GetKeyDown(reloadKey)) {
            readyToThrow = false;
            animatorGun.SetTrigger("Reload");
            Invoke(nameof(ResetThrow), reloadCooldown);
        }

    }
    private void Throw()
    {
        readyToThrow = false;

        GameObject projectile = Instantiate(objetoATirar, attackPoint.position, cam.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceAdd, ForceMode.Impulse);

        totalThrows--;

        animatorGun.SetTrigger("Shoot");

        // Llamar a Invoke para destruir el proyectil después de un tiempo específico
        Invoke(nameof(DestroyProjectile), 2f); // Cambia 2f por el tiempo deseado

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void DestroyProjectile(GameObject projectile)
    {
        Destroy(projectile);
    }


    public void ResetThrow()
    {
        readyToThrow = true;
    }

}
