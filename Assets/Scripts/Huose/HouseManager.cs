using System.Collections.Generic;
using System.Collections;
using MyGame.Buleprint;
using UnityEngine;
using MyGame.Tile;

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
        [SerializeField] List<GameObject> houseDetails;
        private List<InventoryHouse> currenSceneHouse;
        //STORED
        private string outSceneName;
        //STORED
        private Vector3 outPosition;
        public Sprite[] boardSprites;
        public string currentSceneName;
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
        public void EnterHouse(SceneType sceneType, Vector3 backPosition, InventoryHouse inventoryHouse, string targetSceneName, Vector3 positionToGo)
        {
            outSceneName = inventoryHouse.outSceneName;
            outPosition = backPosition;
            StartCoroutine(IEEnterHouse(sceneType, targetSceneName, positionToGo, inventoryHouse.insideSceneName));
        }
        IEnumerator IEEnterHouse(SceneType sceneType, string targetSceneName, Vector3 positionToGo, string insideSceneName)
        {
            EventHandler.CallTransitionEvent(targetSceneName, positionToGo);
            yield return new WaitForSeconds(0.7f);
            PlaceableManager.Instance.OnAfterSceneLoadEvent(SceneType.MyHuose, insideSceneName);
            if (sceneType == SceneType.AnimalHuose)
                Animal.AnimalManager.Instance.LoadSceneAnimal(insideSceneName);
        }
        public void OutHuose()
        {
            EventHandler.CallTransitionEvent(outSceneName, outPosition);
        }
        private void OnAfterSceneLoadEvent(SceneType sceneType, string sceneName)
        {
            currentSceneName = sceneName;
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
            if (houseData.houseType == HouseType.BigAnimalHuose)
            {
                var item = PlaceableManager.Instance.FindPlaceableDetails(2004);
                TileManager.Instance.BuildPlace(item, houseData.position.ToVector3());
                var obj = Instantiate(houseDetails[0]);
                obj.transform.SetParent(houseParent);
                obj.GetComponent<HouseOnMap>()?.InitHouse(houseData, boardSprites[(int)houseData.boardType]);
            }
            else if (houseData.houseType == HouseType.SmallAnimalHuose)
            {
                var item = PlaceableManager.Instance.FindPlaceableDetails(2005);
                TileManager.Instance.BuildPlace(item, houseData.position.ToVector3());
                var obj = Instantiate(houseDetails[1]);
                obj.transform.SetParent(houseParent);
                obj.GetComponent<HouseOnMap>()?.InitHouse(houseData, boardSprites[(int)houseData.boardType]);
            }
            else
            {
                var item = PlaceableManager.Instance.FindPlaceableDetails(2006);
                TileManager.Instance.BuildPlace(item, houseData.position.ToVector3());
                var obj = Instantiate(houseDetails[2]);
                obj.transform.SetParent(houseParent);
                obj.GetComponent<HouseOnMap>()?.InitHouse(houseData, boardSprites[(int)houseData.boardType]);
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
                outSceneName = currentSceneName,
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
        public string outSceneName;
        public string insideSceneName;
        public SerializableVector2 position;
        public HouseType houseType;
        public BoardType boardType;
    }
}