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
    //可被拖上(6排,以及我方墓地)
    public CardsPosition[] AllCardsPosition;
    public CanDrop[] AllCanDrop;
    public CanDrop EnemyRow1;
    public CanDrop EnemyRow2;
    public CanDrop EnemyRow3;
    public CanDrop MyRow1;
    public CanDrop MyRow2;
    public CanDrop MyRow3;
    public CanDrop MyPlance;
    public CanDrop EnemyPlance;
    public CanDrop MyCemetery;//我方墓地可以被拖上

    public GameCardShowControl GameCardShowControl;
    //三个定位点
    public Transform EnemyCemetery;
    public Transform MyDeck;
    public Transform EnemyDeck;

    public CardsPosition MyHand;
    public CardsPosition EnemyHand;

    public CardsPosition MyStayCards;
    public CardsPosition EnemyStayCards;

    public PassCoin PassCoin;
    public LeaderCard MyLeader;
    public LeaderCard EnemyLeader;

    public PassCoin Coin;
    //----
    public GameObject CardPrefab;
    //管道...
    private IAsyncDataSender sender;
    private IAsyncDataReceiver receiver;
    private void Awake() => (sender, receiver) = AsyncDataEndPoint.CreateSimplex();
    //状态信息
    public static CanDrop DropTaget;
    private static CardMoveInfo _selectCard;
    private static CardMoveInfo _dragCard;
    //当前处理的卡牌
    private static CardMoveInfo _myUseCard;
    private static CardMoveInfo _enemyUseCard;
    //是否鼠标停留在硬币上
    private bool IsSelectCoin;
    private bool IsOnCoin;
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
            MyCemetery.IsCanDrop = false;
            switch (_currentPlace)
            {
                case CardUseInfo.ReSet:
                    return;
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
            MyCemetery.IsCanDrop = true;
        }
    }
    private void Start()
    {
        //某些信息
    }

    //当前正在拖拽的卡牌
    public static CardMoveInfo DragCard
    {
        get=>_dragCard;
        set
        {
            if (_dragCard == value) return;
            if (_dragCard != null)//放弃当前所拖
            {
                _dragCard.IsDrag = false;
                _dragCard.ZPosition += 2f;
            }
            _dragCard = value;
            if (value == null)return;
            _dragCard.IsDrag = true;
            _dragCard.ZPosition -= 2f;
        }
    }

    //当前选择
    public static CardMoveInfo SelectCard
    {
        get => _selectCard;
        set
        {
            if (_selectCard == value) return;
            if (_selectCard != null)
            {
                _selectCard.transform.localScale = new Vector3(1, 1, 1);
                _selectCard.ZPosition += 1f;
            }
            _selectCard = value;
            if (value == null) return;
            if (!_selectCard.IsCanSelect||_selectCard.IsOn||_selectCard.IsStay)
            {
                _selectCard = null;
                return;
            }
            _selectCard.transform.localScale = new Vector3(1.05f, 1.05f, 1);
            _selectCard.ZPosition -= 1f;
        }
    }
    //---------------------------------------------------------------------------------

    //鼠标落下
    private void OnMouseDown()
    {
        if (IsSelectCoin)
        {
            IsSelectCoin = false;
            IsOnCoin = true;//按住了！
        }
        if (SelectCard == null||!SelectCard.IsCanDrag||SelectCard.IsStay) return;
        DragCard = SelectCard;
        CurrentPlace = DragCard.CardUseInfo;
        SelectCard = null;
    }

    //鼠标抬起
    private void OnMouseUp()
    {
        if (IsOnCoin)//此方法会被改到其他地方
        {
            sender.SendAsync<RoundInfo>(new RoundInfo(){IsPass = true});
            IsOnCoin = false;
            return;
        }
        if (DragCard == null) return;
        if (DropTaget != null)
        {
            //var dragCard = DragCard;
            //DragCard = null;
            //var index = GetDropIndex(GetRelativePosition(DropTaget), DropTaget.CardsPosition);
            //var handIndex = GetIndex(dragCard.transform);
            //当前排的第几个位置
            //----------------------------------------------------------------------------------
            /*if (DropTaget.IsRowDrop)
            {//单位卡的放置
                DropTaget.CardsPosition.AddCard(dragCard, index);
                //Debug.Log("放置了单位卡");
            }
            else
            {//法术卡的放置
                if(DropTaget.CardsPosition!=null)
                    DropTaget.CardsPosition.AddCard(dragCard, 0);
                if (DropTaget.Id == RowPosition.MyCemetery)
                {
                    CardMoveBack(dragCard, MyCemetery.transform);
                }
                //Debug.Log("放置了法术卡");
            }
            if (_myUseCard != null)
                MyCardEffectEnd();
            if (DropTaget.Id != RowPosition.MyCemetery)
            {
                _myUseCard = dragCard;
                _myUseCard.IsOn = true;
            }
            else*/
            //Debug.Log($"将卡牌【{handIndex}】拖拽到了【Id:{DropTaget.Id}】位置");
            //----------------------------------------------------------------------------------
            //回应服务器的请求
            DragCard.IsStay = true;
            ResetAllTem();//
            sender.SendAsync<RoundInfo>
            (
                new RoundInfo()
                {
                    HandCardIndex = GetIndex(DragCard.transform),
                    CardLocation = new CardLocation()
                    {
                        RowPosition = DropTaget.Id,
                        CardIndex = GetDropIndex(GetRelativePosition(DropTaget), DropTaget.CardsPosition)
                    },
                    IsPass = false,
                }
            );
            //-----------------------------------------------------------------------------------
            DropTaget = null;
        }
        DragCard = null;
        CurrentPlace = CardUseInfo.ReSet;
    }

    //右键点击之类的
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var items = GetMouseAllRaycast();
            var trueitem = items.Select(x=>x.GetComponent<CanRightOn>()).Where(x=>x!=null);
            if (trueitem.Count() == 0)
                return;
            switch (trueitem.First().Type)
            {
                case RightOnType.MyCemetery:
                    GameCardShowControl.ShowMyCemetery();
                    break;
                case RightOnType.EnemyCemetery:
                    GameCardShowControl.ShowEnemyCemetery();
                    break;
                case RightOnType.Card:
                    //待补充
                    break;
            }
        }
    }
    
    //每一帧
    private void Update()
    {
        //if (IsOnCoin)按住硬币会执行的
        var onObjects = GetMouseAllRaycast();//获取鼠标穿透的所有物体
        if (DragCard == null)//如果没有在拖拽的卡牌
        {
            var cards = onObjects.Where(x => x.GetComponent<CardMoveInfo>()!=null);//获取物体集合中的所有卡牌
            if (cards.Count() == 0) SelectCard = null;
            else SelectCard = cards.OrderBy(x=>x.transform.position.z).First().GetComponent<CardMoveInfo>();
            if (SelectCard == null)
            {
                var coin = onObjects.Where(x => x.GetComponent<PassCoin>() != null);
                if (coin.Count() != 0 && coin.First().GetComponent<PassCoin>().IsCanUse)//当鼠标移动到硬币上的时候
                {
                    if(!IsOnCoin)//如果没有按在硬币上
                        IsSelectCoin = true;//选中的硬币
                }
                else//如果没有移动在硬币上
                {
                    IsSelectCoin = false;//取消选中硬币
                    IsOnCoin = false;//取消按住硬币
                }
            }
            return;
        }
        //如果正在拖拽的话..
        var rows = onObjects.Where(x => x.GetComponent<CanDrop>() != null&& x.GetComponent<CanDrop>().IsCanDrop);
        var dropTaget = default(CanDrop);
        if (rows.Count() == 0) dropTaget = null;
        else
        {
            dropTaget = rows.First().GetComponent<CanDrop>().IsCanDrop ? rows.First().GetComponent<CanDrop>() : null;//可拖上的第一个
            if (!dropTaget.IsRowDrop)
            {//如果不是行拖拽区域,则没有悬停效果
                DropTaget = dropTaget;
                return;
            }
            //数量判断
            dropTaget = (dropTaget.CardsPosition.MaxCards + (dropTaget.CardsPosition.IsTem() ? 1 : 0)) <= dropTaget.CardsPosition.GetCardCount() ? null : dropTaget;//数量没满的第一个
        }
        if (DropTaget!=null&&DropTaget.CardsPosition == null)
        {
            DropTaget = dropTaget;
            return;
        }
        //悬停效果处理
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
        if (container == null)
            return -1;
        var count = container.GetCardCount();
        count -= container.IsTem() ? 1 : 0;
        var index = (position.x + container.XSize * (count-1) / 2);
        index /= container.XSize;
        index += 1;
        index = index < 0 ? 0 : index;
        index = index > count ? count : index;
        return (int)index;
    }

    //获得当前鼠标穿过的所有物体
    private IEnumerable<GameObject> GetMouseAllRaycast()
    {
        var ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        var ray = new Ray(ray1.origin, ray1.direction * 100000);
        return  Physics.RaycastAll(ray).Select(x=>x.collider.gameObject);
    }

    //获得某个物体在父物体中的位置
    private int GetIndex(Transform obj)
    {
        var p = obj.parent;
        if (p.gameObject.GetComponent<LeaderCard>() != null)
            return -1;
        var count = p.childCount;
        for(var i = 0; i < count; i++)
        {
            if (p.GetChild(i) == obj)
                return i;
        }
        return 0;
    }

    //将某个卡牌丢到墓地或者卡组
    public void CardMoveBack(CardMoveInfo card,Transform taget)
    {
        var source = card.transform.parent.gameObject.GetComponent<CardsPosition>();
        card.transform.SetParent(taget);
        card.SetResetPoint(Vector3.zero);
        card.transform.localScale = Vector3.one;
        card.IsCanSelect = false;
        Destroy(card.gameObject, 0.8f);
        if (source != null)
            source.ResetCards();
    }

    //------------------------------------------------------------------------------
    //一些小方法
    public CardMoveInfo GetCard(CardLocation location)
    {
        if (location.RowPosition == RowPosition.MyLeader)
            return MyLeader.TrueCard.GetComponent<CardMoveInfo>();
        if (location.RowPosition == RowPosition.EnemyLeader)
            return EnemyLeader.TrueCard.GetComponent<CardMoveInfo>();
        return AllCardsPosition.Single(x => x.Id == location.RowPosition)
            .transform.GetChild(location.CardIndex).GetComponent<CardMoveInfo>();
    }
    //------------------------------------------------------------------------------
    //以下为给服务端调用的方法
    public void CardMove(MoveCardInfo info)//卡牌移动
    {
        var soureCard = default(CardMoveInfo);
        var tagetRow = default(CardsPosition);
        switch (info.Soure.RowPosition)//几个特殊的移动原始位置
        {
            case RowPosition.MyLeader:
                if (MyLeader.TrueCard == null) return;
                soureCard = MyLeader.TrueCard.GetComponent<CardMoveInfo>();
                break;
            case RowPosition.EnemyLeader:
                if (EnemyLeader.TrueCard == null) return;
                soureCard = EnemyLeader.TrueCard.GetComponent<CardMoveInfo>();
                break;
            case RowPosition.MyDeck:
                soureCard = Instantiate(CardPrefab, MyDeck).GetComponent<CardMoveInfo>();
                break;
            case RowPosition.EnemyDeck:
                soureCard = Instantiate(CardPrefab, EnemyDeck).GetComponent<CardMoveInfo>();
                break;
            case RowPosition.MyCemetery:
                soureCard = Instantiate(CardPrefab, MyCemetery.transform).GetComponent<CardMoveInfo>();
                break;
            case RowPosition.EnemyCemetery:
                soureCard = Instantiate(CardPrefab, EnemyCemetery).GetComponent<CardMoveInfo>();
                break;
            case RowPosition.SpecialPlace://这是个错误值
                break;
            default:
                soureCard = GetCard(info.Soure);
                break;
        }
        //以上是来源卡牌
        if (soureCard == null) return;
        if(info.Card!=null)
        {//如果卡牌有额外信息,直接替换掉当前选中的卡牌(例如从牌库,或者手牌抽出的卡牌)
            soureCard.CardShowInfo.CurrentCore = info.Card;
            soureCard.CardShowInfo.SetCard();
        }
        soureCard.IsStay = false;
        //------------------------------------------
        //移动到
        switch (info.Taget.RowPosition)
        {
            case RowPosition.MyDeck:
                CardMoveBack(soureCard, MyDeck);
                break;
            case RowPosition.EnemyDeck:
                CardMoveBack(soureCard, EnemyDeck);
                break;
            case RowPosition.MyCemetery:
                CardMoveBack(soureCard, MyCemetery.transform);
                break;
            case RowPosition.EnemyCemetery:
                CardMoveBack(soureCard, EnemyCemetery.transform);
                break;
            case RowPosition.MyLeader:
            case RowPosition.EnemyLeader:
            case RowPosition.SpecialPlace:
                return;
            default:
                tagetRow = AllCardsPosition.Single(x => x.Id == info.Taget.RowPosition);
                tagetRow.AddCard(soureCard, info.Taget.CardIndex);
                break;
        }
    }

    public void CardOn(CardLocation location)//卡牌抬起
    {
        var card = GetCard(location);
        card.IsOn = true;
    }

    public void CardDown(CardLocation location)//卡牌落下
    {
        MyLeader.AutoSet();
        EnemyLeader.AutoSet();
        //上面这两行偷懒了,或许以后会改
        var card = GetCard(location);
        card.IsOn = false;
    }

    //展示将一些卡丢到墓地
    public void ShowCardsToCemetery(GameCardsPart cards)
    {
        cards.MyRow1Cards.Select(x => MyRow1.CardsPosition.transform.GetChild(x).GetComponent<CardMoveInfo>())
        .ForAll(x => CardMoveBack(x, MyCemetery.transform));
        cards.MyRow2Cards.Select(x => MyRow2.CardsPosition.transform.GetChild(x).GetComponent<CardMoveInfo>())
        .ForAll(x => CardMoveBack(x, MyCemetery.transform));
        cards.MyRow3Cards.Select(x => MyRow3.CardsPosition.transform.GetChild(x).GetComponent<CardMoveInfo>())
        .ForAll(x => CardMoveBack(x, MyCemetery.transform));
        cards.EnemyRow1Cards.Select(x => EnemyRow1.CardsPosition.transform.GetChild(x).GetComponent<CardMoveInfo>())
        .ForAll(x => CardMoveBack(x, EnemyCemetery.transform));
        cards.EnemyRow2Cards.Select(x => EnemyRow2.CardsPosition.transform.GetChild(x).GetComponent<CardMoveInfo>())
        .ForAll(x => CardMoveBack(x, EnemyCemetery.transform));
        cards.EnemyRow3Cards.Select(x => EnemyRow3.CardsPosition.transform.GetChild(x).GetComponent<CardMoveInfo>())
        .ForAll(x => CardMoveBack(x, EnemyCemetery.transform));
    }
    /*
    //显示对手出的牌
    public void EnemyDrag(RoundInfo enemyRoundInfo, CardStatus cardInfo)
    {
        var card = default(CardMoveInfo);
        //当前排的第几个位置
        //----------------------------------------------------------------------------------
        if (enemyRoundInfo.HandCardIndex == -1)
            card = EnemyLeader.transform.GetChild(0).gameObject.GetComponent<CardMoveInfo>();
        else
            card = EnemyHand.transform.GetChild(enemyRoundInfo.HandCardIndex).gameObject.GetComponent<CardMoveInfo>();
        card.GetComponent<CardShowInfo>();
        card.CardShowInfo.CurrentCore = cardInfo;
        card.CardShowInfo.SetCard();
        //这里是弃牌
        if (enemyRoundInfo.CardLocation.RowPosition == RowPosition.EnemyCemetery)
        {
            CardMoveBack(card, EnemyCemetery);
            return;
        }
        //------------------------------------------------------------
        //以下是正常出牌
        var enemyDrop = AllCanDrop.First(x => x.Id == enemyRoundInfo.CardLocation.RowPosition);
        //获得卡牌
        if (enemyDrop.IsRowDrop)
        {
            enemyDrop.CardsPosition.AddCard(card, enemyRoundInfo.CardLocation.CardIndex);
            card.IsOn = true;
        }
        else
        {//法术卡的放置
            EnemyStayCards.AddCard(card, 0);
        }
        if (_enemyUseCard != null)//如果敌人的已经有使用的卡牌
            EnemyCardEffectEnd();//结束卡牌的效果
        _enemyUseCard = card;//将当前卡牌设置为目前使用卡牌
        _enemyUseCard.IsOn = true;//将卡牌设置成"使用中卡牌"
    }*/

    //让玩家使用一个卡牌,或者pass
    public async Task<RoundInfo> GetPlayerDrag()//（RoundStart）
    {
        //解除操作限制,得到情报,增加操作限制,返回结果
        PassCoin.IsCanUse = true;//硬币可用
        MyHand.CardsCanDrag(true);
        MyLeader.SetCanDrag(true);
        var result = await receiver.ReceiveAsync<RoundInfo>();
        PassCoin.IsCanUse = false;//硬币不可用
        MyHand.CardsCanDrag(false);
        MyLeader.SetCanDrag(false);
        return result;
    }
    /*
    //创建一张卡牌
    public void GetCardFrom(RowPosition createPosition,RowPosition taget,int index, CardStatus cardInfo)
    {
        var position = default(Transform);
        switch (createPosition)
        {
            case RowPosition.MyCemetery:
                position = MyCemetery.transform;
                break;
            case RowPosition.MyDeck:
                position = MyDeck;
                break;
            case RowPosition.EnemyCemetery:
                position = EnemyCemetery;
                break;
            case RowPosition.EnemyDeck:
                position = EnemyDeck;
                break;
            default://只能从墓地和手牌中获取牌
                return;
        }
        var card = Instantiate(CardPrefab, position).GetComponent<CardShowInfo>();
        card.CurrentCore = cardInfo;
        card.SetCard();
        var tagetRow = AllCardsPosition.Single(x => x.Id == taget);
        tagetRow.AddCard(card.GetComponent<CardMoveInfo>(), index); //, false);
    }

    //将一张牌移动到另一个地方
    public void SetCardTo(RowPosition rowIndex,int cardIndex, RowPosition tagetRowIndex,int tagetCardIndex)
    {
        var card = AllCardsPosition.Single(x=>x.Id==rowIndex).transform.GetChild(cardIndex).GetComponent<CardMoveInfo>();
        var tagetRow = AllCardsPosition.Single(x => x.Id == tagetRowIndex);
        tagetRow.AddCard(card, tagetCardIndex);//,false);
    }

    //告诉玩家自己卡牌效果结束(要被取代)
    public void MyCardEffectEnd()
    {
        if (_myUseCard == null)
            return;
        _myUseCard.IsOn = false;
        var type = _myUseCard.CardUseInfo;
        if (type == CardUseInfo.AnyPlace || type == CardUseInfo.MyPlace || type == CardUseInfo.EnemyPlace)
        {
            CardMoveBack(_myUseCard,MyCemetery.transform);
        }
        else
        {
            //_myUseCard.transform.localScale = Vector3.one;
            //var cardParent = _myUseCard.transform.parent.gameObject.GetComponent<CardsPosition>();
            //_myUseCard.IsCanSelect = cardParent.IsCanSelect;
            //cardParent.ResetCards();
            MyLeader.AutoSet();
        }
        _myUseCard = null;
    }

    //告诉玩家对方卡牌效果结束(要被取代)
    public void EnemyCardEffectEnd()
    {
        if (_enemyUseCard == null)
            return;
        _enemyUseCard.IsOn = false;
        var type = _enemyUseCard.CardUseInfo;
        if (type == CardUseInfo.AnyPlace || type == CardUseInfo.MyPlace || type == CardUseInfo.EnemyPlace)
        {
            CardMoveBack(_enemyUseCard,EnemyCemetery);
        }
        else
        {
            //_enemyUseCard.transform.localScale = Vector3.one;
            //var cardParent = _enemyUseCard.transform.parent.gameObject.GetComponent<CardsPosition>();
            //_enemyUseCard.IsCanSelect = cardParent.IsCanSelect;
            //cardParent.ResetCards();
            EnemyLeader.AutoSet();
        }
        _enemyUseCard = null;
    }*/

    //设置硬币
    public void SetCoinInfo(bool isBlue)
    {
        Coin.IsMyRound = isBlue;
    }

    //告诉玩家回合结束
    public void RoundEnd()
    {
        //等待修改,处理回合结束相关
        //PassCoin.IsMyRound = false;
    }

    //强制刷新掉所有的辅助灰卡(暂时没必要)
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
