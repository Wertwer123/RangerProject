using System.Collections.Generic;
using UnityEngine;

namespace RangerProject.Scripts.Utils
{
    public class ObjectPool<T> where T : Component
    {
        //the initial amount of objects allocated
        private const int StartSize = 10;

        private List<T> AllPooledObjects = new List<T>();

        public ObjectPool(string ObjectPoolObjectName)
        {
            InitPool(ObjectPoolObjectName);
        }

        public T GetObjectFromPool()
        {
            for (int i = 0; i < AllPooledObjects.Count; i++)
            {
                var Object = AllPooledObjects[i];
            
                if (!Object.gameObject.activeSelf)
                {
                    Object.gameObject.SetActive(true);
                    return Object;
                }
            }

            var NewPoolObject = new GameObject().AddComponent<T>();
        
            AllPooledObjects.Add(NewPoolObject);
            return NewPoolObject;
        }

        public void ReturnObjectToPool(T ObjectToReturn)
        {
            if (AllPooledObjects.Count > StartSize)
            {
                AllPooledObjects.Remove(ObjectToReturn);
                Object.Destroy(ObjectToReturn.gameObject);
                return;
            }
            
            ObjectToReturn.gameObject.SetActive(false);
        }
        
        void InitPool(string ObjectPoolObjectName)
        {
            for (int i = 0; i < StartSize; i++)
            {
                var NewPoolObject = new GameObject(ObjectPoolObjectName).AddComponent<T>();
                NewPoolObject.gameObject.SetActive(false);
                AllPooledObjects.Add(NewPoolObject);
            }
        }


    }
}
