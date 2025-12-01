using UnityEngine;

public class OpenExit : MonoBehaviour
{
    [SerializeField] private DialogueSystem dialogueSystem;
    [SerializeField] private BoxCollider exitCollider;

    private void Start()
    {
        exitCollider.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (dialogueSystem.NPCVisiblelines.text == "Entendo… é um ponto de vista interessante. Se é assim que deseja seguir, vá em direção à saída principal, eu aproveitarei toda força que tiver para abrir uma passagem.")
        {
            exitCollider.enabled = true;
        }
    }
}
