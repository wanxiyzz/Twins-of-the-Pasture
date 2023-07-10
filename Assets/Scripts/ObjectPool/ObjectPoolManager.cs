using UnityEngine;
using MyGame.WeatherSystem;
public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public ObjectPool<RainDrop> rainDropEffect;
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        rainDropEffect.Initialize(transform);
    }


}
