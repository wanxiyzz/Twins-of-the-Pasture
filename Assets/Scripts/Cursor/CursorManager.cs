using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorManager : Singleton<CursorManager>
{
    private Camera mainCamera;
    private ItemDetails currentSelectItem;
    private ItemDetails currentTool;
    public Grid currentGrid;
    public Vector3 mouseWorldPos;
    public Vector3Int mouseGridPos;
    public Transform playerTransform;
    public Image checkImage;
    public bool cursorEnable;
    public bool cursorPositionValid;
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        EventHandler.BeforeSceneLoadEvent += OnBeforeSceneLoadEvent;
        EventHandler.SelectItemEvent += OnSelectItemEvent;
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
        EventHandler.BeforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
        EventHandler.SelectItemEvent -= OnSelectItemEvent;
    }
    private void Start()
    {
        mainCamera = Camera.main;
        playerTransform = FindObjectOfType<Player>().transform;
    }
    private void Update()
    {
        // if (!GameManager.Instance.isWin)
        //     return;
        if (!InteractWithUI() || cursorEnable)
        {
            CheckCursorValid();
            if (Input.GetMouseButtonDown(0) && cursorPositionValid)
            {
                EventHandler.CallUseItemEvent(mouseWorldPos, currentSelectItem);
            }
        }
    }

    private void OnSelectItemEvent(ItemDetails details, bool isSelected)
    {
        //TODO:鼠标变样子
        if (isSelected)
        {
            currentSelectItem = details;
            cursorEnable = true;
        }
        else
        {
            currentSelectItem = null;
            cursorEnable = false;
        }
    }

    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        if (currentGrid != null) mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        if (currentSelectItem == null) return;
        if (TransitionManager.Instance.sceneType == SceneType.Field || TransitionManager.Instance.sceneType == SceneType.PlantHuose)
        {
            if (currentSelectItem.itemType.HaveTheType(ItemType.Seed))
            {
                if (TileManager.Instance.CheckCanPlant(mouseGridPos))
                {
                    checkImage.enabled = true;
                    checkImage.transform.position = mainCamera.WorldToScreenPoint(new Vector3(mouseGridPos.x + 0.5f, mouseGridPos.y + 0.5f, 0));
                    SetCursorValid();
                    return;
                }
                else
                {
                    checkImage.enabled = false;
                    SetCursorInValid();
                }
            }
        }
        //WORKFLOW:其他类型的鼠标情况
        else
        {
            return;
        }
    }
    /// <summary>
    /// 是否和UI交互
    /// </summary>
    /// <returns></returns>
    private bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
    private void OnBeforeSceneLoadEvent()
    {
    }

    private void OnAfterSceneLoadEvent(SceneType sceneType, string sceneName)
    {
        currentGrid = FindObjectOfType<Grid>();
        checkImage.enabled = false;
        checkImage.gameObject.SetActive(true);
        cursorEnable = true;
    }
    /// <summary>
    /// 设置鼠标可用 更改鼠标图片
    /// </summary>
    private void SetCursorValid()
    {
        cursorPositionValid = true;
    }
    /// <summary>
    /// 设置鼠标不可用 变回鼠标图片
    /// </summary>
    private void SetCursorInValid()
    {
        cursorPositionValid = false;
    }
}
