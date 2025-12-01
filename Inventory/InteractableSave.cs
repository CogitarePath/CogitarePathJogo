using UnityEngine;

public abstract class InteractableSave : Interactable
{
    [SerializeField] protected bool destroyObj;
    // guarda se o objeto já realizou sua ação
    public bool WasUsed;

    //identificador para salvar o estado do objeto
    public string InteractableId;

    public abstract void LoadUsed();

    public string CreateObjectId(GameObject obj)
    {
        //cria um id único para cada objeto com sua posição
        return $"{obj.name}_{obj.transform.position.x}_{obj.transform.position.z}";
    }
}
