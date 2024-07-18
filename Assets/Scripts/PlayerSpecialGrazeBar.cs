using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecialGrazeBar : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private Slider _specialGrazeBar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSpecialGrazeCount()
    {
        _specialGrazeBar.value = (float)_player.SpecialGrazeCount / (float)_player.MyShape.SpecialSkillCost;
    }
}
