using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
public class DialogueSystem : MonoBehaviour
{
    public DialogueCollision DialogueCollisionScript;

    public GameObject ButtonObject;
    public GameObject Player;
    public GameObject NPC;

    public bool[] PlayerSequence;
    public bool[] NPCSequence;

    private string[] SavePlayerSentences;
    private string[] SaveNPCSentences;
    private bool[] SavePlayerSequences;
    private bool[] SaveNPCSequences;

    private string[] PlayerLineSave;
    private string[] NPCLineSave;

    [SerializeField] string[] PlayerChoiceResponse1;
    [SerializeField] string[] PlayerChoiceResponse2;
    [SerializeField] string[] PlayerChoiceResponse3;

    [SerializeField] bool[] PlayerChoice1Sequence;
    [SerializeField] bool[] PlayerChoice2Sequence;
    [SerializeField] bool[] PlayerChoice3Sequence;


    [SerializeField] private string[] NPCChoiceResponse1;
    [SerializeField] bool[] NPCChoice1Sequence;

    [SerializeField] private string[] NPCChoiceResponse2;
    [SerializeField] bool[] NPCChoice2Sequence;

    [SerializeField] private string[] NPCChoiceResponse3;
    [SerializeField] bool[] NPCChoice3Sequence;

    public TMP_Text PlayerVisibleLines;
    public TMP_Text Player_Name;
    public String[] PlayerSentences;
    public Image Player_Icon;

    public TMP_Text NPCVisiblelines;
    public TMP_Text NPC_Name;
    public String[] NPCSentences;
    public Image NPC_Icon;

    public GameObject PlayerBackground;
    public GameObject NPCBackground;

    public bool DoesAChoice;
    public bool CanSkip, Skiping;

    public bool PlayerTime;

    public float TypingSpeed;

    public int PlayerDialogueIndex, NPCDialogueIndex;
    public int ChoiceIndex;

    void Start()
    {
        SaveStartSets();
        ButtonObject.SetActive(false);
    }

    private void Update()
    {


        if (PlayerDialogueIndex >= PlayerSentences.Length && NPCDialogueIndex >= NPCSentences.Length)
        {
            ButtonObject.SetActive(true);
            CanSkip = false;
        }
        else
        {
            CanSkip = true;
        }
    }

    private void Choice(int VarChoiceIndex, string[] PlayerVarChoiceResponse, bool[] PlayerChoiceSequence, string[] NPCVarChoiceResponse, bool[] NPCChoiceSequence)
    {
        ChoiceIndex = VarChoiceIndex;
        DoesAChoice = true;

        RestartDialogueIndex();
        ClearVisibleLines();

        ButtonObject.SetActive(false);
        CanSkip = true;

        PlayerSentences = PlayerVarChoiceResponse;
        NPCSentences = NPCVarChoiceResponse;
        PlayerSequence = PlayerChoiceSequence;
        NPCSequence = NPCChoiceSequence;
        StopAllCoroutines();
        StartCoroutine(DialogueTyping());
    }
    public void Choice1(bool VarPlayerTime)
    {
        PlayerTime = VarPlayerTime;
        Choice(1, PlayerChoiceResponse1, PlayerChoice1Sequence, NPCChoiceResponse1, NPCChoice1Sequence);
    }

    public void Choice2(bool VarPlayerTime)
    {
        PlayerTime = VarPlayerTime;
        Choice(2, PlayerChoiceResponse2, PlayerChoice2Sequence, NPCChoiceResponse2, NPCChoice2Sequence);
    }

    public void Choice3(bool VarPlayerTime)
    {
        PlayerTime = VarPlayerTime;
        Choice(3, PlayerChoiceResponse3, PlayerChoice3Sequence, NPCChoiceResponse3, NPCChoice3Sequence);

    }

    public void InsertDialogue(Sprite PlayerIconSprite, string PlayerName, string[] Playerlines, Sprite NPCIconSprite, string NPCName, string[] NPCLines)
    {
        Player_Icon.sprite = PlayerIconSprite;
        NPC_Icon.sprite = NPCIconSprite;
        Player_Name.text = PlayerName;
        NPC_Name.text = NPCName;

        PlayerSentences = Playerlines;
        if (!DoesAChoice)
        {
            NPCSentences = NPCLines;
        }


        PlayerLineSave = Playerlines;
        NPCLineSave = NPCLines;
        StartCoroutine(DialogueTyping());
    }

    public void SkipDialogue()
    {
        if (!CanSkip) return;

        // Player chegou no limite
        if (PlayerDialogueIndex >= PlayerSentences.Length)
        {
            PlayerTime = false;

        }
        // NPC chegou no limite
        if (NPCDialogueIndex >= NPCSentences.Length)
        {
            PlayerTime = true;
        }
        Dialogue(PlayerVisibleLines, PlayerSentences, PlayerDialogueIndex, PlayerSequence, NPCVisiblelines, NPCSentences, NPCDialogueIndex, NPCSequence);
    }


