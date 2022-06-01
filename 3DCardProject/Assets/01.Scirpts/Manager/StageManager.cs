using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [Header("Stages")]
    public static int targetStage = 0;
    public int debug_targetStage = 0; 
    [SerializeField] private StageArraySO stageSO;

    protected override void Awake()
    {
        base.Awake();
        if (debug_targetStage != -1)
        {
            targetStage = debug_targetStage;
        }
    }

    // TODO: �÷��̾ ���������� �����ؼ� �� �����۾��� �� ������ Utills ��ũ��Ʈ�� ����?
    public static List<Item> SetStageBuffer(List<Item> itemList)
    {
        for(int i = 0; i < Instance.stageSO.stages[targetStage].StageData.Length; i++)
        {
            Item item = Instance.stageSO.stages[targetStage].StageData[i].itemSo.item;
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
}
