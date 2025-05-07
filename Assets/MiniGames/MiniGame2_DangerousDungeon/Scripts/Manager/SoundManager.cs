using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace MiniGame2
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)]
        float
            sfxVolume, // 효과음 볼륨
            sfxPitchVariance, // 효과음 피치 랜덤성
            musicVolume; // 배경음 볼륨

        // 0번은 bgm, 나머지는 sfx 재생용 오디오 소스
        List<AudioSource> audioSources = new List<AudioSource>();

        private void Awake()
        {
            audioSources = GetComponentsInChildren<AudioSource>(true).ToList();
        }

        // BGM 변경
        public void ChangeBGM(AudioClip clip)
        {
            audioSources[0].Stop();
            audioSources[0].clip = clip;
            audioSources[0].Play();
        }

        public void PlaySfx(AudioClip clip)
        {
            int i = 1;
            for (; i < audioSources.Count; i++)
            {
                AudioSource tmp = audioSources[i];

                if (!tmp.isPlaying)
                {
                    AudioSetting(tmp, clip);
                    return;
                }
            }

            // 모든 오디오소스가 다 사용중일 때
            // 새 오디오 소스 오브젝트 생성하여 플레이
            GameObject obj_tmp = new GameObject($"sfx{i}", typeof(AudioSource));
            obj_tmp.transform.parent = transform;
            if (obj_tmp.TryGetComponent(out AudioSource audio_tmp))
            {
                audio_tmp.playOnAwake = false;
                audioSources.Add(audio_tmp);
                AudioSetting(audio_tmp, clip);
            }
        }

        void AudioSetting(AudioSource audioSource, AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.volume = sfxVolume;
            audioSource.Play();
            audioSource.pitch = 1f + Random.Range(-sfxPitchVariance, sfxPitchVariance);
        }
    }
}