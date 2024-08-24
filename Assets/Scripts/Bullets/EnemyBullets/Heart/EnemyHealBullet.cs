using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealBullet : MonoBehaviour
{
    private Player _player;

    private SoundManager _soundManager;

    [SerializeField]
    private int _healValue;

    [SerializeField]
    private GameObject _heartHealEffect;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(_ExpandEffect());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator _ExpandEffect()
    {
        Vector3 startScale = new Vector3(0.1f, 0.1f, 1f);
        Vector3 endScale = new Vector3(0.5f, 0.5f, 1f);
        float duration = 0.5f; // 拡大するのにかかる時間
        float currentTime = 0f;

        while (currentTime < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // 最後のサイズを正確に設定
        transform.localScale = endScale;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "DamageCollider")
        {
            _PlaySound("Heal");
            _player.HitPoint += _healValue;
            GameObject effect = Instantiate(_heartHealEffect, _player.transform.position, Quaternion.identity, _player.transform);
            Destroy(effect, 0.5f);
            Debug.Log($"Heal PlayerHP {_healValue}");
        }
    }

    private void _PlaySound(string name)
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.PlaySound(name);
    }
}
