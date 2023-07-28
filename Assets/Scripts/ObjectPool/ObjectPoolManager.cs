using UnityEngine;
using MyGame.WeatherSystem;
public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public ObjectPool<RainDrop> rainDropEffect;
    protected override void Awake()
    {
        base.Awake();
        rainDropEffect.Initialize(transform);
    }
}
