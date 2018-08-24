using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cynthia.Card;
using System.Linq;
using Alsein.Utilities;

public class LeaderCard : MonoBehaviour
{
    public bool IsCardUse;
    public GameObject CardPrefab;
    public bool IsCanSelect;
    public bool IsCanDrag;
    public void SetLeader(GameCard Leader,bool isCardUse)
    {
        IsCardUse = isCardUse;
        DestroyAllChild();
        var newCard = Instantiate(CardPrefab);
        newCard.GetComponent<CardShowInfo>().CurrentCore = Leader;
        newCard.GetComponent<CardMoveInfo>().CardUseInfo = GwentMap.CardMap[Leader.CardIndex].CardUseInfo;
        newCard.GetComponent<CardShowInfo>().SetCard();
        newCard.transform.SetParent(transform);
        newCard.transform.localPosition = new Vector3(0, 0, -0.01f);
        newCard.transform.localScale = Vector3.one;
        newCard.GetComponent<CardMoveInfo>().IsCanDrag = IsCanDrag;
        newCard.GetComponent<CardMoveInfo>().IsCanSelect = IsCanSelect;
    }
    public void SetCanDrag(bool isCanDrag)
    {
        IsCanDrag = isCanDrag;
        var count = transform.childCount;
        for (var i = 0; i < count; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<CardMoveInfo>().IsCanDrag = IsCanDrag;
        }
    }
    public void SetCanSelect(bool isCanSelect)
    {
        IsCanSelect = isCanSelect;
        var count = transform.childCount;
        for (var i = 0; i < count; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<CardMoveInfo>().IsCanSelect = IsCanSelect;
        }
    }
    private void DestroyAllChild()
    {
        var count = transform.childCount;
        for(int i = count; i > 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    private IEnumerable<Transform> GetAllChild()
    {
        var count = transform.childCount;
        for (int i = count; i > 0; i--)
        {
            yield return transform.GetChild(i);
        }
    }
}
