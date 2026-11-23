using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    [Header("Cantidad de niveles")]
    public int cantidadNiveles = 5;

    [Header("UI")]
    public TMP_Text txtPuntajeTotal;
    public TMP_Text txtEstrellasTotales;
    public TMP_Text txtNivelesCompletados;

    void Start()
    {
        ActualizarPerfil();
    }

    void ActualizarPerfil()
    {
        int puntajeTotal = 0;
        int estrellasTotales = 0;
        int nivelesCompletados = 0;

        for (int i = 1; i <= cantidadNiveles; i++)
        {
            int puntaje = SaveManager.ObtenerPuntaje(i);
            int estrellas = SaveManager.ObtenerEstrellas(i);

            puntajeTotal += puntaje;
            estrellasTotales += estrellas;

            if (puntaje > 0)
                nivelesCompletados++;
        }

        txtPuntajeTotal.text = puntajeTotal.ToString();
        txtEstrellasTotales.text = estrellasTotales.ToString();
        txtNivelesCompletados.text = nivelesCompletados + " / " + cantidadNiveles;
    }
}