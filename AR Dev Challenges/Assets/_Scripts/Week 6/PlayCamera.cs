using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCamera : MonoBehaviour
{
    public Material camTexMaterial;
    private WebCamTexture webCamTexture;

    void Update()
    {
        if(webCamTexture == null)
        {
            webCamTexture = new WebCamTexture();
            camTexMaterial.mainTexture = webCamTexture;
        }
        if (!webCamTexture.isPlaying)
        {
            webCamTexture.Play();
        }
    }
}
