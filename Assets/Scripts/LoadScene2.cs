using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        SceneManager.LoadScene("Segundo Nivel", LoadSceneMode.Additive);
    }
}
