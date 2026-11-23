using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Text;
using SenalArte;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public enum EstadoNivel
{
    MostrandoAnimacion,
    EsperandoRespuesta,
    Correcto,
    Incorrecto,
    NivelTerminado
}


public class GameManager : MonoBehaviour
{
    public int numeroNivel = 1;
    public string[] letras = { "A", "E", "I", "O", "U" }; // puedes poner todas
    private int nivelActual = 0;
    public EstadoNivel estadoActual;
    private int erroresTotales = 0;
    private float tiempoInicio;

    private bool primerIntentoActual = true;

    private int aciertosPrimerIntento = 0;

    private int puntuacion = 0;

    private int estrellasObtenidas = 0;
    public GameObject solution;

    public TMP_Text textoObjetivo;
    public TMP_Text textoDetectado;
    public TMP_Text feedback;
    public TMP_Text txtPuntuacionFinal;
    public TMP_Text txtTiempoFinal;
    public TMP_Text txtErroresFinal;
    public TMP_Text txtPrimerIntentoFinal;
    public TMP_Text txtEstrellasFinal;
    public GameObject panelFinNivel;
    public Animator animatorPanelFinal;
    public HandTrackingManager handTracking;
    public HandLandmarkerRunner handLandmarkerRunner;
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip errorSound;
    public AudioClip victorySound;

    void Start()
    {
        tiempoInicio = Time.time;

        CambiarEstado(EstadoNivel.MostrandoAnimacion);

        MostrarNivel();

        feedback.text = "...";

        Invoke("ActivarEvaluacion", 3f);

    }

    public void CambiarEstado(EstadoNivel nuevoEstado)
    {
        estadoActual = nuevoEstado;

        Debug.Log("Estado actual: " + estadoActual);
    }

   bool puedeEvaluar = true;

   private void OnEnable()
    {
        HandTrackingManager.OnLetterDetected += RecibirLetra;
    }

    private void OnDisable()
    {
        HandTrackingManager.OnLetterDetected -= RecibirLetra;
    }

    private string letraUsuario = "";

    void RecibirLetra(string letra)
    {
        letraUsuario = letra;
    }

    void Update()
    {
        //string letraUsuario = handTracking.LetraActual;

        if (!puedeEvaluar) return;
        if (estadoActual != EstadoNivel.EsperandoRespuesta)
            return;
        if (string.IsNullOrEmpty(letraUsuario)) return;

        textoDetectado.text = letraUsuario;

        if (letraUsuario == letras[nivelActual])
        {
            CambiarEstado(EstadoNivel.Correcto);

            if (primerIntentoActual)
            {
                aciertosPrimerIntento++;

                puntuacion += 150;

                Debug.Log("Acierto al primer intento");
            }
            else
            {
                puntuacion += 100;
            }
            audioSource.PlayOneShot(correctSound);

            feedback.text = " Correcto";
            feedback.color = Color.green;
            letraUsuario = "";
            handTracking.LimpiarLetra();
            puedeEvaluar = false;
            Invoke("EsperarSiguienteNivel", 3f);
        }
        else
        {
            CambiarEstado(EstadoNivel.Incorrecto);

            erroresTotales++;

            handTracking.LimpiarLetra();

            puntuacion -= 10;

            if (puntuacion < 0)
                puntuacion = 0;

            primerIntentoActual = false;

            audioSource.PlayOneShot(errorSound);

            feedback.text = " Intenta de nuevo";
            feedback.color = Color.red;
            letraUsuario = "";
            handTracking.LimpiarLetra();
            Debug.Log("Errores: " + erroresTotales);

            puedeEvaluar = false;

            Invoke("ReactivarEvaluacion", 1f);
        }

        Debug.Log("Letra recibida: " + letraUsuario);
    }

    void MostrarNivel()
    {
        textoObjetivo.text = "Haz la vocal: " + letras[nivelActual];
        feedback.text = "";
        feedback.color = Color.white;
    }

