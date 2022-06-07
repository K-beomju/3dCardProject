using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    private Button btn;
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(StageManager.Instance.ChangeScene);
    }
}
