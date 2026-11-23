using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;

namespace SenalArte
{
    public class HandTrackingManagerNumeros : MonoBehaviour
    {
        public static event Action<string> OnNumberDetected;

        public static HandTrackingManagerNumeros Instance;

        private GestureDetectorNumeros detector;

        private HandLandmarkerResult ultimoResultado;
        private bool hayNuevoResultado = false;

        public string NumeroActual { get; private set; } = "";

        public void LimpiarNumero()
        {
            NumeroActual = "";
        }

        private void Awake()
        {
            Debug.Log("HandTrackingManagerNumeros Awake");

            Instance = this;
            detector = new GestureDetectorNumeros();
        }

        // MediaPipe entrega los resultados
        public void ProcessResult(HandLandmarkerResult result)
        {
            ultimoResultado = result;
            hayNuevoResultado = true;
        }

        // Unity procesa los resultados
        private void Update()
        {
            if (!hayNuevoResultado)
                return;

            hayNuevoResultado = false;

            if (ultimoResultado.handLandmarks == null)
                return;

            if (ultimoResultado.handLandmarks.Count == 0)
                return;

            var hand = ultimoResultado.handLandmarks[0];

            Vector2[] puntos = new Vector2[21];

            for (int i = 0; i < 21; i++)
            {
                var p = hand.landmarks[i];
                puntos[i] = new Vector2(p.x, p.y);
            }

            bool manoDerecha = true;

            if (ultimoResultado.handedness != null &&
                ultimoResultado.handedness.Count > 0 &&
                ultimoResultado.handedness[0].categories.Count > 0)
            {
                manoDerecha =
                    ultimoResultado.handedness[0]
                    .categories[0]
                    .categoryName == "Right";
            }

            string numero = detector.Detectar(puntos, manoDerecha);

            if (!string.IsNullOrEmpty(numero))
            {
                NumeroActual = numero;

                Debug.Log("NUMERO DETECTADO: " + numero);

                OnNumberDetected?.Invoke(numero);
            }
        }
    }
}
