using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILockpic : MonoBehaviour
{
    public Player player;
    public GameObject image;


    public void Update()
    {
        if(player.hasLockPick == true)
        {
            image.SetActive(true);
        }
        else
        {
            image.SetActive(false);
        }
    }
}
