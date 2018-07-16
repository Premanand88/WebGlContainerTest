using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLinePos : MonoBehaviour {

    LineRenderer line;
	// Use this for initialization
	void Start () {
        if (line == null)
            line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RestePos()
    {
        if (line == null)
            line = GetComponent<LineRenderer>();
        else
            line.positionCount = 0;
    }

    public void SetPos(int i, Vector3 PosPoint)
    {
        if (line.positionCount > 0)
            line.SetPosition(i - 1, PosPoint);
    }
}
