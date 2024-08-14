using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCautionTile : MonoBehaviour
{
    [SerializeField]
    private GameObject _hexagonCautionEffectPrefab;

    [SerializeField]
    private HexagonMine _hexagonMinePrefab;

    [SerializeField]
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAppearAnimation()
    {
        _animator.SetTrigger("Appear");
    }

    public void PlayDisappearAnimation()
    {
        _animator.SetTrigger("Disappear");
    }

    public IEnumerator SpawnMine()
    {
        Instantiate(_hexagonCautionEffectPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1);

        //地雷設置
        Instantiate(_hexagonMinePrefab, transform.position, Quaternion.identity);

        yield return null;
    }
}
