using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryControl : MonoBehaviour
{
    public static InventoryItem[] items = new InventoryItem[5]; //array do inventário

    public static InventoryItem emptyItem; //item de espaço vazio

    [SerializeField] private DefaultData itemsLookup; //scriptableObject com os dados padrão p/ pegar os itens

    [SerializeField] private GameObject[] itemAmounts;
    [SerializeField] private GameObject[] itemImages;

    [SerializeField] private Sprite[] amountSprites;

    public TextMeshProUGUI interactionMessage; //mensagem que aparece quando o jogador olha/interage

    private bool hasInteractionMessage = false; //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        emptyItem = itemsLookup.defaultItems[0];
        UpdateInventoryUI();

    }

    public bool GetItem(InventoryItem givenItem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            //procura por um item com o mesmo nome do item recebido
            if (items[i].itemName == givenItem.itemName && givenItem.canStack)
            {
                //se esse item puder ser juntado, apenas aumenta a quantidade possuída
                items[i].itemAmount += givenItem.itemAmount;
                UpdateInventoryUI();
                return true;
            }
        }

        // se o jogador não possuir o item ou ele não puder ser juntado
        // procura por um espaço vazio
        for (int i = 0; i < items.Length; i++)
        {
            // pega o espaço vazio pelo nome
            if (items[i].itemName == emptyItem.itemName)
            {
                items[i] = givenItem;
                UpdateInventoryUI();
                return true;
            }
        }

        // se ambas operações acima falharem, não adiciona o item
        return false;
    }

    public bool UseItem(InventoryItem item, int amountUse)
    {
        for (int i = 0; i < items.Length; i++)
        {
            // procura pelo item da interação no inventário pelo nome
            if (items[i].itemName == item.itemName)
            {
                items[i].itemAmount -= amountUse;
                if (items[i].itemAmount < 1)
                {
                    items[i] = emptyItem;
                }
                UpdateInventoryUI();
                return true;
            }
        }
        // se o jogador não possui o item:
        return false;
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < items.Length; i++) {
            if (items[i].itemAmount != 0)
            {
                itemAmounts[i].GetComponent<Image>().sprite = amountSprites[items[i].itemAmount -1];
            }
            else
            {
                // se a quantidade for 0 (item vazio), remove o número
                itemAmounts[i].GetComponent<Image>().sprite = emptyItem.itemIcon;
            }
                itemImages[i].GetComponent<Image>().sprite = items[i].itemIcon;
        }
    }

    void OnTriggerStay(Collider other)
    {
        // verifica se o objeto tem o script de interação
        if (other.gameObject.TryGetComponent<IPlayerInteraction>(out IPlayerInteraction itemScript))
        {
            Interact(itemScript);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!hasInteractionMessage)
        {
            interactionMessage.text = "";
        }
    }

    private void Interact(IPlayerInteraction itemScript)
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(DisplayMessage(itemScript, itemScript.Interaction()));

            return;
        }
        if (!hasInteractionMessage)
        {
            interactionMessage.text = itemScript.LookMessage();
        }

    }


    IEnumerator DisplayMessage(IPlayerInteraction itemScript, bool interactionCompleted)
    {
        //mostra a mensagem por 4 segundos e depois apaga ela
        hasInteractionMessage = true;
        interactionMessage.text = itemScript.InteractionMessage(interactionCompleted);
        yield return new WaitForSeconds(4f);
        interactionMessage.text = "";
        hasInteractionMessage = false;
    }
}
