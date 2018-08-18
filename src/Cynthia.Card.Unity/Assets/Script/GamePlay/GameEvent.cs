using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cynthia.Card.Client;
using Autofac;
using UnityEngine.EventSystems;
using System.Linq;
using Alsein.Utilities;

public class GameEvent : MonoBehaviour
{
    public static GameObject DragCard;
    public static GameObject DropTaget;
    private GameObject _selectCard = null;
    //private GameEventService _gameEventService;
    private void Start()
    {
       // _gameEventService = DependencyResolver.Container.Resolve<GameEventService>();
    }
    private void OnMouseDown()
    {
        // _gameEventService.OnMouseDown();
        Debug.Log("父物体收到了鼠标按下");
    }
    private void OnMouseUp()
    {
        var raycasthits = GetMouseAllRaycast();
        raycasthits.ForAll(x =>
        {
            Debug.Log($"射线穿过了:{x.collider.gameObject.name}");
        });
        /*
        Debug.Log(rayhits.Count());
        if (DragCard == null)
            return;
        var cardObj = DragCard.GetComponent<CardMoveInfo>();
            cardObj.IsDrag = false;
            cardObj.zPosition += 2;
            DragCard = null;
        if (DropTaget == null)
        {
            return;
        }
        var cardSource = cardObj.transform.parent;
        cardObj.transform.SetParent(DropTaget.transform);
        cardObj.transform.SetSiblingIndex(0);
        cardObj.IsCanDrag = DropTaget.GetComponent<CardsPosition>().IsCanDrag;
        DropTaget.GetComponent<CardsPosition>().ResetCards();
        cardSource.GetComponent<CardsPosition>().ResetCards();
        cardObj.transform.localScale = Vector3.one;
        DropTaget = null;
        */
    }
    private void Update()
    {
       
    }
    private RaycastHit[] GetMouseAllRaycast()//获得当前鼠标穿透过的所有子物体
    {
        var ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        var ray = new Ray(ray1.origin, ray1.direction * 99999);
        return  Physics.RaycastAll(ray);
    }
}
