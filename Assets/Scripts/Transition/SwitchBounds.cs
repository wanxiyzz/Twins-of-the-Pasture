
using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SwitchConfinerShape;
    }

    private void SwitchConfinerShape(SceneType type, string arg2)
    {
        PolygonCollider2D confineShape = GameObject.FindGameObjectWithTag("BoundsConfine").GetComponent<PolygonCollider2D>();
        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = confineShape;
        confiner.InvalidatePathCache();
    }
}