    void EsperarSiguienteNivel()
    {
        nivelActual++;

        primerIntentoActual = true;

        if (nivelActual >= letras.Length)
        {
            CancelInvoke();

            //solution.SetActive(false);

            CambiarEstado(EstadoNivel.NivelTerminado);

            //handLandmarkerRunner.Stop();

            audioSource.PlayOneShot(victorySound);

            textoObjetivo.text = "🎉 Terminaste!";
            Debug.Log("MOSTRANDO PANEL");
            panelFinNivel.SetActive(true);

            //animatorPanelFinal.Play("PanelEntrada");
            puedeEvaluar = false;
            float tiempoFinal = Time.time - tiempoInicio;

            estrellasObtenidas = CalcularEstrellas(tiempoFinal);

            MostrarResultados(tiempoFinal);

            // Estadísticas generales
            //SaveManager.GuardarPuntuacionTotal(puntuacion);
            //SaveManager.GuardarEstrellasTotales(estrellasObtenidas);
            //SaveManager.GuardarErroresTotales(erroresTotales);
            //SaveManager.GuardarTiempoTotal(tiempoFinal);
            //SaveManager.GuardarNivelCompletado();

            Debug.Log("HandTracking asignado: " + handTracking);
            Debug.Log("Tiempo total: " + tiempoFinal + " segundos");
            Debug.Log("Errores cometidos: " + erroresTotales);
            Debug.Log("Aciertos al primer intento: " + aciertosPrimerIntento);
            Debug.Log("Puntuación final: " + puntuacion);
            Debug.Log("Estrellas obtenidas: " + estrellasObtenidas);
            SaveManager.GuardarPuntaje("Vocales", numeroNivel, puntuacion);
            SaveManager.GuardarEstrellas("Vocales", numeroNivel, estrellasObtenidas);
            SaveManager.DesbloquearNivel("Vocales", numeroNivel + 1);
            Debug.Log("Puntaje guardado: " + SaveManager.ObtenerPuntaje(numeroNivel));
            Debug.Log("Estrellas guardadas: " + SaveManager.ObtenerEstrellas(numeroNivel));
            Debug.Log("Nivel " + (numeroNivel + 1) + " desbloqueado: " + SaveManager.NivelDesbloqueado(numeroNivel + 1));
            return;
        }

        textoObjetivo.text = "Haz la vocal: " + letras[nivelActual];
        feedback.text = "Retira la manp y prepárate...";
        MostrarNivel();
        
        Invoke("ActivarEvaluacion", 2f); // tiempo para cambiar mano
    }

    void ActivarEvaluacion()
    {
        if (estadoActual == EstadoNivel.NivelTerminado)
        return;

        if (handTracking == null)
            return;

        CambiarEstado(EstadoNivel.EsperandoRespuesta);
        
        handTracking.LimpiarLetra();
        puedeEvaluar = true;
        feedback.text = "";
    }

    void ReactivarEvaluacion()
    {
        handTracking.LimpiarLetra();

        puedeEvaluar = true;

        CambiarEstado(EstadoNivel.EsperandoRespuesta);

        feedback.text = "";
    }

    void MostrarResultados(float tiempoFinal)
    {
        txtPuntuacionFinal.text =
            "Puntuación: " + puntuacion;

        txtTiempoFinal.text =
            "Tiempo: " + tiempoFinal.ToString("F1") + " s";

        txtErroresFinal.text =
            "Errores: " + erroresTotales;

        txtPrimerIntentoFinal.text =
            "Primer intento: " +
            aciertosPrimerIntento + "/" + letras.Length;

        txtEstrellasFinal.text =
            new string('★', estrellasObtenidas);
    }

    public void Reintentar()
    {
        //solution.SetActive(true);
        nivelActual = 0;

        erroresTotales = 0;
        puntuacion = 0;
        estrellasObtenidas = 0;
        aciertosPrimerIntento = 0;

        primerIntentoActual = true;

        tiempoInicio = Time.time;

        panelFinNivel.SetActive(false);

        handTracking.LimpiarLetra();

        textoDetectado.text = "";

        CambiarEstado(EstadoNivel.MostrandoAnimacion);

        MostrarNivel();

        feedback.text = "El robot está enseñando la vocal...";

        puedeEvaluar = false;

        Invoke("ActivarEvaluacion", 3f);
    }

    public void SiguienteNivelManual()
    {
        panelFinNivel.SetActive(false);
        puedeEvaluar = true;
        MostrarNivel();
    }

    public void IrMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    int CalcularEstrellas(float tiempoFinal)
    {
        if (erroresTotales == 0 &&
            aciertosPrimerIntento == letras.Length &&
            tiempoFinal <= 30f)
        {
            return 3;
        }

        if (erroresTotales <= 2 &&
            aciertosPrimerIntento >= 3)
        {
            return 2;
        }

        return 1;
    }
}

