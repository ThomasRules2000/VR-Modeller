using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pen : MonoBehaviour {

    SteamVR_TrackedController controller;

    PrimitiveType currentPrimitiveType = PrimitiveType.Sphere;
    GameObject preview;
    public Material previewMaterial;

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
        if (butt != null)
        {
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

        spawnedPrimitive.AddComponent<ModifiableObject>();
        spawnedPrimitive.transform.parent = objectParentTransform;
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

        Renderer renderer = preview.GetComponent<Renderer>();
        renderer.material = previewMaterial;
    }
}
