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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePrimaryUI()
    {
        _primaryText.text = $"{_player.PrimaryGrazeCount}/{_player.MyShape.PrimaryAttackCost}";
    }
}
