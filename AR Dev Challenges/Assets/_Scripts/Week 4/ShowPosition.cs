using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPosition : MonoBehaviour
{
    public GPSTracking locationService;

    public GameObject marker;

    public float globeRadius = 10.0f;

    public float roataionSpeed = 30.0f;

    public Text posT;

    void Update()
    {
        // Rotate the globe
        transform.rotation *= Quaternion.AngleAxis(roataionSpeed * Time.deltaTime, Vector3.up);

        // Plot position
        float latitude;
        float longitude;
        float altitude;

        if(locationService.RetrieveLocation(out latitude, out longitude, out altitude))
        {
            Vector3 position = globeRadius * new Vector3(Mathf.Cos(latitude * Mathf.Deg2Rad) * Mathf.Cos(longitude * Mathf.Deg2Rad),
                                                        Mathf.Sin(latitude * Mathf.Deg2Rad), Mathf.Cos(latitude * Mathf.Deg2Rad) *
                                                        Mathf.Sin(longitude * Mathf.Deg2Rad));
            marker.transform.localPosition = position;
        }

        posT.text = globeRadius.ToString();
    }

    public void MarkerF()
    {
        globeRadius++;
    }

    public void MarkerB()
    {
        globeRadius--;
    }
}
