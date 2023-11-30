using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DR_MenuHandler : MonoBehaviour
{
    public GameObject loadScreen;
    public Slider loadProgress;
    public List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    // Start is called before the first frame update
    void Start()
    {
        loadScreen.SetActive(false);
    }
    public void NewGame()
    {
        scenesToLoad.Clear();
        loadScreen.SetActive(true);
        scenesToLoad.Add(SceneManager.LoadSceneAsync("IntroCutScene", LoadSceneMode.Additive));
        
        StartCoroutine(LoadProgress());
    }
    public void ContinueGame()
    {
        scenesToLoad.Clear();
        loadScreen.SetActive(true);
        scenesToLoad.Add(SceneManager.LoadSceneAsync("FirstTestLevel", LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Environment", LoadSceneMode.Additive));
        StartCoroutine(LoadProgress());
    }
    IEnumerator LoadProgress()
    {
        //do something to show loading
        float totalProgress = 0;

        for(int i = 0; i < scenesToLoad.Count; ++i)
        {
            while (!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                //do something using the progress report
                loadProgress.value = totalProgress / scenesToLoad.Count;  
                yield return null;
            }
            
        }
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
