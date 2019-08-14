using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public GameObject loadingScreen,ancientLS,deepLS,greatLS;

    public void SceneLoad(){
        Debug.Log(GameController.Map);
        switch(GameController.Map){
            case "Ancient City":
                ancientLS.gameObject.SetActive(true);
                SceneManager.LoadSceneAsync(1);
                break;
            case "Deep Desert":
                deepLS.gameObject.SetActive(true);
                SceneManager.LoadSceneAsync(2);
                break;
            case "Great Cliff":
                greatLS.gameObject.SetActive(true);
                SceneManager.LoadSceneAsync(3);
                break;                
            default:
                loadingScreen.gameObject.SetActive(true);
                SceneManager.LoadSceneAsync(1);
                break;
        }
    }

    public void MainMenu() {
        loadingScreen.gameObject.SetActive(true);
        SceneManager.LoadScene(0);
    }

    public void QuitGame() => Application.Quit();
}
