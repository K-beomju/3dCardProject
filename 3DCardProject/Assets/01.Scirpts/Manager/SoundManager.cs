using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NotOneShot
{
    FirstFalling,
    YoYoGrap,
    MaxCount
}

public class SoundManager
{
    float masterVolume = 1.0f;

    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    float[] volumes = new float[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    // 커스텀
    Dictionary<NotOneShot, AudioSource> _notOneShotDict = new Dictionary<NotOneShot, AudioSource>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound)); // "Bgm", "Effect"
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            // 커스텀
            for (int i = 0; i < (int)NotOneShot.MaxCount; i++)
            {
                GameObject go = new GameObject { name = ((NotOneShot)i).ToString() };

                _notOneShotDict.Add((NotOneShot)i, go.AddComponent<AudioSource>());
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Define.Sound.Bgm].loop = true; // bgm 재생기는 무한 반복 재생
        }

        // 여기에 저장된 볼륨 불러오기.
        masterVolume = SecurityPlayerPrefs.GetFloat("option.masterVolume", 0.5f);

        for (int i = 0; i < volumes.Length; i++)
        {
            volumes[i] = SecurityPlayerPrefs.GetFloat($"option.{(Define.Sound)i}", 0.5f);
        }
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        SecurityPlayerPrefs.SetFloat("option.masterVolume", masterVolume);

        for (int i = 0; i < (int)Define.Sound.MaxCount; i++)
        {
            SetVolume((Define.Sound)i, volumes[i]);
        }
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public void SetVolume(Define.Sound soundType, float value)
    {
        volumes[(int)soundType] = value;
        SecurityPlayerPrefs.SetFloat($"option.{soundType}", volumes[(int)soundType]);

        _audioSources[(int)soundType].volume = value * masterVolume;
    }

    public float[] GetVolumes()
    {
        return volumes;
    }

    public void Clear()
    {
        // 재생기 전부 재생 스탑, 음반 빼기
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // 효과음 Dictionary 비우기
        _audioClips.Clear();
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    // 커스텀
    public void PlayNotOne(string path, NotOneShot type)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, Define.Sound.NotOneShot);
        PlayNotOne(audioClip, type);
    }

    public void PlayNotOne(AudioClip clip, NotOneShot type)
    {
        AudioSource audioSource = _notOneShotDict[type];
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopNotOne(string path, NotOneShot type)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, Define.Sound.NotOneShot);
        StopNotOne(audioClip, type);
    }

    public void StopNotOne(AudioClip clip, NotOneShot type)
    {
        AudioSource audioSource = _notOneShotDict[type];
        audioSource.clip = clip;
        audioSource.Stop();
    }
    //커스텀끝

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void Stop(string path, Define.Sound type = Define.Sound.Effect)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Stop(audioClip, type);
    }

    public void Stop(AudioClip audioClip, Define.Sound type = Define.Sound.Effect)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm) // BGM 배경음악 재생
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            audioSource.Stop();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.Stop();
        }
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";
        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm) // BGM 배경음악 클립 붙이기
        {
            audioClip = Global.Resource.Load<AudioClip>(path);
        }
        else if (type == Define.Sound.Effect)// Effect 효과음 클립 붙이기
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Global.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }
        else if (type == Define.Sound.NotOneShot)
        {
            audioClip = Global.Resource.Load<AudioClip>(path);
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }
}

