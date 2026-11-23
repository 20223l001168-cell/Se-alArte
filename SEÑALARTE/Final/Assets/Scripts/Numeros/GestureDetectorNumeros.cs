using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SenalArte
{
    public class GestureDetectorNumeros
    {
        //=============================
        // TEMPORIZADORES
        //=============================

        private float tiempo1;
        private float tiempo2;
        private float tiempo3;
        private float tiempo4;
        private float tiempo5;

        private const float TIEMPO_DETECCION = 0.15f;

        private string ultimoNumero = "";

        //=============================
        // METODO PRINCIPAL
        //=============================

        public string Detectar(Vector2[] p, bool manoDerecha)
        {
            if (p == null || p.Length < 21)
                return "";

            bool n1 = Detectar1(p);
            bool n2 = Detectar2(p);
            bool n3 = Detectar3(p, manoDerecha);
            bool n4 = Detectar4(p);
            bool n5 = Detectar5(p, manoDerecha);

            Debug.Log(
                "1:" + n1 +
                " 2:" + n2 +
                " 3:" + n3 +
                " 4:" + n4 +
                " 5:" + n5
            );

            ActualizarTiempo(ref tiempo1, n1);
            ActualizarTiempo(ref tiempo2, n2);
            ActualizarTiempo(ref tiempo3, n3);
            ActualizarTiempo(ref tiempo4, n4);
            ActualizarTiempo(ref tiempo5, n5);

            string numero = "";

            if (tiempo1 >= TIEMPO_DETECCION)
                numero = "1";

            else if (tiempo2 >= TIEMPO_DETECCION)
                numero = "2";

            else if (tiempo3 >= TIEMPO_DETECCION)
                numero = "3";

            else if (tiempo4 >= TIEMPO_DETECCION)
                numero = "4";

            else if (tiempo5 >= TIEMPO_DETECCION)
                numero = "5";

            if (numero == "")
            {
                ultimoNumero = "";
                return "";
            }

            if (numero != ultimoNumero)
            {
                ultimoNumero = numero;
                return numero;
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

        private bool PulgarExtendido(Vector2[] p, bool manoDerecha)
        {
            float distanciaPulgar = Distancia(p[4], p[2]);
            float distanciaBase = Distancia(p[2], p[5]);

            return distanciaPulgar > distanciaBase * 0.8f;
        }

        //=============================
        // DETECTAR NUMERO 1
        // Indice extendido
        //=============================

        private bool Detectar1(Vector2[] p)
        {
            bool indexExt = DedoExtendido(p[8], p[6]);

            bool middleFold = DedoDoblado(p[12], p[10]);
            bool ringFold = DedoDoblado(p[16], p[14]);
            bool pinkyFold = DedoDoblado(p[20], p[18]);

            bool thumbFold = Distancia(p[4], p[5]) <
                            Distancia(p[0], p[12]) * 0.35f;

            return indexExt &&
                   middleFold &&
                   ringFold &&
                   pinkyFold &&
                   thumbFold;
        }

        //=============================
        // DETECTAR NUMERO 2
        // Indice y medio extendidos
        //=============================

        private bool Detectar2(Vector2[] p)
        {
            bool indexExt = DedoExtendido(p[8], p[6]);
            bool middleExt = DedoExtendido(p[12], p[10]);

            bool ringFold = DedoDoblado(p[16], p[14]);
            bool pinkyFold = DedoDoblado(p[20], p[18]);

            bool thumbFold = Distancia(p[4], p[5]) <
                            Distancia(p[0], p[12]) * 0.35f;

            float tamanoMano = Distancia(p[0], p[12]);

            float separacion =
                Distancia(p[8], p[12]);

            return indexExt &&
                   middleExt &&
                   ringFold &&
                   pinkyFold &&
                   thumbFold &&
                   separacion > tamanoMano * 0.12f;
        }

        //=============================
        // DETECTAR NUMERO 3
        // Pulgar, indice y medio
        //=============================

        private bool Detectar3(Vector2[] p, bool manoDerecha)
        {
            bool indexExt = DedoExtendido(p[8], p[6]);
            bool middleExt = DedoExtendido(p[12], p[10]);

            bool ringFold = DedoDoblado(p[16], p[14]);
            bool pinkyFold = DedoDoblado(p[20], p[18]);

            bool thumbExt = PulgarExtendido(p, manoDerecha);

            return indexExt &&
                middleExt &&
                ringFold &&
                pinkyFold &&
                thumbExt;
        }

        //=============================
        // DETECTAR NUMERO 4
        // Cuatro dedos extendidos
        //=============================

        private bool Detectar4(Vector2[] p)
        {
            bool indexExt = DedoExtendido(p[8], p[6]);
            bool middleExt = DedoExtendido(p[12], p[10]);
            bool ringExt = DedoExtendido(p[16], p[14]);
            bool pinkyExt = DedoExtendido(p[20], p[18]);

            bool thumbFold = Distancia(p[4], p[5]) <
                            Distancia(p[0], p[12]) * 0.35f;

            return indexExt &&
                   middleExt &&
                   ringExt &&
                   pinkyExt &&
                   thumbFold;
        }

        //=============================
        // DETECTAR NUMERO 5
        // Mano abierta
        //=============================

        private bool Detectar5(Vector2[] p, bool manoDerecha)
        {
            bool indexExt = DedoExtendido(p[8], p[6]);
            bool middleExt = DedoExtendido(p[12], p[10]);
            bool ringExt = DedoExtendido(p[16], p[14]);
            bool pinkyExt = DedoExtendido(p[20], p[18]);

            bool thumbExt = PulgarExtendido(p, manoDerecha);

            return indexExt &&
                middleExt &&
                ringExt &&
                pinkyExt &&
                thumbExt;
        }
    }
}