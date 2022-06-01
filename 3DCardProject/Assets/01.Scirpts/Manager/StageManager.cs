using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    [Header("Stages")]
    public int targetStage = 0;
    public int debug_targetStage = 0; 
    [field: SerializeField]
    public StageArraySO stageSO { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        if (debug_targetStage != -1)
        {
            targetStage = debug_targetStage;
        }
    }

    public StageSO GetCurrentStageData()
    {
        return stageSO.stages[debug_targetStage];
    }

    // TODO: �÷��̾ ���������� �����ؼ� �� �����۾��� �� ������ Utills ��ũ��Ʈ�� ����?
    public static List<Item> SetStageBuffer(List<Item> itemList)
    {
        for(int i = 0; i < Instance.stageSO.stages[Instance.targetStage].StageData.Length; i++)
        {
            Item item = Instance.stageSO.stages[Instance.targetStage].StageData[i].itemSo.item;
            for (int j = 0; j < item.count; j++)
                itemList.Add(item);
        }


        for (int i = 0; i < itemList.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, itemList.Count);
            Item temp = itemList[i];
            itemList[i] = itemList[rand];
            itemList[rand] = temp;

        }

        return itemList;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Minsang 2");
    }

}
