using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class SaveManager
{
    // ESTRELLAS
    public static void GuardarEstrellas(string modulo, int nivel, int estrellas)
    {
        string clave = $"{modulo}_Nivel{nivel}_Estrellas";
        int actuales = PlayerPrefs.GetInt(clave, 0);

        if (estrellas > actuales)
        {
            PlayerPrefs.SetInt(clave, estrellas);
        }

        PlayerPrefs.Save();
    }

    public static int ObtenerEstrellas(string modulo, int nivel)
    {
        return PlayerPrefs.GetInt(
            $"{modulo}_Nivel{nivel}_Estrellas", 0);
    }

    // PUNTAJE
    public static void GuardarPuntaje(string modulo, int nivel, int puntaje)
    {
        string clave = $"{modulo}_Nivel{nivel}_Puntaje";
        int mejor = PlayerPrefs.GetInt(clave, 0);

        if (puntaje > mejor)
        {
            PlayerPrefs.SetInt(clave, puntaje);
        }

        PlayerPrefs.Save();
    }

    public static int ObtenerPuntaje(string modulo, int nivel)
    {
        return PlayerPrefs.GetInt(
            $"{modulo}_Nivel{nivel}_Puntaje", 0);
    }

    // DESBLOQUEAR NIVELES
    public static void DesbloquearNivel(string modulo, int nivel)
    {
        PlayerPrefs.SetInt(
            $"{modulo}_Nivel{nivel}_Desbloqueado", 1);

        PlayerPrefs.Save();
    }

    public static bool NivelDesbloqueado(string modulo, int nivel)
    {
        string clave = $"{modulo}_Nivel{nivel}_Desbloqueado";

        return PlayerPrefs.GetInt(clave, nivel == 1 ? 1 : 0) == 1;
    }
}
