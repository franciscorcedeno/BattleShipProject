using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllMenus : MonoBehaviour
{
    //called by play in main menu and play again in gameover menu
    public void StartSetUp()
    {
        
        ArrayManager.Initiate();

        //SceneManager.UnloadSceneAsync(0);
        //SceneManager.UnloadSceneAsync(3);
        //SceneManager.UnloadSceneAsync(4);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // Use LoadSceneMode.Additive to bring in the scene to the next array
        // if eveything in menu except audio gets set to false with set bool
        // then I could have all scenes play with music
        SceneManager.LoadScene(1);
    }

    //called in gameover menu
    public void MainMenu()
    {
        //SceneManager.UnloadSceneAsync(3);
        //SceneManager.UnloadSceneAsync(4);
        // Menu holds main menu
        // Menu is at index 0
        //LoadSceneMode.Additive
        SceneManager.LoadScene(0);
    }
    
    //called in main and game over menu
    public void QuitGame()
    {
        UnityEngine.Debug.Log("Quit.");
        Application.Quit();
    }

}
