using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVMaterial : MonoBehaviour
{

    public Material linkedMaterial;
    private WebCamTexture camTexture;



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
