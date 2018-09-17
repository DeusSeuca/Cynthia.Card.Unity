using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Cynthia.Card.Client;
using Autofac;

public class UICard : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public GameCodeService GameCodeService { get; set; }

    private void Start()
    {
        GameCodeService = DependencyResolver.Container.Resolve<GameCodeService>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameCodeService.ClickCard(transform.GetSiblingIndex());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().localScale *= 1.05f;
        GameCodeService.SelectCard(transform.GetSiblingIndex());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().localScale /= 1.05f;
        GameCodeService.SelectCard(-1);
    }
}
