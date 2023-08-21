using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using MyGame.Tile;
using MyGame.Buleprint;
using MyGame.PlantSystem;
namespace MyGame.Cursor
{
    //可以细化为每种物体一个检测函数   根据sceneType写一些事件  加上前面需要检测的相应的的函数  场景切换时  CurrentSceneAction切换为相应的事件再进行每帧检测
    public class CursorManager : Singleton<CursorManager>
    {

        private event UnityAction UpdateEvent;

        private Camera mainCamera;
        private ItemDetails currentSelectItem;
        public ToolType currentTool;
        public Grid currentGrid;
        private TileDetails currentTile;

        public Vector3 mouseWorldPos;
        public Vector3Int mouseGridPos;
        public Vector3 screenPosCenter;

        private SceneType currentSceneType;

        public GameObject checkImage;
        public bool cursorEnable;
        private bool cursorItemValid;
        private bool cursorToolValid;
        public Vector3 usePosition;

        //建造
        [SerializeField] SpriteRenderer placeableSprite;
        private PlaceableDetails currentBuleprint;
        private Vector3 buildPos;
        private bool isBuleprint;



        //吃
        public bool canEat;


        //收获
        [SerializeField] LayerMask reapCheckLayer;
        private Collider2D checkRay;
        private IHavested currentHavestedItem;

        //铲子
        private bool shovelPlant;
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
            if (!InteractWithUI() && cursorEnable)
            {
                CheckCursorValid();
                if (Input.GetMouseButtonDown(0))
                {
                    if (isBuleprint && cursorItemValid)
                    {
                        PlaceableManager.Instance.BuildPlace(currentBuleprint, buildPos);
                        return;
                    }
                    if (cursorToolValid)
                    {
                        if (currentTool == ToolType.Reap)
                        {
                            GameManager.Instance.player.MoveToPos(false, mouseWorldPos, () =>
                         currentHavestedItem.Harvested());
                            return;
                        }
                        else if (currentTool == ToolType.Shovel)
                        {
                            if (shovelPlant)
                            {
                                GameManager.Instance.player.MoveToPos(false, currentHavestedItem.Position, () => currentHavestedItem.Shoveled());
                            }
                            else
                            {
                                usePosition = mouseGridPos;
                                var useGridPos = mouseGridPos;
                                GameManager.Instance.player.MoveToPos(false, usePosition, () => TileManager.Instance.ShovelOff(useGridPos));
                            }
                            return;
                        }

                        usePosition = mouseWorldPos;
                        GameManager.Instance.player.MoveToPos(false, mouseWorldPos, () =>
                         EventHandler.CallUseTool(currentTool, usePosition));
                        GameManager.Instance.playerCheck.takeSmallBox = false;
                        return;
                    }
                    if (cursorItemValid)
                    {
                        usePosition = mouseWorldPos;
                        GameManager.Instance.player.MoveToPos(true, mouseWorldPos, () =>
                         EventHandler.CallUseItemEvent(usePosition, currentSelectItem));
                    }

                }
            }
            if (canEat)
            {
                // if(Input.GetKeyDown(KeyCode.E))
                // {
                //吃东西事件
                // }
            }
        }

        private void OnSelectItemEvent(ItemDetails details, bool isSelected)
        {
            //TODO:鼠标变样子
            if (isSelected)
            {
                currentSelectItem = details;
                if (details.itemType.HaveTheType(ItemType.Food)) canEat = true;
                else canEat = false;
                UpdateCurrenEvent();
                if (!details.itemType.HaveTheType(ItemType.Buleprint))
                {
                    placeableSprite.enabled = false;
                }
            }
            else
            {
                placeableSprite.enabled = false;
                currentSelectItem = null;
                isBuleprint = false;
                UpdateEvent = null;
                canEat = false;
                cursorToolValid = false;
                cursorItemValid = false;
            }
        }

        private void CheckCursorValid()
        {
            mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
            if (currentGrid == null) return;
            mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            currentTile = TileManager.Instance.ButtomTile(mouseGridPos);
            screenPosCenter = mainCamera.WorldToScreenPoint(new Vector3(mouseGridPos.x + 0.5f, mouseGridPos.y + 0.5f, 0));

            //先检测工具的鼠标状态  后检测已选择物品的
            //以下为要检测的工具
            if (currentTool == ToolType.Hoe)
            {
                if (CheckHoe(currentTile, screenPosCenter)) { SetToolValid(); return; }
                else SetToolInValid();
            }
            else if (currentTool == ToolType.HoldItem)
            {
                if (CheckHoldItem(mouseWorldPos, screenPosCenter)) { SetToolValid(); return; }
                else SetToolInValid();
            }
            else if (currentTool == ToolType.Reap)
            {
                if (CheckReap(mouseWorldPos)) { SetToolValid(); return; }
                else SetToolInValid();
            }
            else if (currentTool == ToolType.Shovel)
            {
                if (CheckShovel(currentTile, screenPosCenter)) { SetToolValid(); return; }
                else SetToolInValid();
            }
            else SetToolInValid();
            if (currentSelectItem == null) return;
            UpdateEvent?.Invoke();
        }
        /// <summary>
        /// 检测植物是否可以收割
        /// </summary>
        /// <returns></returns>
        public bool CheckReap(Vector3 pos)
        {
            checkRay = Physics2D.OverlapCircle(pos, 0.7f, reapCheckLayer);
            if (checkRay != null)
            {
                if (checkRay.TryGetComponent(out currentHavestedItem))
                {
                    if (currentHavestedItem != null)
                    {
                        if (currentHavestedItem.CanHarvest)
                        {
                            checkImage.SetActive(true);
                            checkImage.transform.position = mainCamera.WorldToScreenPoint(Tools.LocalToCell(currentHavestedItem.Position) + new Vector3(0.5f, 0.5f, 0));
                            return true;
                        }
                    }
                }
            }
            checkImage.SetActive(false);
            return false;
        }

