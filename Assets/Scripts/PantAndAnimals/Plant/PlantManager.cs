using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Tile;
namespace MyGame.Plant
{
    public class PlantManager : Singleton<PlantManager>
    {
        Transform plantParent;
        //所有的种子相对的植物信息
        public PlantDataList plantDataList;
        public string currentSceneName;
        //STORED:所有植物
        public Dictionary<string, List<PlantState>> plantData = new Dictionary<string, List<PlantState>>();

        //STORED:将要长成的植物
        Dictionary<string, List<PlantState>> willMatureplantData = new Dictionary<string, List<PlantState>>();

        public List<PlantState> currentScenePlantState = new List<PlantState>();
        public List<Plant> currentScenePlant = new List<Plant>();
        public GameObject plantPrafab;
        private void OnEnable()
        {
            EventHandler.HourUpdate += OnHourUpdate;
            EventHandler.BeforeSceneLoadEvent += OnBeforeSceneLoadEvent;
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
            EventHandler.PlantAPlant += OnPlantAPlant;
        }
        private void OnDisable()
        {
            EventHandler.HourUpdate -= OnHourUpdate;
            EventHandler.BeforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
            EventHandler.PlantAPlant -= OnPlantAPlant;
        }

        private void OnPlantAPlant(Vector3Int pos, int ID)
        {
            PlantState plant = new PlantState()
            {
                seedID = ID,
                position = new SerializableVector3(pos.x + 0.45f, pos.y + 0.45f, 0)
            };
            plantData[currentSceneName].Add(plant);
            InitOnePlant(plant);
        }
        /// <summary>
        /// 根据ID返回植物信息
        /// </summary>
        /// <param name="seedID"></param>
        /// <returns></returns>
        public PlantDetails FindPlantDetails(int seedID)
        {
            return plantDataList.plantDataList.Find(a => a.seedId == seedID);
        }
        private void OnBeforeSceneLoadEvent()
        {
            currentScenePlant.Clear();
        }
        private void OnAfterSceneLoadEvent(SceneType sceneType, string sceneName)
        {
            this.currentSceneName = sceneName;
            plantParent = GameObject.FindGameObjectWithTag("PlantParent").transform;
            if (plantData.ContainsKey(sceneName))
            {
                currentScenePlantState = plantData[sceneName];
                foreach (var item in currentScenePlantState)
                {
                    InitOnePlant(item);
                }
            }
            else plantData.Add(currentSceneName, new List<PlantState>());

        }
        private void OnHourUpdate()
        {
            StartCoroutine(UpdateAllPlants());
        }
        private IEnumerator UpdateAllPlants()
        {
            foreach (var key in plantData.Keys)
            {
                foreach (var item in plantData[key])
                {
                    item.grouthHuornum++;
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
            foreach (var item in currentScenePlant)
            {
                item.UpdateSprite();
            }
        }
        public void InitOnePlant(PlantState item)
        {
            var plant = Instantiate(plantPrafab, plantParent);
            Plant itemPlant;
            GridManager.Instance.DigPostion(Tools.LocalToCell(item.position));
            if (plant.TryGetComponent<Plant>(out itemPlant))
            {
                itemPlant.Init(item, FindPlantDetails(item.seedID));
                itemPlant.UpdateSprite();
                currentScenePlant.Add(itemPlant);
            }
        }
    }
}