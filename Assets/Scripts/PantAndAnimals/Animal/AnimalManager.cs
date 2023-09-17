using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.Animal
{
    public class AnimalManager : Singleton<AnimalManager>
    {
        [SerializeField] AnimalDataList aniamalDataList;
        //STORED 动物信息
        private Dictionary<string, List<InventoryAnimal>> animalData = new Dictionary<string, List<InventoryAnimal>>();
        private List<InventoryAnimal> currentSceneAnimalState;

        private List<AnimalOnMap> currenSceneAnimals = new List<AnimalOnMap>();
        private Vector2 leftButtom = new Vector2(-4, 0);
        private Vector2 rightTop = new Vector2(8, 6);
        private Transform animalParent;

        //STORED:房子粮草数量
        private Dictionary<string, AnimalHogwash> animalHouseHogwash = new Dictionary<string, AnimalHogwash>();
        private AnimalHogwash currentHogwash;
        //STORED
        private Dictionary<string, List<InventoryEgg>> AllEggs = new Dictionary<string, List<InventoryEgg>>();
        private EggInMap eggManager;

        public int eggGrowthTime = 360;

        private void OnEnable()
        {
            EventHandler.HourUpdate += OnHourUpdate;
            EventHandler.DayChange += OnDayChange;
        }
        private void OnDisable()
        {
            EventHandler.HourUpdate -= OnHourUpdate;
            EventHandler.DayChange -= OnDayChange;
        }

        private void Update()
        {
            //TEST
            if (Input.GetKeyDown(KeyCode.C))
            {
                AnimalManager.Instance.CreatAnimal(AnimalType.Bunny);
            }
        }
        public void LoadSceneAnimal(string sceneName)
        {
            animalParent = GameObject.FindGameObjectWithTag("AnimalParent").transform;

            if (sceneName.Contains("Small"))
            {
                if (AllEggs.ContainsKey(sceneName))
                {
                    eggManager = animalParent.GetComponent<EggInMap>();
                    eggManager.LoadEggNumber(AllEggs[sceneName].Count);
                }
                else
                {
                    AllEggs.Add(sceneName, new List<InventoryEgg>());
                }
            }
            if (animalData.ContainsKey(sceneName))
            {
                currentSceneAnimalState = animalData[sceneName];
            }
            else
            {
                currentSceneAnimalState = new List<InventoryAnimal>();
                animalData.Add(sceneName, currentSceneAnimalState);
            }
            if (animalHouseHogwash.ContainsKey(sceneName))
            {
                currentHogwash = animalHouseHogwash[sceneName];
            }
            else
            {
                currentHogwash = new AnimalHogwash();
                animalHouseHogwash.Add(sceneName, currentHogwash);
            }
            currenSceneAnimals.Clear();
            foreach (var item in currentSceneAnimalState)
            {
                InitAnimal(item);
            }
        }
        public AnimalDetails FindAnimalDetails(AnimalType type) =>
        aniamalDataList.animalDetailsList.Find(a => a.type == type);

        public void InitAnimal(InventoryAnimal animal)
        {
            Vector2 pos = new Vector2(Random.Range(leftButtom.x, rightTop.x), Random.Range(leftButtom.y, rightTop.y));
            AnimalDetails details = FindAnimalDetails(animal.type);
            var item = Instantiate(details.prafab, animalParent);
            item.transform.position = pos;
            item.GetComponent<AnimalOnMap>().Init(animal, details);
        }
        public void InitAnimal(InventoryAnimal animal, AnimalDetails details)
        {
            Vector2 pos = new Vector2(Random.Range(leftButtom.x, rightTop.x), Random.Range(leftButtom.y, rightTop.y));
            var item = Instantiate(details.prafab, animalParent);
            var ani = item.GetComponent<AnimalOnMap>();
            item.transform.position = pos;
            ani.Init(animal, details);
            currenSceneAnimals.Add(ani);
        }
        public void CreatAnimal(AnimalType type)
        {
            AnimalDetails details = FindAnimalDetails(type);
            InventoryAnimal bornAnimal = new InventoryAnimal
            {
                isBaby = true,
                hungerValue = 50,
                thirstValue = 50,
                appetite = details.appetite,
                babyAppetite = details.babyAppetite,
                ripiTime = details.ripeTime,
            };
            InitAnimal(bornAnimal, details);
            currentSceneAnimalState.Add(bornAnimal);
        }
        private void OnDayChange(DayShift shift)
        {
            foreach (var item in currenSceneAnimals)
            {
                item.UpdateAnimation(shift);
            }
        }
        private void OnHourUpdate()
        {
            StartCoroutine(IEHourUpdate());
        }
        IEnumerator IEHourUpdate()
        {
            int needFodder;
            foreach (var key in animalData.Keys)
            {
                foreach (var animal in animalData[key])
                {
                    if (!animal.isDeath)
                    {
                        animal.grouthTime += 1;
                        animal.havestTime += 1;
                        animal.isBaby = animal.grouthTime < animal.ripiTime;
                        animal.hungerValue -= 10;
                        needFodder = animal.isBaby ? animal.babyAppetite : animal.appetite;
                        if (animalHouseHogwash[key].food > 0)
                        {
                            if (animalHouseHogwash[key].food > (100 - animal.hungerValue) * needFodder)
                            {
                                animalHouseHogwash[key].food -= (100 - animal.hungerValue) * needFodder;
                                animal.hungerValue = 100;
                            }
                            else
                            {
                                animal.hungerValue += animalHouseHogwash[key].food / needFodder;
                                animalHouseHogwash[key].food = 0;
                            }
                        }
                        if (animalHouseHogwash[key].water > 0)
                        {
                            if (animalHouseHogwash[key].water > (100 - animal.hungerValue) * needFodder)
                            {
                                animalHouseHogwash[key].water -= (100 - animal.hungerValue) * needFodder;
                                animal.hungerValue = 100;
                            }
                            else
                            {
                                animal.hungerValue += animalHouseHogwash[key].water / needFodder;
                                animalHouseHogwash[key].water = 0;
                            }
                        }
                        animal.isDeath = animal.hungerValue <= 0 || animal.thirstValue <= 0;
                    }
                    yield return new WaitForFixedUpdate();
                }
                yield return UpdateAnimalOnMap();
            }
        }
        IEnumerator UpdateAnimalOnMap()
        {
            foreach (var item in currenSceneAnimals)
            {
                item.UpdateAnimator();
                yield return new WaitForFixedUpdate();
            }
        }
    }
    public class AnimalHogwash
    {
        public int food;
        public int water;
    }
}