using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerinformationSource : MonoBehaviour
{
    [SerializeField]
    Text DebugText;

    private Gyroscope gyro;
    private bool gyroSupported;

    private DatagramCommunication dc;
    private Quaternion gyroAttitude;
    private bool centerPressed;
    private bool triggerPressed;

    void Start()
    {
        dc = new DatagramCommunication();
        DebugText.text = "Set up DatagramCommunication";
        Debug.Log("Set up DatagramCommunication");

        gyroSupported = SystemInfo.supportsGyroscope;
        if (gyroSupported)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            DebugText.text = "Gyro is enabled";
            Debug.Log("Gyro is enabled");
        }
    }

    public void centerClicked() { centerPressed = true; }
    public void triggerClicked() { triggerPressed = true; }
    public void centerRelease() { centerPressed = false; }
    public void triggerRelease() { triggerPressed = false; }
    
    void Update()
    {
        if (gyroSupported)
        {
            transform.rotation = Quaternion.Euler(90, 0, 90) * gyro.attitude * Quaternion.Euler(180, 180, 0);

            gyroAttitude = gyro.attitude;
            DebugText.text = "Sending information";
            Debug.Log("Sending information");
        }
        dc.sendControllerDetails(new ControllerDetails(gyroAttitude, centerPressed, triggerPressed));
    }
}
