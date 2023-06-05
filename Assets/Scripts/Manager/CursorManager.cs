using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorManager : Singleton<CursorManager>
{
    private Camera mainCamera;
    private ItemDetails currentSelectItem;
    public Grid currentGrid;
    public Vector3 mouseWorldPos;
    public Vector3Int mouseGridPos;
    public Transform playerTransform;
    public Image checkImage;
    public bool inTransition;

    private void OnEnable()
    {
        EventHandler.afterSceneLoadEvent += OnAfterSceneLoadEvent;
        EventHandler.beforeSceneLoadEvent += OnBeforeSceneLoadEvent;
    }
    private void OnDisable()
    {
        EventHandler.afterSceneLoadEvent -= OnAfterSceneLoadEvent;
        EventHandler.beforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
    }


    private void Start()
    {
        mainCamera = Camera.main;
        playerTransform = FindObjectOfType<Player>().transform;
        currentSelectItem = ItemManager.Instance.FindItemDetails(1029);

    }
    private void Update()
    {
        // if (!GameManager.Instance.isWin)
        //     return;
        if (!InteractWithUI() || !inTransition)
        {
            CheckCursorValid();
        }
    }
    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        if (currentGrid != null) mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        if (TransitionManager.Instance.sceneType == SceneType.Field || TransitionManager.Instance.sceneType == SceneType.PlantHuose)
        {
            switch (currentSelectItem.itemType)
            {
                case ItemType.Seed:
                    if (GridManager.Instance.CheckMiddleTile(mouseGridPos))
                    {
                        checkImage.enabled = true;
                        checkImage.transform.position = mainCamera.WorldToScreenPoint(new Vector3(mouseGridPos.x + 0.5f, mouseGridPos.y + 0.5f, 0));
                    }
                    else
                    {
                        checkImage.enabled = false;
                    }
                    break;
            }
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

    private void OnAfterSceneLoadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
        checkImage.enabled = false;
        checkImage.gameObject.SetActive(true);
        inTransition = false;
    }
}
