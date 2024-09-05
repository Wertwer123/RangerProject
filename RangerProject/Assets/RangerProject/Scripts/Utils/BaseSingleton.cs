using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
   static private T instance;

   public static T Instance
   {
      get
      {
         if (!instance)
         {
            instance = FindObjectOfType<T>();

            if (!instance)
            {
               instance = new GameObject("SingletonInstance" + nameof(T)).AddComponent<T>();
            }

            return instance;
         }
         else
         {
            return instance;
         }
      }
   }

   protected virtual void Awake()
   {
      if (!instance != this && instance != null)
      {
         Destroy(this.gameObject);
      }
      else if (!instance)
      {
         instance = GetComponent<T>();
      }
   }
}
