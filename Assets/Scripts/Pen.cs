using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Pen : MonoBehaviour {

    SteamVR_TrackedController controller;

    PrimitiveType currentPrimitiveType = PrimitiveType.Sphere;
    GameObject preview;
    public Material previewMaterial;
    Renderer previewRenderer;
    VRButton currentButton;

    Color currentColour = Color.white;
    public Color CurrentColour
    {
        get
        {
            return currentColour;
        }
        set
        {
            currentColour = value;
            previewRenderer.material.color = CurrentColour;
        }
    }

    ModifiableObject currentObject;
    public Transform objectParentTransform;

    RaycastHit hitInfo;
    bool isPointingAtObject = false;

    public float dashes = 10;
    LineRenderer line;

	// Use this for initialization
	void Start ()
    {
        line = GetComponent<LineRenderer>();
        DisplayPreview();
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

        DoColour();
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
        else
        {
            SpawnSelectedAtController();
        }
    }

    void HandlePadClicked(object sender, ClickedEventArgs e)
    {
        //SpawnSelectedAtController();
    }

    void HandleGripped(object sender, ClickedEventArgs e)
    {

    }

    void GetPointedAt()
    {
        PrimitiveButton butt = hitInfo.transform.GetComponent<PrimitiveButton>();
        if (butt)
        {
            if (currentButton)
            {
                currentButton.Deselect();
            }
            currentButton = butt;
            currentPrimitiveType = butt.SelectType();
            DisplayPreview();
        }
        else
        {
            ModifiableObject mo = hitInfo.transform.GetComponent<ModifiableObject>();
            if(mo != null)
            {
                currentObject = mo;
            }
            else if(!hitInfo.transform.GetComponent<Image>())
            {
                SpawnSelectedAtController();
            }
        }
    }

    void SpawnSelectedAtController()
    {
        var spawnedPrimitive = GameObject.CreatePrimitive(currentPrimitiveType);
        spawnedPrimitive.transform.position = transform.position;
        spawnedPrimitive.transform.rotation = transform.rotation;
        spawnedPrimitive.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        spawnedPrimitive.transform.parent = objectParentTransform;

        ModifiableObject mo = spawnedPrimitive.AddComponent<ModifiableObject>();
        mo.Colour = CurrentColour;
    }

    public void DisplayPreview()
    {
        if (preview)
        {
            Destroy(preview);
        }
        preview = GameObject.CreatePrimitive(currentPrimitiveType);
        preview.transform.position = transform.position;
        preview.transform.rotation = transform.rotation;
        preview.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        preview.transform.parent = transform;

        previewRenderer = preview.GetComponent<Renderer>();
        previewRenderer.material = previewMaterial;
        previewRenderer.material.color = new Color(CurrentColour.r, CurrentColour.g, currentColour.b, previewMaterial.color.a);
    }

    void DoColour()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)controller.controllerIndex); //Get Controller
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) //If touched
        {
            Vector2 touchpad = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad); //Get position on touchpad
            float saturation = Mathf.Sqrt(touchpad.x * touchpad.x + touchpad.y * touchpad.y); //Dist from Centre
            float angle = Mathf.Atan2(touchpad.x, touchpad.y);
            if (angle < 0) //arctan is between -pi and pi
            {
                angle += 2 * Mathf.PI;
            }
            Color newCol = Color.HSVToRGB(angle / (2 * Mathf.PI), saturation, 1);
            CurrentColour = new Color(newCol.r, newCol.g, newCol.b, 0.5f);
        }   
    }
}
