using UnityEngine;

[ExecuteInEditMode]
public class CameraControls : MonoBehaviour
{
    public Camera cam;
    [SerializeField] float camDistance = 20;
    float currCamDistance;
    [SerializeField] [Range(1, 100)] float minCamDistance = 5;
    [SerializeField] [Range(1, 100)] float maxCamDistance = 50;
    public bool sightLine = true;
    [SerializeField] [Range(.1f, 2)] float camWallDistance = .25f;
    [Range(100, 1000)] public float raycastDistance = 100;
    //Plane plane;
    public float xSensitivity = 1;
    public float ySensitivity = 1;
    public float zoomSensitivity = 1;
    public float moveSensitivity = 1;
    public Vector3 lerpPosition;
    public float moveLerp = .7f;
    [Range(-179, -1)] public float minAngle = -60;
    [Range(-179, -1)] public float maxAngle = -3;
    public bool rotate = true;
    public bool move = true;
    public bool zoom = true;
    float xRotation = 0;
    [SerializeField] CameraBounds[] boundaries;
    CameraBounds currentBoundary;
    Transform target;
    bool targetting = false;

    float CamDistance
    {
        get
        {
            return camDistance;
        }
        set
        {
            if (value < minCamDistance)
            {
                camDistance = minCamDistance;
            }
            else if (value > maxCamDistance)
            {
                camDistance = maxCamDistance;
            }
            else
            {
                camDistance = value;
            }
        }
    }
    public Plane GroundPlane
    {
        get
        {
            Plane plane = new Plane(Vector3.up, currentBoundary.transform.position);
            return plane;
        }
    }

    void Awake()
    {
        SetupCamera();
        SetupCameraBounds();
    }
    void LateUpdate()
    {
        UpdateMovement();
        UpdateRotationAndZoom();

        UpdateCameraPosition();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, .3f);

        if (currentBoundary)
            Gizmos.DrawWireCube(currentBoundary.Center, currentBoundary.size);
        else
            SetupCameraBounds();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, camDistance);
    }

    void SetupCamera()
    {
        if (!cam)
            cam = Camera.main;
        if (!cam.gameObject.activeSelf)
            cam.gameObject.SetActive(true);

        currCamDistance = camDistance;
    }
    void SetupCameraBounds()
    {
        if (boundaries.Length > 0)
            currentBoundary = boundaries[0]; 
    }
    void UpdateMovement()
    {
        if (move)
        {
            Vector3 direction = Vector3.zero;
            Vector3 newPosition;

            if (targetting)
            {
                newPosition = target.position;
                lerpPosition = newPosition;
            }
            else
            {
                Vector3 forward = Vector3.Cross(cam.transform.right, Vector3.up).normalized;
                direction = forward * Input.GetAxis("Vertical") + cam.transform.right * Input.GetAxis("Horizontal");

                newPosition = transform.position + direction * moveSensitivity;
            }
            
            if (direction != Vector3.zero)
            {
                newPosition = ClampToBoundary(newPosition);
                lerpPosition = newPosition;
            }

            transform.position = Vector3.Lerp(transform.position, lerpPosition, moveLerp);
        }
    }
    void UpdateRotationAndZoom()
    {
        float xInput = 0;
        float yInput = 0;
        if (Input.GetMouseButton(1))
        {
            xInput = Input.GetAxis("Mouse X") * xSensitivity;
            yInput = Input.GetAxis("Mouse Y") * ySensitivity;
        }
        float zoomInput = -Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;

        if (rotate)
            Rotate(xInput, yInput);

        if (zoom)
            Zoom(zoomInput);
    }
    void UpdateCameraPosition()
    {
        Vector3 origin = transform.position;
        Vector3 camDirection = cam.transform.position - transform.position;
        Vector3 camNewPosition = origin + camDirection.normalized * currCamDistance;

        if (sightLine)
        {   
            Ray r = new Ray(origin, camDirection);
            r.origin = r.GetPoint(minCamDistance);
            float raycastDistance = Vector3.Distance(r.origin, camNewPosition);

            if (Physics.Linecast(r.origin, r.origin + camDirection.normalized * raycastDistance, out RaycastHit rch))
            {
                float distance = Vector3.Distance(r.origin, rch.point);
                distance -= camWallDistance;
                camNewPosition = r.GetPoint(distance);
            }
        }

        cam.transform.position = camNewPosition;

        Debug.DrawLine(origin, camNewPosition, Color.white);
        Debug.DrawLine(origin, origin + transform.up, Color.red);
    }
    void Rotate(float xInput, float yInput)
    {
        try
        {
            transform.Rotate(new Vector3(0, xInput * xSensitivity, 0), Space.World);
        }
        catch (System.Exception)
        {
            return;
        }

        xRotation -= yInput * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);
        transform.localEulerAngles = new Vector3(xRotation, transform.localEulerAngles.y, 0);
    }
    void Zoom(float zoomInput)
    {
        CamDistance += zoomInput * zoomSensitivity;
        currCamDistance = Mathf.Lerp(currCamDistance, camDistance, .5f);
    }
    public void SetPosition(Vector3 position)
    {
        lerpPosition = position;
        transform.position = position;
    }
    public void SetLerpPosition(Vector3 position)
    {
        lerpPosition = position;
    }
    public void ChangeBoundary(CameraBounds newBoundary)
    {
        currentBoundary = newBoundary;
        //SetLerpPosition(newBoundary.Center);
    }
    Vector3 ClampToBoundary(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, currentBoundary.Min.x, currentBoundary.Max.x);
        position.z = Mathf.Clamp(position.z, currentBoundary.Min.z, currentBoundary.Max.z);

        return position;
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetting = true;
    }
    public void StopTargetting()
    {
        target = null;
        targetting = false;
    }
}
