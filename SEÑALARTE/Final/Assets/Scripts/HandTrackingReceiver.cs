using TMPro;
using UnityEngine;

public class HandTrackingReceiver : MonoBehaviour
{
    public static HandTrackingReceiver Instance;

    public string letraRecibida = "";

    public TMP_Text textoUI;

    private void Awake()
    {
        Instance = this;
    }

    public void RecibirLetra(string letra)
    {
        if (letra == letraRecibida)
            return;

        letraRecibida = letra;

        Debug.Log("Letra: " + letraRecibida);

        if (textoUI != null)
            textoUI.text = letraRecibida;
    }
}