using System.Collections;
using System.IO;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public CharacterController characterController;

    [SerializeField] private SaveControl saveControl;

    public SoundManager SoundManagerScript;
    public UICleaner UIcleanerScript;

    public Flashlight FlashlightScript;
    public Animator animator;
    public Slider Stamina;

    private bool isResting;
    private bool isCrouching;
    private bool IsRunning;
    public bool SwitchtoFreeLock;

    public bool IsGrounded;
    public bool freeze;

    private float horizontalInput;
    private float verticalInput;

    private float MaxStamina, ActualStamina;
    public float moveSpeed, MoveSpeedVar, RunningSpeed;
    public float playerHeight = 2f;


    public float verticalVelocity = 0f;
    public float gravityStrength = -20f;
    private Vector3 moveDirection;

    private Coroutine WaitTimeRoutine;

    public GameObject PlayerCam;
    public GameObject[] FreeLockCams;

    public Transform spawn;
    public Transform orientation;

    public AudioClip[] PlayerEffects;

    public LayerMask WhatIsGround;

    public SanityBar sanityBar;


    void Awake()
    {
        transform.position = SaveControl.playerPosition;
    }

    void Start()
    {
        GameObject UICleanerRef = GameObject.FindGameObjectWithTag("UICleaner");
        if (UICleanerRef != null)
        {
            UIcleanerScript = UICleanerRef.GetComponent<UICleaner>();
        }
        characterController = GetComponent<CharacterController>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        // Pegando alguns componentes

        Cursor.lockState = CursorLockMode.Locked;

        IsRunning = false;
        UIcleanerScript.ControlAnimations(animator, true, "Normal_Idle");
        MaxStamina = 100;
        Stamina.maxValue = MaxStamina;
        ActualStamina = MaxStamina;
        Stamina.value = ActualStamina;
        // Setando os atributos iniciais da Luiza

    }

    void Update()
    {

        Stamina.value = ActualStamina;
        if (freeze) return;
        GroundVerify();
        MyInput();
        MovePlayer();

        // Corrigido: inicia a corrotina apenas uma vez
        if (!IsRunning && ActualStamina < MaxStamina && !isResting)
        {
            StartCoroutine(Resting(1f));
        }
        if (IsRunning)
        {
            FlashlightScript.isOn = false;
        }
    }

    private void MyInput()
    {
        horizontalInput = UnityEngine.Input.GetAxisRaw("Horizontal");
        verticalInput = UnityEngine.Input.GetAxisRaw("Vertical");
    }

    private void GroundVerify()
    {
        Vector3 rayOrigin = characterController.bounds.center;
        float rayLength = (characterController.height / 2f) + 0.1f;
        IsGrounded = Physics.Raycast(rayOrigin, Vector3.down, rayLength, WhatIsGround);
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.y = 0;
        Vector3 move = moveDirection.normalized * moveSpeed;

        if (IsGrounded) // Se estiver no chão, aplicará uma leve força vertical para mante-lo nele
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f; // fixa o player ao chão sem tremer
        }
        else
        {
            verticalVelocity += -90;
        }
        // Aplicação da gravidade, no caso, aplica uma força vertical que antes era potencializada com o GravityStrength, mas foi mudado para testes

        Vector3 finalMove = move;
        finalMove.y = verticalVelocity;
        // Aplica o movimento no eixo Y, no caso, faz a personagem ir até o chão

        characterController.Move(finalMove * Time.deltaTime);

        Vector3 lookDirection = orientation.forward;
        lookDirection.y = 0f;
        lookDirection.Normalize();

        characterController.Move((move + new Vector3(0, 0)) * Time.deltaTime);
        if (lookDirection != Vector3.zero)
        {
            transform.forward = lookDirection;
        }

        if (horizontalInput != 0 || verticalInput != 0) // Verifica se está tendo inputs
        {
            UIcleanerScript.CleanInterface(PlayerCam, FreeLockCams, true, false);
            UIcleanerScript.ControlAnimations(animator, false, "Run", "Normal_Idle", "IdleLantern");
            IsRunning = false;

            if (!FlashlightScript.isOn)
            {
                FlashlightScript.CantUse = true;
                UIcleanerScript.ControlAnimations(animator, false, "WalkWithFlash");
                UIcleanerScript.ControlAnimations(animator, true, "Walk");
            }
            else
            {
                FlashlightScript.CantUse = false;
                UIcleanerScript.ControlAnimations(animator, false, "Walk");
                UIcleanerScript.ControlAnimations(animator, true, "WalkWithFlash");
            }
            // Se a lanterna estiver desligada, roda o Walk normal, senão, roda o Walk com a lanterna

            moveSpeed = 2 + MoveSpeedVar;

            if(WaitTimeRoutine != null)
            {
                StopCoroutine(WaitTimeRoutine);
            }
            
            SwitchtoFreeLock = false;

            if (UnityEngine.Input.GetKey(KeyCode.LeftShift) && ActualStamina > 0)
            {
                IsRunning = true;
                UIcleanerScript.ControlAnimations(animator, false, "Walk", "WalkWithFlash", "Crouch");
                UIcleanerScript.ControlAnimations(animator, true, "Run");

                SoundManagerScript.PlayAudio(SoundManagerScript.SoundEffectSource, PlayerEffects[1]);
                moveSpeed = RunningSpeed + MoveSpeedVar;
                ActualStamina -= Time.deltaTime * 10;

            }

            // Se o jogador apertar o shift e a estamina for maior que 0, toca a animação de corrida, aumenta a velocidade e diminui de forma dinâmica estamina

            if (UnityEngine.Input.GetKey(KeyCode.C))
            {
                IsRunning = false;
                isCrouching = !isCrouching;
                UIcleanerScript.ControlAnimations(animator, false, "Walk", "Run", "Normal_Idle", "WalkWithFlash", "IdleLantern");
                UIcleanerScript.ControlAnimations(animator, true, "Crouch");
                moveSpeed = MoveSpeedVar / 2;
                ActualStamina -= Time.deltaTime * 1;
                PatrolEnemy.shadowEnemyRange = PatrolEnemy.shadowEnemyRange / 2;
                // Fazer diminuir o alcance dos inimigos Sombra de Bruxa
            }
            SoundManagerScript.PlayAudio(SoundManagerScript.SoundEffectSource, PlayerEffects[0]);
        }
        else
        {
            SoundManagerScript.StopAudio(SoundManagerScript.SoundEffectSource);
            FlashlightScript.CantUse = false;
            IsRunning = false;

            if (!FlashlightScript.isOn)
            {
                //Debug.Log("Estado da animação: Idle normal");
                UIcleanerScript.ControlAnimations(animator, false, "Walk", "WalkWithFlash", "IdleLantern", "Run", "Crouch");
                UIcleanerScript.ControlAnimations(animator, true, "Normal_Idle");
            }
            else if (FlashlightScript.isOn)
            {
                UIcleanerScript.ControlAnimations(animator, false, "Walk", "WalkWithFlash", "Run", "Crouch", "Normal_Idle");
                UIcleanerScript.ControlAnimations(animator, true, "IdleLantern");

                //Debug.Log("Estado da animação: Idle lanterna");
            }
            if (!SwitchtoFreeLock)
            {
                WaitTimeRoutine = StartCoroutine(WaitTime());
            }
            // Se não ter trocado as câmeras para o modo livre, inicia a coroutina para contar o tempo
        }
    }

    private IEnumerator WaitTime()
    {
        SwitchtoFreeLock = true;
        yield return new WaitForSeconds(60);
        Debug.Log("Estado da Coroutine WaitTime: Rodando!");
        UIcleanerScript.CleanInterface(PlayerCam, FreeLockCams, false, true);
    }
    // Espera 60 segundos que o jogador esteja AFK para trocar para as câmeras livres
    private IEnumerator Resting(float DelayTime)
    {
        isResting = true;

        // Espera o tempo configurado
        yield return new WaitForSeconds(DelayTime);

        // Recupera a stamina gradualmente
        while (!IsRunning && ActualStamina < MaxStamina)
        {
            ActualStamina += Time.deltaTime * 15f; // taxa de recuperação
            if (ActualStamina > MaxStamina)
                ActualStamina = MaxStamina;

            Stamina.value = ActualStamina;
            yield return null; // espera 1 frame
        }

        isResting = false;
    }

    // Método para resetar posição do jogador ao morrer
    public void DeathReset()
    {
        characterController.enabled = false; // Desativa CharacterController para reposicionar
        transform.position = spawn.position;  // Move para ponto de spawn
        characterController.enabled = true;   // Reativa CharacterController

        sanityBar.ActualSanity = sanityBar.MaxSanity;
    }
}