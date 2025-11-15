using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParpadeoLuz : MonoBehaviour
{
    private Light luz;        
    public float velocidad = 5f;
    public float intensidadMin = 0f;
    public float intensidadMax = 1f;

    void Start()
    {
        luz = GetComponentInChildren<Light>();

        if (luz == null)
            Debug.LogError("No se encontró ningún componente Light en los hijos de " + gameObject.name);
    }

    void Update()
    {
        if (luz == null) return;

        float intensidad = Mathf.PingPong(Time.time * velocidad, intensidadMax - intensidadMin) + intensidadMin;
        luz.intensity = intensidad;
    }
}
