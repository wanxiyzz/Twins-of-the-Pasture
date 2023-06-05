using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : Singleton<PlantManager>
{
    Transform plantParent;
    public PlantDataList plantDataList;
    public Dictionary<string, List<PlantState>> plantData = new Dictionary<string, List<PlantState>>();
    Dictionary<string, List<PlantState>> willMatureplantData = new Dictionary<string, List<PlantState>>();
    public List<Plant> currentScenePlant = new List<Plant>();
    public GameObject plantPrafab;
    private void OnEnable()
    {
        EventHandler.hourUpdate += OnHourUpdate;
        EventHandler.beforeSceneLoadEvent += OnBeforeSceneLoadEvent;
        EventHandler.afterSceneLoadEvent += OnAfterSceneLoadEvent;
        PlantState lj = new PlantState()
        {
            seedID = 1030,
            position = new SerializableVector3(4.5f, 0.5f, 0)
        };
        List<PlantState> ab = new List<PlantState>();
        ab.Add(lj);
        plantData.Add("01.Field", ab);
    }
    private void OnDisable()
    {
        EventHandler.hourUpdate -= OnHourUpdate;
        EventHandler.beforeSceneLoadEvent -= OnBeforeSceneLoadEvent;
        EventHandler.afterSceneLoadEvent -= OnAfterSceneLoadEvent;
    }
    public PlantDetails FindPlantDetails(int seedID)
    {
        foreach (var item in plantDataList.plantDataList)
        {
            if (item.seedId == seedID)
            {
                return item;
            }
        }
        return null;
    }
    private void OnBeforeSceneLoadEvent()
    {
        currentScenePlant.Clear();
    }
    private void OnAfterSceneLoadEvent()
    {

        plantParent = GameObject.FindGameObjectWithTag("PlantParent").transform;
        if (plantData.ContainsKey(TransitionManager.Instance.currentSceneName))
        {
            List<PlantState> currentPlants = plantData[TransitionManager.Instance.currentSceneName];
            foreach (var item in currentPlants)
            {
                var plant = Instantiate(plantPrafab, plantParent);
                Plant itemPlant;
                if (plant.TryGetComponent<Plant>(out itemPlant))
                {
                    itemPlant.Init(item, FindPlantDetails(item.seedID));
                    itemPlant.UpdateSprite();
                    currentScenePlant.Add(itemPlant);
                }
            }
        }
        else plantData.Add(TransitionManager.Instance.currentSceneName, new List<PlantState>());

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
    public void InitOnePlant(Vector3 pos, int seedID)
    {
        PlantState item = new PlantState();
        item.Init(pos, seedID);
        var plant = Instantiate(plantPrafab, plantParent);
        Plant itemPlant;
        if (plant.TryGetComponent<Plant>(out itemPlant))
        {
            itemPlant.Init(item, FindPlantDetails(item.seedID));
            itemPlant.UpdateSprite();
            currentScenePlant.Add(itemPlant);
        }
        // if (plantData.ContainsKey(TransitionManager.Instance.currentSceneName))
        plantData[TransitionManager.Instance.currentSceneName].Add(item);
        // else
        // {
        //     List<PlantState> plantStates = new List<PlantState>();
        //     plantStates.Add(item);
        //     plantData.Add(TransitionManager.Instance.currentSceneName, plantStates);
        // }
    }
}
