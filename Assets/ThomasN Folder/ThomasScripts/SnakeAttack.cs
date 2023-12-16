using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAttack : MonoBehaviour
{
    public float hold;
    public GameObject target;
    static public bool inrange = false;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            inrange = true;
        }
    }

    public void Wrap()
    {
        //if player in range "Wrap"
        if (inrange = true)
        {
            //player cannot hurt Snakeyokai while wrapped
            //damage player over time
            //DamageonTouch
            //lower players speed
            //model must move with player while wrapped
            //StatusEffects.Immobalize
            //end wrap after timer runs out

            //StartCoroutine(Release());
        }
        //IEnumerator Release()
        {
            float elapsedTime = 0f;
            if (elapsedTime < hold)
            {
                //SnakeMovement.Retreat;

            }
        }
    }
}