        public bool CheckHoldItem(Vector3 mouseWorldPos, Vector3 screenPosCenter)
        {
            if (TileManager.Instance.CheckCanBuild(mouseWorldPos))
            {
                checkImage.SetActive(true);
                checkImage.transform.position = screenPosCenter;
                return true;
            }
            else
            {
                checkImage.SetActive(false);
                return false;
            }
        }

        public bool CheckHoe(TileDetails tile, Vector3 screenPosCenter)
        {
            if (tile != null)
            {
                if (!tile.haveTop && !tile.canPlant && tile.seedID < 0)
                {
                    checkImage.SetActive(true);
                    checkImage.transform.position = screenPosCenter;
                    return true;
                }
            }
            checkImage.SetActive(false);
            return false;
        }

        public bool CheckShovel(TileDetails currentTile, Vector3 posCenter)
        {
            checkRay = Physics2D.OverlapCircle(mouseWorldPos, 0.3f, reapCheckLayer);
            if (checkRay != null)
            {
                if (checkRay.TryGetComponent(out currentHavestedItem))
                {
                    checkImage.SetActive(true);
                    checkImage.transform.position = mainCamera.WorldToScreenPoint(Tools.LocalToCell(currentHavestedItem.Position) + new Vector3(0.5f, 0.5f, 0));
                    shovelPlant = true;
                    return true;
                }
            }
            if (currentTile == null)
            {
                checkImage.SetActive(false);
                return false;
            }
            if (currentTile.haveTop)
            {
                {
                    checkImage.SetActive(true);
                    checkImage.transform.position = posCenter;
                    shovelPlant = false;
                    return true;
                }
            }
            checkImage.SetActive(false);
            return false;
        }


        private void OnBeforeSceneLoadEvent()
        {
            placeableSprite.enabled = false;
            currentSelectItem = null;
            isBuleprint = false;
        }

        private void OnAfterSceneLoadEvent(SceneType sceneType, string sceneName)
        {
            currentGrid = FindObjectOfType<Grid>();
            checkImage.SetActive(false);
            currentSceneType = sceneType;
            cursorEnable = true;
            UpdateCurrenEvent();
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
            return !tile.haveTop && tile.seedID < 1 && tile.canPlant;
        }
        private void OnPickUpTool(ToolType type)
        {
            currentTool = type;
            checkImage.SetActive(false);
        }
        /// <summary>
        /// 选中物体和更改场景
        /// </summary>
        private void UpdateCurrenEvent()
        {
            UpdateEvent = null;
            placeableSprite.enabled = false;
            isBuleprint = false;
            if (currentSelectItem == null) return;
            if (currentSceneType == SceneType.PlantHuose)
            {
                if (currentSelectItem.itemType.HaveTheType(ItemType.Seed))
                {
                    UpdateEvent += SeedCheck;
                }
                else if (currentSelectItem.itemType.HaveTheType(ItemType.Buleprint))
                {
                    SelectBulePrint();
                }
                else if (currentSelectItem.itemType.HaveTheType(ItemType.Floor))
                {
                    UpdateEvent += FloorCheck;
                }
            }
            else if (currentSceneType == SceneType.AnimalHuose)
            {
                if (currentSelectItem.itemType.HaveTheType(ItemType.Buleprint))
                {
                    SelectBulePrint();
                }
            }
            else if (currentSceneType == SceneType.Field)
            {
                if (currentSelectItem.itemType.HaveTheType(ItemType.Seed))
                {
                    UpdateEvent += SeedCheck;
                }
                else if (currentSelectItem.itemType.HaveTheType(ItemType.Buleprint))
                {
                    SelectBulePrint();
                }
                else if (currentSelectItem.itemType.HaveTheType(ItemType.Lawn))
                {
                    UpdateEvent += FloorCheck;
                }
            }
            else if (currentSceneType == SceneType.PeopleHome)
            {

            }
            else if (currentSceneType == SceneType.MyHuose)
            {
                if (currentSelectItem.itemType.HaveTheType(ItemType.Buleprint))
                {
                    SelectBulePrint();
                }
            }
        }

        private void FloorCheck()
        {
            if (!currentTile.haveTop && currentTile.seedID <= 0)
            {
                checkImage.SetActive(true);
                checkImage.transform.position = mainCamera.WorldToScreenPoint(new Vector3(mouseGridPos.x + 0.5f, mouseGridPos.y + 0.5f, 0));
                SetItemValid();
                return;
            }
            checkImage.SetActive(false);
            SetItemInValid();
        }

