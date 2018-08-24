using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDrop : MonoBehaviour
{
    public int Id = -1;
    private bool _isCanDrop;
    public CardsPosition CardsPosition;
    public bool IsCanDrop
    {
        get => _isCanDrop;
        set
        {
            _isCanDrop = value && (CardsPosition==null||CardsPosition.MaxCards > CardsPosition.transform.childCount);
            DropShow.SetActive(_isCanDrop);
        }
    }
    public GameObject DropShow;

    private void Start()
    {
        IsCanDrop = false;
    }
}
