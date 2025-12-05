using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaver : MonoBehaviour, IPlayerInteraction
{
    public static string saveLookMessage = "Salvar o jogo (E)";
    public static string saveMessage = "Jogo Salvo";

    [SerializeField] private SaveControl saveControl;

    [SerializeField] private Vector3 savePoint;

    void Start()
    {
        savePoint = gameObject.transform.GetChild(0).position;
    }

    public bool Interaction()
    {
        saveControl.LoadGameData();
        SaveControl.savedSceneName = SceneManager.GetActiveScene().name;
        SaveControl.playerPosition = savePoint;

        saveControl.SaveGameData();
        return true;
    }

    public string LookMessage()
    {
        return saveLookMessage;
    }

    public string InteractionMessage(bool completedInteraction)
    {
        return saveMessage;
    }
}
