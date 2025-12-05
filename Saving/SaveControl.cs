using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveControl : MonoBehaviour
{
    // dados padrão do jogo, sem nada salvo
    [SerializeField] private DefaultData defaultData;

    private string saveFile; //o caminho do arquivo de salvamento
    public static Vector3 playerPosition; //posição do jogador quando ele salvou
    public static string savedSceneName; //cena onde o jogo foi salvo

    [SerializeField] private string instanceScene;

    //todos os objetos com uma interação, cujo estado pode ser alterado e salvo
    private static InteractableSave[] interactables;


    private static InteractableSave[] residentialInteractables;
    private static InteractableSave[] comercialInteractables;
    private static InteractableSave[] parkInteractables;
    private static InteractableSave[] policeInteractables;

    private void Start()
    {
        //encontra o arquivo com as informações salvas 
        saveFile = Path.Combine(Application.persistentDataPath, "GameSave.json");

        instanceScene = SceneManager.GetActiveScene().name;

        //pega todos os objetos com o script para interação

        interactables = FindObjectsByType<InteractableSave>(FindObjectsSortMode.None);

        CheckScene(instanceScene);

        LoadGameData();
    }

    void CheckScene(string scene)
    {
        switch (scene)
        {
            case "AreaResidencial":
                residentialInteractables = interactables;
                break;
            case "AreaComercial":
                comercialInteractables = interactables;
                break;
            case "AreaParque":
                parkInteractables = interactables;
                break;
            case "Delegacia":
                policeInteractables = interactables;
                break;
        }
    }

    public void SaveGameData()
    {
        Debug.Log(playerPosition);
        Debug.Log(savedSceneName);
        /*Debug.Log(residentialInteractables[0].InteractableId);
        Debug.Log(residentialInteractables[1].InteractableId);*/

        SaveData saveData = new()
        {
            //salva a posição do ponto de salvamento
            savedPosition = playerPosition,

            //salva a cena onde ocorreu o salvamento
            savedScene = savedSceneName,

            residentialData = SaveObjectData(residentialInteractables),
            comercialData = SaveObjectData(comercialInteractables),
            parkData = SaveObjectData(parkInteractables),
            policeData = SaveObjectData(policeInteractables),


            savedInventory = SaveInventory()

        };

        //escreve os dados acima (da classe SaveData) em um arquivo json
        // true formata o arquivo para ser mais legível
        File.WriteAllText(saveFile, JsonUtility.ToJson(saveData, true));
    }


    private List<ObjectInteractionData> SaveObjectData(InteractableSave[] interactableSaves)
    {
        List<ObjectInteractionData> ObjectStates = new();

        if (interactableSaves != null)
        {

            foreach (InteractableSave interactable in interactableSaves)
            {
                ObjectInteractionData objectInteractionData = new()
                {
                    //para cada objeto salvo, cria uma instância da classe com os dados salvos
                    objectId = interactable.InteractableId,
                    wasUsed = interactable.WasUsed,
                };

                //adiciona essa instância a uma lista, que se torna uma lista de objetos no json
                ObjectStates.Add(objectInteractionData);
            }
        }
        return ObjectStates;
    }

    private List<InventoryData> SaveInventory()
    {
        List<InventoryData> savedList = new();

        for (int i = 0; i < InventoryControl.items.Length; i++)
        {

            if (InventoryControl.items[i] != null)
            {
                InventoryData inventoryData = new()
                {
                    itemId = InventoryControl.items[i].id,
                    itemAmount = InventoryControl.items[i].itemAmount,
                };
                savedList.Add(inventoryData);
            }
            else
            {
                // se o inventário não tiver nada salvo, salva ele só com o item vazio
                InventoryData inventoryData = new()
                {
                    itemId = defaultData.defaultItems[0].id,
                    itemAmount = defaultData.defaultItems[0].itemAmount,
                };
                savedList.Add(inventoryData);
            }

        }
        return savedList;
    }

    public void LoadGameData()
    {
        if (File.Exists(saveFile))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFile));

            playerPosition = saveData.savedPosition;
            savedSceneName = saveData.savedScene;


            LoadInventory(saveData.savedInventory);

            LoadObjectStates(LoadSceneState(instanceScene, saveData));


        }
        else
        {
            SaveGameData(); // se não houver arquivo salvo(jogo aberto pela primeira vez) salva os dados padrão
        }
    }

    private List<ObjectInteractionData> LoadSceneState(string scene, SaveData saveData)
    {
        switch (scene)
        {
            case "AreaResidencial":
                return saveData.residentialData;
            case "AreaComercial":
                return saveData.comercialData;
            case "AreaParque":
                return saveData.parkData;
            case "Delegacia":
                return saveData.policeData;
        }
        Debug.Log("null");
        return null;
    }

    private void LoadObjectStates(List<ObjectInteractionData> ObjectStates)
    {

        foreach (InteractableSave interactable in interactables)
        {

            //para cada objeto com interação encontrado
            // busca na lista de objetos salvos por um com o mesmo id
            ObjectInteractionData objData = ObjectStates.FirstOrDefault(c => c.objectId == interactable.InteractableId);
            Debug.Log(interactable.InteractableId);
            Debug.Log(ObjectStates.FirstOrDefault(c => c.objectId == interactable.InteractableId));
            Debug.Log(objData.wasUsed);
            if (objData != null)
            {

                // se houver algo salvo, atribui os valores salvo ao objeto
                // e chama sua função de carregamento
                interactable.WasUsed = objData.wasUsed;
                if (interactable.WasUsed)
                {
                    Debug.Log(interactable.gameObject);
                    interactable.LoadUsed();
                }

            }
        }
    }

    private void LoadInventory(List<InventoryData> data)
    {
        //pega a lista no arquivo de salvamento
        for (int i = 0; i < InventoryControl.items.Length; i++)
        {
            //cria um instância e a coloca no inventário para não alterar o original
            InventoryControl.items[i] = ScriptableObject.Instantiate(defaultData.defaultItems[data[i].itemId]);
            InventoryControl.items[i].itemAmount = data[i].itemAmount;
        }
    }

    //substitui os dados salvos pelo padrão
    public void RewriteSave()
    {
        savedSceneName = defaultData.startSceneName;
        playerPosition = defaultData.startPosition;

        SaveData saveData = new()
        {
            //salva a posição do ponto de salvamento
            savedPosition = defaultData.startPosition,

            //salva a cena onde ocorreu o salvamento
            savedScene = defaultData.startSceneName,

            residentialData = new(),
            comercialData = new(),
            parkData = new(),
            policeData = new(),

            savedInventory = EmptyInventory()


        };
        File.WriteAllText(saveFile, JsonUtility.ToJson(saveData, true));
    }

    private List<InventoryData> EmptyInventory()
    {
        //no arquivo de salvamento, salva um inventário com as informações do item vazio
        List<InventoryData> inventoryDatas = new();
        for (int i = 0; i < 5; i++)
        {
            InventoryData data = new();
            InventoryControl.items[i] = defaultData.defaultItems[0];
            data.itemAmount = defaultData.defaultItems[0].itemAmount;
            data.itemId = defaultData.defaultItems[0].id;
        }
        return inventoryDatas;
    }

}

