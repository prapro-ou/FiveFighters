using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHpText : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private TextMeshProUGUI _hpText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHpUI()
    {
        _hpText.text = _player.HitPoint + "/" + _player.MaxHitPoint;
    }
}