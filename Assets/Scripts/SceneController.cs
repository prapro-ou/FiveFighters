using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private List<string> _scenes = new List<string>{"TitleScene", "MainScene", "ShopScene"};

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene(int number)
    {
        SceneManager.LoadScene(_scenes[number]);
    }
}
