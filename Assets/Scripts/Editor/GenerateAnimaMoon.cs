using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteToAnimation))] // 替换 YourScript 为你的脚本的类名
public class YourScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 显示默认的 Inspector 部分

        SpriteToAnimation script = (SpriteToAnimation)target;

        // 在 Inspector 中创建一个按钮
        if (GUILayout.Button("Generate Animation"))
        {
            // 在按钮被点击时执行的代码
            script.CreateAnimation(); // 调用你的创建动画的方法
        }
    }
}


