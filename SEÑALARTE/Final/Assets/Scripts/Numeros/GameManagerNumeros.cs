using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SenalArte;

public class GameManagerNumeros : MonoBehaviour
{
    public int numeroNivel = 1;

    public string[] numeros = { "1", "2", "3", "4", "5" };

    private int nivelActual = 0;

    public EstadoNivel estadoActual;

    private int erroresTotales = 0;
    private float tiempoInicio;

    private bool primerIntentoActual = true;

    private int aciertosPrimerIntento = 0;
    private int puntuacion = 0;
    private int estrellasObtenidas = 0;

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

    public HandTrackingManagerNumeros handTracking;

    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip errorSound;
    public AudioClip victorySound;

    private bool puedeEvaluar = true;
    private string numeroUsuario = "";

    private void OnEnable()
    {
        HandTrackingManagerNumeros.OnNumberDetected += RecibirNumero;
    }

    private void OnDisable()
    {
        HandTrackingManagerNumeros.OnNumberDetected -= RecibirNumero;
    }

    void Start()
    {
        tiempoInicio = Time.time;

        CambiarEstado(EstadoNivel.MostrandoAnimacion);

        MostrarNivel();

        feedback.text = "...";

        puedeEvaluar = false;

        Invoke(nameof(ActivarEvaluacion), 3f);
    }

    public void CambiarEstado(EstadoNivel nuevoEstado)
    {
        estadoActual = nuevoEstado;

        Debug.Log("Estado actual: " + estadoActual);
    }

    void RecibirNumero(string numero)
    {
        numeroUsuario = numero;
    }

    void Update()
    {
        if (!puedeEvaluar)
            return;

        if (estadoActual != EstadoNivel.EsperandoRespuesta)
            return;

        if (string.IsNullOrEmpty(numeroUsuario))
            return;

        textoDetectado.text = numeroUsuario;

        if (numeroUsuario == numeros[nivelActual])
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

            feedback.text = "Correcto";
            feedback.color = Color.green;

            numeroUsuario = "";

            handTracking.LimpiarNumero();

            puedeEvaluar = false;

            Invoke(nameof(EsperarSiguienteNivel), 3f);
        }
        else
        {
            CambiarEstado(EstadoNivel.Incorrecto);

            erroresTotales++;

            puntuacion -= 10;

            if (puntuacion < 0)
                puntuacion = 0;

            primerIntentoActual = false;

            audioSource.PlayOneShot(errorSound);

            feedback.text = "Intenta de nuevo";
            feedback.color = Color.red;

            numeroUsuario = "";

            handTracking.LimpiarNumero();

            puedeEvaluar = false;

            Invoke(nameof(ReactivarEvaluacion), 1f);
        }
    }

    void MostrarNivel()
    {
        textoObjetivo.text = "Haz el número: " + numeros[nivelActual];

        feedback.text = "";
        feedback.color = Color.white;
    }

    void EsperarSiguienteNivel()
    {
        nivelActual++;

        primerIntentoActual = true;

        if (nivelActual >= numeros.Length)
        {
            CancelInvoke();

            CambiarEstado(EstadoNivel.NivelTerminado);

            audioSource.PlayOneShot(victorySound);

            textoObjetivo.text = "¡Terminaste!";

            panelFinNivel.SetActive(true);

            puedeEvaluar = false;

            float tiempoFinal = Time.time - tiempoInicio;

            estrellasObtenidas = CalcularEstrellas(tiempoFinal);

            MostrarResultados(tiempoFinal);

            Debug.Log("Tiempo total: " + tiempoFinal);
            Debug.Log("Errores: " + erroresTotales);
            Debug.Log("Aciertos al primer intento: " + aciertosPrimerIntento);
            Debug.Log("Puntuación: " + puntuacion);
            Debug.Log("Estrellas: " + estrellasObtenidas);

            // Guardado de resultados.
            SaveManager.GuardarPuntaje("Numeros", numeroNivel, puntuacion);
            SaveManager.GuardarEstrellas("Numeros", numeroNivel, estrellasObtenidas);
            SaveManager.DesbloquearNivel("Numeros", numeroNivel + 1);

            return;
        }

        MostrarNivel();

        feedback.text = "Retira la mano y prepárate...";

        Invoke(nameof(ActivarEvaluacion), 2f);
    }

    void ActivarEvaluacion()
    {
        if (estadoActual == EstadoNivel.NivelTerminado)
            return;

        if (handTracking == null)
        {
            Debug.LogError("HandTrackingManagerNumeros no está asignado.");
            return;
        }

        CambiarEstado(EstadoNivel.EsperandoRespuesta);

        handTracking.LimpiarNumero();

        numeroUsuario = "";

        puedeEvaluar = true;

        feedback.text = "";
    }

    void ReactivarEvaluacion()
    {
        handTracking.LimpiarNumero();

        numeroUsuario = "";

        puedeEvaluar = true;

        CambiarEstado(EstadoNivel.EsperandoRespuesta);

        feedback.text = "";
    }

    void MostrarResultados(float tiempoFinal)
    {
        txtPuntuacionFinal.text = "Puntuación: " + puntuacion;

        txtTiempoFinal.text =
            "Tiempo: " + tiempoFinal.ToString("F1") + " s";

        txtErroresFinal.text = "Errores: " + erroresTotales;

        txtPrimerIntentoFinal.text =
            "Primer intento: " +
            aciertosPrimerIntento + "/" + numeros.Length;

        txtEstrellasFinal.text =
            new string('★', estrellasObtenidas);
    }

    public void Reintentar()
    {
        CancelInvoke();

        nivelActual = 0;

        erroresTotales = 0;
        puntuacion = 0;
        estrellasObtenidas = 0;
        aciertosPrimerIntento = 0;

        primerIntentoActual = true;

        tiempoInicio = Time.time;

        panelFinNivel.SetActive(false);

        handTracking.LimpiarNumero();

        numeroUsuario = "";

        textoDetectado.text = "";

        CambiarEstado(EstadoNivel.MostrandoAnimacion);

        MostrarNivel();

        feedback.text = "El robot está enseñando el número...";

        puedeEvaluar = false;

        Invoke(nameof(ActivarEvaluacion), 3f);
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
            aciertosPrimerIntento == numeros.Length &&
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
