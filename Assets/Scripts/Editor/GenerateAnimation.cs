using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class SpriteToAnimation : EditorWindow
{
    private Sprite[] sprites = new Sprite[4];
    private int frameRate = 6; // 帧速率，默认为12帧每秒
    private string animationName = "";

    [MenuItem("Window/Sprite to Animation")]
    public static void ShowWindow()
    {
        GetWindow<SpriteToAnimation>("Sprite to Animation");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select 4 Sprites:");
        for (int i = 0; i < 4; i++)
        {
            sprites[i] = (Sprite)EditorGUILayout.ObjectField("Sprite " + (i + 1), sprites[i], typeof(Sprite), false);
        }

        frameRate = EditorGUILayout.IntField("Frame Rate", frameRate);

        animationName = EditorGUILayout.TextField("Name", animationName);
        if (GUILayout.Button("Create Animation"))
        {
            CreateAnimation();
        }
    }

    public void CreateAnimation()
    {

        var a = GameObject.Find("Sprite").GetComponent<SpiteMgr>();
        for (int i = 0; i < 4; i++)
        {
            sprites[i] = a.sprites[i];
        }
        // 创建动画剪辑
        AnimationClip animationClip = new AnimationClip();
        animationClip.frameRate = frameRate;

        // 创建关键帧
        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[4];

        for (int i = 0; i < 4; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe();
            keyframes[i].time = i * (1.0f / frameRate);
            keyframes[i].value = sprites[i];
        }

        // 创建动画曲线
        AnimationUtility.SetObjectReferenceCurve(animationClip, new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        }, keyframes);

        // 保存动画剪辑
        AssetDatabase.CreateAsset(animationClip, "Assets/" + animationName + ".anim");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Animation created!");
    }
}