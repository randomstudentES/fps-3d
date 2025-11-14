using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject firstGunThrowable;
    public GameObject secondGunThrowable;

    [Header("Settings")]
    public int maxThrows;
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;

    [Header("Reload")]
    public float reloadCooldown = 1;


    bool readyToThrow;

    [Header("Gun")]
    public GameObject gun;
    public Animator animatorGun;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Transform gunTransform;

    [Header("KeyCodes")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode firstGunKey = KeyCode.Alpha1;
    public KeyCode secondGunKey = KeyCode.Alpha2;

    [Header("Guns")]
    private int currentGun = 1;
    public Mesh firstGunMesh;
    public Mesh secondGunMesh;

    public Material firstGunMainMaterial;
    public Material firstGunWhiskersMaterial;
    public Material secondGunMaterial;

    private GameObject objetoATirar;

    private void Start()
    {
        readyToThrow = true;
        animatorGun = gun.GetComponent<Animator>();
        meshFilter = gun.GetComponent<MeshFilter>();
        meshRenderer = gun.GetComponent<MeshRenderer>();
        gunTransform = gun.GetComponent<Transform>();
    }

    private void Update()
    {
        myInputs();

    }

    private void myInputs()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }

        if (Input.GetKeyDown(reloadKey))
        {
            readyToThrow = false;
            animatorGun.SetTrigger("Reload");
            Invoke(nameof(ResetThrow), reloadCooldown);
        }

        if (Input.GetKeyDown(firstGunKey) && currentGun != 1)
        {
            StartCoroutine(ChangeGun(1));
        } else if (Input.GetKeyDown(secondGunKey) && currentGun != 2)
        {
            StartCoroutine(ChangeGun(2));
        }

    }

    private IEnumerator ChangeGun(int gun)
    {
        animatorGun.SetTrigger("HideGun");
        readyToThrow = false;

        yield return new WaitForSeconds(animatorGun.GetCurrentAnimatorStateInfo(0).length);

        if (gun == 1)
        {
            meshFilter.mesh = firstGunMesh;
            meshRenderer.materials = new Material[] { firstGunMainMaterial, firstGunWhiskersMaterial };
            gunTransform.localScale = new Vector3(2f, -2f, 2f);
        }
        else if (gun == 2)
        {
            meshFilter.mesh = secondGunMesh;
            meshRenderer.materials = new Material[] { secondGunMaterial };
            gunTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        currentGun = gun;

        animatorGun.SetTrigger("ShowGun");
        readyToThrow = true;
    }

    private void Throw()
    {
        readyToThrow = false;

        if (currentGun == 1)
        {
            objetoATirar = firstGunThrowable;
        } else if (currentGun == 2)
        {
            objetoATirar = secondGunThrowable;
        }

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
