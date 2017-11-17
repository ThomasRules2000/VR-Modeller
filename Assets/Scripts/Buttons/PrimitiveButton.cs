using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveButton : VRButton
{
    public PrimitiveType type;

	// Use this for initialization
	new void Start ()
    {
        base.Start();
	}

    public PrimitiveType SelectType()
    {
        base.Select();
        return type;
    }
}
