using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] CameraControls cameraControls;
    Player player;
    [SerializeField] SpriteRenderer target;

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        UpdateRaycast();
    }
    void UpdateRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rch = Raycast(ray);

            IInteractable interactable;
            if (rch.collider != null)
            {
                interactable = rch.collider.GetComponent<IInteractable>();
                if (interactable != null)
                    interactable.Interact();
                else
                {
                    player.SendFollowerToDistract(rch.point);
                }
            }
            else
            {
                player.SendFollowerToDistract(rch.point);
            }

            float duration = 3;
            Debug.DrawLine(cameraControls.cam.transform.position, ray.origin, Color.yellow, duration);
            Debug.DrawLine(ray.origin, rch.point, Color.red, duration);
        }
    }
    public RaycastHit Raycast(Ray ray)
    {
        RaycastHit raycastHit;
        if (!Physics.Raycast(ray, out raycastHit))
        {
            if (cameraControls.GroundPlane.Raycast(ray, out float distance))
            {
                raycastHit.point = ray.GetPoint(distance);
            }
        }
        
        return raycastHit;
    }
}
