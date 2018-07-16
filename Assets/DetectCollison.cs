using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollison : MonoBehaviour {

    private static List<GameObject> colidedObject;
	// Use this for initialization
	void Start () {
        colidedObject = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WallPlaced")
        {
            colidedObject.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (colidedObject.Count > 0)
        {
            foreach (GameObject wall in colidedObject)
            {
                wall.SetActive(true);
            }
        }
    }
    public void OnUndo()
    {
        if (colidedObject.Count > 0)
        {
            foreach (GameObject wall in colidedObject)
            {
                wall.SetActive(true);
            }
        }
    }
}
