using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    public string nextLevelName;
    public string LevelName;
    // Start is called before the first frame update
    void Awake()
    {
        if (levelManager == null)
            levelManager = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(nextLevelName, LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(WaitForSceneLoad());

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
    }

}
