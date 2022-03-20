using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CultGuardSM>())
        {
            Debug.Log("Trigger entered");
            CultGuardSM CultGuard = other.GetComponent<CultGuardSM>();
            CultGuard.navIndex++;
            CultGuard.SetState(CultGuardSM.States.Patrol);
        }
    }
}
