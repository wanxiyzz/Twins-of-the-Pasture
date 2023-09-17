using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class SpiteMgr : MonoBehaviour
{
    public Sprite[] sprites;
    public int frameRate = 6;
    public string animalName = "";
    public string[] animName ={
        "RunForWord","RunBack","RunRight","RunLeft","Sleep"
    };
    public string[] idleName ={
        "IdleForword","IdleBack","IdleRight","IdleLeft","IdleSleep"
    };
    public string folderPath;

    [InspectorButton("生成")]
    public void Generate()
    {
        folderPath = "Assets/Animations/Animal/" + animalName;
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh(); // 刷新资源数据库，使新文件夹在Unity中可见
        }
        for (int i = 0; i < animName.Length; i++)
        {
            ToAimation(animName[i], i, 4);
            ToAimation(idleName[i], i, 1);
        }
    }
    public void ToAimation(string animationName, int num, int max)
    {

        // 创建动画剪辑
        AnimationClip animationClip = new AnimationClip();
        animationClip.frameRate = frameRate;

        // 创建关键帧
        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[max];

        for (int i = 0; i < max; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe();
            keyframes[i].time = i * (1.0f / frameRate);
            keyframes[i].value = sprites[num * 4 + i];
        }

        // 创建动画曲线
        AnimationUtility.SetObjectReferenceCurve(animationClip, new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        }, keyframes);

        // 保存动画剪辑
        AssetDatabase.CreateAsset(animationClip, "Assets/Animations/Animal/" + animalName + "/" + animalName + animationName + ".anim");

        string savePath = "Assets/Animations/Animal/" + animalName + "/" + animalName + ".overrideController";

        // 创建一个空的Override Controller
        AnimatorOverrideController overrideController = new AnimatorOverrideController();

        // 将Override Controller保存到指定的路径
        AssetDatabase.CreateAsset(overrideController, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
