using Assets.ReferenceClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WallLine : MonoBehaviour {
    bool creating;
    public List<GameObject> Tile;
    public GameObject cursor;
    GameObject tileAdded;
    public static int TilePicked;
    bool overlapTile;

    public static List<WallAdded> wallsList;

    public GameObject WorldCenter;
    private ObjectsLoaded objLoad;
    private static Item cartItem;
    private ShopScrollList scrollList;
    private static Vector3 prevRot, prevPos,curPos, dragPos;
    bool isDrag = false;
    bool holdZ, holdX, newStart;
    private float posX, posZ;

    //Spawning
    private GameObject[] spawnPrefab;
    private Transform hs;
    private int spawnCount = 0, spawnAmount = 40;
    private float distance = .29f;
    private bool isGood = false;
    

    //lines
    private LineRenderer line;
    public List<Vector3> pointsList;
    private static Vector3 prevScale;

    // Use this for initialization
    void Start () {
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
                cursor = (GameObject)Instantiate(Tile[0], new Vector3(), Quaternion.identity);
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
                //measureLine = 
                //prevScale = measureLine.transform.localScale;
                line = cursor.GetComponentInChildren<LineRenderer>();
                line.material = new Material(Shader.Find("Particles/Additive"));
                line.positionCount = 0;
                line.startWidth = .25f;
                line.endWidth = .25f;
                line.startColor = Color.green;
                line.endColor = Color.green;
                line.useWorldSpace = true;
                pointsList = new List<Vector3>();
                line.positionCount = 0;
                pointsList.RemoveRange(0, pointsList.Count);
            }
            else// if (TilePicked == 1)
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
                tileAdded = (GameObject)Instantiate(Tile[TilePicked], pos, Tile[TilePicked].transform.rotation);
                tileAdded.gameObject.tag = "WallPlaced";
                objLoad.PushItem(tileAdded);
                //prevRot.y = tileAdded.transform.rotation.y;
                if (scrollList != null)
                    scrollList.TryTransferItemToOtherShop(cartItem);

                tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
                WorldCenter.GetComponentInChildren<ObjectsLoaded>().CamAdjust = true;
            }
            //else if (TilePicked == 2)
            //{
            //    GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<ObjectsLoaded>().CamAdjust = true;
            //    creating = false;
            //    Vector3 pos = Vector3.zero;
            //    pos.z = 0.5f;
            //    pos.x = 0.5f;
            //    //cursor = (GameObject)Instantiate(tile3, new Vector3(), Quaternion.identity); 
            //    foreach (GameObject chk in GameObject.FindGameObjectsWithTag("WallPlaced"))
            //    {
            //        if (chk.transform.position == pos)
            //        {
            //            //chk.transform.Rotate(0,90,0);
            //            //prevRot.y = chk.transform.rotation.y;
            //            return;
            //        }
            //    }
            //    tileAdded = (GameObject)Instantiate(Tile[2], pos, Tile[2].transform.rotation);
            //    tileAdded.gameObject.tag = "WallPlaced";
            //    objLoad.PushItem(tileAdded);
            //    //prevRot.y = tileAdded.transform.rotation.y;
            //    if (scrollList != null)
            //        scrollList.TryTransferItemToOtherShop(cartItem);
            //    tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
            //    WorldCenter.GetComponentInChildren<ObjectsLoaded>().CamAdjust = true;
            //}
            if (cursor != null)
                cursor.tag = "Cursor";
            //cursor.transform.rotation = Quaternion.LookRotation(prevRot, Vector3.up);

        }

        if (cursor != null && EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = false;
            cursor.SetActive(false);
        }

        if (cursor != null && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonUp(0) && isDrag)
        {
            curPos = cursor.transform.position;

            //pointsList.Add(curPos);
            //line.positionCount = pointsList.Count;
            //line.GetComponent<CreateLinePos>().SetPos(curPos);

            SpawnItem(prevPos, curPos, cursor);
            isDrag = false;
            newStart = true;
            return;
        }
        else if (isDrag)
        {
            pointsList = new List<Vector3>();
            curPos = cursor.transform.position;
            pointsList.Add(prevPos);
            Vector3 pos1, pos2;
            pos2 = curPos;
            pos1 = prevPos;
            dragPos = prevPos;
            //line.positionCount = pointsList.Count; 
            //if (holdX)
            //{
            //    pos1.x = pos1.x - .29f;
            //    pos2.x = pos2.x + .29f;
            //    //if (pos2.x > pos1.x)
            //    //{
            //    //    //pos1.x = pos1.x - .29f;
            //    //    pos2.x = pos2.x - .29f;
            //    //}
            //    //else
            //    //{
            //    //    //pos1.x = pos1.x + .29f;
            //    //    pos2.x = pos2.x + .29f;
            //    //}
            //}
            //if (holdZ)
            //{
            //    if (pos2.z > pos1.z)
            //    {
            //        //pos1.z = pos1.z - .29f;
            //        pos2.z = pos2.z - .29f;
            //    }
            //    else
            //    {
            //        //pos1.z = pos1.z + .29f;
            //        pos2.z = pos2.z + .29f;
            //    }
            //}
            line.GetComponent<CreateLinePos>().SetPos(pointsList.Count, pos1);

            float dist = Vector3.Distance(dragPos, curPos);
            //Debug.Log(dist);
            if (dist > .29f)
            {
                pointsList.Add(curPos);
                line.GetComponent<CreateLinePos>().SetPos(pointsList.Count, pos2);
                
                //    if (holdX)
                //    {
                //        Debug.Log(prevScale.x + "dist " + dist);
                //        Vector3 scale = measureLine.transform.localScale;
                //        scale.x = prevScale.x + dist;
                //        measureLine.transform.localScale = scale;
                //        //startPos.x = tileAdded.transform.position.x+distance;// = Mathf.Floor(orginal.x + 0.5f);
                //    }
                //    if (holdZ)
                //    {
                //        Vector3 scale = measureLine.transform.localScale;
                //        scale.z = prevScale.z + dist;
                //        measureLine.transform.localScale = scale;
                //        //startPos.z = tileAdded.transform.position.z + distance;
                //    }
            }
        }
        if (cursor != null && !EventSystem.current.IsPointerOverGameObject())
        {

            cursor.SetActive(true);
            cursor.transform.position = snapPosition(GetWorldPoint());

            GetInput();
        }
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

    private void GetInput()
    {
        if (creating && !EventSystem.current.IsPointerOverGameObject() && overlapTile)
        {

            if (cursor != null && creating && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) || isDrag)
            {
                float dist = Vector3.Distance(cursor.transform.position, prevPos);
                //foreach (GameObject chk in GameObject.FindGameObjectsWithTag("WallPlaced"))
                //{
                //    if (chk.transform.position == cursor.transform.position)
                //    {
                //        //chk.transform.Rotate(0,90,0);
                //        //prevRot.y = chk.transform.rotation.y;
                //        return;
                //    }
                //}
                Vector3 pos = cursor.transform.position;

                pos.y = 0;
                if (!isDrag)
                {
                    posX = pos.x;
                    posZ = pos.z;
                    prevPos = pos;
                    isDrag = true;
                    newStart = false;
                    
                    //pointsList.Add(prevPos);
                    //line.positionCount = pointsList.Count;
                    //line.GetComponent<CreateLinePos>().SetPos(prevPos); Debug.Log(prevPos);
                }
                
                isDrag = true;
            }
            if (cursor != null && creating && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(1))
            {
                cursor.transform.Rotate(0, 90, 0);
                Debug.Log(cursor.transform.rotation);
                holdX = !holdX;
                holdZ = !holdZ;
                return;
            }

        }
    }

    public Vector3 snapPosition(Vector3 orginal)
    {
        Vector3 snapped;
        //if (isDrag)
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

                    if (distWall <= .4F)
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

    public void UseTile()
    {
        creating = true;
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

    void SpawnItem(Vector3 startPos, Vector3 endPos, GameObject selected)
    {
        spawnCount = 0;
        
        while (spawnCount < spawnAmount)
        {
            float distWall = Vector3.Distance(startPos, endPos);
            Debug.Log(distWall);

            //If new position is too close, don't bother with this random position
            if (Math.Round(startPos.x,1) == Math.Round(endPos.x, 1) && Math.Round(startPos.y, 1) == Math.Round(endPos.y, 1) && Math.Round(startPos.z, 1) == Math.Round(endPos.z, 1))
            {

                tileAdded = (GameObject)Instantiate(cursor, startPos, cursor.transform.rotation);
                tileAdded.gameObject.tag = "WallPlaced";
                objLoad.PushItem(tileAdded);
                prevRot.y = tileAdded.transform.rotation.y;
                prevPos = startPos;
                if (scrollList != null)
                    scrollList.TryTransferItemToOtherShop(cartItem);
                tileAdded.GetComponent<BoxCollider>().enabled = true;
                tileAdded.GetComponentInChildren<LineRenderer>().enabled = false;
                tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform; WallAdded data = new WallAdded();
                data.wall = tileAdded;
                data.wallPos = holdX ? startPos.x : startPos.z;
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
                //if (holdX)
                //{
                //    if (endPos.x > 0)
                //        startPos.x = tileAdded.transform.position.x + .29f;
                //    else
                //        startPos.x = tileAdded.transform.position.x - .29f;
                //    //startPos.x = tileAdded.transform.position.x+distance;// = Mathf.Floor(orginal.x + 0.5f);
                //}
                //if (holdZ)
                //{
                //    if (endPos.z > 0)
                //        startPos.z = tileAdded.transform.position.z + .29f;
                //    else
                //        startPos.z = tileAdded.transform.position.z - .29f;
                //    //startPos.z = tileAdded.transform.position.z + distance;
                //}
            }
            if (Vector3.Distance(startPos, endPos) < distance )
            {
                isGood = false;
                break;
            }
            else
                isGood = true;

            if (isGood && !EventSystem.current.IsPointerOverGameObject())
            {
                
                spawnCount++;
                tileAdded = Instantiate(selected, startPos, cursor.transform.rotation);
                Vector3 pos = startPos;
                tileAdded.gameObject.tag = "WallPlaced";
                objLoad.PushItem(tileAdded);
                //prevRot.y = tileAdded.transform.rotation.y;
                if (scrollList != null)
                    scrollList.TryTransferItemToOtherShop(cartItem);

                tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
                tileAdded.GetComponentInChildren<LineRenderer>().enabled = false;
                WorldCenter.GetComponentInChildren<ObjectsLoaded>().CamAdjust = true;
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
                if (holdX)
                {
                    if (endPos.x > startPos.x)
                        startPos.x = tileAdded.transform.position.x + .29f;
                    else
                        startPos.x = tileAdded.transform.position.x - .29f;
                    //startPos.x = tileAdded.transform.position.x+distance;// = Mathf.Floor(orginal.x + 0.5f);
                }
                if (holdZ)
                {
                    if (endPos.z > startPos.z)
                        startPos.z = tileAdded.transform.position.z + .29f;
                    else
                        startPos.z = tileAdded.transform.position.z - .29f;
                    //startPos.z = tileAdded.transform.position.z + distance;
                }
            }

        }
    }
}
