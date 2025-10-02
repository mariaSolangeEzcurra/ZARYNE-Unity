using UnityEngine;

public class Billboard : MonoBehaviour
{
    
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(cam!=null)
            transform.forward=cam.transform.forward;
    }
}
