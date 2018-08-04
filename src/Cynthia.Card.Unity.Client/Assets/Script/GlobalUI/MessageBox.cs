using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MessageBox : MonoBehaviour
{
    public Text TitleText;
    public Text MessageText;
    public Text YesText;
    public Text NoText;


    /*
    public Task<bool> Show(string title,string message,string yes = "确定",string no = "取消")
    {
        gameObject.SetActive(true);
        TitleText.text = title;
        MessageText.text = message;
        YesText.text = yes;
        NoText.text = no;
        return stream.ReceiveAsync<bool>();
    }
    public void YesClick()
    {
        gameObject.SetActive(false);
        stream.SendAsync<bool>(true);
    }
    public void NoClick()
    {
        gameObject.SetActive(false);
        stream.SendAsync<bool>(false);
    }*/
}
