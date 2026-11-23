using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimacionNum : MonoBehaviour
{
    public TextMeshProUGUI texto;

    private string[] numeros = { "1", "2", "3", "4", "5" };
    private int indice = 0;

    public float tiempoCambio = 1f;

    void Start()
    {
        texto.text = numeros[indice];
        InvokeRepeating(nameof(CambiarNumero), tiempoCambio, tiempoCambio);
    }

    void CambiarNumero()
    {
        indice++;

        if (indice >= numeros.Length)
            indice = 0;

        texto.text = numeros[indice];
    }
}