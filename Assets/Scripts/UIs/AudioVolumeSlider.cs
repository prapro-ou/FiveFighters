using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBgmVolume();
        UpdateSeVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBgmVolume()
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.UpdateBgmVolume(_slider.value);
    }

    public void UpdateSeVolume()
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.UpdateSeVolume(_slider.value);
    }
}
