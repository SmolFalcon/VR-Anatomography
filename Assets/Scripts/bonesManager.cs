#if UNITY_EDITOR
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class bonesManager : MonoBehaviour
{
    public Material bonesMaterial;
    public GameObject nameDisplayLeft;
    public GameObject nameDisplayRight;
    public GameObject factPanelRight;
    public GameObject reattachButton;
    public GameObject cullSphere;
    public bool processBones = false;

    void OnValidate()
    {
        if (!Application.isPlaying && processBones)
        {
            foreach (Transform child in transform) // Child is offset null
            {
                GameObject bone;
                bone = child.GetChild(0).gameObject; // Get the gameobject of the bone

                bone.SetActive(true);

                UnityEditor.EditorApplication.delayCall += () => //delay execution to allow for component removal
                {
                    RemoveAllAttachedComponents(bone);

                    bone.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;

                    // Add necessary components
                    bone.AddComponent<Rigidbody>();
                    bone.AddComponent<MeshCollider>();
                    bone.AddComponent<XRGrabInteractable>();
                    bone.AddComponent<boneInteract>();

                    // Setup components
                    bone.GetComponent<XRGrabInteractable>().useDynamicAttach = true;
                    bone.GetComponent<MeshRenderer>().material = bonesMaterial;
                    bone.GetComponent<Rigidbody>().linearDamping = 1.0f;
                    bone.GetComponent<Rigidbody>().angularDamping = 0.5f;
                    bone.GetComponent<Rigidbody>().useGravity = false;
                    bone.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                    bone.GetComponent<Rigidbody>().includeLayers = 8; //Include only bone colliders
                    bone.GetComponent<Rigidbody>().excludeLayers = -1; //Exclude all layers
                    bone.GetComponent<MeshCollider>().convex = true;
                    bone.GetComponent<boneInteract>().nameDisplayLeft = nameDisplayLeft;
                    bone.GetComponent<boneInteract>().nameDisplayRight = nameDisplayRight;
                    bone.GetComponent<boneInteract>().factPanelRight = factPanelRight;
                    bone.GetComponent<boneInteract>().reattachButton = reattachButton;
                    bone.GetComponent<boneInteract>().cullSphere = cullSphere;
                };

            }

            processBones = false;
        }
    }

    public void RemoveAllAttachedComponents(GameObject pGameObject)
    {
        // Get all components attached to this GameObject
        // Store them in a list to avoid issues with modifying the collection during iteration
        List<Component> components = new List<Component>(pGameObject.GetComponents<Component>());
        components.Reverse();
        // Iterate through the collected components
        foreach (Component component in components)
        {
            // Do not destroy the Transform component, as every GameObject requires one
            if (!(component is Transform || component is MeshRenderer || component is MeshFilter))
            {
                // Destroy the component
                // Use DestroyImmediate in editor scripts for immediate removal
                // Use Destroy in runtime code
                DestroyImmediate(component);
            }
        }
    }
}
#endif