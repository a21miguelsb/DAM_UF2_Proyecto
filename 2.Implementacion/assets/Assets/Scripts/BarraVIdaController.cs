using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVIdaController : MonoBehaviour
{
    public Image barraVida;
    public float vidaActual;
    private float vidaMaxima = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        barraVida.fillAmount = vidaActual / vidaMaxima;
        
    }
}
