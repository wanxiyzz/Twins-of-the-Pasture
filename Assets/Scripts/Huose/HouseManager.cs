using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.HouseSystem
{
    public class HouseManager : Singleton<HouseManager>
    {
        public Transform houseParent;
        //STORED
        private int playerHouseLevel = 1;

        [SerializeField] GameObject[] playerHouses;

        //STORED
        private Dictionary<string, List<InventoryHouse>> allHouse = new Dictionary<string, List<InventoryHouse>>();
        [SerializeField] List<HouseDetails> houseDetails;
        private List<InventoryHouse> currenSceneHouse;
        //STORED
        public InventoryHouse currentHouse;
        //STORED
        public Vector3 outPosition;
        public Sprite[] boardSprites;
        public int HouseCount
        {
            get
            {
                int num = 0;
                foreach (var item in allHouse.Keys)
                {
                    num += allHouse[item].Count;
                }
                return num;
            }
        }
        private void OnEnable()
        {
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        }
        private void OnDisable()
        {
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
        }
        public void EnterHuose(Vector3 backPosition, InventoryHouse inventoryHouse)
        {
            currentHouse = inventoryHouse;
            outPosition = backPosition;

        }
        private void OnAfterSceneLoadEvent(SceneType sceneType, string sceneName)
        {

            if (sceneName == "01.Field")
            {
                LoadPlayerHouse();
            }
            if (sceneType == SceneType.Field)
            {
                houseParent = GameObject.FindWithTag("houseParent").transform;
                if (allHouse.ContainsKey(sceneName))
                {
                    foreach (var houseDetails in currenSceneHouse)
                    {
                        InitHouse(houseDetails);
                    }
                }
                else
                {
                    allHouse.Add(sceneName, new List<InventoryHouse>());
                }
                currenSceneHouse = allHouse[sceneName];
            }
        }
        public void InitHouse(InventoryHouse houseData)
        {
            for (int i = 0; i < houseDetails.Count; i++)
            {
                if (houseDetails[i].houseType == houseData.houseType)
                {
                    var item = Instantiate(houseDetails[i].prefab);
                    item.transform.SetParent(houseParent);
                    item.GetComponent<HouseOnMap>()?.InitHouse(houseData, boardSprites[(int)houseData.boardType]);
                }
            }
        }
        public void BuildHouse(HouseType houseType, Vector3 pos)
        {
            InventoryHouse item = new InventoryHouse()
            {
                insideSceneName = houseType.ToString() + HouseCount,
                houseType = houseType,
                boardType = BoardType.None,
                position = pos,
                outSceneName = TransitionManager.Instance.currentSceneName,
            };
            InitHouse(item);
            currenSceneHouse.Add(item);
        }
        public void LoadPlayerHouse()
        {
            Instantiate(playerHouses[playerHouseLevel - 1], new Vector3(0, 0, 0), Quaternion.identity, houseParent);
        }
    }
    [System.Serializable]
    public class InventoryHouse
    {
        //
        public string insideSceneName;
        public string outSceneName;
        public SerializableVector2 position;
        public HouseType houseType;
        public BoardType boardType;
    }
}