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
            if (CultGuard.navIndex == CultGuard.navPoints.Length - 1)
            {
                CultGuard.navIndex = 0;
            }
            else
            {
                CultGuard.navIndex++;
            }
            if (CultGuard.currState != CultGuardSM.States.Chase && CultGuard.currState != CultGuardSM.States.Distract)
            {
                CultGuard.SetState(CultGuardSM.States.Patrol);
            }
            else
            {
                return;
            }
        }
    }
}
