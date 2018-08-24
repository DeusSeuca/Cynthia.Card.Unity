using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cynthia.Card.Client;
using Autofac;
using Cynthia.Card;
using UnityEngine.EventSystems;
using System.Linq;
using Alsein.Utilities;
using System.Threading.Tasks;
using Alsein.Utilities.IO;

public class GameEvent : MonoBehaviour
{
    public CanDrop EnemyRow1;
    public CanDrop EnemyRow2;
    public CanDrop EnemyRow3;
    public CanDrop MyRow1;
    public CanDrop MyRow2;
    public CanDrop MyRow3;
    public CanDrop MyPlance;
    public CanDrop EnemyPlance;
    public CanDrop MyCemetery;
    public CardsPosition MyHand;
    public LeaderCard MyLeader;
    //----
    public GameObject CardPrefab;
    //管道...
    private IAsyncDataSender sender;
    private IAsyncDataReceiver receiver;
    private void Awake()
    {
        (sender, receiver) = AsyncDataEndPoint.CreateSimplex();
    }
    //状态信息
    public static CanDrop DropTaget;
    private static CardMoveInfo _selectCard = null;
    private static CardMoveInfo _dragCard;
    //设置当前场地显示
    private CardUseInfo _currentPlace;
    public CardUseInfo CurrentPlace
    {
        get => _currentPlace;
        set
        {
            if (_currentPlace == value) return;
            _currentPlace = value;
            EnemyRow1.IsCanDrop = false;
            EnemyRow2.IsCanDrop = false;
            EnemyRow3.IsCanDrop = false;
            EnemyPlance.IsCanDrop = false;
            MyRow1.IsCanDrop = false;
            MyRow2.IsCanDrop = false;
            MyRow3.IsCanDrop = false;
            MyPlance.IsCanDrop = false;
            switch (_currentPlace)
            {
                case CardUseInfo.ReSet:
                    break;
                case CardUseInfo.MyPlace:
                    MyPlance.IsCanDrop = true;
                    break;
                case CardUseInfo.EnemyPlace:
                    EnemyPlance.IsCanDrop = true;
                    break;
                case CardUseInfo.AnyPlace:
                    EnemyPlance.IsCanDrop = true;
                    MyPlance.IsCanDrop = true;
                    break;
                case CardUseInfo.MyRow:
                    MyRow1.IsCanDrop = true;
                    MyRow2.IsCanDrop = true;
                    MyRow3.IsCanDrop = true;
                    break;
                case CardUseInfo.EnemyRow:
                    EnemyRow1.IsCanDrop = true;
                    EnemyRow2.IsCanDrop = true;
                    EnemyRow3.IsCanDrop = true;
                    break;
                case CardUseInfo.AnyRow:
                    EnemyRow1.IsCanDrop = true;
                    EnemyRow2.IsCanDrop = true;
                    EnemyRow3.IsCanDrop = true;
                    MyRow1.IsCanDrop = true;
                    MyRow2.IsCanDrop = true;
                    MyRow3.IsCanDrop = true;
                    break;
            }
        }
    }
    /// <summary>
    /// 当前正在拖拽的卡牌
    /// </summary>
    public static CardMoveInfo DragCard
    {
        get=>_dragCard;
        set
        {
            if (_dragCard == value) return;
            if (_dragCard != null)//放弃当前所拖
            {
                _dragCard.IsDrag = false;
                _dragCard.zPosition += 2f;
            }
            _dragCard = value;
            if (value == null)return;
            _dragCard.IsDrag = true;
            _dragCard.zPosition -= 2f;
        }
    }
    /// <summary>
    /// 当前选择
    /// </summary>
    public static CardMoveInfo SelectCard
    {
        get => _selectCard;
        set
        {
            if (_selectCard == value) return;
            if (_selectCard != null)
            {
                _selectCard.transform.localScale = new Vector3(1, 1, 1);
                _selectCard.zPosition += 1f;
            }
            _selectCard = value;
            if (value == null) return;
            if (!_selectCard.IsCanSelect)
            {
                _selectCard = null;
                return;
            }
            _selectCard.transform.localScale = new Vector3(1.05f, 1.05f, 1);
            _selectCard.zPosition -= 1f;
        }
    }//---------------------------------------------------------------------------------
    /// <summary>
    /// 鼠标落下
    /// </summary>
    private void OnMouseDown()
    {
        var onObjects = GetMouseAllRaycast();
        if (SelectCard == null||!SelectCard.IsCanDrag) return;
        DragCard = SelectCard;
        CurrentPlace = DragCard.CardUseInfo;
        SelectCard = null;
    }
    /// <summary>
    /// 鼠标抬起
    /// </summary>
    private void OnMouseUp()
    {
        if (DragCard == null) return;
        if (DropTaget != null)
        {
            var dragCard = DragCard;
            DragCard = null;
            var index = GetDropIndex(GetRelativePosition(DropTaget), DropTaget.CardsPosition);
            DropTaget.CardsPosition.AddCard(dragCard,index);//应该返回
            Debug.Log($"将卡牌【{GetIndex(dragCard.transform)}】拖拽到了【Id:{DropTaget.Id}】位置");
            sender.SendAsync<RoundInfo>
            (
                new RoundInfo()
                {
                    HandCardIndex = GetIndex(dragCard.transform),
                    RowIndex = DropTaget.Id,
                    CardIndex = index,
                    IsPass = false
                }
            );
            DropTaget = null;
        }
        DragCard = null;
        CurrentPlace = CardUseInfo.ReSet;
    }
    /// <summary>
    /// 每一帧
    /// </summary>
    private void Update()
    {
        var onObjects = GetMouseAllRaycast();//获取鼠标穿透的所有物体
        if (DragCard == null)//如果没有处于拖拽状态
        {
            var cards = onObjects.Where(x => x.GetComponent<CardMoveInfo>()!=null);//获取物体集合中的所有卡牌
            if (cards.Count() == 0) SelectCard = null;
            else SelectCard = cards.First().GetComponent<CardMoveInfo>();
            return;
        }
        //如果正在拖拽的话..
        var rows = onObjects.Where(x => x.GetComponent<CanDrop>() != null&& x.GetComponent<CanDrop>().IsCanDrop);
        var dropTaget = default(CanDrop);
        if (rows.Count() == 0) dropTaget = null;
        else
        {
            dropTaget = rows.First().GetComponent<CanDrop>().IsCanDrop ? rows.First().GetComponent<CanDrop>() : null;//可拖上的第一个
            dropTaget = (dropTaget.CardsPosition.MaxCards + (dropTaget.CardsPosition.IsTem() ? 1 : 0)) <= dropTaget.CardsPosition.transform.childCount ? null : dropTaget;//数量没满的第一个
        }
        if (DropTaget != dropTaget && DropTaget != null)DropTaget.CardsPosition.AddTemCard(null, -1);
        DropTaget = dropTaget;
        if (DropTaget != null)
        {
            var index = GetDropIndex(GetRelativePosition(DropTaget),DropTaget.CardsPosition);
            DropTaget.CardsPosition.AddTemCard(DragCard.CardShowInfo.CurrentCore, index);
            //Debug.Log($"将卡牌拖到了该排第{index}个位置");//##################卡牌虚化效果
        }
    }
    private Vector3 GetRelativePosition(CanDrop dropTaget)
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit);
        return dropTaget.transform.InverseTransformPoint(hit.point);
    }
    private int GetDropIndex(Vector3 position,CardsPosition container)
    {
        var count = container.transform.childCount;
        count -= container.IsTem() ? 1 : 0;
        var index = (position.x + container.Size * (count-1) / 2);
        index /= container.Size;
        index += 1;
        index = index < 0 ? 0 : index;
        index = index > count ? count : index;
        return (int)index;
    }
    /// <summary>
    /// 获得当前鼠标穿过的所有物体
    /// </summary>
    private IEnumerable<GameObject> GetMouseAllRaycast()
    {
        var ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        var ray = new Ray(ray1.origin, ray1.direction * 100000);
        return  Physics.RaycastAll(ray).Select(x=>x.collider.gameObject);
    }
    /// <summary>
    /// 获得某个物体在父物体中的位置
    /// </summary>
    private int GetIndex(Transform obj)
    {
        var p = obj.parent;
        var count = p.childCount;
        for(var i = 0; i < count; i++)
        {
            if (p.GetChild(i) == obj)
                return i;
        }
        return 0;
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// 让玩家做某个举动
    /// </summary>
    public async Task<RoundInfo> GetPlayerDrag()
    {//解除操作限制,得到情报,增加操作限制,返回结果
        MyHand.CardsCanDrag(true);
        MyLeader.SetCanDrag(true);
        var result = await receiver.ReceiveAsync<RoundInfo>();
        MyHand.CardsCanDrag(false);
        MyLeader.SetCanDrag(false);
        return result;
    }
    public void ResetAllTem()
    {
        EnemyRow1.CardsPosition.AddTemCard(null,-1);
        EnemyRow2.CardsPosition.AddTemCard(null, -1);
        EnemyRow3.CardsPosition.AddTemCard(null, -1);
        MyRow1.CardsPosition.AddTemCard(null, -1);
        MyRow2.CardsPosition.AddTemCard(null, -1);
        MyRow3.CardsPosition.AddTemCard(null, -1);
    }
}
