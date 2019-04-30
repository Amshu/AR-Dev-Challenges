using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.IO;
using System;

public class AccessOpenCV : MonoBehaviour
{
    public Material cameraMaterial;

    public GameObject markerTemplate;
    public GameObject markerParent;

    private bool modelReady = false;
    private float delaytime = 0.0f;

    public Text text;

    private string[] CLASSES = { "background", "aeroplane", "bicycle", "bird", "boat", "bottle",
        "bus", "car", "chair", "cow", "diningtable", "dog", "horse", "motorbike", "person", "pottedplant",
        "sheep", "sofa", "train", "tvmonitor"};

    //-----Expose the required function from the OpenCV library

    // The interface with the Unity software requires that the functions exposed use internal naming 
    // formats consistent with the C language. This is achienved by wrapping them in: extern "C" 2 {3}
    [DllImport("VisualRecognition")]
    private static extern void prepareModel(string dirname);

    [DllImport("VisualRecognition")]
    private static extern int doRecognise(byte[] imageData, int width, int height);

    [DllImport("VisualRecognition")]
    private static extern void retriveMatch(int i, ref int category, ref float condidence, ref float sx, 
        ref float sy, ref float ex, ref float ey);

    void Start()
    {
        StartCoroutine(prepareModel());
    }

    // Creates the OpenCV data structure that contains the data for identifying objects 
    IEnumerator prepareModel()
    {
        yield return StartCoroutine(extractFile("", "MobileNetSSD_deploy.caffemodel"));
        yield return StartCoroutine(extractFile("", "MobileNetSSD_deploy.protext.txt"));

        prepareModel(Application.persistentDataPath);

        modelReady = true;
    }

    private void clearVisuals()
    {
        foreach(Transform child in markerParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void addVisual(string name, float confidence, float sx, float sy, float ex, float ey)
    {
        GameObject g = GameObject.Instantiate(markerTemplate);
        g.transform.position = new Vector3(5.0f * (sx + ex) - 5.0f, -5.0f * (sy + ey) + 5.0f, 0);
        g.transform.localScale = new Vector3(10.0f * Mathf.Abs(sx - ex), 10.0f * Mathf.Abs(sx - ex), 1);
        g.GetComponentInChildren<TextMesh>().text = name + "/n"+ confidence;
        g.transform.SetParent(markerParent.transform, false);
    }

    private void Update()
    {
        delaytime += Time.deltaTime;

        if(modelReady && (delaytime > 2.0f))
        {
            // After every 2 seconds
            clearVisuals();
            delaytime = 0.0f;
            
            //-----Image is converted to a texture inorder for the OpenCV library to access it

            // Create an image with the width and height from the camera texture
            Texture2D image = new Texture2D(cameraMaterial.mainTexture.width, cameraMaterial.mainTexture.height,
                TextureFormat.RGBA32, false);
            // Create a renderdering destination with the same dimensions and depth = 32
            RenderTexture renderTexture = new RenderTexture(cameraMaterial.mainTexture.width,
                cameraMaterial.mainTexture.height, 32);
            // Put the source into the destination - Image to the Render
            Graphics.Blit(cameraMaterial.mainTexture, renderTexture);
            // Enable the destination
            RenderTexture.active = renderTexture;
            // Read from the image captured by the camera
            image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            image.Apply();

            //-----Scan by iterating over each byte of the image

            // The concept of a pointer to this array is shared among all the languages involved and so is suitable 
            // for transfering the image to the external library.
            int numMatch = doRecognise(image.GetRawTextureData(), image.width, image.height);

            text.text = "Matches: " + numMatch;

            for(int i = 0; i < numMatch; i++)
            {
                int category = -1;
                float confidence = 0.0f;
                // Opposite corners of the 2D Box - sx, sy, ex, ey
                float sx = 0, sy = 0, ex = 0, ey = 0;
                retriveMatch(i, ref category, ref confidence, ref sx, ref sy, ref ex, ref ey);
                if(confidence > 0.2f)
                {
                    Debug.Log("Match: " + category + " " + confidence + " " + sx + " " + sy + " "
                        + ex + " " + ey);
                    addVisual(CLASSES[category], confidence, sx, sy, ex, ey);
                }
            }
        }
    }

    // Copy file from the android package to a readable/writable region of the host file system
    IEnumerator extractFile(string assetPath, string assetFile)
    {
        // Source is the streaming assets path.
        string sourcePath = Application.streamingAssetsPath + "/" + assetPath + assetFile;
        if((sourcePath.Length > 0) && (sourcePath[0] == '/'))
        {
            sourcePath = "file://" + sourcePath;
        }
        string destinationpath = Application.persistentDataPath + "/" + assetFile;

        // Files must be handled via a WWW to extract
        WWW w = new WWW(sourcePath);
        yield return w;
        try
        {
            File.WriteAllBytes(destinationpath, w.bytes);
        }
        catch(Exception e)
        {
            Debug.Log("Issue writing " + destinationpath + " " + e);
        }
        Debug.Log(sourcePath + " -> " + destinationpath + " " + w.bytes.Length);
    }
}
