namespace MyChess
{
    using System;
    using UnityEngine;

    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);

        public static T Instance => LazyInstance.Value;

        private static T CreateSingleton()
        {
            var instance = FindObjectOfType<T>(); // because it already exists in scene
            DontDestroyOnLoad(instance.gameObject);
            return instance;
        }
    }
}