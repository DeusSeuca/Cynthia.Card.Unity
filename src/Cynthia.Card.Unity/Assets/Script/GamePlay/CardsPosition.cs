using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsPosition : MonoBehaviour
{
    public static GameObject DragCard = null;
    public GameObject CardPrefab;
    private float _size = 1.8f;
    public float Width = 11f;
    public bool IsCanDrag;

    private void Start()
    {
        ResetCards();
    }

    public void ResetCards()//将所有卡牌定位到应有的位置
    {
        var size = _size;
        var count = transform.childCount;
        if ((count - 1f) * size > Width)
        {
            size = Width / (count - 1f);
        }
        for (var i = 0; i < count; i++)
        {
            var item = transform.GetChild(i).gameObject.GetComponent<CardMoveInfo>();
            item.IsCanDrag = IsCanDrag;
            item.SetResetPoint(new Vector3(-(count - 1f) * size / 2f + i * size, 0, -0.1f - 0.01f * i));
        }
    }
    public void CardsCanDrag(bool isCanDrag)
    {//疑点---?设定子物体中所有的卡牌无法拖动
        IsCanDrag = isCanDrag;
        var count = transform.childCount;
        for (var i = 0; i < count; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<CardMoveInfo>().IsCanDrag = IsCanDrag;
        }
    }
    public void AddCard(GameObject card, int cardIndex)
    {
        card.GetComponent<CardMoveInfo>().IsCanDrag = IsCanDrag;
        var source = card.transform.parent.gameObject.GetComponent<CardsPosition>();
        card.transform.SetParent(transform);

    }
}
