using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwivel : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform cam;
    float sensitivity = 20f;
    float xRotation = 0f;
    float minAngle = -89f;
    float maxAngle = 89f;
    float minCamDistance = 30;
    public float camDistance;
    float currCamDistance;
    float maxCamDistance = 500;

    public CameraSwivel I { get; private set; }

    float CamDistance
    {
        get
        {
            return camDistance;
        }
        set
        {
            if (value <= minCamDistance)
            {
                camDistance = minCamDistance;
            }
            else if (value >= maxCamDistance)
            {
                camDistance = maxCamDistance;
            }
            else
            {
                camDistance = value;
            }
        }
    }

    void Awake()
    {
        CamDistance = (maxCamDistance + minCamDistance) / 2f;
        currCamDistance = camDistance;
        SetCameraPosition();
    }
    void LateUpdate()
    {
        transform.position = target.transform.position;

        if (Input.GetMouseButton(0))
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivity, 0), Space.World);

            xRotation -= Input.GetAxis("Mouse Y") * sensitivity / 1.25f;
            xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);
            transform.localEulerAngles = new Vector3(xRotation, transform.localEulerAngles.y, 0);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            CamDistance -= scroll * sensitivity * 3;
            SetCameraPosition();
        }
    }
    void SetCameraPosition()
    {
        Vector3 dir = new Vector3(0, 0, -1);
        currCamDistance = Mathf.Lerp(currCamDistance, camDistance, .5f);
        cam.localPosition = dir * currCamDistance;
    }
    //public void ResetCameraSwivel()
    //{
    //    xRotation = 0;
    //    transform.position = player.position;
    //    transform.rotation = player.rotation;
    //    transform.GetChild(0).GetComponent<PlayerCamera>().ResetCamera();
    //}
}
