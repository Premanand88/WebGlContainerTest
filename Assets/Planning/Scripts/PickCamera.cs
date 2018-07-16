using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickCamera : MonoBehaviour {

    public Camera firstPersonCamera;
    public Camera overheadCamera;
    private List<GameObject> gmObj;
    public GameObject GameManager;
    private Vector3 camPos;
    private Quaternion camRot;

    public void Start()
    {
        //ShowOverheadView();
        gmObj = new List<GameObject>();
        camPos = firstPersonCamera.transform.position;
        camRot = firstPersonCamera.transform.rotation;
    }

    
    public void Update()
    {
        ObjectsLoaded getLoaded = GameManager.GetComponentInChildren<ObjectsLoaded>();
        //getLoaded.GmObj.
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Front");
       
        if (overheadCamera.enabled)
        {

            //gmObj.Add( GameObject.FindGameObjectsWithTag("Top"));
            foreach (GameObject top in temp)
            {
                if (top.activeInHierarchy)
                {
                    top.GetComponent<Renderer>().enabled =true;
                }
            }
        }
        if (firstPersonCamera.enabled)
        {
            //gmObj.Add( GameObject.FindGameObjectsWithTag("Top"));
            foreach (GameObject top in temp)
            {
                if (top.activeInHierarchy)
                {
                    top.GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }

    public void ShowOverheadView()
    {
        firstPersonCamera.enabled = false;
        overheadCamera.enabled = true;
    }

    public void ShowFirstPersonView()
    {
        firstPersonCamera.transform.SetPositionAndRotation(camPos, camRot);
        overheadCamera.enabled = false;
        firstPersonCamera.enabled = true;

    }

}
