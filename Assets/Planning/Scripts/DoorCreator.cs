using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoorCreator : MonoBehaviour {

    bool creating;
    public GameObject tile1;
    public GameObject tile2;
    public GameObject tile3;
    public GameObject cursor;
    GameObject tileAdded;
    public static int TilePicked;

    public GameObject WorldCenter;
    private ObjectsLoaded objLoad;
    private static Item cartItem;
    private ShopScrollList scrollList;

    bool overlapTile;

    // Use this for initialization
    void Start()
    {
        objLoad = WorldCenter.GetComponent<ObjectsLoaded>();
        scrollList = GameObject.FindGameObjectWithTag("MenuList").GetComponent<ShopScrollList>();
    }

    // Update is called once per frame
    void Update()
    {
        if (creating && cursor == null)
        {
            if (TilePicked == 0)
            {
                cursor = (GameObject)Instantiate(tile1, new Vector3(), Quaternion.identity);
            }
            else if (TilePicked == 1)
            {
                cursor = (GameObject)Instantiate(tile2, new Vector3(), Quaternion.identity); ;
            }
            else if (TilePicked == 2)
            {
                cursor = (GameObject)Instantiate(tile3, new Vector3(), Quaternion.identity); ;
            }
            cursor.SetActive(true);
        }
        if (cursor != null && !EventSystem.current.IsPointerOverGameObject())
        {
            cursor.SetActive(true);
            cursor.transform.position = snapPosition(GetWorldPoint());

            GetInput();
        }

        if (cursor != null && EventSystem.current.IsPointerOverGameObject())
        {
            cursor.SetActive(false);
        }
    }


    void GetInput()
    {
        if (creating && !EventSystem.current.IsPointerOverGameObject())
        {

            if (cursor != null && creating && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) && !overlapTile)
            {
                foreach (GameObject chk in GameObject.FindGameObjectsWithTag("DoorPlaced"))
                {
                    if (chk.transform.position == cursor.transform.position)
                    {
                        return;
                    }
                }
                Vector3 pos = cursor.transform.position;
                pos.y = 0;
                cursor.GetComponent<DetectCollison>().OnUndo();
                tileAdded = (GameObject)Instantiate(cursor, pos, cursor.transform.rotation);                
                tileAdded.gameObject.tag = "DoorPlaced";                
                objLoad.PushItem(tileAdded);
                if (scrollList != null)
                    scrollList.TryTransferItemToOtherShop(cartItem);
                creating = false;
                Destroy(cursor);
                cursor = null;
                tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
            }
            if (cursor != null && creating && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(1))
            {
                cursor.transform.Rotate(0, 90, 0);
                return;
            }
            //else if (Input.GetMouseButtonDown(0))
            //{
            //    creating = true;
            //}
            //else if (Input.GetMouseButtonUp(0))
            //{
            //    creating = false;
            //}
            if (Input.GetKeyDown("1"))
            {
                TilePicked = 0;
                Destroy(cursor);
            }
            if (Input.GetKeyDown("2"))
            {
                TilePicked = 1;
                Destroy(cursor);
            }
            if (Input.GetKeyDown("3"))
            {
                TilePicked = 2;
                Destroy(cursor);
            }

            //else
            //{
            //    if (creating)
            //    {
            //        Adjust();
            //    }
            //}
        }
        else if (!creating && Input.GetMouseButtonDown(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.tag == "DoorPlaced")
                {
                    hit.transform.Rotate(0, 90, 0);
                }
                //else

            }
        }
    }

    public void SetTile(int n, Item picked)
    {
        if (!creating)
            creating = true;
        Destroy(cursor);
        TilePicked = n;
        cursor = null;
        cartItem = picked;
    }

    public void UseTile()
    {
        creating = true;
    }

    public void StopTile()
    {
        creating = false;
        TilePicked = 0;
        cursor = null;
    }
    public Vector3 snapPosition(Vector3 orginal)
    {
        Vector3 snapped;
        if (TilePicked == 0)
        {
            snapped.x = Mathf.Floor(orginal.x + 0.5f);
            snapped.y = Mathf.Floor(orginal.y + 0.5f);
            snapped.z = Mathf.Floor(orginal.z + 0.5f);
        }
        else if (TilePicked == 1)
        {
            snapped.x = Mathf.Floor(orginal.x + 0.25f);
            snapped.y = Mathf.Floor(orginal.y + 0.25f);
            snapped.z = Mathf.Floor(orginal.z + 0.25f);
        }
        else if (TilePicked == 2)
        {
            snapped.x = Mathf.Floor(orginal.x + 0.35f);
            snapped.y = Mathf.Floor(orginal.y + 0.35f);
            snapped.z = Mathf.Floor(orginal.z + 0.35f);
        }
        else
        {
            snapped = Vector3.zero;
        }
        return orginal;// snapped;
    }

    Vector3 GetWorldPoint()
    {
        overlapTile = false;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            //if (hit.collider.tag == "FloorPlaced")
            //{
            //    overlapTile = false;
            //    return hit.point;
            //}
            //else
            overlapTile = false;
            return hit.point;

        }
        overlapTile = true;
        return cursor.transform.position;
    }
}
