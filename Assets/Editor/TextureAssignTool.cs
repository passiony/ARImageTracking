using System;
using UnityEditor;
using UnityEngine;

public class TextureAssignTool : EditorWindow
{
    private string materialFolder = "Assets/Artwork/Materials/";
    private string textureFolder = "Assets/Artwork/Textures/";
    private string material_ext = "_2D_View_1001";

    [MenuItem("Tools/Assign Textures")]
    private static void OpenWindow()
    {
        TextureAssignTool window = GetWindow<TextureAssignTool>("Texture Assign Tool");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Material Folder:");
        materialFolder = EditorGUILayout.TextField(materialFolder);

        GUILayout.Label("Texture Folder:");
        textureFolder = EditorGUILayout.TextField(textureFolder);

        if (GUILayout.Button("Assign Textures"))
        {
            AssignTextures();
        }
    }

    private void AssignTextures()
    {
        string[] materialPaths = AssetDatabase.FindAssets("t:Material", new[] { materialFolder });
        string[] texturePaths = AssetDatabase.FindAssets("t:Texture", new[] { textureFolder });

        foreach (string materialPath in materialPaths)
        {
            Material material = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(materialPath));

            if (material != null)
            {
                string texName = material.name + material_ext;
                Debug.Log($"查找材质：{material.name}=> 贴图：=>{texName}");

                // 查找同名贴图
                string texturePath = Array.Find(texturePaths,
                    p => AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(p)).name == texName);

                if (!string.IsNullOrEmpty(texturePath))
                {
                    Texture texture =
                        AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(texturePath));

                    if (texture != null)
                    {
                        // 赋值贴图给材质
                        material.mainTexture = texture;
                        material.EnableKeyword("_EMISSION");
                        material.SetTexture("_EmissionMap", texture);
                        material.SetColor("_EmissionColor", Color.white);

                        Debug.Log("Assigned texture to material: " + material.name);
                    }
                    else
                    {
                        Debug.LogWarning("Texture not found for material: " + material.name);
                    }
                }
                else
                {
                    Debug.LogWarning("Texture not found for material: " + material.name);
                }
            }
        }

        Debug.Log("Texture assignment complete.");
    }
}