using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSpecialText : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private TextMeshProUGUI _specialText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSpecialUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSpecialUI()
    {
        _specialText.text = _player.SpecialGrazeCount + "/" + _player.MyShape.SpecialSkillCost;
    }
}
