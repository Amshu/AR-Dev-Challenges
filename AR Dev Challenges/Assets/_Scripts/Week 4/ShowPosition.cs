using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPosition : MonoBehaviour
{
    [SerializeField]
    GPSTracking locationService;
    [SerializeField]
    GameObject marker;
    [SerializeField]
    Text post;

    [SerializeField, Space]
    Camera camera;
    [SerializeField]
    GameObject IntialCameraPosition;
    [SerializeField]
    GameObject MarkerCameraPosition;

    [SerializeField, Space]
    Text zoom_text;

    [SerializeField, Space]
    float globeRadius = 0.5f;
    [SerializeField]
    float rotationSpeed = 25.0f;

    [SerializeField, Space]
    float DurationOfLerpPos = 1.5f;
    [SerializeField]
    float DurationOfLerpRot = 3.5f;

    public Vector3 Offset;

    private bool zoomIn = true;

    void Update()
    {
        // Rotate the globe
        transform.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up);

        // Plot position
        float latitude;
        float longitude;
        float altitude;

        if(locationService.RetrieveLocation(out latitude, out longitude, out altitude))
        {
            Vector3 position = globeRadius * new Vector3(Mathf.Cos(latitude * Mathf.Deg2Rad) 
                * Mathf.Cos(longitude * Mathf.Deg2Rad), Mathf.Sin(latitude * Mathf.Deg2Rad), 
                Mathf.Cos(latitude * Mathf.Deg2Rad) * Mathf.Sin(longitude * Mathf.Deg2Rad));
            marker.transform.localPosition = position;
            marker.transform.rotation = Quaternion.LookRotation(transform.position - marker.transform.position);
        }
        
    }

    public void Zoom()
    {
        //MarkerCameraPosition.transform.localPosition = marker.transform.position + Offset;
        if (zoomIn)
        {
            zoomIn = false;
            zoom_text.text = "Zoom Out";
            StartCoroutine(LerpIn());
        }
        else
        {
            zoomIn = true;
            zoom_text.text = "Zoom In";
            StartCoroutine(LerpOut());
        }
    }

    IEnumerator LerpOut()
    {
        rotationSpeed = 25.0f;

        float LerpStartTime = Time.time;
        while (zoomIn)
        {
            float SinceLerpStart = Time.time - LerpStartTime;
            float percentageCompletePos = SinceLerpStart / (DurationOfLerpPos + 2.0f);
            float percentageCompleteRot = SinceLerpStart / (DurationOfLerpRot + 3.0f);

            yield return new WaitForEndOfFrame();

            camera.transform.position = Vector3.Lerp(camera.transform.position, 
                IntialCameraPosition.transform.position,
                percentageCompletePos);

            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation,
                IntialCameraPosition.transform.rotation,
                percentageCompleteRot);
        }
        yield return null;
    }

    IEnumerator LerpIn()
    {
        rotationSpeed = 0.0f;

        float LerpStartTime = Time.time;
        while (!zoomIn)
        {
            float SinceLerpStart = Time.time - LerpStartTime;
            float percentageCompletePos = SinceLerpStart / (DurationOfLerpPos);
            float percentageCompleteRot = SinceLerpStart / (DurationOfLerpRot);

            yield return new WaitForEndOfFrame();

            camera.transform.position = Vector3.Lerp(camera.transform.position, 
                MarkerCameraPosition.transform.position,
                percentageCompletePos * Time.deltaTime);

            camera.transform.rotation = Quaternion.Lerp(
                camera.transform.rotation,
                MarkerCameraPosition.transform.rotation, percentageCompleteRot);
        }
        yield return null;
    }
}
