using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimacionVocales : MonoBehaviour
{
    public TextMeshProUGUI texto;

    private string[] vocales = { "A", "E", "I", "O", "U" };
    private int indice = 0;

    public float tiempoCambio = 1f;

    void Start()
    {
        texto.text = vocales[indice];
        InvokeRepeating(nameof(CambiarVocal), tiempoCambio, tiempoCambio);
    }

    void CambiarVocal()
    {
        indice++;

        if (indice >= vocales.Length)
            indice = 0;

        texto.text = vocales[indice];
    }
}