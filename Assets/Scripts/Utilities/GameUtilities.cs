using System;
using System.IO;
using UnityEngine;

namespace Game.Common
{
    public class GameUtilities
    {
        internal static void ShowLog(object inMessage)
        {
#if DEBUG || UNITY_EDITOR
            Debug.Log($"[DEBUG] : {inMessage}");
#endif
        }


        public static T InstantiateObjectOfType<T>(T inInstance, Vector3 inPos, Transform inParent) where T : UnityEngine.Object
        {
            T instance;

            instance = GameObject.Instantiate<T>(inInstance, inPos, Quaternion.identity, inParent);

            return instance;

        }
        public static GameObject GetGameObjectFromPath(string inPath, Transform inParent = null)
        {
            GameObject newObject = null;

            var prefab = LoadFromResource<GameObject>(inPath);

            if (prefab != null)
            {
                if (inParent != null)
                    newObject = GameObject.Instantiate(prefab, inParent) as GameObject;
                else
                    newObject = GameObject.Instantiate(prefab) as GameObject;
                prefab = null;
            }

            return newObject;
        }


        public static T LoadFromResource<T>(string inPath) where T : UnityEngine.Object
        {
            if (!string.IsNullOrEmpty(inPath))
            {
                T resource = Resources.Load<T>(inPath);

                return resource;
            }
            return null;
        }

        internal static void SaveToFile(string inFileName, string jsonStr)
        {
            string fullPath = Application.persistentDataPath + inFileName;
            if (!File.Exists(fullPath)) File.Create(fullPath);
            File.WriteAllText(fullPath, jsonStr);
        }

        internal static string LoadFromFile(string inFIleName)
        {
            string content = "";
            string fullPath = Application.persistentDataPath + inFIleName;
            if (File.Exists(fullPath))
                content = File.ReadAllText(fullPath);

            return content;
        }
    }
}