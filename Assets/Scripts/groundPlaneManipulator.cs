using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class groundPlaneManipulator : MonoBehaviour
{
    // Layer mask to specify what is considered "ground".
    public LayerMask groundLayer;

    private XRGrabInteractable grabInteractable;
    private IXRSelectInteractor interactor = null; // The controller holding the object.
    private bool isGrabbed = false;

    void Awake()
    {
        // Get the XRGrabInteractable component attached to this object.
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        // Subscribe to the grab and release events.
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        // Unsubscribe from the events to prevent memory leaks.
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Store a reference to the controller that is grabbing the object.
        interactor = args.interactorObject;
        isGrabbed = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Clear the interactor reference.
        interactor = null;
        isGrabbed = false;
    }

    void Update()
    {
        // Only run the logic if the object is currently being held.
        if (isGrabbed)
        {            
            // --- Position Logic: Project controller position onto the ground ---
            Ray ray = new Ray(interactor.transform.position, interactor.transform.forward);
            RaycastHit hitInfo;

            // Perform a raycast downwards from the controller.
            if (Physics.Raycast(ray, out hitInfo, 10f, groundLayer))
            {
                // If it hits the ground, move this object to the hit point.
                transform.position = Vector3.Lerp(transform.position, hitInfo.point, Time.deltaTime * 3f);
            }

            // --- Rotation Logic: Apply controller's Y-axis rotation ---
            // Get the controller's rotation.
            Quaternion controllerRotation = interactor.transform.rotation;

            Quaternion targetRotation = Quaternion.Euler(0, controllerRotation.eulerAngles.z * 2.0f, 0);

            Vector3 forward = interactor.transform.forward; forward.y = 0; forward.Normalize();

            targetRotation = Quaternion.LookRotation( -forward, Vector3.up) * targetRotation;

            // Apply the Y-only rotation to this object.
            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,Time.deltaTime * 7f);
        }
    }
}