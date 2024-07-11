using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecialGrazeBar : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private Slider _specialGrazeBar;

    // Start is called before the first frame update
    void Start()
    {
        _specialGrazeBar = GameObject.Find("PlayerSpecialGrazeBar").GetComponent<Slider>();
        UpdateSpecialGrazeCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSpecialGrazeCount()
    {
        _specialGrazeBar.value = (float)_player.SpecialGrazeCount/(float)100/*_player.MyShape.SpecialSkillCost*/;
    }
}
