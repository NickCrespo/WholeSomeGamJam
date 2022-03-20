using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Sense : MonoBehaviour
{
    public List<Prisoner> nearbyPrisoners = new List<Prisoner>();
    SphereCollider sphereCollider;
    public float periphery = .5f;

    public bool TooCrowdedInFront
    {
        get
        {
            List<Prisoner> crowd = new List<Prisoner>();
            foreach (var prisoner in nearbyPrisoners)
            {
                Vector3 sight = transform.position + transform.forward;
                Vector3 toPrisoner = prisoner.transform.position - transform.position;
                float cos = Vector3.Dot(sight, toPrisoner) / toPrisoner.magnitude;
                if (cos > periphery && toPrisoner.magnitude < prisoner.PersonalSpace)
                {
                    crowd.Add(prisoner);
                }
            }
            if (crowd.Count > 1)
            {
                return true;
            }
            return false;
        }
    }

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    void Update()
    {
        UpdateNearbyPrisoners();
    }

    void OnTriggerEnter(Collider other)
    {
        Prisoner prisoner = other.GetComponent<Prisoner>();
        if (prisoner && !nearbyPrisoners.Contains(prisoner))
        {
            nearbyPrisoners.Add(prisoner);
        }
    }
    void UpdateNearbyPrisoners()
    {
        for (int i = nearbyPrisoners.Count - 1; i >= 0; i--)
        {
            float distance = Vector3.Distance(transform.position, nearbyPrisoners[i].transform.position);
            if (distance > sphereCollider.radius)
            {
                nearbyPrisoners.RemoveAt(i);
            }
        }
    }
}
