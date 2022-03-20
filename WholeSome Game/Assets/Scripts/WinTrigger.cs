using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject WinScreen;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Player player = other.GetComponent<Player>();
            if (player.followers.Count >= 2)
            {
                WinScreen.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
