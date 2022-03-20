using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CameraControls cameraControls;
    public float speed = 7f;
    public float moveLerp = .9f;
    public float rotationLerp = .65f;
    Vector3 moveInputVelocity;
    // add normal velocity for falling

    public List<Prisoner> followers = new List<Prisoner>();
    public float followDistance = 5;

    public Vector3 FollowPosition
    {
        get
        {
            return transform.position - transform.forward * followDistance;
        }
    }
    public bool IsMoving
    {
        get
        {
            if (moveInputVelocity.magnitude > 0)
            {
                return true;
            }
            return false;
        }
    }

    void Start()
    {
        cameraControls.SetTarget(transform);
    }
    void Update()
    {
        MovePlayer();
        //LeadFollowers();
    }

    void MovePlayer()
    {
        Vector3 right = cameraControls.transform.right;
        Vector3 forward = Vector3.Cross(right, Vector3.up).normalized;

        Vector3 direction = (forward * Input.GetAxis("Vertical")) + (right * Input.GetAxis("Horizontal"));
        moveInputVelocity = direction * speed * Time.deltaTime;
        Vector3 newPosition = transform.position + moveInputVelocity;

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
    public void AddFollower(Prisoner newFollower)
    {
        followers.Add(newFollower);
        newFollower.FollowPlayer();
    }
    //void RemoveFollower(Prisoner oldFollower)
    //{
    //    followers.Remove(oldFollower);
    //}
    public void SendFollowerToDistract(Vector3 distractPosition)
    {
        for (int i = followers.Count - 1; i >= 0; i--)
        {
            if (followers[i].Focused)
            {
                followers[i].Distract(distractPosition);
                followers.Remove(followers[i]);
                return;
            }
        }
    }
}
