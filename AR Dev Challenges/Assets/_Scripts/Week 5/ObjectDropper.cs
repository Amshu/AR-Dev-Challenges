using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDropper : MonoBehaviour
{

    public GameObject objectTemplate;

    public Vector3 startPoint;

    public int numberOfObjects = 30;

    public float initialSpeed =  1.0f;

    public float timeInterval = 0.3f;

    private float currentTime = 0.0f;

    private void Update()
    {
        if (numberOfObjects > 0)
            Spawn();
    }

    private void Spawn()
    { 
        currentTime += Time.deltaTime;
        if ((currentTime > timeInterval) && (numberOfObjects > 0))
        {
            currentTime = 0.0f;
            numberOfObjects--;
            GameObject g = Instantiate(objectTemplate);
            g.transform.position = startPoint;
            g.transform.parent = gameObject.transform;
            g.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * initialSpeed;
            g.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0.0f, 1.0f, 0.5f, 1.0f, 0.5f, 1.0f);
        }
    }
}
