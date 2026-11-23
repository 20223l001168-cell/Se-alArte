using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SenalArte
{
    public class GestureDetector
    {
        //=============================
        // VARIABLES
        //=============================

        private float tiempoA;
        private float tiempoE;
        private float tiempoI;
        private float tiempoO;
        private float tiempoU;

        private const float TIEMPO_DETECCION = 0.15f;

        private string ultimaLetra = "";

        //=============================
        // MÉTODO PRINCIPAL
        //=============================

        public string Detectar(Vector2[] p, bool manoDerecha)
        {
            if (p == null || p.Length < 21)
                return "";

            bool a = DetectarA(p, manoDerecha);
            bool e = DetectarE(p);
            bool i = DetectarI(p);
            bool o = DetectarO(p);
            bool u = DetectarU(p);
            Debug.Log(
            "A:"+a+
            " E:"+e+
            " I:"+i+
            " O:"+o+
            " U:"+u
            );

            ActualizarTiempo(ref tiempoA, a);
            ActualizarTiempo(ref tiempoE, e);
            ActualizarTiempo(ref tiempoI, i);
            ActualizarTiempo(ref tiempoO, o);
            ActualizarTiempo(ref tiempoU, u);

            string letra = "";

            if (tiempoA >= TIEMPO_DETECCION)
                letra = "A";

            else if (tiempoE >= TIEMPO_DETECCION)
                letra = "E";

            else if (tiempoI >= TIEMPO_DETECCION)
                letra = "I";

            else if (tiempoO >= TIEMPO_DETECCION)
                letra = "O";

            else if (tiempoU >= TIEMPO_DETECCION)
                letra = "U";

            if (letra == "")
            {
                ultimaLetra = "";
                return "";
            }

            if (letra != ultimaLetra)
            {
                ultimaLetra = letra;
                return letra;
            }

            return "";
        }

        //=============================
        // TEMPORIZADOR
        //=============================

        private void ActualizarTiempo(ref float tiempo, bool activa)
        {
            if (activa)
                tiempo += Time.deltaTime;
            else
                tiempo = 0f;
        }

        //=============================
        // UTILIDADES
        //=============================

        private float Distancia(Vector2 a, Vector2 b)
        {
            return Vector2.Distance(a, b);
        }

        private bool DedoExtendido(Vector2 tip, Vector2 pip)
        {
            return tip.y < pip.y;
        }

        private bool DedoDoblado(Vector2 tip, Vector2 pip)
        {
            return tip.y > pip.y;
        }

        //=============================
        // LETRA A
        //=============================

private bool DetectarA(Vector2[] p, bool manoDerecha)
{
    Vector2 wrist = p[0];

    Vector2 thumbTip = p[4];
    Vector2 thumbMcp = p[2];

    Vector2 indexTip = p[8];
    Vector2 indexPip = p[6];

    Vector2 middleTip = p[12];
    Vector2 middlePip = p[10];

    Vector2 ringTip = p[16];
    Vector2 ringPip = p[14];

    Vector2 pinkyTip = p[20];
    Vector2 pinkyPip = p[18];

    bool indexFold = DedoDoblado(indexTip, indexPip);
    bool middleFold = DedoDoblado(middleTip, middlePip);
    bool ringFold = DedoDoblado(ringTip, ringPip);
    bool pinkyFold = DedoDoblado(pinkyTip, pinkyPip);

    float tamanoMano = Distancia(wrist, middleTip);

    float distPulgarIndice = Distancia(thumbTip, indexTip);

    bool pulgarAbierto;

    if (manoDerecha)
        pulgarAbierto = thumbTip.x > thumbMcp.x;
    else
        pulgarAbierto = thumbTip.x < thumbMcp.x;

    return indexFold &&
           middleFold &&
           ringFold &&
           pinkyFold &&
           pulgarAbierto &&
           distPulgarIndice > tamanoMano * 0.18f;
}

        //=============================
        // LETRA E
        //=============================

private bool DetectarE(Vector2[] p)
{
    Vector2 wrist = p[0];

    Vector2 thumbTip = p[4];

    Vector2 indexTip = p[8];
    Vector2 indexPip = p[6];

    Vector2 middleTip = p[12];
    Vector2 middlePip = p[10];

    Vector2 ringTip = p[16];
    Vector2 ringPip = p[14];

    Vector2 pinkyTip = p[20];
    Vector2 pinkyPip = p[18];

    bool indexFold = DedoDoblado(indexTip, indexPip);
    bool middleFold = DedoDoblado(middleTip, middlePip);
    bool ringFold = DedoDoblado(ringTip, ringPip);
    bool pinkyFold = DedoDoblado(pinkyTip, pinkyPip);
    float distPulgarMedio = Distancia(thumbTip, middleTip);
    float distPulgarAnular = Distancia(thumbTip, ringTip);

    float tamanoMano = Distancia(wrist, middleTip);

    float distPulgarIndice = Distancia(thumbTip, indexTip);

    return indexFold &&
           middleFold &&
           ringFold &&
           pinkyFold &&
           distPulgarIndice < tamanoMano * 0.30f &&
            distPulgarMedio > tamanoMano * 0.18f &&
            distPulgarAnular > tamanoMano * 0.20f;
}

        //=============================
        // LETRA I
        //=============================

        private bool DetectarI(Vector2[] p)
        {
            Vector2 wrist = p[0];

            Vector2 indexTip = p[8];
            Vector2 indexPip = p[6];

            Vector2 middleTip = p[12];
            Vector2 middlePip = p[10];

            Vector2 ringTip = p[16];
            Vector2 ringPip = p[14];

            Vector2 pinkyTip = p[20];
            Vector2 pinkyPip = p[18];

            bool indexFold = DedoDoblado(indexTip, indexPip);
            bool middleFold = DedoDoblado(middleTip, middlePip);
            bool ringFold = DedoDoblado(ringTip, ringPip);

            bool pinkyExt = DedoExtendido(pinkyTip, pinkyPip);

            float distPinkyRing = Distancia(p[20], p[16]);
            float tamanoMano = Distancia(wrist, middleTip);
            bool thumbFold = DedoDoblado(p[4], p[3]);

            return pinkyExt &&
                    indexFold &&
                    middleFold &&
                    ringFold &&
                    distPinkyRing > tamanoMano * 0.35f;
        }

        //=============================
        // LETRA O
        //=============================

private bool DetectarO(Vector2[] p)
{
    Vector2 wrist = p[0];

    Vector2 thumbTip = p[4];

    Vector2 indexTip = p[8];
    Vector2 indexPip = p[6];

    Vector2 middleTip = p[12];
    Vector2 middlePip = p[10];

    Vector2 ringTip = p[16];
    Vector2 ringPip = p[14];

    Vector2 pinkyTip = p[20];
    Vector2 pinkyPip = p[18];

    float tamanoMano = Distancia(wrist, middleTip);

    float distIndicePulgar = Distancia(indexTip, thumbTip);
    float distMedioPulgar = Distancia(middleTip, thumbTip);
    float distAnularPulgar = Distancia(ringTip, thumbTip);

    bool indexExt = DedoExtendido(indexTip, indexPip);
    bool middleExt = DedoExtendido(middleTip, middlePip);
    bool ringExt = DedoExtendido(ringTip, ringPip);
    bool pinkyExt = DedoExtendido(pinkyTip, pinkyPip);

    return
        distIndicePulgar < tamanoMano * 0.28f &&
        distMedioPulgar < tamanoMano * 0.40f &&
        distAnularPulgar < tamanoMano * 0.50f;
}
        //=============================
        // LETRA U
        //=============================

        private bool DetectarU(Vector2[] p)
        {
            Vector2 wrist = p[0];

            Vector2 indexTip = p[8];
            Vector2 indexPip = p[6];

            Vector2 middleTip = p[12];
            Vector2 middlePip = p[10];

            Vector2 ringTip = p[16];
            Vector2 ringPip = p[14];

            Vector2 pinkyTip = p[20];
            Vector2 pinkyPip = p[18];

            bool indexExt = DedoExtendido(indexTip, indexPip);
            bool middleExt = DedoExtendido(middleTip, middlePip);

            bool ringFold = DedoDoblado(ringTip, ringPip);
            bool pinkyFold = DedoDoblado(pinkyTip, pinkyPip);

            float tamanoMano = Distancia(wrist, middleTip);

            return indexExt &&
                   middleExt &&
                   ringFold &&
                   pinkyFold &&
                   Mathf.Abs(indexTip.x - middleTip.x) < tamanoMano * 0.45f &&
                   Mathf.Abs(indexTip.y - middleTip.y) < tamanoMano * 0.30f;
        }
    }
}