using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pen : MonoBehaviour {

    SteamVR_TrackedController controller;
    PrimitiveType currentPrimitiveType = PrimitiveType.Sphere;

    RaycastHit hitInfo;
    bool isPointingAtObject = false;

    public float dashes = 10;
    LineRenderer line;

	// Use this for initialization
	void Start ()
    {
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo))
        {
            line.SetPositions(new Vector3[] { transform.position, hitInfo.point });
            line.material.mainTextureScale = new Vector2(Vector3.Distance(transform.position, hitInfo.point) * dashes, 1);
            isPointingAtObject = true;
        }
        else
        {
            line.SetPositions(new Vector3[] { transform.position, transform.forward * 100});
            line.material.mainTextureScale = new Vector2(100 * dashes, 1);
            isPointingAtObject = false;
        }
	}

    void OnEnable()
    {
        controller = GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += HandleTriggerClicked;
        controller.PadClicked += HandlePadClicked;
        controller.Gripped += HandleGripped;
    }

    void OnDisable()
    {
        controller.TriggerClicked -= HandleTriggerClicked;
        controller.PadClicked -= HandlePadClicked;
        controller.Gripped -= HandleGripped;
    }

    void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (isPointingAtObject)
        {
            GetPointedAt();
        }
    }

    void HandlePadClicked(object sender, ClickedEventArgs e)
    {
        SpawnSelectedAtController();
    }

    void HandleGripped(object sender, ClickedEventArgs e)
    {

    }

    void GetPointedAt()
    {
        Button butt = hitInfo.transform.GetComponent<Button>();
        if (butt != null)
        {
            butt.onClick.Invoke();
        }
    }

    void SpawnSelectedAtController()
    {
        var spawnedPrimitive = GameObject.CreatePrimitive(currentPrimitiveType);
        spawnedPrimitive.transform.position = transform.position;
        spawnedPrimitive.transform.rotation = transform.rotation;

        spawnedPrimitive.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void SelectCube()
    {
        currentPrimitiveType = PrimitiveType.Cube;
    }

    public void SelectSphere()
    {
        currentPrimitiveType = PrimitiveType.Sphere;
    }

    public void SelectCylinder()
    {
        currentPrimitiveType = PrimitiveType.Cylinder;
    }

    public void SelectCapsule()
    {
        currentPrimitiveType = PrimitiveType.Capsule;
    }
}
