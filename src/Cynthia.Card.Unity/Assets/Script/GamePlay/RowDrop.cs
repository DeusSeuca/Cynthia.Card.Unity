using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowDrop : MonoBehaviour
{
    private void OnMouseEnter()
    {
        GameEvent.DropTaget = gameObject;
    }
    private void OnMouseExit()
    {
        GameEvent.DropTaget = null;
    }
}
