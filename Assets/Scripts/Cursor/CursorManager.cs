using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MyGame.Tile;
namespace MyGame.Cursor
{
    public class CursorManager : Singleton<CursorManager>
    {
        private Camera mainCamera;
        private ItemDetails currentSelectItem;
        public ToolType currentTool;
        public Grid currentGrid;
        private TileDetails currentTile;

        public Vector3 mouseWorldPos;
        public Vector3Int mouseGridPos;



        public GameObject checkImage;
        public bool cursorEnable;
        private bool cursorItemValid;
        private bool cursorToolValid;
        public Vector3 usePosition;
        private void OnEnable()
        {
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
            EventHandler.BeforeSceneLoadEvent += OnBeforeSceneLoadEvent;
            EventHandler.SelectItemEvent += OnSelectItemEvent;
            EventHandler.PickUpTool += OnPickUpTool;
        }
        private void OnDisable()
        {
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
            EventHandler.BeforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
            EventHandler.SelectItemEvent -= OnSelectItemEvent;
            EventHandler.PickUpTool -= OnPickUpTool;
        }



        private void Start()
        {
            mainCamera = Camera.main;
        }
        private void Update()
        {
            // if (!GameManager.Instance.isWin)
            //     return;
            if (!InteractWithUI() || cursorEnable)
            {
                CheckCursorValid();
                if (Input.GetMouseButtonDown(0))
                {
                    usePosition = mouseWorldPos;
                    if (cursorItemValid)
                    {
                        GameManager.Instance.player.MoveToPos(true, mouseWorldPos, () =>
                        EventHandler.CallUseItemEvent(usePosition, currentSelectItem));
                        Debug.Log("使用物品");
                    }
                    if (cursorToolValid)
                    {
                        GameManager.Instance.player.MoveToPos(false, mouseWorldPos, () =>
                        EventHandler.CallUseTool(currentTool, usePosition));
                        Debug.Log("使用工具");
                    }
                }
            }
        }

        private void OnSelectItemEvent(ItemDetails details, bool isSelected)
        {
            //TODO:鼠标变样子
            if (isSelected)
            {
                currentSelectItem = details;
            }
            else
            {
                currentSelectItem = null;
            }
        }

        private void CheckCursorValid()
        {
            mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
            if (currentGrid == null) return;
            mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            currentTile = TileManager.Instance.ButtomTile(mouseGridPos);
            //先检测工具的鼠标状态  后检测已选择物品的
            if (currentTool == ToolType.Hoe)
            {
                if (currentTile != null)
                {
                    if (!currentTile.haveTop && !currentTile.canPlant && currentTile.seedID < 0)
                    {
                        SetToolValid();
                        checkImage.SetActive(true);
                        checkImage.transform.position = mainCamera.WorldToScreenPoint(new Vector3(mouseGridPos.x + 0.5f, mouseGridPos.y + 0.5f, 0));
                        return;
                    }
                    else
                    {
                        SetToolInValid();
                        checkImage.SetActive(false);
                    }
                }
            }
            CheckItemCursor();

        }
        /// <summary>
        /// 物品的选中之后的交互
        /// </summary>
        private void CheckItemCursor()
        {
            if (currentSelectItem == null) return;
            if (TransitionManager.Instance.sceneType == SceneType.Field || TransitionManager.Instance.sceneType == SceneType.PlantHuose)
            {
                if (currentSelectItem.itemType.HaveTheType(ItemType.Seed))
                {
                    if (CheckCanPlant(currentTile))
                    {
                        checkImage.SetActive(true);
                        checkImage.transform.position = mainCamera.WorldToScreenPoint(new Vector3(mouseGridPos.x + 0.5f, mouseGridPos.y + 0.5f, 0));
                        SetItemValid();
                        return;
                    }
                    else
                    {
                        checkImage.SetActive(false);
                        SetItemInValid();
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
            cursorEnable = false;
        }

        private void OnAfterSceneLoadEvent(SceneType sceneType, string sceneName)
        {
            currentGrid = FindObjectOfType<Grid>();
            checkImage.SetActive(false);
            cursorEnable = true;
        }


        /// <summary>
        /// 设置鼠标可用 更改鼠标图片
        /// </summary>
        private void SetItemValid()
        {
            cursorItemValid = true;
        }
        private void SetToolValid()
        {
            cursorToolValid = true;
            cursorItemValid = false;
        }
        /// <summary>
        /// 设置鼠标不可用 变回鼠标图片
        /// </summary>
        private void SetItemInValid()
        {
            cursorItemValid = false;
        }
        private void SetToolInValid()
        {
            cursorToolValid = false;
        }


        public bool CheckCanPlant(TileDetails tile)
        {
            if (tile == null)
                return false;
            return !tile.haveTop && tile.seedID < 1;
        }


        private void OnPickUpTool(ToolType type)
        {
            currentTool = type;
        }
    }
}
