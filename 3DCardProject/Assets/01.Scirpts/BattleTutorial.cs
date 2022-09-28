using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTutorial : Singleton<BattleTutorial>
{
    public bool isAttak = false;         // 공격 카드를 발동해야할 때 
    public bool isEnemyTurn = false;     // 공격카드를 내고 움직이기전 설명할 때 
    public bool isTurnChange = false;    // 적이 벽 카드를 내기전 공격카드 설명할 때 
    public bool isChangeCard = false;    // 적이 벽 카드를 내고 플레이어 턴으로 감  
    public bool isChangeDir = false;     // 체인지 카드를 발동해야할 때 
    public bool isStop = false;          // 적이 스탑 카드를 낼떄 
    public bool isTrap = false;          // 트랩카드를 발동해야할때
    public bool isNullity = false;       // 무효카드를 사용할수있을때1
    public bool isReflectAble = false;       // 무효카드를 사용할수있을때2
    public bool isDoneNullity = false;   // 무효카드를 사용했을때

    private void Start()
    {
        if(TutorialManager.Instance.isTutorial)
        {
            StartCoroutine(BattleTutorialCo());
        }
    }

    private IEnumerator BattleTutorialCo()
    {
        yield return TutorialManager.Instance.ExplainCol("\"전투\"튜토리얼입니다.", 0); //2.5f 초
        yield return TutorialManager.Instance.ExplainCol("승리시 재화를 획득하고 패배시 HP가 줄어듭니다.", 0);
        yield return TutorialManager.Instance.ExplainCol("HP가 0 이하로 줄어들면 게임오버입니다.", 0);
        yield return TutorialManager.Instance.ExplainCol("승리해봅시다.", 0, 1, 2);

        yield return new WaitForSeconds(3f);

        yield return TutorialManager.Instance.ExplainCol("시작과 함께 7장 중 5장을 드로우합니다.", 0,1,3f);
        yield return TutorialManager.Instance.ExplainCol("남은 카드는 플레이어 턴이 될때마다 드로우합니다.", 0,1,3f);
        yield return TutorialManager.Instance.ExplainCol("소지한 카드가 없을때 다시 5장 드로우 합니다.", 0, 1, 3f);

        yield return TutorialManager.Instance.ExplainCol("카드는 드래그하여 발동할수있습니다.", 0,1,3f);
        yield return TutorialManager.Instance.ExplainCol("일반카드는 가운데 필드에 발동할수있으며", 0,1,3f);
        SoundManager.Instance.PlayFXSound("HilightField", 0.1f);
        
        Outline hackOutline = FindObjectOfType<Hack>().GetComponent<Outline>();
        hackOutline.OutlineColor = Utils.WhiteColor;
        hackOutline.enabled = true;
        yield return new WaitForSeconds(2f);
        hackOutline.enabled = false;

        yield return TutorialManager.Instance.ExplainCol("설치카드는 토템의 좌우에 설치할수있습니다.", 0,1,3f);
        SoundManager.Instance.PlayFXSound("HilightField", 0.1f);
        var playernode = NewFieldManager.Instance.GetPlayerNodeByData();
        Outline outline = playernode.NextNode.Data.GetComponent<Outline>();
        Outline outline1 = playernode.PrevNode.Data.GetComponent<Outline>();
        outline.enabled = true;
        outline1.enabled = true;
        yield return new WaitForSeconds(2f);
        outline.enabled = false;
        outline1.enabled = false;

        yield return TutorialManager.Instance.ExplainCol("공격 카드를 발동해봅시다.", 0,1,3f);
        yield return new WaitForSeconds(1f);
        hackOutline.OutlineColor = Utils.WhiteColor;
        hackOutline.enabled = true;
        CardManager.Instance.TutorialCardOutLine(100);

        yield return new WaitWhile(() => !isAttak);

        yield return TutorialManager.Instance.ExplainCol("공격 카드는 핵의 소유권을 가져올 수 있습니다.", 0,1, 2);
        yield return TutorialManager.Instance.ExplainCol("상대 토템을 잡거나 3번의 사이클 이후 핵을 소유시 승리입니다.", 0,1, 3);
       
        yield return new WaitForSeconds(1f);
        isEnemyTurn = true;
        yield return new WaitForSeconds(1f);
        yield return TutorialManager.Instance.ExplainCol("카드를 발동하면 토템이 한 칸씩 이동합니다.", 0, 1, 2);
        yield return new WaitForSeconds(2f);
        isTurnChange = true;
        yield return new WaitWhile(() => !isChangeCard);
        isChangeCard = false;
        yield return TutorialManager.Instance.ExplainCol("벽 카드가 설치된곳으로 이동하려하면 벽 카드만 사라집니다.", 0, 1, 2);
        yield return TutorialManager.Instance.ExplainCol("체인지 카드를 발동해봅시다", 0, 1, 2);
        hackOutline.OutlineColor = Utils.WhiteColor;
        hackOutline.enabled = true;
        CardManager.Instance.TutorialCardOutLine(102);
        isChangeCard = true;
        yield return new WaitWhile(() => !isChangeDir);
        yield return new WaitForSeconds(1f);

        isTurnChange = false;
        yield return TutorialManager.Instance.ExplainCol("체인지 카드는 이동 방향을 바꿀 수 있습니다", 0, 1, 2);
        isTurnChange = true;
        Debug.Log("ISSTOPA");

        yield return new WaitWhile(() => !isStop);
        Debug.Log("ISSTOPB");
        yield return new WaitForSeconds(3f);

        yield return TutorialManager.Instance.ExplainCol("스탑 카드는 움직이지 않고 턴을 넘길 수 있습니다", 0, 1, 2);

        yield return TutorialManager.Instance.ExplainCol("덫 카드를 발동해 봅시다", 0, 1, 2);

        NewFieldManager.Instance.GetPlayerNodeByData().PrevNode.Data.isEnterRange = false;
        outline = NewFieldManager.Instance.GetPlayerNodeByData().NextNode.Data.GetComponent<Outline>();
        outline.OutlineColor = Utils.WhiteColor;
        outline.enabled = true;
        CardManager.Instance.TutorialCardOutLine(106);
        isTurnChange = true;

        yield return new WaitWhile(() => !isTrap);
        yield return new WaitForSeconds(1f);

        yield return TutorialManager.Instance.ExplainCol("토템이 덫 카드를 밟는다면 손패의 카드중 하나를 버립니다.", 0, 1, 2);
        isTurnChange = false;

        yield return new WaitWhile(() => !isNullity);
        yield return TutorialManager.Instance.ExplainCol("적이 공격카드를 사용했습니다.", 0);
        yield return TutorialManager.Instance.ExplainCol("무효카드는 손패에 있을때 상대방의 발동을 제지할 수 있습니다.", 0, 1, 2);
        yield return TutorialManager.Instance.ExplainCol("무효카드를 사용해봅시다.", 0, 1, 2);
        isReflectAble = true;
        yield return new WaitWhile(() => !isDoneNullity);

        yield return new WaitWhile(() => !isChangeCard);
        isTurnChange = true;
        yield return new WaitForSeconds(3f);

        yield return TutorialManager.Instance.ExplainCol("적을 잡기 위해선 뜀틀 카드를 써야 합니다", 0, 1, 2);
        yield return TutorialManager.Instance.ExplainCol("뜀틀 카드는 한 칸 더 이동할 수 있습니다", 0, 1, 2);

        NewFieldManager.Instance.GetPlayerNodeByData().NextNode.Data.isEnterRange = false;
        outline = NewFieldManager.Instance.GetPlayerNodeByData().PrevNode.Data.GetComponent<Outline>();
        outline.OutlineColor = Utils.WhiteColor;
        outline.enabled = true;
        CardManager.Instance.TutorialCardOutLine(103);
        yield return null;
    }

}
