using UnityEngine;
public class Player : MonoBehaviour
{
    [Header("常规")]
    private Rigidbody2D rigi;
    private Animator[] animators;

    [Header("移动")]
    public float speed;
    private float inputX;
    private float inputY;
    private Vector2 movementInput;
    private bool isMoving;
    public bool playerInput;


    [Header("Rest")]
    private float restTime;
    private float currentRestTime;

    private float sleepyTime = 4;
    private float currentSleepyTime;


    [Header("三维")]
    public float currentHungry;//饥饿值
    private float hungry;
    public float currentMental;//精神值
    private float mental;
    public float currentPhysical;
    private float physical;


    private void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
        playerInput = true;
    }
    private void OnEnable()
    {
        EventHandler.moveToPosition += OnMoveToPosition;
        EventHandler.beforeSceneLoadEvent += OnBeforeSceneLoadEvent;
    }

    private void OnDisable()
    {
        EventHandler.moveToPosition -= OnMoveToPosition;
        EventHandler.beforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
    }

    private void OnMoveToPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    private void Update()
    {
        if (playerInput)
            PlayerInput();
        SwitchAnimation();
    }
    private void FixedUpdate()
    {
        if (playerInput)
            Movement();
    }
    private void Movement()
    {
        rigi.MovePosition(rigi.position + movementInput * speed * Time.deltaTime);
    }
    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        if (inputX != 0 && inputY != 0)
        {
            inputX = inputX * 0.6f;
            inputY = inputY * 0.6f;
        }
        movementInput = new Vector2(inputX, inputY);
        isMoving = movementInput != Vector2.zero;
    }
    private void SwitchAnimation()
    {
        foreach (var animator in animators)
        {
            animator.SetBool("Run", isMoving);
            if (isMoving)
            {
                animator.SetFloat("InputX", inputX);
                animator.SetFloat("InputY", inputY);
            }
        }
        if (!isMoving)
        {
            if (currentSleepyTime <= 0)
            {
                currentSleepyTime = sleepyTime;
                foreach (var animator in animators)
                {
                    animator.SetTrigger("Sleepy");
                }
            }
            else currentSleepyTime -= Time.deltaTime;
        }
        else currentSleepyTime = sleepyTime;
    }
    private void OnBeforeSceneLoadEvent()
    {
        movementInput = new Vector2(0, 0);
        isMoving = false;
    }



}
