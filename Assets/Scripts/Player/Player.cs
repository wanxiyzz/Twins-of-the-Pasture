using System;
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
    /// <summary>
    /// 当前饥饿值
    /// </summary>
    public float currentHungry;
    private float hungry;

    /// <summary>
    /// 当前精神值
    /// </summary>
    public float currentMental;//精神值
    private float mental;

    /// <summary>
    /// 当前体力
    /// </summary>
    public float currentPhysical;
    private float physical;

    private int noSleepyTime;

    private void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
        playerInput = true;
    }
    private void OnEnable()
    {
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.BeforeSceneLoadEvent += OnBeforeSceneLoadEvent;
        EventHandler.HourUpdate += OnhourUpdate;
    }

    private void OnDisable()
    {
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.BeforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
        EventHandler.HourUpdate -= OnhourUpdate;
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
    private void OnhourUpdate()
    {
        noSleepyTime += 1;
        if (noSleepyTime < 6)
        {
            currentHungry -= 5;
            currentMental -= 5;
            currentPhysical -= 5;
        }
        else
        {
            currentHungry -= 10;
            currentMental -= 10;
            currentPhysical -= 10;
        }
        //TODO:三维低的时候应该的反馈
    }
    /// <summary>
    /// 角色休息
    /// </summary>
    private void Sleepy()
    {
        if (currentHungry <= 45)
        {
            return;
        }
        noSleepyTime = 0;
        currentHungry -= 40;
    }


}
