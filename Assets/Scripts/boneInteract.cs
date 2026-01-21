using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class boneInteract : MonoBehaviour
{
    XRGrabInteractable grabInteractable;

    Renderer renderer;
    public GameObject nameDisplayLeft;
    public GameObject nameDisplayRight;
    public GameObject reattachButton;
    public GameObject factPanelRight;
    public GameObject cullSphere;

    bool selected = false;
    bool rightHand = false;
    bool leftHand = false;
    bool floating = false;
    bool grabbed = false;

    float glow = 0.0f;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        renderer = GetComponent<Renderer>();

    }

    void OnEnable()
    {
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        grabInteractable.hoverExited.AddListener(OnHoverExited);

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
        grabInteractable.hoverExited.RemoveListener(OnHoverExited);

        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
    // Update is called once per frame
    void Update()
    {
        float lDist = 0;
        if ( transform.parent != null)
        {
            lDist = Vector3.Distance(transform.position, transform.parent.position);
        }

        if (reattachButton.GetComponent<buttonsManager>().reattachButtonPressed) 
        {
            transform.position = Vector3.Lerp(transform.position, transform.parent.position, Time.deltaTime / lDist);

            transform.rotation = Quaternion.Lerp(transform.rotation, transform.parent.rotation, Time.deltaTime / lDist);

            if(lDist < 0.1f)
            {
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }

        if (lDist > 0.1f)
        {
            floating = true;
            GetComponent<Rigidbody>().includeLayers = -1; //Include all layers
            GetComponent<Rigidbody>().excludeLayers = 0; //Exclude no layers
        }
        else
        {
            floating = false;
            GetComponent<Rigidbody>().includeLayers = 8; //Include only bone colliders
            GetComponent<Rigidbody>().excludeLayers = -1; //Exclude all layers
        }

        if (lDist > 0.001f)
        {
            floating = true;
        }
        else
        {
            floating = false;
        }

        if (selected)
        {
            glow += Time.deltaTime;
            if (rightHand)
            {
                nameDisplayRight.GetComponent<TextMeshProUGUI>().SetText(this.name.Replace('_', ' '));
            }
            if (leftHand)
            {
                nameDisplayLeft.GetComponent<TextMeshProUGUI>().SetText(this.name.Replace('_', ' '));
            }
        }
        else
        {
            glow -= Time.deltaTime;
        }

        glow = Mathf.Clamp01(glow);

        renderer.material.SetColor("_selected", new Color(glow,0,0));


        //Culling

        if (cullSphere.activeSelf && !floating && !selected && !grabbed && (cullSphere.transform.position - transform.position).magnitude > cullSphere.transform.localScale.x / 2.0f )
        //if (dotProduct < 0 && distancFromPlane > 0.1f) // Object is behind center plane
        {
            renderer.enabled = false;
            grabInteractable.enabled = false;
            GetComponent<MeshCollider>().enabled = false;
        }
        else
        {
            renderer.enabled = true;
            grabInteractable.enabled = true;
            GetComponent<MeshCollider>().enabled = true;
        }
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        selected = true;

        //Get the hand that is interacting

        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;

        if ((interactor != null))
        {
            if (interactor.transform.parent.gameObject.name.Contains("Left"))
            {
                leftHand = true;
            }
            else if (interactor.transform.parent.gameObject.name.Contains("Right"))
            {
                rightHand = true;
            }
        }

    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        selected = false;
        rightHand = false;
        leftHand = false;
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        selected = false;
        grabbed = true;
        //Debug.Log(AnatomyFactManager.Instance.GetAnatomyFact(this.name));
        factPanelRight.GetComponent<TextMeshProUGUI>().SetText(this.name.Replace('_', ' ') + "\n\n" + AnatomyFactManager.Instance.GetAnatomyFact(this.name));

    }

    void OnRelease(SelectExitEventArgs args)
    {
        selected = false;
        grabbed = false;
    }
}
