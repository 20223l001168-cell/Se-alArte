using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using System;

namespace SenalArte
{
    public class HandTrackingManager : MonoBehaviour
    {
        public static event Action<string> OnLetterDetected;

        public static HandTrackingManager Instance;

        private GestureDetector detector;

        private HandLandmarkerResult ultimoResultado;
        private bool hayNuevoResultado = false;

        public string LetraActual { get; private set; } = "";

        public void LimpiarLetra()
        {
            LetraActual = "";
        }

        private void Awake()
        {
            Debug.Log("HandTrackingManager Awake");
            Instance = this;
            detector = new GestureDetector();
        }

        // MediaPipe llama este método desde otro hilo
        public void ProcessResult(HandLandmarkerResult result)
        {
            Debug.Log("ProcessResult llamado");
            ultimoResultado = result;
            hayNuevoResultado = true;
        }

        // Unity llama Update() en el hilo principal
        private void Update()
        {
            Debug.Log("Update HandTrackingManager");
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

            string letra = detector.Detectar(puntos, manoDerecha);

            if (!string.IsNullOrEmpty(letra))
            {
                LetraActual = letra;

                Debug.Log("LETRA DETECTADA: " + letra);

                OnLetterDetected?.Invoke(letra);

                GestureEvents.Instance?.OnLetterDetected(letra);
            }

            if (HandTrackingReceiver.Instance != null)
            {
                HandTrackingReceiver.Instance.RecibirLetra(letra);
            }
        }
    }
}