using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    [Header("Stages")]
    public int curStageIndex = 0;
    public int debug_targetStage = 0; 
    [field: SerializeField]
    public List<ItemArraySO> stageArray { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
        if (debug_targetStage != -1)
        {
            curStageIndex = debug_targetStage;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void FirstLoad()
    {
        PlayerPrefs.DeleteKey("StageValue");

    }

    public List<Item> GetCurrentStageData()
    {
        List<Item> data = DeckData.ArraySOToItemList(stageArray[curStageIndex]);
        return data;
    }
/*
    // TODO: 플레이어도 스테이지도 공유해서 씀 개인작업이 다 끝나면 Utills 스크립트로 이전?
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
    }*/

    public void ChangeScene()
    {
        SceneManager.LoadScene("Minsang 2");
    }

}
