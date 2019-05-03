using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSTracking : MonoBehaviour
{ 
    /*
     *Retrieve the location from the location service, typically using the GPS
     *  Returns true if operation succeeds
     *  False is location not available
     */

    public bool RetrieveLocation(out float latitude, out float longitude, out float altitude)
    {
        latitude = 0.0f;
        longitude = 0.0f;
        altitude = 0.0f;

        if(!Input.location.isEnabledByUser)
        {
            Debug.Log("Location service needs to be enabled");
            return false;
        }
        if(Input.location.status != LocationServiceStatus.Running)
        {
            Debug.Log("Starting location service");
            if(Input.location.status == LocationServiceStatus.Stopped)
            {
                Input.location.Start();
            }
            return false;
        }
        else
        {
            // Valid data is available
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            altitude = Input.location.lastData.altitude;
            return true;
        }
    }
}
