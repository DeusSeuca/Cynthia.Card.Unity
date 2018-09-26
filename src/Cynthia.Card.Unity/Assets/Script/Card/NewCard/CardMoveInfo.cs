using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cynthia.Card;
using System.Threading.Tasks;
using Alsein.Utilities.IO;
using Cynthia.Card.Client;
using Autofac;

public class CardMoveInfo : MonoBehaviour
{
    public CardShowInfo CardShowInfo;
    public Vector2 ResetPoint;//应该在的位置
    public float ZPosition { get=>_zPosition; set
        {
            _zPosition = value;
            transform.position = new Vector3(transform.position.x,transform.position.y,_zPosition);
        }}
    private float _zPosition;
    public bool IsCanDrag = true;//是否能拖动
    public bool IsCanSelect = true;
    public bool IsDrag = false;//是否正在拖动
    public bool IsOn
    {
        get => _isOn; set
        {
            if (value)
            {
                transform.localScale = Vector3.one * 1.15f;
                if (!_isOn) ZPosition -= 2;
                _isOn = value;
            }
            else
            {
                if (_isOn) ZPosition += 2;
                _isOn = value;
                var p = transform.parent.GetComponent<CardsPosition>();
                if (p != null)
                    p.ResetCards();
            }
        }
    }//是否处于抬起状态(锁定变大)
    private bool _isOn = false;
    public float Speed = 35f;
    public CardUseInfo CardUseInfo = CardUseInfo.AnyPlace;
    /*
    private IAsyncDataSender _sender1;
    private IAsyncDataReceiver _receiver1;
    private bool _isMoveToPosition = false;
    private void Awake() => (_sender1, _receiver1) = AsyncDataEndPoint.CreateSimplex();
    public async Task MoveToPosition(Vector2 taget,float speed,Space relativeTo = Space.Self)
    {
        _isMoveToPosition = true;
        while (await _receiver1.ReceiveAsync<bool>() && SetNextPosition(taget, speed, relativeTo)) ;
        _isMoveToPosition = false;
        return;
    }*/

    void Update()
    {
        var tagetText = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //if(_isMoveToPosition)
            //_sender1.SendAsync<bool>(true);
        //下一帧应该移动到的位置
        if (IsDrag)
        {
            Speed = 30f;
            if (!IsCanDrag)
            {
                IsDrag = false;
                GameEvent.DragCard = null;
                return;
            }
            SetNextPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), Speed,Space.World);
            var taget = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition),0.2f);
            //transform.position = new Vector3(taget.x, taget.y, ZPosition - 1);
        }
        else
        {
            var taget = Vector2.Lerp(transform.localPosition, ResetPoint, 0.25f);
            transform.localPosition = new Vector3(taget.x, taget.y, ZPosition);
            //SetNextPosition(ResetPoint, Speed, Space.Self);
        }
    }
    public bool SetNextPosition(Vector2 taget, float speed, Space relativeTo = Space.Self)
    {
        if (relativeTo == Space.Self)
            Debug.DrawLine(taget, transform.localPosition);
        else
            Debug.DrawLine(taget, transform.position);
        var dir = default(Vector2);
        if (relativeTo == Space.World)
            dir = taget - new Vector2(transform.position.x, transform.position.y);//指定方向
        else
            dir = taget - new Vector2(transform.localPosition.x, transform.localPosition.y);//指定方向
        float distance = speed * Time.deltaTime;
        if (dir.magnitude <= distance)
        {
            if(relativeTo == Space.World)
                transform.position = new Vector3(taget.x,taget.y,transform.position.z);
            else
                transform.localPosition = new Vector3(taget.x, taget.y, transform.localPosition.z);
            return false;
        }
        var nextPoint = dir.normalized * distance;
        transform.Translate(new Vector3(nextPoint.x,nextPoint.y,0));
        return true;
    }
    public void SetResetPoint(Vector3 rPoint)
    {
        //设置默认位置
        ResetPoint = rPoint;
        ZPosition = rPoint.z;
    }
}
