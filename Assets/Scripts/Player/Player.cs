using System.Collections;
using System;
using MyGame.Cursor;
using UnityEngine;
namespace MyGame.Player
{
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
        private float hungry = 100;

        /// <summary>
        /// 当前精神值
        /// </summary>
        public float currentMental;//精神值
        private float mental = 100;

        /// <summary>
        /// 当前体力
        /// </summary>
        public float currentPhysical;
        private float physical = 100;

        private int noSleepTime;


        //移动至一个地方
        private float stopDistance;

        private Action UseItemAction;
        private Coroutine moveCoroutine;
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
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            UseItemAction = null;
            movementInput = new Vector2(0, 0);
            isMoving = false;
        }
        private void OnhourUpdate()
        {
            noSleepTime += 1;
            if (noSleepTime < 6)
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
            noSleepTime = 0;
            currentHungry -= 40;
        }
        private void OnPlantAPlant()
        {
            for (int i = 0; i < 2; i++)
            {
                animators[i].SetTrigger("Plant");
            }
            StartCoroutine(PlayerInputPause(0.3f));
        }

        private void OnUseTool(ToolType type, Vector3 pos)
        {
            //WORKFLOW:所有工具的动画
            switch (type)
            {
                case ToolType.Axe:
                case ToolType.Hoe:
                case ToolType.Hammer:
                    for (int i = 0; i < 2; i++)
                    {
                        animators[i].SetTrigger("Brandish");
                    }
                    StartCoroutine(PlayerInputPause(0.1f));
                    break;
                case ToolType.HoldItem:
                    for (int i = 0; i < 2; i++)
                    {
                        animators[i].SetTrigger("Plant");
                    }
                    StartCoroutine(PlayerInputPause(0.1f));
                    break;
                case ToolType.Reap:
                    for (int i = 0; i < 2; i++)
                    {
                        animators[i].SetTrigger("UseReap");
                    }
                    StartCoroutine(PlayerInputPause(0.1f));
                    break;
                case ToolType.Shovel:
                    for (int i = 0; i < 2; i++)
                    {
                        animators[i].SetTrigger("UseShovel");
                    }
                    StartCoroutine(PlayerInputPause(0.1f, 0.2f));
                    break;
                default: break;
            }
        }
        IEnumerator PlayerInputPause(float time, float waittime = 0.4f)
        {
            playerInput = false;
            yield return new WaitForSeconds(waittime);
            UseItemAction?.Invoke();
            yield return new WaitForSeconds(time);
            playerInput = true;
        }
        public bool MoveToPos(bool isItem, Vector3 pos, Action action)
        {
            if (playerInput)
            {
                moveCoroutine = StartCoroutine(MoveToTargetPosCoroutine(isItem, pos, action));
                return true;
            }
            return false;
        }
        IEnumerator MoveToTargetPosCoroutine(bool isItem, Vector3 targetPos, Action action)
        {
            float time = 0;
            playerInput = false;
            if (isItem) stopDistance = 0.5f;
            else stopDistance = 0.9f;
            while (Vector3.Distance(transform.position, targetPos) > stopDistance)
            {
                if (time > 5)
                {
                    yield break;
                }
                Vector3 direction = (targetPos - transform.position).normalized * speed;
                inputX = direction.x;
                inputY = direction.y;
                for (int i = 0; i < 2; i++)
                {
                    animators[i].SetFloat("InputX", inputX);
                    animators[i].SetFloat("InputY", inputY);
                    animators[i].SetBool("Run", true);
                }
                isMoving = true;
                time += Time.deltaTime;
                rigi.velocity = direction;
                yield return null;
            }
            for (int i = 0; i < 2; i++)
            {
                animators[i].SetBool("Run", false);
            }
            playerInput = true;
            isMoving = false;
            rigi.velocity = new Vector2(0, 0);
            UseItemAction = action;
            if (!isItem) OnUseTool(CursorManager.Instance.currentTool, targetPos);
            else OnPlantAPlant();
        }
    }
}