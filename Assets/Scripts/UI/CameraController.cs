using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movePosition = new Vector3(target.position.x, target.position.y + 1.5f, transform.position.z);
        transform.position = movePosition;
    }
}
