using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SenalArte
{
    public class GestureEvents : MonoBehaviour
    {
        public static GestureEvents Instance;

        public event Action<string> OnGestureDetected;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public void OnLetterDetected(string letra)
        {
            Debug.Log("Evento recibido: " + letra);

            OnGestureDetected?.Invoke(letra);
        }
    }
}
