using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class CameraOrbit : MonoBehaviour
{

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float xRotation = 10.0f;

    public float distanceMin = .5f;
    public float distanceMax = 200f;
    GameObject topSurface;

    private Rigidbody rigidbody;

    WallCreator WallCreator = new WallCreator();
    bool enableOrbit = false;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
        if (WallCreator && WallCreator.enabled)
        {
            enableOrbit = false;
        }

        topSurface = GameObject.FindGameObjectWithTag("Top");
    }

    private void Update()
    {
        //Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    if (hit.collider.tag == "Wall")
        //    {
        //        target = hit.collider.gameObject.transform;
        //    }
        //}
            if (WallCreator == null || !WallCreator.enabled)
        {
            enableOrbit = true;
        }
        else
        {
            enableOrbit = false;
        }
    }

    Vector3 GetWorldPoint()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.tag == "Wall")
                return hit.collider.transform.position;
            else
                return hit.point;

        }
        return transform.position;
    }

    public Vector3 snapPosition(Vector3 orginal)
    {
        Vector3 snapped;
        snapped.x = Mathf.Floor(orginal.x + 0.5f);
        snapped.y = Mathf.Floor(orginal.y + 0.5f);
        snapped.z = Mathf.Floor(orginal.z + 0.5f);
        return snapped;
    }

    void LateUpdate()
    {
        if (enableOrbit)
        {

            if (target && Input.GetMouseButton(0))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                //y -= transform.rotation.y * ySpeed * 0.02f;
                //x += transform.rotation.x * xSpeed * distance * 0.02f;
                //y -= transform.rotation.y * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);
                //y = transform.rotation.y;
                Quaternion rotation = Quaternion.Euler(y, x, 0);
                //Quaternion rotation = Quaternion.Euler(0, x, 0);
                //rotation.x = transform.rotation.x;

                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit))
                {
                    distance -= hit.distance;
                }

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;
            }
            if (target && Input.GetKeyDown(KeyCode.UpArrow))
            {
                //y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                y = y + xRotation;

                Quaternion rotation = Quaternion.Euler(y, x, 0);
                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit))
                {
                    distance -= hit.distance;
                }
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;
            }
            if (target && Input.GetKeyDown(KeyCode.DownArrow))
            {
                //y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                y = y - xRotation;

                Quaternion rotation = Quaternion.Euler(y, x, 0);
                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit))
                {
                    distance -= hit.distance;
                }
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;
            }

            if (target)
            {
                RaycastHit hit;

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
                if (Physics.Linecast(target.position, transform.position, out hit))
                {
                    distance -= hit.distance;
                }
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = transform.rotation * negDistance + target.position;
                transform.position = position;
            }

            if (transform.rotation.x > .40 && transform.rotation.x <= 0.93)
            {
                if (topSurface != null)
                    topSurface.SetActive(false);
            }
            else
            {
                if (topSurface != null)
                    topSurface.SetActive(true);
            }
        }
    
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}