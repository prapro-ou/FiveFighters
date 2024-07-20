using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowerBar : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private Slider _powerBarSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePowerMultiplier()
    {
        _powerBarSlider.value = (float)_player.PowerMultiplier / (float)100;
    }
}
