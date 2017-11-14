using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

    public float rotationSpeed = 0.01f;
    public float translationSpeed = 0.01f;
    public float movementDist = 5f;
    bool up = true;


    float startPosZ = 0;
    float minZ = -1;
    float maxZ = 1;

	// Use this for initialization
	void Start ()
    {
        startPosZ = transform.localPosition.z;
        minZ = startPosZ - movementDist;
        maxZ = startPosZ + movementDist;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Random.Range(minZ, maxZ));
	}
	
	// Update is called once per frame
	void Update ()
    {
        float rotateAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(Random.onUnitSphere*rotationSpeed);

        float translateAmount = translationSpeed * Time.deltaTime;
        if (up)
        {
            transform.Translate(Vector3.forward * translateAmount, transform.parent);
            if(transform.localPosition.z > maxZ)
            {
                up = false;
            }
        }
        else
        {
            transform.Translate(Vector3.back * translateAmount, transform.parent);
            if (transform.localPosition.z < minZ)
            {
                up = true;
            }
        }
	}
}