        /// <summary>
        /// 选中的物体是蓝图的
        /// </summary>
        private void SelectBulePrint()
        {
            UpdateEvent += BuleprintCheck;
            isBuleprint = true;
            currentBuleprint = PlaceableManager.Instance.FindPlaceableDetails(currentSelectItem.itemID);
            placeableSprite.enabled = true;
            placeableSprite.sprite = currentBuleprint.prefab.GetComponent<SpriteRenderer>().sprite;
        }
        /// <summary>
        /// 种子相应的检测
        /// </summary>
        public void SeedCheck()
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
        /// <summary>
        /// 蓝图相应的检测
        /// </summary>
        public void BuleprintCheck()
        {
            if (currentBuleprint == null) return;
            if (currentBuleprint.evenCell)
            {
                mouseWorldPos += new Vector3(0.5f, 0, 0);
                buildPos = Tools.LocalToCell(mouseGridPos);
                placeableSprite.transform.position = buildPos;
                var currentbuildPos = buildPos - new Vector3(currentBuleprint.aboutCell, 0, 0);
                if (!TileManager.Instance.CheckCanBuild(currentbuildPos))
                {
                    placeableSprite.color = new Color(1, 0, 0, 0.5f);
                    SetItemInValid();
                    return;
                }
                for (int j = 0; j < currentBuleprint.aboveCell; j++)
                {
                    var topPos = currentbuildPos + new Vector3(0, 1, 0);
                    if (TileManager.Instance.CheckCanBuild(topPos))
                    {
                        continue;
                    }
                    else
                    {
                        placeableSprite.color = new Color(1, 0, 0, 0.5f);
                        SetItemInValid();
                        return;
                    }
                }
                for (int i = 0; i < 2 * currentBuleprint.aboutCell - 1; i++)
                {
                    currentbuildPos += new Vector3(1, 0, 0);
                    if (TileManager.Instance.CheckCanBuild(currentbuildPos))
                    {
                        TileManager.Instance.CheckCanBuild(currentbuildPos);
                        for (int j = 0; j < currentBuleprint.aboveCell; j++)
                        {
                            var topPos = currentbuildPos + new Vector3(0, 1, 0);
                            if (TileManager.Instance.CheckCanBuild(topPos))
                            {
                                continue;
                            }
                            else
                            {
                                SetBulePrintInValid();
                                return;
                            }
                        }
                    }
                    else
                    {
                        SetBulePrintInValid();
                        return;
                    }
                }
                SetBulePrintValid();
            }
            else
            {
                buildPos = mouseGridPos + new Vector3(0.5f, 0, 0);
                placeableSprite.transform.position = buildPos;
                if (!TileManager.Instance.CheckCanBuild(mouseWorldPos))
                {
                    placeableSprite.color = new Color(1, 0, 0, 0.5f);
                    SetItemInValid();
                    return;
                }
                for (int j = 0; j < currentBuleprint.aboveCell; j++)
                {
                    var topPos = mouseGridPos + new Vector3(0, 1, 0);
                    if (TileManager.Instance.CheckCanBuild(topPos))
                    {
                        continue;
                    }
                    else
                    {
                        placeableSprite.color = new Color(1, 0, 0, 0.5f);
                        SetItemInValid();
                        return;
                    }
                }
                for (int i = 1; i < currentBuleprint.aboutCell + 1; i++)
                {
                    var currentbuildPos = mouseGridPos + new Vector3Int(0, i, 0);
                    for (int j = 0; j < currentBuleprint.aboveCell; j++)
                    {
                        var topPos = currentbuildPos + new Vector3(0, 1, 0);
                        if (TileManager.Instance.CheckCanBuild(topPos))
                        {
                            continue;
                        }
                        else
                        {
                            placeableSprite.color = new Color(1, 0, 0, 0.5f);
                            SetItemInValid();
                            return;
                        }
                    }
                    if (!TileManager.Instance.CheckCanBuild(currentbuildPos))
                    {
                        SetBulePrintInValid();
                        return;
                    }
                    currentbuildPos = mouseGridPos - new Vector3Int(0, i, 0);
                    for (int j = 0; j < currentBuleprint.aboveCell; j++)
                    {
                        var topPos = currentbuildPos + new Vector3(0, 1, 0);
                        if (TileManager.Instance.CheckCanBuild(topPos))
                        {
                            continue;
                        }
                        else
                        {
                            SetBulePrintInValid();
                            return;
                        }
                    }
                    if (!TileManager.Instance.CheckCanBuild(currentbuildPos))
                    {
                        SetBulePrintInValid();
                        return;
                    }
                }
                SetBulePrintValid();
            }
        }
        private void SetBulePrintInValid()
        {
            placeableSprite.color = new Color(1, 0, 0, 0.5f);

            SetItemInValid();
        }
        private void SetBulePrintValid()
        {
            placeableSprite.color = new Color(1, 1, 1, 0.5f);
            SetItemValid();
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
    }
}
