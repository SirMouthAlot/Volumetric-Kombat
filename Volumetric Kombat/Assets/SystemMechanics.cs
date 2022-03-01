using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UFE3D;

public class SystemMechanics : MonoBehaviour
{

    [SerializeField]
    public GameObject _BattleUI;

    public GameObject _P1EXBar, _P2EXBar;


    //UFE.GetControlsScript(1).myInfo.maxGaugePoints = 100; // player 1
    //UFE.GetControlsScript(2).myInfo.maxGaugePoints = 100; // player 2


    // Start is called before the first frame update
    void Start()
    {
        Scene _currentScene = SceneManager.GetActiveScene();

        string _sceneName = _currentScene.name;
        

        _P1EXBar = GameObject.FindWithTag("P1EX");
        
    }

    // Update is called once per frame
    void Update()
    {

       

    }
}
