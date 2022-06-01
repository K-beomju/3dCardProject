using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public int stageNum = 0;
    [SerializeField] private StageArraySO stageSO;

    // TODO: 플레이어도 스테이지도 공유해서 씀 개인작업이 다 끝나면 Utills 스크립트로 이전?
    public static List<Item> SetStageBuffer(List<Item> itemList)
    {
        for(int i = 0; i < Instance.stageSO.stages[Instance.stageNum].StageData.Length; i++)
        {
            Item item = Instance.stageSO.stages[Instance.stageNum].StageData[i].itemSo.item;
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