    private IEnumerator DialogueTyping()
    {
        if (PlayerTime)
        {
            Player.SetActive(true);
            PlayerBackground.SetActive(true);
            NPCBackground.SetActive(false);
            NPC.SetActive(false);
            if (PlayerDialogueIndex < PlayerSentences.Length)
            {
                foreach (char playerletter in PlayerSentences[PlayerDialogueIndex].ToCharArray())
                {
                    PlayerVisibleLines.text += playerletter;
                    yield return new WaitForSeconds(TypingSpeed);
                }
            }
        }
        else
        {
            Player.SetActive(false);
            NPC.SetActive(true);
            PlayerBackground.SetActive(false);
            NPCBackground.SetActive(true);

            if (NPCDialogueIndex < NPCSentences.Length)
            {


                foreach (char letter in NPCSentences[NPCDialogueIndex].ToCharArray())
                {
                    NPCVisiblelines.text += letter;
                    yield return new WaitForSeconds(TypingSpeed);
                }
            }
        }

        // Se as sentenças terem sido terminadas e forem iguais ao texto visível, contará como se a vez do jogador já tenha passado /

    }

    public void ClearVisibleLines() // Método para limpar as linhas de diálogo visíveis
    {
        PlayerVisibleLines.text = ""; // Zera o texto visível 
        NPCVisiblelines.text = ""; // Zera o texto visível 

    }
    public void RestartDialogueIndex(bool JustRestartOne = false) // Método para limpar os diálogos. Possui dois atributos opcionais. DialogueIndex será zerado caso JustRestartOne for verdadeiro
    {
        if (!JustRestartOne) // Se não for para reiniciar uma linha específica, roda o código abaixo
        {
            PlayerDialogueIndex = 0; // Volta para a 1° linha de diálogo (no caso, primeiro elemento do Array) do player
            NPCDialogueIndex = 0; // Volta para a 1° linha de diálogo (no caso, primeiro elemento do Array) do NPC
        }
        else // Se for verdadeiro, zerá o index recebido pelo parâmetro
        {
            // Zera o index de um diálogo especificado
            NPCDialogueIndex = 0;
        }
    }

    public void SaveStartSets()
    {
        SavePlayerSentences = PlayerSentences;
        SaveNPCSentences = NPCSentences;
        SavePlayerSequences = PlayerSequence;
        SaveNPCSequences = PlayerSequence;
    }

    public void SetStartDialogue()
    {
        PlayerSentences = SavePlayerSentences;
        NPCSentences = SaveNPCSentences;
        PlayerSequence = SavePlayerSequences;
        NPCSequence = SaveNPCSequences;
    }

    public void MouseState(bool Lock)
    {
        if (Lock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void Dialogue(TMP_Text PlayerVisibleLines, string[] PlayerSentences, int VarPlayerDialogueIndex, bool[] PlayerSequence, TMP_Text NPCVisibleLines, string[] NPCSentences, int VarNPCDialogueIndex, bool[] NPCSequence, bool HaveCutscene = false)
    {
        if (PlayerTime)
        {
            if (PlayerVisibleLines.text == PlayerSentences[PlayerDialogueIndex]) // Se todas as falas serem feitas
            {
                if (PlayerDialogueIndex < PlayerSentences.Length)
                {
                    if (PlayerDialogueIndex + 1 < PlayerSequence.Length && PlayerSequence[PlayerDialogueIndex + 1])
                    {
                        Debug.Log("Entrou no if do Player.");
                        PlayerTime = true;
                        PlayerVisibleLines.text = "";
                        VarPlayerDialogueIndex++;
                        PlayerDialogueIndex = VarPlayerDialogueIndex;

                        StartCoroutine(DialogueTyping());
                    }
                    else
                    {
                        Debug.Log("Entrou no else do Player.");
                        // --- NPC chegou no limite ---
                        if (NPCDialogueIndex >= NPCSentences.Length)
                        {
                            PlayerTime = true;      // Player fala sozinho
                                                    // também não retorna, para o Player continuar falando
                        }
                        else
                        {
                            PlayerTime = false;
                        }
                        PlayerVisibleLines.text = "";
                        if (PlayerDialogueIndex < PlayerSentences.Length)
                        {
                            VarPlayerDialogueIndex++;
                            PlayerDialogueIndex = VarPlayerDialogueIndex;
                            StartCoroutine(DialogueTyping());
                        }
                        else
                        {
                            ClearVisibleLines();
                            PlayerSentences = PlayerLineSave;
                            NPCSentences = NPCLineSave;
                            RestartDialogueIndex();
                            PlayerTime = true;
                            DoesAChoice = false;
                            ButtonObject.SetActive(false);
                            StartCoroutine(DialogueTyping());
                        }
                    }

                }
            }

        }
        if (!PlayerTime)
        {
            if (NPCVisibleLines.text == NPCSentences[NPCDialogueIndex] && !PlayerTime)
            {
                if (NPCDialogueIndex < NPCSentences.Length)
                {
                    if (NPCDialogueIndex + 1 < NPCSequence.Length && NPCSequence[NPCDialogueIndex + 1])
                    {
                        Debug.Log("Entrou no if do NPC.");
                        PlayerTime = false;
                        NPCVisiblelines.text = "";
                        VarNPCDialogueIndex++;
                        NPCDialogueIndex = VarNPCDialogueIndex;

                        StartCoroutine(DialogueTyping());
                    }
                    else
                    {
                        Debug.Log("Entrou no else do NPC.");
                        if (PlayerDialogueIndex >= PlayerSentences.Length)
                        {
                            PlayerTime = false;
                        }
                        else
                        {
                            PlayerTime = true;
                        }
                        NPCVisibleLines.text = "";
                        if (NPCDialogueIndex < NPCSentences.Length)
                        {
                            VarNPCDialogueIndex++;
                            NPCDialogueIndex = VarNPCDialogueIndex;
                            StartCoroutine(DialogueTyping());
                        }



                    }
                }
            }
        }
    }
}