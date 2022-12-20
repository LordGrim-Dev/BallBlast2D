using System;
using UnityEngine;

namespace Game.Common
{
    public class GameUtilities
    {
        internal static void ShowLog(string inMessage)
        {
#if DEBUG || UNITY_EDITOR
            Debug.Log("[DEBUG] : " + inMessage);
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


    }
}