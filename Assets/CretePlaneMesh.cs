using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CretePlaneMesh : MonoBehaviour
{

    public float width = 50f;
    public float height = 50f;
    MeshFilter mf;
    Mesh mesh;
    Vector3 startPoint;
    Vector3 endPoint;
    public GameObject menuList;
    private ShopScrollList scrollList;
    GameObject tileAdded;
    public GameObject target;
    int tilePicked;
    private static Item item;
    bool creating= false;
    public bool oppDirection,started= false;

    public List<GameObject> Tile;
    
    private ObjectsLoaded objLoad;

    bool isDrag= false;
    /// <summary>
    /// https://www.youtube.com/watch?v=R_kV3YiJqEw
    /// </summary>
    // Use this for initialization
    void Start()
    {
        objLoad = target.GetComponent<ObjectsLoaded>();

        scrollList = menuList.GetComponent<ShopScrollList>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mesh != null && creating)
        {
            tileAdded = null;
            DrawMesh(Vector3.zero, Vector3.zero);
            GetComponent<MeshRenderer>().sharedMaterial = Tile[tilePicked].GetComponent<MeshRenderer>().sharedMaterial;
            creating = false;
            //if (TilePicked == 0)
            //{
            //    cursor = (GameObject)Instantiate(Tile[TilePicked], new Vector3(), Quaternion.identity);
            //}
            //else if (TilePicked == 1)
            //{
            //    cursor = (GameObject)Instantiate(tile2, new Vector3(), Quaternion.identity); ;
            //}
            //else if (TilePicked == 2)
            //{
            //    cursor = (GameObject)Instantiate(tile3, new Vector3(), Quaternion.identity); ;
            //}
        }
        if (mesh != null && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) && !isDrag)
        {
            GetInput();
        }
        if (isDrag && mesh != null)
        {
            GetInput();
            if (Input.GetAxis("Mouse X") >= 0 && !oppDirection)
            {
                //for left move
                DrawMesh(startPoint, endPoint, oppDirection);
                started = true;
            }
            else if (Input.GetAxis("Mouse X") <= 0)
            {
                //play animation for right move
                oppDirection = true;
                DrawMesh(startPoint, endPoint, oppDirection);
            }
            else if (Input.GetAxis("Mouse X") >= 0)
            {
                oppDirection = false;
            }
            //else
            //{
            //    oppDirection = !oppDirection;
            //}
        }
        if (Input.GetMouseButtonUp(0) && isDrag && mesh != null)
        {
            tileAdded = (GameObject)Instantiate(gameObject, transform.position, Quaternion.identity);
            tileAdded.gameObject.tag = "FloorPlaced";
            objLoad.PushItem(tileAdded);
            if (scrollList != null)
                scrollList.TryTransferItemToOtherShop(item);
            tileAdded.transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
            mesh = null;
        }
    }
    private void GetInput()
    {
        if (mesh != null && !EventSystem.current.IsPointerOverGameObject() || isDrag)
        {
            if (!isDrag)
            {
                startPoint = GetWorldPoint();
                isDrag = true;
            }
            else if (isDrag)
            {
                endPoint = GetWorldPoint();
            }

        }
    }

    public void SetTile(int n, Item picked)
    {
        isDrag = false;
        tilePicked = n;
        item = picked;
        oppDirection = false;
        mf = GetComponent<MeshFilter>();
        creating = true;
        mesh = new Mesh();
    }

    public void DrawMesh(Vector3 startPos, Vector3 endPos, bool opposite = false)
    {
        mf.mesh = mesh;

        //Vertices
        startPos.y = 0.08f;
        endPos.y = 0.08f;
        
          Vector3[] vertices = new Vector3[4]
          {
            //new Vector3(0,0,0),new Vector3(width,0,0),new Vector3(0,height,0),new Vector3(width,height,0)           
            startPos,new Vector3(endPos.x,0.08f,startPos.z),new Vector3(startPos.x,0.08f,endPos.z),endPos
          };

        //Triangles --- always goes clockwise in vertex order
        int[] tri = new int[6];

        //Normals(only if needed to be displayed in screen)
        Vector3[] normals = new Vector3[4];

        float dist = Vector3.Distance(startPos, endPos);
        int uvsize = Mathf.FloorToInt(20 / dist);
        uvsize = uvsize < 1 ? 2 : uvsize*2;
        if ( !opposite)
        {            
            tri[0] = 0;
            tri[1] = 2;
            tri[2] = 1;
            tri[3] = 2;
            tri[4] = 3;
            tri[5] = 1;

            normals[0] = -Vector3.forward;
            normals[1] = -Vector3.forward;
            normals[2] = -Vector3.forward;
            normals[3] = -Vector3.forward;

        }
        else
        {
            tri[0] = 0;
            tri[1] = 1;
            tri[2] = 2;
            tri[3] = 1;
            tri[4] = 3;
            tri[5] = 2;


            normals[0] = -Vector3.forward;
            normals[1] = -Vector3.forward;
            normals[2] = -Vector3.forward;
            normals[3] = -Vector3.forward;
        }


        //UVs(how textures are displayed)
        Vector2[] uv = new Vector2[4];
        Debug.Log(startPos +" other"+ endPos);
        uv[0] = new Vector2(0, 0);//origin (width,height)
        uv[1] = new Vector2(0, uvsize);//( 0,full height);
        uv[2] = new Vector2(uvsize, 0);//(full width ,0);
        uv[3] = new Vector2(uvsize, uvsize);//(full width, height);
        //2         3
        //----------//
        //|\        |
        //|  \      |
        //|    \    |
        //|      \  |
        //----------//
        //0         1


        //Assign Arrays 
        if (vertices != null && mesh != null)
        {
            mesh.vertices = vertices;
            mesh.triangles = tri;
            mesh.normals = normals;
            mesh.uv = uv;
        }
    }

    Vector3 GetWorldPoint()
    {
        GameObject maincamera = GameObject.FindGameObjectWithTag("MainCamera");
        Ray ray = maincamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.tag == ("Floor"))
            {
                return hit.point;
            }
            //else if (hit.collider.tag == ("FloorPlaced"))
            //{
            //    return cursor.transform.position;
            //}
            return hit.point;

        }
        return hit.point;        
    }
}
