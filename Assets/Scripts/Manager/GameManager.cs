using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public bool isWin;
    public int screenHeight;
    public float toolTipHeight;
    protected override void Awake()
    {
        base.Awake();
        isWin = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
        screenHeight = Screen.height;
        toolTipHeight = screenHeight * 0.138889f;
    }
}
