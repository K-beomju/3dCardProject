using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer card;
    [SerializeField] private SpriteRenderer cardImage;

    [SerializeField] private SpriteRenderer crystal;
    [SerializeField] private SpriteRenderer cardBorder;

    [SerializeField] private Sprite crystal_blue;
    [SerializeField] private Sprite crystal_red;

    [SerializeField] private TMP_Text costTMP;
    [SerializeField] private TMP_Text atkTMP;
    [SerializeField] private TMP_Text hpTMP;
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text descriptionTMP;
    [SerializeField] private GameObject modelPrefab;
    [SerializeField] private GameObject outline;
    [SerializeField] private TMP_Text typeTMP;

    [field:SerializeField]
    public CardModelBrain LinkedModel { get; private set; }

    public GameObject avtar;
    public Animator avtarAnim;

    public Item item;
    public PRS originPRS;
    private bool isFront;
    public bool isOnField = false;

    public bool isPlayerCard = true;

    public Field curField;
    public bool isAttack = false;
    public bool canInteract = true;

    public bool isDisposable = false;

    public Action OnSpawnAct;
    public void Setup(Item item, bool isFront, bool isPlayerCard)
    {
        this.isPlayerCard = isPlayerCard;
        this.item = item;
        this.isFront = isFront;

        if (this.isFront)
        {
            RefreshInform();
            
        }
        else
        {
            atkTMP.text = "";
            nameTMP.text = "";
            costTMP.text = "";
            descriptionTMP.text = "";
            typeTMP.text = "";
        }
    }

    void OnMouseOver()
    {
        if (StageManager.Instance.SceneState == SceneState.STAGE) return;
        if (curField != null||isDisposable)
        {
            CardInfoUI.Instance.ItemData = item;
        }

        if (canInteract && !isDisposable)
        {
            //Debug.Log("ABABA");
            CardManager.Instance.CardMouseOver(this);
          
        }

    }

    void OnMouseExit()
    {
        if (StageManager.Instance.SceneState == SceneState.STAGE) return;
        if(!isDisposable)
        {
            CardManager.Instance.CardMouseExit(this);
        }
        CardInfoUI.Instance.ActiveUI(false);
    }

    void OnMouseDown()
    {
        if (StageManager.Instance.SceneState == SceneState.STAGE || !isPlayerCard) return;
        if (isDisposable || StageManager.Instance.SceneState == SceneState.Title)
        {
            CardManager.Instance.ArrowMove(this,false);

            return;
        }
        
        if (TurnManager.Instance != null && TurnManager.CurReturnType() != TurnType.Player) return;

        CardManager.Instance.CardMouseDown(this);

        //if (!isOnField)
        //{
        //    if (!CardManager.Instance.MyCardIsFull())
        //    {
        //        CardManager.Instance.DeleteMyCard(this);
        //        CardManager.Instance.CardAlignment();
        //    }
        //}


    }

    void OnMouseUp()
    {
        if (StageManager.Instance.SceneState == SceneState.STAGE) return;
        CardManager.Instance.CardMouseUp();
    }

    public void SetDeleteObject()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject, 1);
    }
    public void RefreshInform()
    {
        cardImage.sprite = this.item.sprite;
        crystal.sprite = isPlayerCard ? crystal_blue : crystal_red;
        nameTMP.text = this.item.itemName;
        costTMP.text = this.item.cost.ToString();
        descriptionTMP.text = this.item.description;
        string tmpTxt = "";
        if(this.item.IsAvatar)
        {
            tmpTxt = "아바타";
        }
        else if(this.item.IsStructCard)
        {
            tmpTxt = "설치";
        }
        else if(this.item.IsReflectCard)
        {
            tmpTxt = "받아치기";
        }
        else
        {
            tmpTxt = "일반";
        }
        typeTMP.text = tmpTxt;
    }
    
    public void MoveTransform(PRS prs, bool useDotween, float dotWeenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotWeenTime);
            transform.DORotateQuaternion(prs.rot, dotWeenTime);
            transform.DOScale(prs.scale, dotWeenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    public void Emphasize(Action act)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleY(transform.localScale.y + .5f, 0.15f).SetLoops(2, LoopType.Yoyo));
        seq.Join(transform.DOScaleX(transform.localScale.x + .5f, 0.15f).SetLoops(2, LoopType.Yoyo));
        seq.AppendCallback(() => {  act?.Invoke(); });
    }
   
    public void Attack(Field field,Action act = null)
    {
        if (!isAttack && (field.curCard != this || field.upperCard != this||field.avatarCard != this))
        {
            isAttack = true;
            int originOrder = GetComponent<Order>().GetOriginalOrder();
            GetComponent<Order>().SetOriginOrder(10);
            Vector3 firstPos = transform.position;
            Vector3 fieldPos = field.transform.position;
            fieldPos.y += 1f;
            avtar.transform.DOMove(fieldPos, .15f).SetEase(Ease.InElastic).OnComplete(() => {

                avtar.transform.DOMove(firstPos, .3f).OnComplete(() => {
                    act?.Invoke();
                    GetComponent<Order>().SetOriginOrder(originOrder);
                    if (field.curCard != null)
                    {
                        Debug.Log("카드 공격");
                        field.curCard.OnDamage();
                    }
                    else if(field.upperCard != null)
                    {
                        Debug.Log("Upper 카드 공격");
                        field.upperCard.OnDamage();
                    }
                    else if( field.avatarCard != null)
                    {
                        Debug.Log("아바타 카드 공격");
                        field.avatarCard.OnDamage();
                        avtarAnim.SetTrigger("Attack");
                    }
                    Debug.Log("ATACK");
                    isAttack = false;
                    TurnManager.Instance.CanChangeTurn = true;
                    OnAttack();
                });
            });
        }
    }

    // 덫 부분 구현
    public void CommonAction(Field field)
    {
        if (field.curCard != this)
        {
            if (field.curCard != null)
            {
                field.curCard.OnAttack();
            }
        }
    }


    public void OnCreateCard()
    {
        Debug.Log("OnCreate : " + item.itemName);

        CardAction(item.OnCreate);
    }
    public void OnAttack()
    {
        Debug.Log("ONATTACK : " + item.itemName);
        if(item.HitEffectPrefab != null)
        Instantiate(item.HitEffectPrefab).transform.position = transform.position + new Vector3(0, 1, 0);

        CardAction(item.OnAttack);
    }
    public void OnDamage()
    {
        Debug.Log("ONDAMAGE: " + item.itemName);
        CardAction(item.OnDamage);
    }
    public void OnDie()
    {
        Debug.Log("ONDIE: " + item.itemName);
        if (LinkedModel != null)
            Destroy(LinkedModel.ModelObject.gameObject);

        CardAction(item.OnDie);
       
    }
    public void OnSpawn()
    {
        Debug.Log("ONSPAWN : " + item.itemName);
        Emphasize(() =>
        {
            OnSpawnAct?.Invoke();
            if(item.IsStructCard || item.IsAvatar)
            {
                Debug.Log("모델 생성 시작 : " + item.itemName);

                LinkedModel = Instantiate(modelPrefab, transform.position - new Vector3(0, item.SpawnModelYPos,0), Utils.QI).GetComponent<CardModelBrain>();
                var model = Resources.Load<GameObject>(item.uid.ToString());
                if (model != null)
                {
                    LinkedModel.ModelObject = model;
                    avtar = LinkedModel.ModelObject;
                    avtarAnim = avtar.GetComponent<Animator>();
                    if(!isPlayerCard)
                    {
                        avtar.transform.Rotate(new Vector3(0, -180, 0));
                    }
                }

                DetactiveCardView();
            }
            //종류별로 넣어주시면 됩니다
            Effect effect= null;
            switch (item.cardType)
            {
                case CardType.Jump:
                case CardType.Wall:
                case CardType.Trap:
                    effect = Global.Pool.GetItem<Effect_Spawn>();
                    break;
                case CardType.Attack:
                    break;
                case CardType.Avoid:
                    break;
                case CardType.Change:
                    break;
                case CardType.Stop:
                    break;
                default:
                    break;
            }
            if(effect != null)
            {
                effect.transform.position = transform.position + new Vector3(0, 1, 0);
            }
            CardManager.Instance.LastUsedCardItem = item.ShallowCopy();
            GetComponent<Order>().SetEnable(false);
            CardAction(item.OnSpawn);
        });
    }

    private void CardAction(CardActionCondition[] act)
    {
        if (act.Length < 1) return;

        foreach (var item in act)
        {
            if (item == null || item.action == null)
            {
                continue;
            }
            int check = 0;
            foreach (var condition in item.condition)
            {
                if (!condition.CheckCondition(this))
                {
                    check++;
                }
            }
            if (check == 0)
                item.action.TakeAction(this);
        }
    }

    public Card ShallowCopy()
    {
        return (Card)this.MemberwiseClone();
    }

    public void DetactiveCardView()
    {
        GetComponentInChildren<SpriteMask>().enabled = false;
        card.enabled = false;
        cardImage.enabled = false;
        cardBorder.enabled = false;
        nameTMP.GetComponent<MeshRenderer>().enabled = false;
        descriptionTMP.GetComponent<MeshRenderer>().enabled = false;
        typeTMP.GetComponent<MeshRenderer>().enabled = false;
        
    }

    public void DeleteTrapCard()
    {
        card.DOFade(0, 1);
        cardImage.DOFade(0, 1);
        cardBorder.DOFade(0, 1);
        nameTMP.DOFade(0, 1);
        descriptionTMP.DOFade(0, 1);

    }

    public void SelectOutlineCard()
    {
        outline.SetActive(true);
    }

}
