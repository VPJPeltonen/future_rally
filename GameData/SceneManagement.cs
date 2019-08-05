using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public GameObject loadingScreen;

    public void SceneLoad(){
        Debug.Log(GameController.Map);
        switch(GameController.Map){
            case "Ancient City":
                loadingScreen.gameObject.SetActive(true);
                SceneManager.LoadScene(1);
                break;
            case "Deep Desert":
                loadingScreen.gameObject.SetActive(true);
                SceneManager.LoadScene(2);
                break;
            default:
                loadingScreen.gameObject.SetActive(true);
                SceneManager.LoadScene(1);
                break;
        }
    }

    public void QuitGame() => Application.Quit();
}
