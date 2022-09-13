using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTutorial : MonoBehaviour
{
    public static bool isAttak = false;         // ���� ī�带 �ߵ��ؾ��� �� 
    public static bool isEnemyTurn = false;     // ����ī�带 ���� �����̱��� ������ �� 
    public static bool isTurnChange = false;    // ���� �� ī�带 ������ ����ī�� ������ �� 
    public static bool isChangeCard = false;    // ���� �� ī�带 ���� �÷��̾� ������ ��  
    public static bool isChangeDir = false;     // ü���� ī�带 �ߵ��ؾ��� �� 
    public static bool isStop = false;          // ���� ��ž ī�带 ���� 

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
        TutorialManager.Instance.Explain("ī�带 �Ἥ ����� ������ ��ƺ��ô�", 0);
        yield return new WaitForSeconds(3f);
        TutorialManager.Instance.Fade(false);

        yield return new WaitForSeconds(1f);
        yield return TutorialManager.Instance.ExplainCol("�÷��̾�� ī�带 ��ο� �߽��ϴ�", 0);
        yield return TutorialManager.Instance.ExplainCol("���� ī�带 �ߵ��غ��ô�", 0);
        yield return new WaitForSeconds(1f);

        TutorialManager.Instance.Fade(true);
        TutorialManager.Instance.Explain("���� ī�带 �ߵ��غ��ô�", 450);
        CardManager.Instance.TutorialCardOutLine(100);

        yield return new WaitWhile(() => !isAttak);
        TutorialManager.Instance.Fade(false);
        yield return new WaitForSeconds(2f);
        yield return TutorialManager.Instance.ExplainCol("���� ī��� ���� �� ������ �ٲ� �� �ֽ��ϴ�", 0, 2);
        yield return TutorialManager.Instance.ExplainCol("ī�� ��ȯ�� 1�Ͽ� 1���� ������ �� �ֽ��ϴ�", 0, 2);
        yield return new WaitForSeconds(1f);
        isEnemyTurn = true;
        yield return new WaitForSeconds(1f);
        yield return TutorialManager.Instance.ExplainCol("ī�带 ��ȯ�� ������ ��ĭ �� ������ �� �ֽ��ϴ�", 0, 2);
        yield return new WaitForSeconds(2f);
        isTurnChange = true;
        yield return new WaitWhile(() => !isChangeCard);

        yield return TutorialManager.Instance.ExplainCol("�� ī��� ������ ���θ� ������ �� �ֽ��ϴ�", 0, 2);
        yield return TutorialManager.Instance.ExplainCol("ü���� ī�带 �ߵ��غ��ô�", 0, 2);
        yield return new WaitForSeconds(1f);

        CardManager.Instance.TutorialCardOutLine(102);
        isTurnChange = false;
        yield return new WaitWhile(() => !isChangeDir);
        yield return TutorialManager.Instance.ExplainCol("ü���� ī��� �̵� ������ �ݴ�� �ٲ� �� �ֽ��ϴ�", 0, 2);
        isTurnChange = true;

        yield return new WaitWhile(() => !isStop);
        yield return new WaitForSeconds(3f);

        yield return TutorialManager.Instance.ExplainCol("��ž ī��� �������� �ʰ� ���� �ѱ� �� �ֽ��ϴ�", 0, 2);
        yield return TutorialManager.Instance.ExplainCol("���� �ѹ��� ��� ���ؼ� ��Ʋ ī�带 ����մϴ�", 0, 2);
        yield return TutorialManager.Instance.ExplainCol("��Ʋ ī��� �� ĭ �� �̵��� �� �ֽ��ϴ�", 0, 2);
        CardManager.Instance.TutorialCardOutLine(103);




        Debug.LogWarning("����");

        yield return null;
    }

}
