using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    public GameObject[] enemigos;
    private MouseLogic enemigo;
    private DogGunFollower dog;


    private void OnTriggerEnter(Collider other)
    {
        for (global::System.Int32 i = 0; i < enemigos.Length; i++) { 
        
            if (enemigos[i].CompareTag("1LifeHamster"))
            {
                enemigo = enemigos[i].GetComponent<MouseLogic>();
                enemigo.puedePerseguir = true;
            } else if (enemigos[i].CompareTag("DogWithGun"))
            {
                dog = enemigos[i].GetComponent<DogGunFollower>();
                dog.puedePerseguir = true;
            }
            
        }
    }

}
