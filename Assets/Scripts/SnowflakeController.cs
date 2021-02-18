using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowflakeController : MonoBehaviour
{
    // definition variables
    private float positionYOffset = 6f;

    void Awake()
    {
        // set position relative to camera
        Vector3 cameraPos = Camera.main.transform.position;
        this.transform.position = new Vector3(cameraPos.x, cameraPos.y + positionYOffset, 0);
        
    }

    // Update is called once per frame
    // TOOD: make snow look good when travelling vertically
    void Update()
    {
        // Follows camera
        
        this.transform.position = new Vector3(  Camera.main.transform.position.x,
                                                Camera.main.transform.position.y + positionYOffset,
                                                0);
        
    }
}
