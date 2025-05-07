using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace MiniGame2
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)]
        float
            sfxVolume, // ȿ���� ����
            sfxPitchVariance, // ȿ���� ��ġ ������
            musicVolume; // ����� ����

        // 0���� bgm, �������� sfx ����� ����� �ҽ�
        List<AudioSource> audioSources = new List<AudioSource>();

        private void Awake()
        {
            audioSources = GetComponentsInChildren<AudioSource>(true).ToList();
        }

        // BGM ����
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

            // ��� ������ҽ��� �� ������� ��
            // �� ����� �ҽ� ������Ʈ �����Ͽ� �÷���
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