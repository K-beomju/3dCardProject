using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private Button titleButton;
    [SerializeField]
    private Button restartButton;

    private void Start()
    {
        titleButton.onClick.AddListener(()=> {
            Global.LoadScene.LoadScene("Title");
        });
        restartButton.onClick.AddListener(()=> {
            SaveManager.Instance.NewGame();
            Global.LoadScene.LoadScene("Stage");
        });
    }
}
