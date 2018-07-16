using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collison : MonoBehaviour {

     WallLine OrthoCamera;
     ObjectsLoaded UndoList;

    //private static bool isCursor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider col)
    {
        if (gameObject.tag == "FloorPlaced" && col.gameObject.tag == "CursorFloor")
        {            
            Destroy(col.gameObject);
        }
        if (gameObject.tag == "FloorPlaced" && col.gameObject.tag == "FloorPlaced")
        {
            UndoList = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectsLoaded>();
            if (UndoList != null)
            {
                UndoList.RemoveUndoItem(col.gameObject);
            }
            Destroy(col.gameObject);
        }
        if (gameObject.tag == "FurnPlaced" && col.gameObject.tag != "FloorPlaced")
        {
            UndoList = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectsLoaded>();
            if (UndoList != null)
            {
                UndoList.RemoveUndoItem(gameObject);
            }
            Destroy(gameObject);
        }
        if (gameObject.tag == "SideBeam" && col.gameObject.tag == "SideBeamL" && gameObject.GetComponentInParent<Transform>().rotation == col.gameObject.GetComponentInParent<Transform>().rotation)
        {
            col.gameObject.GetComponent<Renderer>().enabled =false;
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        if (gameObject.tag.Contains("SideBeam") && col.gameObject.tag == "DoorPlaced" && gameObject.GetComponentInParent<Transform>().rotation == col.gameObject.GetComponentInParent<Transform>().rotation)
        {
            //col.gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (gameObject.tag == "WallPlaced" && col.gameObject.tag == "WallPlaced" )
        {
            float dist = Vector3.Distance(gameObject.transform.position, col.gameObject.transform.position);
            if (dist < 0.28 && gameObject.transform.rotation == col.gameObject.transform.rotation)
            {
                OrthoCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<WallLine>();
                UndoList = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectsLoaded>();
                if (OrthoCamera != null)
                {
                    OrthoCamera.RemoveWallItem(gameObject);                    
                }
                if(UndoList != null)
                {
                    UndoList.RemoveUndoItem(gameObject);
                }
                Renderer[] childrens = col.gameObject.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < childrens.Length; i++)
                {
                    if (childrens[i].tag.Contains("SideBeam"))
                    {
                        childrens[i].GetComponent<Collison>().enabled = true;
                    }
                }
                Destroy(gameObject);
            }
        }
    }
    //public void OnTriggerExit(Collider other)
    //{
    //    if (gameObject.tag.Contains("SideBeamL")&& other.pa)
    //    {
    //        gameObject.GetComponent<Renderer>().enabled = true;
    //    }
    //}
    //public void SetAsCursor()
    //{
    //    isCursor = true;
    //}

    //public bool isObjCursor()
    //{
    //    return isCursor;
    //}

}
