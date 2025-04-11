using UnityEditor;
using UnityEngine;

public class RenameSprites : EditorWindow
{
    [MenuItem("Tools/Rename Sprites from 1")]
    static void Rename()
    {
        Object obj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(obj);
        Object[] assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);

        int index = 1;
        foreach (Object o in assets)
        {
            if (o is Sprite sprite)
            {
                sprite.name = $"Tower_{index}";
                EditorUtility.SetDirty(sprite);
                index++;
            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Sprites renamed starting from 1!");
    }
}
