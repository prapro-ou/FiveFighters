using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowerTile : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private List<Sprite> _sprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePowerTile()
    {
        int number = (int)((_player.PowerMultiplier - 1) * 10);
        if(number != 0)
        {
            _animator.SetTrigger("Rotate");
            GetComponent<Image>().sprite = _sprites[number];
        }
    }
}
