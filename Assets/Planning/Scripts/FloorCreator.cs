using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloorCreator : MonoBehaviour {

    bool creating;
    public GameObject MeshGenerator;
    public GameObject tile1;
    public GameObject tile2;
    public GameObject tile3;
    public GameObject cursor;
    private static Item cartItem;
    private ShopScrollList scrollList;
    GameObject tileAdded;
    public GameObject target;
    public static int TilePicked;
    bool overlapTile;
    bool isDrag = false;

    public static List<GameObject> FloorItems;

    private ObjectsLoaded objLoad;

    // Use this for initialization
    void Start () {
        objLoad = target.GetComponent<ObjectsLoaded>();
        scrollList = GameObject.FindGameObjectWithTag("MenuList").GetComponent<ShopScrollList>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (creating && cursor == null)
        //{
        //    if (TilePicked == 0)
        //    {
        //        cursor = (GameObject)Instantiate(tile1, new Vector3(), Quaternion.identity);
        //    }
        //    else if (TilePicked == 1)
        //    {
        //        cursor = (GameObject)Instantiate(tile2, new Vector3(), Quaternion.identity); ;
        //    }
        //    else if (TilePicked == 2)
        //    {
        //        cursor = (GameObject)Instantiate(tile3, new Vector3(), Quaternion.identity); ;
        //    }

        //    cursor.SetActive(true);
        //    cursor.tag = "CursorFloor";
        //}
        

        if (cursor != null && !EventSystem.current.IsPointerOverGameObject())
        {

            cursor.SetActive(true);
            if (Input.GetMouseButtonUp(0))
            {
                isDrag = false;
                return;
            }
            cursor.transform.position = snapPosition(GetWorldPoint());
            GetInput();
        }
        if (cursor != null && EventSystem.current.IsPointerOverGameObject())
        {
            cursor.SetActive(false);
        }
    }

    public void SetTile(int n, Item picked)
    {
        //if (!creating)
        //    creating = true;
        //Destroy(cursor);
        //TilePicked = n;
        //cursor = null;

        //cartItem = picked;        
        MeshGenerator.GetComponent<CretePlaneMesh>().SetTile(n, picked);
    }
    void GetInput()
    {
        //Debug.Log(overlapTile ? "true" : "false");
        if (creating && !EventSystem.current.IsPointerOverGameObject() && overlapTile)
        {

            if (cursor != null && creating && !EventSystem.current.IsPointerOverGameObject() && (Input.GetMouseButtonDown(0) || isDrag))
            {
                foreach (GameObject chk in GameObject.FindGameObjectsWithTag("FloorPlaced"))
                {
                    if (chk.transform.position == cursor.transform.position)
                    {
                        return;
                    }
                }
                Vector3 pos = cursor.transform.position;
                pos.y = 0;
                tileAdded = (GameObject)Instantiate(cursor, pos, Quaternion.identity);
                tileAdded.gameObject.tag = "FloorPlaced";
                objLoad.PushItem(tileAdded);
                if (scrollList != null)
                    scrollList.TryTransferItemToOtherShop(cartItem);
                //target.transform.position = tileAdded.transform.position;
                isDrag = true;
                tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
                //if (tileAdded != null)
                //    FloorItems.Add(tileAdded);
            }
            if (cursor != null && creating && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(1))
            {
                float[] numbers = new float[3] { 1f, .5f, .25f };
                float pick = numbers[Random.Range(0, numbers.Length)];
                cursor.transform.localScale = new Vector3(pick, 0.05f,pick);
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
        snapped.x = Mathf.Floor(orginal.x);
        snapped.y = Mathf.Floor(orginal.y + .5f);
        snapped.z = Mathf.Floor(orginal.z + .5f);
        //if (TilePicked == 0)
        //{
        //    snapped.x = Mathf.Floor(orginal.x);
        //    snapped.y = Mathf.Floor(orginal.y + .5f);
        //    snapped.z = Mathf.Floor(orginal.z + .5f);
        //}
        //else if (TilePicked == 1)
        //{
        //    snapped.x = Mathf.Floor(orginal.x + 0.25f);
        //    snapped.y = Mathf.Floor(orginal.y + 0.25f);
        //    snapped.z = Mathf.Floor(orginal.z + 0.25f);
        //}
        //else if (TilePicked == 2)
        //{
        //    snapped.x = Mathf.Floor(orginal.x + 0.35f);
        //    snapped.y = Mathf.Floor(orginal.y + 0.35f);
        //    snapped.z = Mathf.Floor(orginal.z + 0.35f);
        //}
        //else
        //{
        //    snapped = Vector3.zero;
        //}
        return orginal;
    }

    Vector3 GetWorldPoint()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            
            overlapTile = false;
            if (hit.collider.tag == ("Floor"))
            {
                overlapTile = true;
                return hit.point;
            }
            else if (hit.collider.tag == ("FloorPlaced"))
            {
                overlapTile = true;
                return cursor.transform.position;
            }
            else
                overlapTile = true;
            return hit.point;

        }
        overlapTile = true;
        return hit.point;
    }
}
