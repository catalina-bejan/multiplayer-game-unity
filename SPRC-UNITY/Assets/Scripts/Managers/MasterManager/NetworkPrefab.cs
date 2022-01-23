using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetworkPrefab
{
    public GameObject prefab;
    public string path;

    public NetworkPrefab(GameObject prefab, string path)
    {
        this.prefab = prefab;
        this.path = ReturnPrefabPathModified(path);
    }

    private string ReturnPrefabPathModified(string path)
    {
        // What we have: Assets/Resources/.../File.prefab
        // What we need: Resources/.../File
        int extentionLenght = System.IO.Path.GetExtension(path).Length;
        int startIndex = path.ToLower().IndexOf("resx");

        if (startIndex == -1) return string.Empty;

        return path.Substring(startIndex, path.Length - (startIndex + extentionLenght));
    }
}
