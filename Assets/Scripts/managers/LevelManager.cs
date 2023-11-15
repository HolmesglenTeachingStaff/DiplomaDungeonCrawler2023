using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
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
    [SerializeField] Image spiritSlider;
    [SerializeField] ParticleSystem spiritCollection;
    [SerializeField] ParticleSystem spiritPowerCollection;
    [SerializeField] ParticleSystem spiritRelease;
    [SerializeField] ParticleSystem spiritRespawn;
    [SerializeField] LightFader spiritLight;

    [SerializeField] GameObject restartScreen, gameOverScreen, levelCompleteScreen;
    [SerializeField] CanvasGroup[] gamerOverScreenFaders;
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
        //spiritSlider.maxValue = 100f;
        spiritSlider.fillAmount = spiritPower / 100;
        UpdateUI();
        respawnPosition = player.transform.position;
        // InvokeRepeating("AttemptRespawnUpdate", 1, 10f);
        Debug.Log("start game spirits = " + spirits);
    }

    private void ResetValues()
    {
        spirits = 1;
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
            if (j < spirits)
            {
                spiritImages[j].SetActive(true);
                //brokenSpiritImages[j].SetActive(false);
            }
            else
            {
                spiritImages[j].SetActive(false);
                //brokenSpiritImages[j].SetActive(true);
            }
        }
        //update score
        //scoreText.text = points.ToString();
        spiritSlider.fillAmount = spiritPower / 100;
        //update enemies killed
        enemyText.text = enemiesKilled + " / " + totalEnemiesInLevel;
    }

   /* public void CollectKey()
    {
        keysCollected++;
        if (keysCollected > keysInLevel) keysCollected = keysInLevel;
        UpdateUI();
    }*/
    
    public void UpdateSpirit(float spiritIncrease)
    {
        spiritPower += spiritIncrease;
        spiritLight.transform.position = player.transform.position + (Vector3.up * 2);
        spiritLight.StartCoroutine("FadeLight");
        //increase the spirits
        if(spiritPower >= 100)
        {
            spirits++;
            spiritPower -= 100;
            spiritCollection.transform.position = player.transform.position + Vector3.up * 0.1f;
            spiritCollection.Play();
        }
        else
        {
            spiritPowerCollection.transform.position = player.transform.position + Vector3.up * 0.1f;
            spiritPowerCollection.Play();
        }
        UpdateUI();
        
    }
    
    public void KillPlayer()
    {
        // Debug.Log("KillPlayer");
        if (playerIsDead) return;
        playerIsDead = true;
        
        
        PlayerDeath.Invoke();
        if(spirits > 0)
        {
            StartCoroutine(RespawnPlayer());
            spiritRelease.transform.position = player.transform.position + Vector3.up * 0.01f;
            spiritRelease.Play();
        }
        else
        {
            gameOver = true;
            StartCoroutine("LevelEnd");
            //levelCompleter.StartCoroutine("DisplayScores");
        }
        spirits--;
        Debug.Log("killed player spirits = " + spirits);
    }
    public IEnumerator RespawnPlayer()
    {
        if (isSpawning) yield return null;
        isSpawning = true;

        gameOverScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
        restartScreen.SetActive(true);

        player.GetComponent<PlayerMovment>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        lantern.GetComponent<LanternAttack>().enabled = false;
        player.GetComponent<MeleeAttack>().enabled = false;
        player.GetComponentInChildren<PlayerIKHandling>().ikActive = false;

        WaitForEndOfFrame frame = new WaitForEndOfFrame();

        yield return new WaitForSeconds(4);

        float timer = 0f;
        while(timer < 1)
        {
            screenFader.alpha = Mathf.Lerp(0, 1, timer);
            timer += screenFadeTime * 0.01f;
            yield return frame;
        }

        screenFader.alpha = 1f;
        player.transform.position = respawnPosition;
        yield return new WaitForSeconds(1);

        Stats playerstats = player.GetComponent<Stats>();
        playerstats.currentArmour = playerstats.maxArmour;
        playerstats.currentHealth = playerstats.maxHealth;
        playerstats.OnDamaged.Invoke();

        yield return frame;
        // player.GetComponent<PlayerRespawn>().Respawn();


       
        screenFader.alpha = 0f;
        playerIsDead = false;
        
        player.GetComponentInChildren<Animator>().SetBool("Dead", false);
        spiritRespawn.transform.position = player.transform.position + Vector3.up * 0.1f;
        spiritRespawn.Play();
        UpdateUI();

        timer = 0;
        while (timer < 1)
        {
            screenFader.alpha = Mathf.Lerp(1, 0, timer);
            timer += (screenFadeTime *0.01f);
            yield return frame;
        }
        
        yield return new WaitForSeconds(5);
        player.GetComponent<PlayerMovment>().enabled = true;
        player.GetComponentInChildren<PlayerIKHandling>().ikActive = false;
        lantern.GetComponent<LanternAttack>().enabled = true;
        player.GetComponent<MeleeAttack>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;

        playerstats.isDead = false;
        isSpawning = false;
        Debug.Log("respawning, spirits = " + spirits);
    }
   
    public IEnumerator LevelEnd()
    {

        gameOverScreen.SetActive(true);
        levelCompleteScreen.SetActive(false);
        restartScreen.SetActive(false);

        player.GetComponent<PlayerMovment>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        lantern.GetComponent<LanternAttack>().enabled = false;
        player.GetComponent<MeleeAttack>().enabled = false;
        player.GetComponentInChildren<PlayerIKHandling>().ikActive = false;

        yield return new WaitForSeconds(2);
        CanvasGroup gameOverScreenFader = gameOverScreen.GetComponent<CanvasGroup>();
        float timer = 0f;
        WaitForEndOfFrame frame = new WaitForEndOfFrame();
        
        timer = 0;
        while (timer < 1)
        {
            gamerOverScreenFaders[0].alpha = Mathf.Lerp(0, 1, timer);
            timer += screenFadeTime * 0.001f;
            yield return frame;
        }
        //yield return new WaitForSeconds(1);
        timer = 0;
        while (timer < 1)
        {
            gamerOverScreenFaders[1].alpha = Mathf.Lerp(0, 1, timer);
            timer += screenFadeTime * 0.001f;
            yield return frame;
        }
       
        timer = 0;
        while (timer < 1)
        {
            gamerOverScreenFaders[2].alpha = Mathf.Lerp(0, 1, timer);
            timer += screenFadeTime * 0.001f;
            yield return frame;
        }
       


        
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
