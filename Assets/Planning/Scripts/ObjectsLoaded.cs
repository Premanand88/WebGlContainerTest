using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsLoaded : MonoBehaviour {

    public static List<GameObject> instanciatedObjects;
    private GameObject costText;
    private static int total;
    private ShopScrollList scrollList;
    string prefix = "Total Amount:$";
    int m_Size;

    public Camera orthoCamera;
    private static bool camAdjust;

    private static List<GameObject> gmObj;

    public static List<GameObject> GmObj
    {
        get
        {
            return gmObj;
        }

        set
        {
            gmObj = value;
        }
    }

    public bool CamAdjust
    {
        get
        {
            return camAdjust;
        }

        set
        {
            camAdjust = value;
        }
    }

    // Use this for initialization
    void Start () {
        camAdjust = true;
        instanciatedObjects = new List<GameObject>();
        costText = GameObject.FindGameObjectWithTag("CostField");
        scrollList = GameObject.FindGameObjectWithTag("Cart").GetComponent<ShopScrollList>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Size != GetComponentsInChildren<Transform>().Length)
        {
            m_Size = GetComponentsInChildren<Transform>().Length;
            CalculateLocalBounds();
        }
    }

    private void CalculateLocalBounds()
    {
        if (camAdjust)
        {
            Quaternion currentRotation = this.transform.rotation;
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            Bounds bounds = new Bounds(this.transform.position, Vector3.zero);

            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            }

            Vector3 localCenter = bounds.center - this.transform.position;
            bounds.center = localCenter;
            //Debug.Log("The local bounds of this model is " + bounds);

            float camPos = orthoCamera.orthographicSize;
            camPos = (bounds.extents.z + camPos) / 1.5f;
            if (camPos > 5)
            {
                orthoCamera.orthographicSize = camPos;
            }
            camAdjust = false;
        }
    }

    public void PushItem(GameObject objectAdded)
    {
        instanciatedObjects.Add(objectAdded);
        if (objectAdded.name.Contains("Chair"))
        {
            total = total + 100;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        if (objectAdded.name.Contains("Desk"))
        {
            total = total + 200;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        if (objectAdded.name.Contains("SideWall1"))
        {
            total = total + 100;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        else if (objectAdded.name.Contains("FloorCube1"))
        {
            total = total + 25;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        
        if (objectAdded.name.Contains("FloorCube4"))
        {
            total = total + 30;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        if (objectAdded.name.Contains("FloorCube5"))
        {
            total = total + 32;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        if (objectAdded.name.Contains("z9e_prefab"))
        {
            total = total + 250;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        if (objectAdded.name.Contains("Door1"))
        {
            total = total + 250;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        if (objectAdded.name.Contains("GlassDoor"))
        {
            total = total + 1000;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        if (objectAdded.name.Contains("LongGlass"))
        {
            total = total + 2500;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        if (objectAdded.name.Contains("WallCorner"))
        {
            total = total + 250;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        if (objectAdded.name.Contains("WallCorner2"))
        {
            total = total + 250;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
        if (objectAdded.name.Contains("WindowWall"))
        {
            total = total + 250;
            costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
        }
    }

    public void RemoveUndoItem(GameObject obj)
    {
        List<GameObject> removeItem = new List<GameObject>();
        foreach (GameObject objItem in instanciatedObjects)
        {

            try
            {
                if (obj == objItem)
                {
                    removeItem.Add(objItem);
                }
            }
            catch
            {
                Debug.Log("Error");
            }
        }
        foreach (GameObject objItem in removeItem)
        {
            instanciatedObjects.Remove(objItem);
        }
    }

    public void UndoAction()
    {
        foreach (GameObject obj in instanciatedObjects)
        {
            if (obj == null)
            {
                instanciatedObjects.Remove(obj);
            }
        }
        if (instanciatedObjects.Count > 0)
        {
            //GameObject removeObj = new GameObject();
            
            int index = instanciatedObjects.Count - 1;
            if(instanciatedObjects[index] == null)
            {
                instanciatedObjects.Remove(instanciatedObjects[index]);
                UndoAction();
            }
            var removeObj = instanciatedObjects[index];
            if (removeObj.name.Contains("Chair"))
            {
                total = total - 100;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if (removeObj.name.Contains("Desk"))
            {
                total = total - 200;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if (removeObj.name.Contains("FloorCube1 1"))
            {
                total = total - 100;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            else if (removeObj.name.Contains("FloorCube1"))
            {
                total = total - 25;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }

            if (removeObj.name.Contains("FloorCube4"))
            {
                total = total - 30;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if (removeObj.name.Contains("FloorCube5"))
            {
                total = total - 32;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if (removeObj.name.Contains("z9e_prefab"))
            {
                total = total - 250;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if (removeObj.name.Contains("Door1"))
            {
                total = total - 250;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if (removeObj.name.Contains("GlassDoor"))
            {
                total = total - 1000;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if (removeObj.name.Contains("LongGlass"))
            {
                total = total - 2500;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if (removeObj.name.Contains("WallCorner"))
            {
                total = total - 250;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if (removeObj.name.Contains("WallCorner2"))
            {
                total = total - 250;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if (removeObj.name.Contains("WindowWall"))
            {
                total = total - 250;
                costText.GetComponentInChildren<Text>().text = prefix + total.ToString();
            }
            if(removeObj.tag.Contains("Door") || removeObj.tag.Contains("Window"))
            removeObj.GetComponent<DetectCollison>().OnUndo();
            if(removeObj)
            Destroy(removeObj);
            instanciatedObjects.RemoveAt(instanciatedObjects.Count - 1);
            if(scrollList != null)
            {
                scrollList.RemoveLastItem();
            }
            CamAdjust = true;
        }
    }
}
