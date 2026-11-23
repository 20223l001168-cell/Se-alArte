using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CambiarPanel : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public TMP_Text textoBoton;
    public GameObject boton;
    public GameObject boton2;

    public void IrANivel1()
    {
        SceneManager.LoadScene("Vocales");
    }

    public void IrAlPanel3()
    {
        boton2.SetActive(true);
        boton.SetActive(true);
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(true);
    }

    public void IrAlPanel2()
    {
        boton.SetActive(true);
        panel1.SetActive(false);
        panel2.SetActive(true);
    }

    public void VolverAlPanel1()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
    }
}
