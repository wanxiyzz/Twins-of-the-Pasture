using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MyGame.Tile;
using MyGame.GameTime;

namespace MyGame.GrassSystem
{
    public class GrassManager : Singleton<GrassManager>
    {
        //STORED:草丛信息
        private Dictionary<string, List<InventoryGrass>> grassDict = new Dictionary<string, List<InventoryGrass>>();
        private List<InventoryGrass> currentSceneGrasses;
        private Transform grassParent;
        [SerializeField] GameObject grassPrefabs;
        public Sprite[] smallGarssSprite;
        public Sprite[] bigGarssSprite;
        public Tilemap tileMap;
        public string currentSceneName;

        private int spriteNum;
        private void OnEnable()
        {
            EventHandler.DayChange += OnDayChange;
            EventHandler.SeasonChange += OnSeasonChange;
        }
        private void OnDisable()
        {
            EventHandler.DayChange -= OnDayChange;
            EventHandler.SeasonChange -= OnSeasonChange;
        }

        private void OnSeasonChange(Season season)
        {
            spriteNum = (int)season;
        }

        public void OnAfterSceneLoad(SceneType type, string sceneName, Tilemap TopTileMap)
        {
            if (TopTileMap == tileMap) return;
            this.currentSceneName = sceneName;
            tileMap = TopTileMap;
            if (type == SceneType.Field)
            {
                grassParent = GameObject.FindGameObjectWithTag("GrassParent").transform;
                if (grassDict.ContainsKey(sceneName))
                {
                    currentSceneGrasses = grassDict[sceneName];
                    foreach (var grass in currentSceneGrasses)
                    {
                        InitGrass(grass);
                    }
                }
                else
                {
                    grassDict.Add(sceneName, new List<InventoryGrass>());
                    currentSceneGrasses = grassDict[sceneName];
                }
            }
        }
        private void OnDayChange(DayShift shift)
        {
            if (TimeManager.Instance.season == Season.冬) return;
            StartCoroutine(GrassGrouth());
        }
        IEnumerator GrassGrouth()
        {
            foreach (var key in grassDict.Keys)
            {
                for (int i = 0; i < 2; i++)
                {
                    int posX = Random.Range(-29, 36);
                    int posY = Random.Range(-19, 19);

                    bool isbig = Random.Range(0, 10) < 3;
                    var pos = new SerializableVector2Int(posX, posY);
                    InventoryGrass grass = new InventoryGrass()
                    {
                        position = pos,
                        isBig = isbig,
                    };
                    grassDict[key].Add(grass);
                    if (key == currentSceneName)
                    {
                        InitGrass(grass);
                    }
                    yield return new WaitForFixedUpdate();
                }
            }
        }
        private void InitGrass(InventoryGrass grass)
        {
            if (tileMap.GetTile(grass.position.ToVector3Int()) == null)
            {
                if (!TileManager.Instance.ButtomTile(grass.position).haveTop)
                {
                    grassDict[currentSceneName].Remove(grass);
                    return;
                }
                TileManager.Instance.SetTopEmpty(grass.position.ToVector3Int());
                var item = Instantiate(grassPrefabs, grassParent);
                item.transform.position = grass.position.ToVector3() + new Vector3(0.5f, 0, 0);
                item.GetComponent<GrassLogic>().inventoryGrass = grass;
                if (grass.isBig)
                {
                    item.GetComponent<SpriteRenderer>().sprite = bigGarssSprite[spriteNum];
                }
                else
                {
                    item.GetComponent<SpriteRenderer>().sprite = smallGarssSprite[spriteNum];
                }
            }
            else
            {
                grassDict[currentSceneName].Remove(grass);
            }

        }
        public void RemoveGrass(InventoryGrass inventoryGrass)
        {
            currentSceneGrasses.Remove(inventoryGrass);
        }
    }
}