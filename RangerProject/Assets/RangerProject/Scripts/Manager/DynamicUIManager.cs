using System.Collections;
using RangerProject.Scripts.UI;
using RangerProject.Scripts.Utils;
using UnityEngine;

namespace RangerProject.Scripts.Manager
{
   public class DynamicUIManager : BaseSingleton<DynamicUIManager>
   {
      [SerializeField] private DmgPopUp DmgPopUpPrefab;
      private ObjectPool<DmgPopUp> DmgPopUpPool;

      public void SpawnDmgPopUp(string DmgText, Color Color, Vector3 SpawnPoint)
      {
         DmgPopUp SpawnedPopUp = DmgPopUpPool.GetObjectFromPool();
         SpawnedPopUp.SetDmgPopUpText(DmgText, Color);
         SpawnedPopUp.transform.position = SpawnPoint;
         
         StartCoroutine(ReturnPopUpToPool(SpawnedPopUp.GetPopUpPlayTime(), SpawnedPopUp));
      }

      IEnumerator ReturnPopUpToPool(float ReturnTime, DmgPopUp PopUp)
      {
         yield return new WaitForSeconds(ReturnTime);
            
         DmgPopUpPool.ReturnObjectToPool(PopUp);
      }
      protected override void Awake()
      {
         base.Awake();
         DmgPopUpPool = new ObjectPool<DmgPopUp>("DmgPopUp", DmgPopUpPrefab);
      }
      
   }
}
