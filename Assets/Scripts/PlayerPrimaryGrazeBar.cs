using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrimaryGrazeBar : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private Slider _primaryGrazeBar;

    // Start is called before the first frame update
    void Start()
    {
        _primaryGrazeBar = GameObject.Find("PlayerPrimaryGrazeBar").GetComponent<Slider>();
        UpdatePrimaryGrazeCount();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePrimaryGrazeCount()
    {
        _primaryGrazeBar.value = (float)100/*_player.PrimaryGrazeCount*//(float)_player.MyShape.PrimaryAttackCost;
    }
}
