using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRButton : MonoBehaviour {

    Image image;
    public Color normalColour = Color.white;
    public Color highlightedColor = Color.red;

	// Use this for initialization
	public void Start ()
    {
        image = GetComponent<Image>();
	}

    public void Select()
    {
        image.color = highlightedColor;
    }

    public void Deselect()
    {
        image.color = normalColour;
    }
}
