using System.IO;
using System.Linq;
using UnityEditor;

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
        const string root = "./Assets/MarioForever/";

        foreach (var file in
            Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories)
        ) {
            if (!File.Exists(file) || file.EndsWithAny(".meta", ".cs", ".asset")) {
                continue;
            }

            string assetPath = Path.GetRelativePath(root, file);
            string assetName = Path.GetFileName(file);
            string dir = assetPath[..^assetName.Length];

            string newAssetName;
            if (file.EndsWithAny(".png", ".jpg") || !assetName.StartsWith("T_")) {
                newAssetName = "T_" + assetName;
            } else if (file.EndsWith(".anim")       || !assetName.StartsWith("An_")) {
                newAssetName = "An_" + assetName;
            } else if (file.EndsWith(".controller") || !assetName.StartsWith("Asm_")) {
                newAssetName = "Asm_" + assetName;
            } else if (file.EndsWith(".mat")        || !assetName.StartsWith("M_")) {
                newAssetName = "M_" + assetName;
            } else if (file.EndsWith(".shader")     || !assetName.StartsWith("S_")) {
                newAssetName = "S_" + assetName;
            } else {
                continue;
            }

            newAssetName = newAssetName.Replace(" ", "");
            if (assetName != newAssetName) {
                AssetDatabase.RenameAsset(assetPath, dir + newAssetName);
            }
        }
    }

    private static bool EndsWithAny(this string self, params string[] suffixes) {
        return suffixes.Any(self.EndsWith);
    }
}
}