using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagDrop : MonoBehaviour
{
    public GameObject[] Drops;
    private int itemNum;
    private int randNum;
    private Transform Epos;

    public void Awake()
    {
        Epos = GetComponent<Transform>();
    }

    public void Item()
    {
        //if death = true spawn item from list
        if (Stats.Die = true)
        {
            //get random number
            randNum = Random.Range(0, 101);

            if (randNum => 50)
            {
                itemNum = 0;
                Instantiate(Drops[itemNum], Epos.position, Quaternion.identity);
            }
            else if (randNum => 20 && randNum < 50)
            {
                itemNum = 1;
                Instantiate(Drops[itemNum], Epos.position, Quaternion.identity);
            }
            else if (randNum => 1 && randNum < 20)
            {
                itemNum = 2;
                Instantiate(Drops[itemNum], Epos.position, Quaternion.identity);
            }
        }
    }
}
