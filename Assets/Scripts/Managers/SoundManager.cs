using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
 
public class SoundManager : MonoBehaviour
{
    public static bool isLoad = false;

    [System.Serializable]
    public class BGMData
    {
        public string name;
        public AudioClip audioClip;
        [Range(0,1)]
        public float volume;
    }

    [System.Serializable]
    public class SEData
    {
        public string name;
        public AudioClip audioClip;
        public Coroutine coroutine;
        public float delay;
    }
 
    [SerializeField]
    private BGMData[] _bgmDatas;

    [SerializeField]
    private SEData[] _seDatas;

    [SerializeField]
    private AudioMixer _mixer;

    [SerializeField]
    private AudioMixerGroup _bgmAudioMixerGroup;

    [SerializeField]
    private AudioMixerGroup _seAudioMixerGroup;

    private AudioSource _bgmAudioSource = new AudioSource();

    private AudioSource[] _seAudioSourceList = new AudioSource[40];
 
    private Dictionary<string, BGMData> _bgmDictionary = new Dictionary<string, BGMData>();

    private Dictionary<string, SEData> _seDictionary = new Dictionary<string, SEData>();
 
    private void Awake()
    {
        if(isLoad == true)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            isLoad = true;
            DontDestroyOnLoad(this.gameObject);
        }

        _bgmAudioSource = gameObject.AddComponent<AudioSource>();
        _bgmAudioSource.outputAudioMixerGroup = _bgmAudioMixerGroup;
        _bgmAudioSource.loop = true;

        for (int i = 0; i < _seAudioSourceList.Length; i++)
        {
            _seAudioSourceList[i] = gameObject.AddComponent<AudioSource>();
            _seAudioSourceList[i].outputAudioMixerGroup = _seAudioMixerGroup;
        }
 
        foreach(BGMData bgmData in _bgmDatas)
        {
            _bgmDictionary.Add(bgmData.name, bgmData);
        }

        foreach(SEData seData in _seDatas)
        {
            _seDictionary.Add(seData.name, seData);
        }
    }

    private AudioSource _GetUnusedSeAudioSource()
    {
        for(int i = 0; i < _seAudioSourceList.Length; ++i)
        {
            if(_seAudioSourceList[i].isPlaying == false)
            {
                return _seAudioSourceList[i];
            }
        }

        return null;
    }

    public void PlayBGM(string name)
    {
        if(_bgmDictionary.TryGetValue(name, out BGMData bgmData))
        {
            _bgmAudioSource.clip = bgmData.audioClip;
            _bgmAudioSource.volume = bgmData.volume;
            _bgmAudioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Cannot find AudioClip: {name}");
        }
    }

    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }

private IEnumerator _PlaySeAudioClip(AudioClip clip, float delay, System.Action onComplete)
{
    AudioSource audioSource = _GetUnusedSeAudioSource();
    if (audioSource == null)
    {
        yield break;
    }
    audioSource.clip = clip;
    audioSource.Play();

    yield return new WaitForSeconds(delay);

    onComplete?.Invoke();
}

public void PlaySound(string name)
{
    if (_seDictionary.TryGetValue(name, out SEData seData))
    {
        if (seData.coroutine == null)
        {
            seData.coroutine = StartCoroutine(_PlaySeAudioClip(seData.audioClip, seData.delay, () =>
            {
                seData.coroutine = null;
            }));
        }
        else
        {
            // Debug.Log($"The sound is in delay: {name}");
        }
    }
    else
    {
        Debug.Log($"Cannot find AudioClip: {name}");
    }
}

    public void UpdateBgmVolume(float sliderValue)
    {
        float value = ConvertVolume2dB(sliderValue);
        _mixer.SetFloat("BGMVolumeValue", value);
    }

    public void UpdateSeVolume(float sliderValue)
    {
        float value = ConvertVolume2dB(sliderValue);
        _mixer.SetFloat("SEVolumeValue", value);

        if(sliderValue != 0.5f)
        {
            PlaySound("Submit");
        }
    }

    private float ConvertVolume2dB(float volume)
    {
        return Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);
    }
}