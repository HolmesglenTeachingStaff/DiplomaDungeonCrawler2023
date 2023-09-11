using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompleter : MonoBehaviour
{
    [SerializeField] GameObject[] panels;
    [SerializeField] TMP_Text[] results;
    [SerializeField] TMP_Text title;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("collided with ender");
        if(other.tag == "Player")
        {
          //  Debug.Log("Confirmed player");
            if (LevelManager.levelComplete == false)
            {
              //  Debug.Log("Not yet completed");
                LevelManager.levelComplete = true;
                LevelManager.instance.StartCoroutine("LevelEnd");
              //  Debug.Log("attempted Coroutine");
                StartCoroutine("DisplayScores");
               // Debug.Log("attempt second Coroutine");
                
            }
        }
    }
    IEnumerator DisplayScores()
    {
        if (LevelManager.levelComplete == true && LevelManager.gameOver == false)
        {
            title.text = "Level Completed";
        }
        else if(LevelManager.levelComplete == false && LevelManager.gameOver == true)
        {
            title.text = "Game Over";
        }
        float timeStep = 1;
        //Debug.Log("run second Coroutine");
        //tally kills
        results[0].text = "0 / " + LevelManager.totalEnemiesInLevel;
        panels[0].SetActive(true);
        yield return new WaitForSeconds(timeStep);
        int tempNum = 0;
        while(tempNum < LevelManager.enemiesKilled)
        {
            tempNum += 1;
            tempNum = Mathf.Clamp(tempNum, 0, LevelManager.enemiesKilled);
            results[0].text = tempNum + " / " + LevelManager.totalEnemiesInLevel;
            timeStep *= 0.8f;
            timeStep = Mathf.Clamp(timeStep, 0.1f, 1f);
            yield return new WaitForSeconds(timeStep);
        }

        //tally keys
        timeStep = 1;
        results[1].text = "0 / 5";
        panels[1].SetActive(true);
        yield return new WaitForSeconds(timeStep);
        tempNum = 0;
        while (tempNum < LevelManager.keysCollected)
        {
            tempNum += 1;
            tempNum = Mathf.Clamp(tempNum, 0, LevelManager.keysCollected);
            results[1].text = tempNum + " / 5";
            timeStep *= 0.8f;
            timeStep = Mathf.Clamp(timeStep, 0.1f, 1f);
            yield return new WaitForSeconds(timeStep);
        }

        //Tally points
        timeStep = 1;
        results[2].text = "0";
        panels[2].SetActive(true);
        yield return new WaitForSeconds(timeStep);
        tempNum = 0;
        while (tempNum < LevelManager.points)
        {
            tempNum += 1;
            tempNum = Mathf.Clamp(tempNum, 0, LevelManager.points);
            results[2].text = tempNum.ToString();
            timeStep *= 0.8f;
            timeStep = Mathf.Clamp(timeStep, 0.1f, 1f);
            yield return new WaitForSeconds(timeStep);
        }
        //Tally deaths
        int deaths = 2 - LevelManager.lives;
        timeStep = 1;
        results[3].text = "0";
        panels[3].SetActive(true);
        yield return new WaitForSeconds(timeStep);
        tempNum = 0;
        while (tempNum < deaths)
        {
            tempNum += 1;
            tempNum = Mathf.Clamp(tempNum, 0, deaths);
            results[3].text = tempNum.ToString();
            timeStep *= 0.8f;
            timeStep = Mathf.Clamp(timeStep, 0.1f, 1f);
            yield return new WaitForSeconds(timeStep);
        }

        yield return null;
    }
}
