using Alsein.Utilities.IO;
using Cynthia.Card;
using Cynthia.Card.Client;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameCardShowControl : MonoBehaviour
{
    public GameObject UICardPrefab;
    public GameObject NullCardPrefab;
    public RectTransform CardsContent;
    public Text ShowCardMessage;
    public Scrollbar Scroll;
    public GameObject CardSelectUI;
    public GameEvent GameEvent;
    //-------
    public GameObject OpenButton;//显示卡牌
    public GameObject MulliganEndButton;
    public GameObject CloseButton;
    public GameObject AffirmButton;
    public GameObject HideButton;
    //---------------------------------
    public int NowMulliganCount { get; set; }
    public int NowMulliganTotal { get; set; }
    //------
    private (bool, bool, bool, bool) UseButtonShow { get; set; }
    private bool IsUseMenuShow { get; set; }
    private string useCardTitle { get; set; }
    private UseCardShowType NowUseMenuType;
    //---------------------------------
    public IList<CardStatus> UseCardList;
    public IList<CardStatus> MyCemetery;
    public IList<CardStatus> EnemyCemetery;
    //
    private IAsyncDataSender sender;
    private IAsyncDataReceiver receiver;
    private void Awake() => (sender, receiver) = AsyncDataEndPoint.CreateSimplex();

    //------------------------------------------------------------------------------------------
    public void OpenButtonClick()//显示卡牌
    {
        OpenNowUseMenu();
    }
    public async void MulliganEndButtonClick()//手牌调度完毕
    {
        await sender.SendAsync<int>(-1);
    }
    public void CloseButtonClick()//关闭
    {
        CardSelectUI.SetActive(false);
    }
    public void AffirmButtonClick()//确认
    {
        Debug.Log("确认");
    }
    public void HideButtonClick()//隐藏卡牌
    {
        IsUseMenuShow = false;
        CardSelectUI.SetActive(false);
    }
    //------------------------------------------------------------------------------------------
    public void SelectCard(int index)
    {
        if(index == -1)
        {
            //移出和
        }
        else
        {
            //移入
        }
    }
    public async void ClickCard(int index)
    {
        switch (NowUseMenuType)
        {
            case UseCardShowType.Mulligan:
                await sender.SendAsync<int>(index);
                break;
            case UseCardShowType.None:
                break;
            case UseCardShowType.Select:
                break;
            default:
                break;
        }
    }

    public void ShowMyCemetery()
    {
        if (MyCemetery == null || MyCemetery.Count == 0)
            return;
        ShowCardMessage.text = "我方墓地";
        SetCardInfo(MyCemetery);
        CardSelectUI.SetActive(true);
        SetButtonShow(IsCloseShow: true);
        IsUseMenuShow = false;
    }
    public void ShowEnemyCemetery()
    {
        if (EnemyCemetery == null || EnemyCemetery.Count == 0)
            return;
        ShowCardMessage.text = "敌方墓地";
        SetCardInfo(EnemyCemetery);
        CardSelectUI.SetActive(true);
        SetButtonShow(IsCloseShow: true);
        IsUseMenuShow = false;
    }
    //------------------------------------------------------------------------------------------------
    //调度开始
    public void MulliganStart(IList<CardStatus> cards,int total)//调度界面
    {
        NowMulliganCount = 0;
        NowMulliganTotal = total;
        useCardTitle = $"选择1张卡重抽。[{NowMulliganCount}/{NowMulliganTotal}]";
        UseCardList = cards;
        OpenButton.SetActive(true);//打开显示按钮
        //IsMulliganEndShow,IsCloseShow,IsAffirmShow,IsHideShow
        UseButtonShow = (true,false,false,true);
        OpenNowUseMenu();
    }
    //调度结束
    public void MulliganEnd()
    {
        NowUseMenuType = UseCardShowType.None;
        useCardTitle = "错误";
        UseCardList = null;
        OpenButton.SetActive(false);//打开
        CardSelectUI.SetActive(false);
    }
    //更新信息(需要更改),动画之类的
    public void MulliganData(int index, CardStatus card)
    {
        UseCardList[index] = card;
        var mCard = CardsContent.GetChild(index).GetComponent<CardShowInfo>();
        mCard.CurrentCore = card;
        mCard.SetCard();
        //--------------------------
        mCard = GameEvent.MyHand.transform.GetChild(index).GetComponent<CardShowInfo>();
        mCard.CurrentCore = card;
        mCard.SetCard();
    }
    //获取调度信息
    public async void GetMulliganInfo(LocalPlayer player)
    {
        NowUseMenuType = UseCardShowType.Mulligan;
        var task = await receiver.ReceiveAsync<int>();
        NowUseMenuType = UseCardShowType.None;
        if(task!=-1)
            NowMulliganCount++;
        useCardTitle = $"选择1张卡重抽。[{NowMulliganCount}/{NowMulliganTotal}]";
        if(IsUseMenuShow)
            ShowCardMessage.text = useCardTitle;
        await player.SendAsync(UserOperationType.MulliganInfo, task);
    }
    //-------------------------------
    public void OpenNowUseMenu()
    {
        //将存起来的标题和卡牌赋值
        ShowCardMessage.text = useCardTitle;
        SetCardInfo(UseCardList);
        SetButtonShow(UseButtonShow);
        IsUseMenuShow = true;
        CardSelectUI.SetActive(true);
    }
    //------------------------------------------------------------------------------------------------
    public void SetCardInfo(IList<CardStatus> cards)
    {
        var count = cards.Count;
        RemoveAllChild();
        for (var i = 0; i < count; i++)
        {
            var card = Instantiate(UICardPrefab).GetComponent<CardShowInfo>();
            card.CurrentCore = cards[i];
            card.SetCard();
            card.transform.SetParent(CardsContent, false);
        }
        var nullcount = count <= 10 ? 10 - count : (count % 5 == 0 ? 0 : 5 - count % 5);
        for (var i = 0; i < nullcount; i++)
        {
            var card = Instantiate(NullCardPrefab);
            card.transform.SetParent(CardsContent, false);
        }
        //------------------------------------------------------------------------
        var height = count <= 10 ? 780f : (108f + 276 * (count % 5 > 0 ? count / 5 + 1 : count / 5));
        CardsContent.sizeDelta = new Vector2(0, height);
        if (count <= 10)
            CardsContent.GetComponent<GridLayoutGroup>().padding.top = 190;
        else
            CardsContent.GetComponent<GridLayoutGroup>().padding.top = 130;
        Scroll.value = 1;
    }
    public void SetButtonShow(bool IsMulliganEndShow = false, bool IsCloseShow = false, bool IsAffirmShow = false, bool IsHideShow = false)
    {
        MulliganEndButton.SetActive(IsMulliganEndShow);
        CloseButton.SetActive(IsCloseShow);
        AffirmButton.SetActive(IsAffirmShow);
        HideButton.SetActive(IsHideShow);
    }
    public void SetButtonShow((bool,bool,bool,bool) ButtonShow)
    {
        var (IsMulliganEndShow, IsCloseShow, IsAffirmShow, IsHideShow) = ButtonShow;
        MulliganEndButton.SetActive(IsMulliganEndShow);
        CloseButton.SetActive(IsCloseShow);
        AffirmButton.SetActive(IsAffirmShow);
        HideButton.SetActive(IsHideShow);
    }
    public void RemoveAllChild()
    {
        for (var i = CardsContent.childCount - 1; i >= 0; i--)
        {
            Destroy(CardsContent.GetChild(i).gameObject);
        }
        CardsContent.DetachChildren();
    }
}
