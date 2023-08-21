using MyGame.Buleprint;
using MyGame.GameTime;
using UnityEngine;
namespace MyGame.HouseSystem
{
    public class HouseOnMap : MonoBehaviour
    {
        [SerializeField] SceneType sceneType;
        [SerializeField] string targetSceneName;
        public Vector3 positionToGo;
        public Vector3 backPosition;
        [SerializeField] Transform backPoint;
        public InventoryHouse houseData;
        [SerializeField] HouseSprite houseSprite;
        [SerializeField] SpriteRenderer boardSpritRenderer;
        public void InitHouse(InventoryHouse house, Sprite boradSprite)
        {
            backPosition = backPoint.transform.position;
            transform.position = house.position.ToVector3();
            houseData = house;
            houseSprite.ChangeImage(TimeManager.Instance.season, TimeManager.Instance.dayShift);
            boardSpritRenderer.sprite = boradSprite;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                EventHandler.CallTransitionEvent(targetSceneName, positionToGo);
                HouseManager.Instance.EnterHuose(backPosition, houseData);
                PlaceableManager.Instance.OnAfterSceneLoadEvent(sceneType, houseData.insideSceneName);
            }
        }


    }
}