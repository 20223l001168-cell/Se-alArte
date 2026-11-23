using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using System.IO;

public class VideoReceiver : MonoBehaviour
{
    public RawImage rawImage;

    TcpClient client;
    NetworkStream stream;
    Texture2D texture;

    void Start()
    {
        client = new TcpClient("127.0.0.1", 12345);
        stream = client.GetStream();

        texture = new Texture2D(2, 2);
    }

    void Update()
    {
        if (stream.DataAvailable)
        {
            // Leer tamaño
            byte[] sizeBytes = new byte[4];
            stream.Read(sizeBytes, 0, 4);
            int size = System.BitConverter.ToInt32(sizeBytes, 0);

            // Leer imagen
            byte[] imageBytes = new byte[size];
            int totalRead = 0;

            while (totalRead < size)
            {
                int read = stream.Read(imageBytes, totalRead, size - totalRead);
                totalRead += read;
            }

            // Convertir a textura
            texture.LoadImage(imageBytes);
            rawImage.texture = texture;
        }
    }
}
