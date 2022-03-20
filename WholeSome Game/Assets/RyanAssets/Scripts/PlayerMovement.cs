using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CameraControls cameraControls;
    public float speed = 7f;
    public float moveLerp = .9f;
    public float rotationLerp = .8f;

    void Start()
    {
        cameraControls.SetTarget(transform);
    }
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 right = cameraControls.transform.right;
        Vector3 forward = Vector3.Cross(right, Vector3.up).normalized;

        Vector3 direction = (forward * Input.GetAxis("Vertical")) + (right * Input.GetAxis("Horizontal"));
        Vector3 velocity = direction * speed * Time.deltaTime;
        Vector3 newPosition = transform.position + velocity;

        Move(newPosition);
        Rotate(direction);
    }
    public void Move(Vector3 position)
    {
        transform.position = Vector3.Lerp(transform.position, position, moveLerp);
    }
    void Rotate(Vector3 direction)
    {
        transform.LookAt(transform.position + Vector3.Lerp(transform.forward, direction, rotationLerp), Vector3.up);
    }
}
