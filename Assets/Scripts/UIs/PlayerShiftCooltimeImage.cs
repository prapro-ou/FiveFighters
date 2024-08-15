using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShiftCooltimeImage : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCooltimeImage(float progress)
    {
        _image.fillAmount = progress;
    }
}
