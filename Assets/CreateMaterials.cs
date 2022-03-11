using System;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using UnityEditor;
using UnityEngine;

namespace ObscuritasRiichiMahjong
{
    public class CreateMaterials
    {
        // Start is called before the first frame update
        [MenuItem("Assets/Create Materials")]
        public static void CreateMaterialsFromTextures()
        {
            foreach (var o in Selection.objects)
            {
                if (o.GetType() != typeof(Texture2D))
                {
                    Debug.LogError("This isn't a texture: " + o);
                    continue;
                }

                Debug.Log("Creating material from: " + o);

                var selected = o as Texture2D;

                var material = new Material(Shader.Find("Standard")) { mainTexture = (Texture)o };

                var savePath = AssetDatabase.GetAssetPath(selected);
                savePath = savePath.Substring(0, savePath.LastIndexOf('/') + 1);

                if (selected is null)
                    throw new ArgumentException("given object is not a Texture2D");

                var newAssetName = savePath + selected.name + ".mat";

                AssetDatabase.CreateAsset(material, newAssetName);

                AssetDatabase.SaveAssets();
            }
        }

        [MenuItem("Assets/Create Mahjong Tile")]
        public static void CreateMahjongTileFromMaterial()
        {
            foreach (var o in Selection.objects)
            {
                if (o.GetType() != typeof(Material))
                {
                    Debug.LogError("This isn't a texture: " + o);
                    continue;
                }

                if (!(o is Material material))
                    throw new ArgumentException("selected object is not a Material");

                var asset = ScriptableObject.CreateInstance<MahjongTile>();
                asset.Material = material;
                asset.Name = material.name;

                try
                {
                    asset.Number = byte.Parse($"{material.name.Last()}");
                }
                catch
                {
                    asset.Number = 10;
                }

                AssetDatabase.CreateAsset(asset,
                    $"Assets/Mahjong Tiles/Tiles/{material.name}.asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = asset;
            }
        }
    }
}