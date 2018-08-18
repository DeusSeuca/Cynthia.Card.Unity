using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMoveInfo : MonoBehaviour
{
    public Vector3 ResetPoint;//应该在的位置
    public float zPosition;
    public bool IsCanDrag = true;//是否能拖动
    public bool IsDrag = false;//是否正在拖动

    public void SetResetPoint(Vector3 rPoint)
    {
        //设置默认位置
        ResetPoint = rPoint;
        zPosition = rPoint.z;
    }

    void Update()
    {
        //下一帧应该移动到的位置
        var taget = default(Vector3);
        if (!IsDrag)
        {
            taget = Vector3.Lerp(transform.localPosition, ResetPoint, 0.2f);
            transform.localPosition = new Vector3(taget.x, taget.y, zPosition);
        }
        if (IsDrag)
        {
            if (!IsCanDrag)
            {
                IsDrag = false;
                GameEvent.DragCard = null;
                return;
            }
            taget = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.2f);
            transform.position = new Vector3(taget.x, taget.y, zPosition-1);
        }
    }
    private void OnMouseEnter()
    {
        transform.localScale = new Vector3(1.05f, 1.05f, 1);
        zPosition -= 1f;
    }
    private void OnMouseExit()
    {
        transform.localScale = new Vector3(1, 1, 1);
        zPosition += 1f;
    }
    private void OnMouseDown()
    {
        Debug.Log("子物体收到了按键按下");
        if (!IsCanDrag)
            return;
        zPosition -= 2f;
        IsDrag = true;
        GameEvent.DragCard = gameObject;
        transform.localScale = new Vector3(1, 1, 1);
    }
    private void OnMouseUp()
    {/*
        if (!IsCanDrag || !IsDrag)
            return;
        zPosition += 2f;
        IsDrag = false;
        GameEvent.DragCard = null;*/
    }
}
