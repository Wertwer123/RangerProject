using System.Collections.Generic;
using UnityEngine;

namespace RangerProject.Scripts.Utils
{
    public class ObjectPool<T> where T : Component
    {
        //the initial amount of objects allocated
        private const int StartSize = 10;
        private T PrefabToUse;
        private List<T> AllPooledObjects = new List<T>();

        public ObjectPool(string ObjectPoolObjectName, T PrefabToUse)
        {
            InitPool(ObjectPoolObjectName, PrefabToUse);
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

            var NewPoolObject = Object.Instantiate(PrefabToUse);
        
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
        
        void InitPool(string ObjectPoolObjectName, T PoolObject)
        {
            PrefabToUse = PoolObject;
            
            for (int i = 0; i < StartSize; i++)
            {
                var NewPoolObject = Object.Instantiate(PoolObject);
                NewPoolObject.gameObject.SetActive(false);
                AllPooledObjects.Add(NewPoolObject);
            }
        }


    }
}
