using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVMaterial : MonoBehaviour
{

    public Material linkedMaterial;
    WebCamTexture camTexture;

    public Text outputText;
    int currentCamera = 0;

    void showCameras()
    {
        outputText.text = "";
        foreach(WebCamDevice d in WebCamTexture.devices)
        {
            outputText.text += d.name + (d.name == camTexture.deviceName ? "*" : "") + "\n";
        }
    }

    public void nextCamera()
    {
        currentCamera = (currentCamera + 1) % WebCamTexture.devices.Length;

        // Change camera only works if the camera is stopped
        camTexture.Stop();
        camTexture.deviceName = WebCamTexture.devices[currentCamera].name;
        camTexture.Play();
        showCameras();
    }

    // Start is called before the first frame update
    void Awake()
    {
        camTexture = new WebCamTexture();
        linkedMaterial.mainTexture = camTexture;
        camTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
