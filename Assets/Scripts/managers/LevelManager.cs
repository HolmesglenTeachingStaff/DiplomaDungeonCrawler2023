using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class LevelManager : MonoBehaviour
{
#region variables
    //Player Static Data
    public static int spirits;
    public float spiritPower;
    public static bool playerIsDead;
    //public static int keysCollected;
    //public static int points;
    public static int enemiesKilled;

    //Level Settings
    //public int keysInLevel;
    public float screenFadeTime;
    //[HideInInspector]
    public Vector3 respawnPosition;

    //level static Data
    public static bool levelComplete;
    public static bool gameOver;
    public static int totalEnemiesInLevel;
    bool isSpawning = false;
    //Unity Events
    public UnityEvent PlayerDeath;
    public UnityEvent PlayerRespawn;
    public UnityEvent EndLevel;

    //Object References
    //[SerializeField] GameObject[] keyImages;
    [SerializeField] GameObject[] spiritImages;
    [SerializeField] GameObject[] brokenSpiritImages;
    [SerializeField] GameObject restartScreen, levelEndScreen;
    public LevelCompleter levelCompleter;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text enemyText;
    [SerializeField] CanvasGroup screenFader;
    public GameObject player;
    public GameObject lantern;
    #endregion
    #region signleton
    public static LevelManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ResetValues();
        CollectEnemiesInLevel();
        UpdateUI();
        respawnPosition = player.transform.position;
       // InvokeRepeating("AttemptRespawnUpdate", 1, 10f);
    }

    private void ResetValues()
    {
        spirits = 2;
        playerIsDead = false;
        //keysCollected = 0;
        //points = 0;
        enemiesKilled = 0;
        levelComplete = false;
        gameOver = false;
    }
    void CollectEnemiesInLevel()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemiesInLevel = Enemies.Length;
    }

    public void UpdateUI()
    {
        /*//update keys
        for(int i = 0; i < keyImages.Length; i++)
        {
            if( (i + 1)<= keysCollected) keyImages[i].SetActive(true);
            else keyImages[i].SetActive(false);
        }*/
        //updateLives
        for (int j = 0; j < spiritImages.Length; j++)
        {
            if (j <= spirits)
            {
                spiritImages[j].SetActive(true);
                brokenSpiritImages[j].SetActive(false);
            }
            else
            {
                spiritImages[j].SetActive(false);
                brokenSpiritImages[j].SetActive(true);
            }
        }
        //update score
        //scoreText.text = points.ToString();

        //update enemies killed
        enemyText.text = enemiesKilled + " / " + totalEnemiesInLevel;
    }

   /* public void CollectKey()
    {
        keysCollected++;
        if (keysCollected > keysInLevel) keysCollected = keysInLevel;
        UpdateUI();
    }*/

    public void KillPlayer()
    {
       // Debug.Log("KillPlayer");
        playerIsDead = true;
        spirits--;
        
        PlayerDeath.Invoke();
        if(spirits > 0)
        {
            StartCoroutine(RespawnPlayer());
        }
        else
        {
            gameOver = true;
            StartCoroutine("LevelEnd");
            levelCompleter.StartCoroutine("DisplayScores");
        }
    }
    public IEnumerator RespawnPlayer()
    {
        if (isSpawning) yield return null;
        isSpawning = true;

        levelEndScreen.SetActive(false);
        restartScreen.SetActive(true);

        player.GetComponent<PlayerMovment>().enabled = false;
        lantern.GetComponent<LanternAttack>().enabled = false;
        player.GetComponent<MeleeAttack>().enabled = false;
        WaitForEndOfFrame frame = new WaitForEndOfFrame();

        float timer = 0f;
        while(timer < 1)
        {
            screenFader.alpha = Mathf.Lerp(0, 1, timer);
            timer += screenFadeTime;
            yield return frame;
        }

        screenFader.alpha = 1f;
        Stats playerstats = player.GetComponent<Stats>();
        playerstats.currentArmour = playerstats.maxArmour;
        playerstats.currentHealth = playerstats.maxHealth;
        playerstats.OnDamaged.Invoke();
        player.transform.position = respawnPosition;
       // player.GetComponent<PlayerRespawn>().Respawn();

        yield return new WaitForSeconds(2);
        timer = 0;
        while (timer < 1)
        {
            screenFader.alpha = Mathf.Lerp(1, 0, timer);
            timer += (screenFadeTime);
            yield return frame;
        }
        
        lantern.GetComponent<LanternAttack>().enabled = true;
        player.GetComponent<MeleeAttack>().enabled = true;
        screenFader.alpha = 0f;
        playerIsDead = false;
        UpdateUI();
        player.GetComponent<PlayerMovment>().enabled = true;
    }
   
    public IEnumerator LevelEnd()
    {
        
        levelEndScreen.SetActive(true);       
        restartScreen.SetActive(false);
        float timer = 0f;
        WaitForEndOfFrame frame = new WaitForEndOfFrame();
        while (timer < 1)
        {
            screenFader.alpha = Mathf.Lerp(0, 1, timer);
            timer += screenFadeTime;
            yield return frame;
        }
        player.GetComponent<PlayerMovment>().enabled = false;
        player.GetComponent<BowAttack>().enabled = false;
        player.GetComponent<MeleeAttack>().enabled = false;
    }

    void AttemptRespawnUpdate()
    {
        if(player.GetComponent<CharacterController>().isGrounded && !CheckForEnemies())
        {
            respawnPosition = player.transform.position;
        }
    }
    bool CheckForEnemies()
    {
        Collider[] possibleEnemies = Physics.OverlapSphere(player.transform.position, 10f);
        foreach(Collider item in possibleEnemies)
        {
            if (item.tag == "Enemy")
            {
                return true;
            }
        }
        return false;
    }
}
