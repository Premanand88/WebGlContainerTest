using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereaPan : MonoBehaviour {

    private static float originalZoom;
	// Use this for initialization
	void Start () {
        originalZoom = 0;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveCamera(string Action)
    {
        Vector3 pos = transform.position;
        float moveBy = .25f;
        switch (Action)
        {
            case "UP":
                pos.x = pos.x + moveBy;
                break;
            case "DOWN":
                pos.x = pos.x - moveBy;
                break;
            case "RIGHT":
                pos.z = pos.z - moveBy;
                break;
            case "LEFT":
                pos.z = pos.z + moveBy;
                break;
            case "ZOOMIN":
                gameObject.GetComponent<Camera>().orthographicSize -= .25f ;
                break;
            case "ZOOMOUT":
                gameObject.GetComponent<Camera>().orthographicSize += .25f;
                break;
        }
        transform.position = pos;
    }

    public void ZoomCamera(float value)
    {
        if (originalZoom < value)
        {
            gameObject.GetComponent<Camera>().orthographicSize -= .25f;
        }
        else if(originalZoom > value)
        {
            gameObject.GetComponent<Camera>().orthographicSize += .25f;
        }
    }
}
