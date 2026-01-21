using UnityEngine;

public class anatomyPosition : MonoBehaviour
{
    public GameObject targetPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = targetPos.transform.position;
        transform.rotation = targetPos.transform.rotation;
    }
}
