using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPrimaryText : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private TextMeshProUGUI _primaryText;

    // Start is called before the first frame update
    void Start()
    {
        UpDatePrimaryUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpDatePrimaryUI()
    {
        _primaryText.text = _player.PrimaryGrazeCount + "/" + _player.MyShape.PrimaryAttackCost;
    }
}
