using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiableObject : MonoBehaviour
{
    Renderer renderer;

    Color colour;
    public Color Colour
    {
        get
        {
            return colour;
        }
        set
        {
            colour = value;
            renderer.material.color = new Color(value.r, value.g, value.b, renderer.material.color.a);
        }
    }

    // Use this for initialization
    void Start ()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.color = Colour;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void Select()
    {

    }

    void Deselect()
    {

    }
}
