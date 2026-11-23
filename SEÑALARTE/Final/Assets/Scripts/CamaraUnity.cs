using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamaraUnity : MonoBehaviour
{
    public RawImage rawImage;
    private WebCamTexture webcam;

    void Start()
    {
        webcam = new WebCamTexture();
        rawImage.texture = webcam;
        webcam.Play();
    }
}
