using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CameraControls cameraControls;
    public float speed = 7f;
    public float moveLerp = .9f;
    public float rotationLerp = .65f;
    public Vector3 moveInputVelocity;
    CharacterController characterController;
    Vector3 gravity = new Vector3(0, -9.81f, 0);

    public GameObject escapeWall;
    public GameObject lockWall;

    public bool hasLockPick = false;
    public int lockpickCounter;

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

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        lockWall.SetActive(true);
        escapeWall.SetActive(false);
    }
    void Start()
    {
        cameraControls.SetTarget(transform);
    }
    void Update()
    {
        hasLockPick = lockpickCounter > 0;
        MovePlayer();
        if (followers.Count >= 2)
        {
            lockWall.SetActive(false);
            escapeWall.SetActive(true);
        }
        else
        {
            lockWall.SetActive(true);
            escapeWall.SetActive(false);
        }
        //LeadFollowers();
    }

    void MovePlayer()
    {
        Vector3 right = cameraControls.transform.right;
        Vector3 forward = Vector3.Cross(right, Vector3.up).normalized;

        Vector3 direction = (forward * Input.GetAxis("Vertical")) + (right * Input.GetAxis("Horizontal"));
        //moveInputVelocity = direction * speed * Time.deltaTime;
        //Vector3 newPosition = transform.position + moveInputVelocity;

        moveInputVelocity = direction * speed;

        Move(moveInputVelocity + gravity);

        //Move(newPosition);
        Rotate(direction);
    }
    public void Move(Vector3 direction)
    {
        characterController.Move(direction);
    }
    //public void Move(Vector3 force)
    //{
    //    rigidbody.AddForce(force);
    //    //rigidbody.MovePosition();
    //}
    //public void Move(Vector3 position)
    //{
    //    transform.position = Vector3.Lerp(transform.position, position, moveLerp);
    //}
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
