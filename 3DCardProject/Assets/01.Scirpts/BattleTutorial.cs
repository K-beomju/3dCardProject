using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTutorial : Singleton<BattleTutorial>
{
    public bool isAttak = false;         // ���� ī�带 �ߵ��ؾ��� �� 
    public bool isEnemyTurn = false;     // ����ī�带 ���� �����̱��� ������ �� 
    public bool isTurnChange = false;    // ���� �� ī�带 ������ ����ī�� ������ �� 
    public bool isChangeCard = false;    // ���� �� ī�带 ���� �÷��̾� ������ ��  
    public bool isChangeDir = false;     // ü���� ī�带 �ߵ��ؾ��� �� 
    public bool isStop = false;          // ���� ��ž ī�带 ���� 
    public bool isTrap = false;          // Ʈ��ī�带 �ߵ��ؾ��Ҷ�
    public bool isNullity = false;       // ��ȿī�带 ����Ҽ�������1
    public bool isReflectAble = false;       // ��ȿī�带 ����Ҽ�������2
    public bool isDoneNullity = false;   // ��ȿī�带 ���������

    private void Start()
    {
        if(TutorialManager.Instance.isTutorial)
        {
            StartCoroutine(BattleTutorialCo());
        }
    }

    private IEnumerator BattleTutorialCo()
    {
        yield return TutorialManager.Instance.ExplainCol("\"����\"Ʃ�丮���Դϴ�.", 0); //2.5f ��
        yield return TutorialManager.Instance.ExplainCol("�¸��� ��ȭ�� ȹ���ϰ� �й�� HP�� �پ��ϴ�.", 0);
        yield return TutorialManager.Instance.ExplainCol("HP�� 0 ���Ϸ� �پ��� ���ӿ����Դϴ�.", 0);
        yield return TutorialManager.Instance.ExplainCol("�¸��غ��ô�.", 0, 1, 2);

        yield return new WaitForSeconds(3f);

        yield return TutorialManager.Instance.ExplainCol("���۰� �Բ� 7�� �� 5���� ��ο��մϴ�.", 0,1,3f);
        yield return TutorialManager.Instance.ExplainCol("���� ī��� �÷��̾� ���� �ɶ����� ��ο��մϴ�.", 0,1,3f);

        yield return TutorialManager.Instance.ExplainCol("�Ϲ�ī��� ��� �ʵ忡 �ߵ��Ҽ�������", 0,1,3f);
        SoundManager.Instance.PlayFXSound("HilightField", 0.1f);
        
        Outline hackOutline = FindObjectOfType<Hack>().GetComponent<Outline>();
        hackOutline.OutlineColor = Utils.WhiteColor;
        hackOutline.enabled = true;
        yield return new WaitForSeconds(2f);
        hackOutline.enabled = false;

        yield return TutorialManager.Instance.ExplainCol("��ġī��� \"����\"�� �¿쿡 ��ġ�Ҽ��ֽ��ϴ�.", 0,1,3f);
        SoundManager.Instance.PlayFXSound("HilightField", 0.1f);
        var playernode = NewFieldManager.Instance.GetPlayerNodeByData();
        Outline outline = playernode.NextNode.Data.GetComponent<Outline>();
        Outline outline1 = playernode.PrevNode.Data.GetComponent<Outline>();
        outline.enabled = true;
        outline1.enabled = true;
        yield return new WaitForSeconds(2f);
        outline.enabled = false;
        outline1.enabled = false;

        yield return TutorialManager.Instance.ExplainCol("���� ī�带 �巡���Ͽ� �ߵ��غ��ô�.", 0,1,3f);
        yield return new WaitForSeconds(1f);
        hackOutline.OutlineColor = Utils.WhiteColor;
        hackOutline.enabled = true;
        CardManager.Instance.TutorialCardOutLine(100);

        yield return new WaitWhile(() => !isAttak);

        yield return TutorialManager.Instance.ExplainCol("���� ī��� \"����\"�� �������� ������ �� �ֽ��ϴ�.", 0,1, 2);
        yield return TutorialManager.Instance.ExplainCol("��� ī�� ��� �� \"����\"�� ������ �¸��Դϴ�.", 0,1, 3);
       
        yield return new WaitForSeconds(1f);
        isEnemyTurn = true;
        yield return new WaitForSeconds(1f);
        yield return TutorialManager.Instance.ExplainCol("ī�带 �ߵ��ϸ� \"����\"�� �� ĭ�� �̵��մϴ�.", 0, 1, 2);
        yield return new WaitForSeconds(2f);
        isTurnChange = true;
        yield return new WaitWhile(() => !isChangeCard);
        isChangeCard = false;
        yield return TutorialManager.Instance.ExplainCol("�� ī�尡 ��ġ�Ȱ����� �̵��Ϸ��ϸ� �� ī�常 ������ϴ�.", 0, 1, 2);
        yield return TutorialManager.Instance.ExplainCol("ü���� ī�带 �ߵ��غ��ô�", 0, 1, 2);
        hackOutline.OutlineColor = Utils.WhiteColor;
        hackOutline.enabled = true;
        CardManager.Instance.TutorialCardOutLine(102);
        isChangeCard = true;
        yield return new WaitWhile(() => !isChangeDir);
        yield return new WaitForSeconds(1f);

        isTurnChange = false;
        yield return TutorialManager.Instance.ExplainCol("ü���� ī��� �̵� ������ �ٲ� �� �ֽ��ϴ�", 0, 1, 2);
        isTurnChange = true;
        Debug.Log("ISSTOPA");

        yield return new WaitWhile(() => !isStop);
        Debug.Log("ISSTOPB");
        yield return new WaitForSeconds(3f);

        yield return TutorialManager.Instance.ExplainCol("��ž ī��� �������� �ʰ� ���� �ѱ� �� �ֽ��ϴ�", 0, 1, 2);

        yield return TutorialManager.Instance.ExplainCol("�� ī�带 �ߵ��� ���ô�", 0, 1, 2);

        NewFieldManager.Instance.GetPlayerNodeByData().PrevNode.Data.isEnterRange = false;
        outline = NewFieldManager.Instance.GetPlayerNodeByData().NextNode.Data.GetComponent<Outline>();
        outline.OutlineColor = Utils.WhiteColor;
        outline.enabled = true;
        CardManager.Instance.TutorialCardOutLine(106);
        isTurnChange = true;

        yield return new WaitWhile(() => !isTrap);
        yield return new WaitForSeconds(1f);

        yield return TutorialManager.Instance.ExplainCol("\"����\"�� �� ī�带 ��´ٸ� ������ ī���� �ϳ��� �����ϴ�.", 0, 1, 2);
        isTurnChange = false;

        yield return new WaitWhile(() => !isNullity);
        yield return TutorialManager.Instance.ExplainCol("���� ����ī�带 ����߽��ϴ�.", 0);
        yield return TutorialManager.Instance.ExplainCol("��ȿī��� ���п� ������ ������ �ߵ��� ������ �� �ֽ��ϴ�.", 0, 1, 2);
        yield return TutorialManager.Instance.ExplainCol("��ȿī�带 ����غ��ô�.", 0, 1, 2);
        isReflectAble = true;
        yield return new WaitWhile(() => !isDoneNullity);

        yield return new WaitWhile(() => !isChangeCard);
        isTurnChange = true;
        yield return new WaitForSeconds(3f);

        yield return TutorialManager.Instance.ExplainCol("��Ʋ ī��� �� ĭ �� �̵��� �� �ֽ��ϴ�", 0, 1, 2);
        yield return TutorialManager.Instance.ExplainCol("��Ʋ ī�带 ����Ͽ� ���� ��� ���ô�.", 0, 1, 2);

        NewFieldManager.Instance.GetPlayerNodeByData().NextNode.Data.isEnterRange = false;
        outline = NewFieldManager.Instance.GetPlayerNodeByData().PrevNode.Data.GetComponent<Outline>();
        outline.OutlineColor = Utils.WhiteColor;
        outline.enabled = true;
        CardManager.Instance.TutorialCardOutLine(103);
        yield return null;
    }

}
