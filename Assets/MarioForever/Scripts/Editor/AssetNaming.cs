using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Editor {
/// <summary>
/// 假设文件名            = SomeAsset
/// Sprite / Texture    = T_SomeAsset.png
/// Animation           = An_SomeAsset.anim
/// 动画状态机            = Asm_SomeAsset.controller
/// 材质                 = M_SomeAsset.mat
/// Shader              = S_SomeAsset.shader
/// </summary>
public static class AssetNaming {
    [MenuItem("MarioForever/规范资源命名")]
    public static void RegulateAssetNaming() {
        const string root = "Assets/MarioForever/";

        foreach (string assetPath in
            Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories)
        ) {
            if (!File.Exists(assetPath) || assetPath.EndsWithAny(".meta", ".cs", ".asset")) {
                continue;
            }

            string assetName = Path.GetFileName(assetPath);
            string dir = assetPath[..^assetName.Length];

            string newAssetName;
            if (assetPath.EndsWithAny(".png", ".jpg") || !assetName.StartsWith("T_")) {
                newAssetName = "T_" + assetName;
            } else if (assetPath.EndsWith(".anim")       || !assetName.StartsWith("An_")) {
                newAssetName = "An_" + assetName;
            } else if (assetPath.EndsWith(".controller") || !assetName.StartsWith("Asm_")) {
                newAssetName = "Asm_" + assetName;
            } else if (assetPath.EndsWith(".mat")        || !assetName.StartsWith("M_")) {
                newAssetName = "M_" + assetName;
            } else if (assetPath.EndsWith(".shader")     || !assetName.StartsWith("S_")) {
                newAssetName = "S_" + assetName;
            } else {
                Debug.Log($"Skip asset {assetPath} -> ");
                continue;
            }

            newAssetName = newAssetName.Replace(" ", "");
            if (assetName != newAssetName) {
                string newAssetPath = dir + newAssetName;
                AssetDatabase.RenameAsset(assetPath, newAssetPath);
                Debug.Log($"Rename asset {assetPath} -> {newAssetPath}");
            }
        }
        
        AssetDatabase.Refresh();
    }

    private static bool EndsWithAny(this string self, params string[] suffixes) {
        return suffixes.Any(self.EndsWith);
    }
}
}