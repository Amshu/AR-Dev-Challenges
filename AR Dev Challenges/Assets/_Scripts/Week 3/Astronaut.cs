using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");
        if (other.gameObject.transform.tag == "Oxygen")
        {
            Debug.Log("Animation Changed");
            gameObject.GetComponent<Animator>().SetBool("Check", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("TriggerExit");
        if (other.gameObject.transform.tag == "Oxygen")
        {
            Debug.Log("Animation Restored");
            gameObject.GetComponent<Animator>().SetBool("Check", false);
        }
    }
}
