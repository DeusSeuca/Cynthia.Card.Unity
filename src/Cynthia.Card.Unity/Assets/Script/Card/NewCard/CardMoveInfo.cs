using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cynthia.Card;

public class CardMoveInfo : MonoBehaviour
{
    public CardShowInfo CardShowInfo;
    public Vector3 ResetPoint;//应该在的位置
    public float zPosition;
    public bool IsCanDrag = true;//是否能拖动
    public bool IsCanSelect = true;
    public bool IsDrag = false;//是否正在拖动
    public CardUseInfo CardUseInfo = CardUseInfo.MyRow;

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
}
