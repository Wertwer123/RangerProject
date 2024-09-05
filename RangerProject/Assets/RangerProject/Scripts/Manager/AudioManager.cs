using System.Collections;
using RangerProject.Scripts.Utils;
using UnityEngine;

namespace RangerProject.Scripts.Manager
{
    public class AudioManager : BaseSingleton<AudioManager>
    {
        private ObjectPool<AudioSource> AudioPool;

        protected override void Awake()
        {
            base.Awake();
            AudioPool = new ObjectPool<AudioSource>("AudioObject");
        }

        public void PlayAudioFileWithRandomParams(ParamterizedAudiofile AudioFileToPlay, Vector3 Position)
        {
            var AudioObject = AudioPool.GetObjectFromPool();
            
            AudioObject.transform.position = Position;
            AudioObject.pitch = Random.Range(AudioFileToPlay.GetMinPitch(), AudioFileToPlay.GetMaxPitch());
            AudioObject.volume = AudioFileToPlay.GetVolume();
            AudioObject.clip = AudioFileToPlay.GetAudioClip();
            AudioObject.Play();
            
            StartCoroutine(ReturnAudioObjectToPoolAfterPlayTime(AudioFileToPlay.GetAudioClip().length, AudioObject));
        }

        IEnumerator ReturnAudioObjectToPoolAfterPlayTime(float ClipLength, AudioSource AudioObjectToReturn)
        {
            yield return new WaitForSeconds(ClipLength);
            
            AudioPool.ReturnObjectToPool(AudioObjectToReturn);
        }
    }
}
