using UnityEngine;
using UnityEngine.Events;

public class buttonsManager : MonoBehaviour
{
    public bool reattachButtonPressed = false;
    public bool musclesButtonReleased = false;
    public bool skeletonButtonReleased = false;
    public GameObject muscles;
    public GameObject skeleton;
    public GameObject cullSphere;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(musclesButtonReleased)
        {
            if (muscles.activeSelf)
            {
                muscles.SetActive(false);
            }
            else
            {
                muscles.SetActive(true);
            }
            musclesButtonReleased = false;
        }
        if (skeletonButtonReleased)
        {
            if (skeleton.activeSelf)
            {
                skeleton.SetActive(false);
            }
            else
            {
                skeleton.SetActive(true);
            }
            skeletonButtonReleased = false;
        }
    }

    public void reattachButtonPress()
    {
        reattachButtonPressed = true;
    }
    public void reattachButtonRelease()
    {
        reattachButtonPressed = false;
    }

    public void musclesButtonPress()
    {
        musclesButtonReleased = false;
    }
    public void musclesButtonRelease()
    {
        musclesButtonReleased = true;
    }
    public void skeletonButtonPress()
    {
        skeletonButtonReleased = false;
    }
    public void skeletonButtonRelease()
    {
        skeletonButtonReleased = true;
    }
    public void cullSphereButtonRelease()
    {
        if(cullSphere.activeSelf)
        {
            cullSphere.SetActive(false);
        }
        else
        {
            cullSphere.SetActive(true);
        }
    }
}
