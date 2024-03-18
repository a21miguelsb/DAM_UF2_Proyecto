using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject target;
    private float velocidadCamara= 15f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nuevaPosicion = transform.position;
        nuevaPosicion.x = target.transform.position.x;

        transform.position = Vector3.Lerp(transform.position,nuevaPosicion,velocidadCamara * Time.deltaTime);    
    }
}
