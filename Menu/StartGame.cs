using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] SaveControl saveScript;


    void Start()
    {
        //deixa o cursor visível e controlável dentro da tela
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void NewGame()
    {
        Flashlight.battery = 100f;
        //substitui os dados salvos pelo padrão

        //salva o arquivo com o padrão
        saveScript.RewriteSave();


        //saveScript.SaveTemporaryData();
        saveScript.SaveGameData();
        /*saveScript.SaveTemporaryData();

        saveScript.LoadTemporaryData();*/

        SceneManager.LoadScene(SaveControl.savedSceneName);
        ScenePositionControl.sceneStartPosition = SaveControl.playerPosition;
        //SaveControl.gameData = SaveControl.savedData;
        SaveControl.savedSceneName = "AreaResidencial";
    }

    public void LoadGame()
    {
        //SaveControl.gameData = SaveControl.savedData;
        // envia para a cena onde o jogador salvou o jogo
        //saveScript.LoadGameData();
        SceneManager.LoadScene(SaveControl.savedSceneName);
        ScenePositionControl.sceneStartPosition = SaveControl.playerPosition;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void Options(string SceneName)
    {
        SceneManager.LoadSceneAsync(SceneName);
    }
}

