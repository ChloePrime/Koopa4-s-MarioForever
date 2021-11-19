using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Editor {
/// <summary>
/// 假设文件名            = SomeAsset
/// Sprite / Texture    = T_SomeAsset.png
/// Prefab              = GO_SomeAsset.prefab
/// Animation           = An_SomeAsset.anim
/// 动画状态机            = Asm_SomeAsset.controller
/// 材质                 = M_SomeAsset.mat
/// Shader              = S_SomeAsset.shader
/// </summary>
public static class AssetNaming {
    [MenuItem("MarioForever/规范资源命名")]
    public static void RegulateAssetNaming() {
        const string root = "Assets/MarioForever/";

        foreach (string file in
            Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories)
        ) {
            if (!File.Exists(file) || file.EndsWithAny(".meta", ".cs", ".asset")) {
                continue;
            }

            string assetPath = file.Replace('\\', '/');
            string assetName = Path.GetFileName(assetPath);
            string dir = assetPath[..^assetName.Length];

            string newAssetName = FixNaming(assetName, dir);
            if (assetName == newAssetName) {
                Debug.Log($"Skip asset {assetName}");
                continue;
            }

            string err = AssetDatabase.RenameAsset(assetPath, newAssetName);
            string newAssetPath = dir + newAssetName;
            Debug.Log(err == ""
                ? $"Rename asset {assetPath} -> {newAssetPath}"
                : $"Failed to Rename {assetPath} -> {newAssetPath}:\n{err}");
        }
        
        AssetDatabase.Refresh();
    }

    [MenuItem("MarioForever/处理 SpriteSheet 的资源命名")]
    public static void RegulateSpriteAtlas() {
        // 修改这个值为你想要处理的 Texture Asset
        const string asset = "Assets/MarioForever/Art/Sprites/Enemies/T_Enemies.png";
        const string meta = asset + ".meta";

        Dictionary<string, string> renameTable = new();
        foreach (Sprite slice in AssetDatabase.LoadAllAssetsAtPath(asset).OfType<Sprite>()) {
            if (!slice.name.StartsWith("T_")) {
                renameTable[slice.name] = "T_" + slice.name;
            }

            if (slice.name.Contains(' ')) {
                renameTable[slice.name] = slice.name.Replace(" ", "");
            }
        }

        StringBuilder sb = new();
        foreach (string line in File.ReadLines(meta, Encoding.UTF8)) {
            // 当前行被 ':' 分割的结果
            string[] parts = line.Split(':');

            string newLine;
            if (parts.Count(s => !string.IsNullOrEmpty(s)) == 1) {
                newLine = line;
            } else {
                newLine = string.Join(':', parts.Select(ReplaceToken));
            }

            sb.Append(newLine).Append('\n');
        }

        using StreamWriter w = new(File.OpenWrite(meta));
        w.Write(sb.ToString());
        
        string ReplaceToken(string token) {
            string trimmed = token.Trim();
            return renameTable.TryGetValue(trimmed, out string replacement) 
                ? token.Replace(trimmed, replacement) 
                : token;
        }
    }

    private static bool EndsWithAny(this string self, params string[] suffixes) {
        return suffixes.Any(self.EndsWith);
    }

    private static string FixNaming(string oldName, string directory) {
        string newName;
        if (oldName.EndsWithAny(".png", ".jpg") && !oldName.StartsWith("T_")) {
            newName = "T_" + oldName;
        } else if (oldName.EndsWith(".anim")       && !oldName.StartsWith("An_")) {
            newName = "An_" + oldName;
        } else if (oldName.EndsWith(".prefab")) {
            newName = FixNamingForPrefab(oldName, directory);
        } else if (oldName.EndsWith(".controller") && !oldName.StartsWith("Asm_")) {
            newName = "Asm_" + oldName;
        } else if (oldName.EndsWith(".mat")        && !oldName.StartsWith("M_")) {
            newName = "M_" + oldName;
        } else if (oldName.EndsWith(".shader")     && !oldName.StartsWith("S_")) {
            newName = "S_" + oldName;
        } else if (oldName.Contains(' ')) {
            return oldName.Replace(" ", "");
        } else {
            return oldName;
        }

        return newName.Replace(" ", "");
    }

    private static string FixNamingForPrefab(string oldName, string directory) {
        bool isTilemapRelated = directory.Contains("/Tile", StringComparison.OrdinalIgnoreCase);
        if (isTilemapRelated) {
            if (oldName.StartsWith("GO_")) {
                // _SomeAsset.prefab
                return oldName[2..];
            } else if (!oldName.StartsWith("_")) {
                return "_" + oldName;
            } else {
                return oldName;
            }
        }

        if (!oldName.StartsWith("GO_")) {
            return oldName.StartsWith('_') ? "GO" + oldName : "GO_" + oldName;
        }

        return oldName;
    }
}
}