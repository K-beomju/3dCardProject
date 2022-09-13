using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTutorial : MonoBehaviour
{
    public static bool isAttak = false;         // 공격 카드를 발동해야할 때 
    public static bool isEnemyTurn = false;     // 공격카드를 내고 움직이기전 설명할 때 
    public static bool isTurnChange = false;    // 적이 벽 카드를 내기전 공격카드 설명할 때 
    public static bool isChangeCard = false;    // 적이 벽 카드를 내고 플레이어 턴으로 감  
    public static bool isChangeDir = false;     // 체인지 카드를 발동해야할 때 
    public static bool isStop = false;          // 적이 스탑 카드를 낼떄 

    private void Start()
    {
        if(TutorialManager.Instance.isTutorial)
        {
            StartCoroutine(BattleTutorialCo());
        }
    }

    private IEnumerator BattleTutorialCo()
    {
        yield return new WaitForSeconds(7f);
        TutorialManager.Instance.Fade(true);
        TutorialManager.Instance.Explain("카드를 써서 상대의 토템을 잡아봅시다", 0);
        yield return new WaitForSeconds(3f);
        TutorialManager.Instance.Fade(false);

        yield return new WaitForSeconds(1f);
        yield return TutorialManager.Instance.ExplainCol("플레이어에게 카드를 드로우 했습니다", 0);
        yield return TutorialManager.Instance.ExplainCol("공격 카드를 발동해봅시다", 0);
        yield return new WaitForSeconds(1f);

        TutorialManager.Instance.Fade(true);
        TutorialManager.Instance.Explain("공격 카드를 발동해봅시다", 450);
        CardManager.Instance.TutorialCardOutLine(100);

        yield return new WaitWhile(() => !isAttak);
        TutorialManager.Instance.Fade(false);
        yield return new WaitForSeconds(2f);
        yield return TutorialManager.Instance.ExplainCol("공격 카드는 핵을 내 것으로 바꿀 수 있습니다", 0, 2);
        yield return TutorialManager.Instance.ExplainCol("카드 소환은 한 턴에 힌 번만 실행할 수 있습니다", 0, 2);
        yield return new WaitForSeconds(1f);
        isEnemyTurn = true;
        yield return new WaitForSeconds(1f);
        yield return TutorialManager.Instance.ExplainCol("카드를 소환한 토템은 한 칸씩 움직일 수 있습니다", 0, 2);
        yield return new WaitForSeconds(2f);
        isTurnChange = true;
        yield return new WaitWhile(() => !isChangeCard);

        yield return TutorialManager.Instance.ExplainCol("벽 카드는 상대방의 진로를 방해할 수 있습니다", 0, 2);
        yield return TutorialManager.Instance.ExplainCol("체인지 카드를 발동해봅시다", 0, 2);
        yield return new WaitForSeconds(1f);
        TutorialManager.Instance.Fade(true);
        TutorialManager.Instance.Explain("체인지 카드를 발동해봅시다", 450);

        CardManager.Instance.TutorialCardOutLine(102);
        isTurnChange = false;
        yield return new WaitWhile(() => !isChangeDir);
        TutorialManager.Instance.Fade(false);
        yield return new WaitForSeconds(1f);

        yield return TutorialManager.Instance.ExplainCol("체인지 카드는 이동 방향을 반대로 바꿀 수 있습니다", 0, 2);
        isTurnChange = true;

        yield return new WaitWhile(() => !isStop);
        yield return new WaitForSeconds(3f);

        yield return TutorialManager.Instance.ExplainCol("스탑 카드는 움직이지 않고 턴을 넘길 수 있습니다", 0, 2);
        yield return TutorialManager.Instance.ExplainCol("적을 한 번에 잡기 위해선 뜀틀 카드를 써야 합니다", 0, 2);
        yield return TutorialManager.Instance.ExplainCol("뜀틀 카드는 한 칸 더 이동할 수 있습니다", 0, 2);
        yield return new WaitForSeconds(1f);

        TutorialManager.Instance.Fade(true);
        TutorialManager.Instance.Explain("뜀틀 카드를 발동해봅시다", 450);
        CardManager.Instance.TutorialCardOutLine(103);

        yield return null;
    }

}
