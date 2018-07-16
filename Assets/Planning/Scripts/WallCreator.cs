using Assets.ReferenceClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WallCreator : MonoBehaviour {
    //   bool creating;
    //   public GameObject start;
    //   public GameObject end;
    //   public GameObject wallPrefab;
    //   public GameObject cursor;
    //   GameObject wall;
    //   // Use this for initialization
    //   void Start () {
    //}
    //// Update is called once per frame
    //void Update () {
    //       cursor.transform.position = snapPosition(GetWorldPoint());
    //       GetInput();
    //}
    //   void GetInput()
    //   {
    //       if (GetComponent<Camera>().isActiveAndEnabled && !EventSystem.current.IsPointerOverGameObject())
    //       {
    //           if (Input.GetMouseButtonDown(0))
    //           {
    //               SetStart();
    //           }
    //           else if (Input.GetMouseButtonUp(0))
    //           {
    //               SetEnd();
    //           }
    //           else
    //           {
    //               if (creating)
    //               {
    //                   Adjust();
    //               }
    //           }
    //       }
    //   }
    //   void Adjust()
    //   {
    //       if (GetComponent<Camera>().isActiveAndEnabled)
    //       {
    //           Vector3 pos = GetWorldPoint();
    //           pos.y = 4;// pos.y / 2 + pos.y;
    //           end.transform.position = pos;
    //           AdjustWall();
    //       }
    //   }
    //   void AdjustWall()
    //   {
    //       start.transform.LookAt(end.transform.position);
    //       end.transform.LookAt(start.transform.position);
    //       float distance = Vector3.Distance(start.transform.position, end.transform.position);
    //       AddWall(distance, wall);
    //       //wall.transform.position = start.transform.position + distance / 2 * start.transform.forward;
    //       //wall.transform.rotation = start.transform.rotation;
    //       //wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance);
    //   }
    //   void AddWall (float dist,GameObject wallNew)
    //   {
    //       if (GetComponent<Camera>().isActiveAndEnabled)
    //       {
    //           wallNew.transform.position = start.transform.position + dist / 2 * start.transform.forward;
    //           wallNew.transform.rotation = start.transform.rotation;
    //           wallNew.transform.localScale = new Vector3(wallNew.transform.localScale.x, wallNew.transform.localScale.y, dist+0.5f);
    //       }
    //   }
    //   void SetEnd()
    //   {
    //       if (GetComponent<Camera>().isActiveAndEnabled)
    //       {
    //           creating = false;
    //           Vector3 pos = GetWorldPoint();
    //           pos.y = 4;// pos.y / 2 + pos.y;
    //           end.transform.position = pos;// GetWorldPoint();
    //       }
    //   }
    //   void SetStart()
    //   {
    //       if (GetComponent<Camera>().isActiveAndEnabled)
    //       {
    //           creating = true;
    //           Vector3 pos = GetWorldPoint();
    //           pos.y = 4;// pos.y / 2 + pos.y;
    //           start.transform.position = pos;// GetWorldPoint();
    //           wall = (GameObject)Instantiate(wallPrefab, start.transform.position, Quaternion.identity);
    //       }
    //   }
    //   Vector3 GetWorldPoint()
    //   {
    //       Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
    //       RaycastHit hit;
    //       if (Physics.Raycast(ray, out hit))
    //       {

    //           if (hit.collider.tag == "Wall")                
    //               return hit.collider.transform.position;
    //           else
    //               return hit.point;

    //       }
    //       return Vector3.zero;
    //   }
    //   public Vector3 snapPosition(Vector3 orginal)
    //   {
    //       Vector3 snapped;
    //       snapped.x = Mathf.Floor(orginal.x + 0.25f);
    //       snapped.y = Mathf.Floor(orginal.y + 0.25f);
    //       snapped.z = Mathf.Floor(orginal.z + 0.25f);
    //       return snapped;
    //   }
    bool creating;
    public GameObject tile1;
    public GameObject tile2;
    public GameObject tile3;
    public GameObject cursor;
    GameObject tileAdded;
    public static int TilePicked;
    bool overlapTile;

    public static List<WallAdded> wallsList;
    
    public GameObject WorldCenter;
    private ObjectsLoaded objLoad;
    private static Item cartItem;
    private ShopScrollList scrollList;
    private static Vector3 prevRot,prevPos;
    bool isDrag = false;
    bool holdZ, holdX,newStart;
    private float posX, posZ;

    // Use this for initialization
    void Start()
    {
        objLoad = WorldCenter.GetComponent<ObjectsLoaded>();
        scrollList = GameObject.FindGameObjectWithTag("MenuList").GetComponent<ShopScrollList>();
        wallsList = new List<WallAdded>();
    }

    // Update is called once per frame
    void Update()
    {
        if (creating && cursor == null)
        {
            if (TilePicked == 0)
            {
                cursor = (GameObject)Instantiate(tile1, new Vector3(), Quaternion.identity);
                holdX = true;
                holdZ = false;
                newStart = true;
                cursor.SetActive(true);
                cursor.GetComponent<BoxCollider>().enabled = false;
                Collison[] col = cursor.GetComponentsInChildren<Collison>();
                for (int i = 0; i < col.Length; i++)
                {
                    if (col[i].tag.Contains("CursorMode"))
                        col[i].GetComponent<Collison>().enabled = false;
                }
            }
            else if (TilePicked == 1)
            {
                GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<ObjectsLoaded>().CamAdjust = true;
                creating = false;
                 //cursor = (GameObject)Instantiate(tile2, new Vector3(), Quaternion.identity); 
                 Vector3 pos = Vector3.zero;
                pos.z = 0.5f;
                pos.x = 0.5f;
                foreach (GameObject chk in GameObject.FindGameObjectsWithTag("WallPlaced"))
                {
                    if (chk.transform.position == pos)
                    {
                        //chk.transform.Rotate(0,90,0);
                        //prevRot.y = chk.transform.rotation.y;
                        return;
                    }
                }
                tileAdded = (GameObject)Instantiate(tile2, pos, tile2.transform.rotation);
                tileAdded.gameObject.tag = "WallPlaced";
                objLoad.PushItem(tileAdded);
                //prevRot.y = tileAdded.transform.rotation.y;
                if (scrollList != null)
                    scrollList.TryTransferItemToOtherShop(cartItem);

                tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
                WorldCenter.GetComponentInChildren<ObjectsLoaded>().CamAdjust = true;
            }
            else if (TilePicked == 2)
            {
                GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<ObjectsLoaded>().CamAdjust = true;
                creating = false;
                Vector3 pos = Vector3.zero;
                pos.z = 0.5f;
                pos.x = 0.5f;
                //cursor = (GameObject)Instantiate(tile3, new Vector3(), Quaternion.identity); 
                foreach (GameObject chk in GameObject.FindGameObjectsWithTag("WallPlaced"))
                {
                    if (chk.transform.position == pos)
                    {
                        //chk.transform.Rotate(0,90,0);
                        //prevRot.y = chk.transform.rotation.y;
                        return;
                    }
                }
                tileAdded = (GameObject)Instantiate(tile3, pos, tile3.transform.rotation);
                tileAdded.gameObject.tag = "WallPlaced";
                objLoad.PushItem(tileAdded);
                //prevRot.y = tileAdded.transform.rotation.y;
                if (scrollList != null)
                    scrollList.TryTransferItemToOtherShop(cartItem);

                tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
                WorldCenter.GetComponentInChildren<ObjectsLoaded>().CamAdjust = true;
            }
            if(cursor!=null)
                cursor.tag = "Cursor";
            //cursor.transform.rotation = Quaternion.LookRotation(prevRot, Vector3.up);

        }

        if (cursor != null && EventSystem.current.IsPointerOverGameObject())
        {
            cursor.SetActive(false);
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
            newStart = true;
            return;
        }
        if (cursor != null && !EventSystem.current.IsPointerOverGameObject())
        {
            cursor.SetActive(true);
            cursor.transform.position = snapPosition(GetWorldPoint());
            GetInput();

        }

    }


    void GetInput()
    {
        if (creating && !EventSystem.current.IsPointerOverGameObject() && overlapTile)
        {

            if (cursor != null && creating && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) || isDrag)
            {
                float dist = Vector3.Distance(cursor.transform.position, prevPos);
                foreach (GameObject chk in GameObject.FindGameObjectsWithTag("WallPlaced"))
                {
                    if (chk.transform.position == cursor.transform.position)
                    {
                        //chk.transform.Rotate(0,90,0);
                        //prevRot.y = chk.transform.rotation.y;
                        return;
                    }
                }
                Vector3 pos = cursor.transform.position;

                pos.y = 0;
                if (!isDrag)
                {
                    posX = pos.x;
                    posZ = pos.z;
                    tileAdded = (GameObject)Instantiate(cursor, pos, cursor.transform.rotation);
                    tileAdded.gameObject.tag = "WallPlaced";
                    objLoad.PushItem(tileAdded);
                    prevRot.y = tileAdded.transform.rotation.y;
                    prevPos = tileAdded.transform.position;
                    if (scrollList != null)
                        scrollList.TryTransferItemToOtherShop(cartItem);
                    tileAdded.GetComponent<BoxCollider>().enabled = true;
                    isDrag = true;
                    tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform; WallAdded data = new WallAdded();
                    data.wall = tileAdded;
                    data.wallPos = holdX ? pos.x : pos.z;
                    data.Vertical = holdX;
                    wallsList.Add(data);
                    tileAdded.name = "Wall" + wallsList.Count.ToString();
                    Collison[] col = tileAdded.GetComponentsInChildren<Collison>();
                    for (int i = 0; i < col.Length; i++)
                    {
                        if (col[i].tag.Contains("CursorMode"))
                        {
                            col[i].tag = "SideBeam";
                            col[i].GetComponent<Collison>().enabled = true;
                        }
                    }
                    newStart = false;
                }
                if (dist > .29f)
                {

                    Debug.Log(dist);
                    tileAdded = (GameObject)Instantiate(cursor, pos, cursor.transform.rotation);
                    tileAdded.gameObject.tag = "WallPlaced";
                    objLoad.PushItem(tileAdded);
                    prevRot.y = tileAdded.transform.rotation.y;
                    prevPos = tileAdded.transform.position;
                    if (scrollList != null)
                        scrollList.TryTransferItemToOtherShop(cartItem);
                    tileAdded.GetComponent<BoxCollider>().enabled = true; 
                    tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
                    WallAdded data = new WallAdded();
                    data.wall = tileAdded;
                    data.wallPos = holdX ? pos.x : pos.z;
                    data.Vertical = holdX;
                    wallsList.Add(data);
                    tileAdded.name = "Wall" + wallsList.Count.ToString();
                    tileAdded.GetComponent<Collison>().enabled = true; Collison[] col = tileAdded.GetComponentsInChildren<Collison>();
                    for (int i = 0; i < col.Length; i++)
                    {
                        if (col[i].tag.Contains("CursorMode"))
                        {
                            col[i].tag = "SideBeam";
                            col[i].GetComponent<Collison>().enabled = true;
                        }
                    }
                }

                isDrag = true;
            }
            if (cursor != null && creating && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(1))
            {
                cursor.transform.Rotate(0, 90, 0);
                holdX = !holdX;
                holdZ = !holdZ;
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

    public void RemoveWallItem(GameObject wall)
    {
        List<WallAdded> removeItem = new List<WallAdded>();
        foreach (WallAdded wallItem in wallsList)
        {

            try
            {
                if (wall == wallItem.wall)
                {
                    removeItem.Add(wallItem);
                }
            }
            catch
            {
                Debug.Log("Error");
            }
        }
        foreach (WallAdded wallItem in removeItem)
        {
            wallsList.Remove(wallItem);
        }
    }

    public Vector3 snapPosition(Vector3 orginal)
    {
        Vector3 snapped;
        //if (isDrag)
        //{
        //    snapped.x = orginal.x + 0.3048f;// Mathf.Floor(orginal.x+0.5f);
        //    snapped.y = orginal.y + 0.3048f;//Mathf.Floor(orginal.y + 0.5f);
        //    snapped.z = orginal.z + 0.3048f;//Mathf.Floor(orginal.z + 0.5f);
        //}
        //else
        //{
        //    snapped.x = Mathf.Floor(orginal.x+0.5f);
        //    snapped.y = Mathf.Floor(orginal.y + 0.5f);
        //    snapped.z = Mathf.Floor(orginal.z + 0.5f);
        //}
        snapped.x = orginal.x;//+ 0.3048f;// Mathf.Floor(orginal.x+0.5f);
        snapped.y = orginal.y; //+ 0.3048f;//Mathf.Floor(orginal.y + 0.5f);
        snapped.z = orginal.z;//+ 0.3048f;//Mathf.Floor(orginal.z + 0.5f);
        List<WallAdded> removedComp = new List<WallAdded>();
        if (Input.GetMouseButtonDown(0))
        {
            foreach (WallAdded wallItem in wallsList)
            {

                try
                {
                    if (wallItem.wall == null)
                    {
                        removedComp.Add(wallItem);
                        continue;
                    }
                    float distWall = Vector3.Distance(wallItem.wall.transform.position, snapped);

                    Debug.Log(distWall +wallItem.wall.name);
                    if (distWall <=.4F)
                    {
                        if (newStart && holdX && wallItem.Vertical)
                        {
                            float dist;
                            if (snapped.z > wallItem.wall.transform.position.z)
                            {
                                dist = Math.Abs(Math.Abs(snapped.z) - Math.Abs(wallItem.wall.transform.position.z));
                            }
                            else
                            {
                                dist = Math.Abs(Math.Abs(wallItem.wall.transform.position.z) - Math.Abs(snapped.z));
                            }
                            if (dist < .3)
                            {
                                posZ = wallItem.wall.transform.position.z;
                                orginal.z = wallItem.wall.transform.position.z;
                                if (orginal.x > 0)
                                    orginal.x = wallItem.wall.transform.position.x + .29f;
                                else
                                    orginal.x = wallItem.wall.transform.position.x - .29f;

                            }
                            break;
                        }
                        else if (newStart && holdZ && !wallItem.Vertical && Input.GetMouseButtonDown(0))
                        {
                            float dist;
                            if (snapped.z > wallItem.wall.transform.position.z)
                            {
                                dist = Math.Abs(Math.Abs(snapped.x) - Math.Abs(wallItem.wall.transform.position.x));
                            }
                            else
                            {
                                dist = Math.Abs(Math.Abs(wallItem.wall.transform.position.x) - Math.Abs(snapped.x));
                            }
                            if (dist < .3)
                            {
                                posX = wallItem.wall.transform.position.x;
                                orginal.x = wallItem.wall.transform.position.x;
                                if (orginal.z > 0)
                                    orginal.z = wallItem.wall.transform.position.z + .29f;
                                else
                                    orginal.z = wallItem.wall.transform.position.z - .29f;
                            }
                            break;
                        }
                    }
                }
                catch
                {
                    Debug.Log("Error");
                }
            }
        }
        if (isDrag)
        {
            if (holdX)
            {
                orginal.z = posZ;
                orginal.x = snapped.x;// = Mathf.Floor(orginal.x + 0.5f);
            }
            if (holdZ)
            {
                orginal.x = posX;
                orginal.z = snapped.z;
            }
        }
        return orginal;// snapped;
    }

    Vector3 GetWorldPoint()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            overlapTile = false;
            if (hit.collider.tag.Contains("Floor"))
            {
                overlapTile = true;
                return hit.point;
            }
            else
                overlapTile = false;
            return cursor.transform.position;

        }
        overlapTile = true;
        return cursor.transform.position;
    }
    
}


