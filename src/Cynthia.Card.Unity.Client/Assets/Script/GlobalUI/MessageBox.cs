using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Alsein.Utilities.IO;

public class MessageBox : MonoBehaviour
{
    public Text TitleText;
    public Text MessageText;
    public Text YesText;
    public Text NoText;
    private IAsyncDataSender sender;
    private IAsyncDataReceiver receiver;
    private void Awake()
    {
        (sender, receiver) = AsyncDataEndPoint.CreateSimplex();
    }
    public Task<bool> Show(string title,string message,string yes = "确定",string no = "取消")
    {
        gameObject.SetActive(true);
        TitleText.text = title;
        MessageText.text = message;
        YesText.text = yes;
        NoText.text = no;
        return receiver.ReceiveAsync<bool>();
    }
    public void YesClick()
    {
        sender.SendAsync<bool>(true);
        gameObject.SetActive(false);
    }
    public void NoClick()
    {
        sender.SendAsync<bool>(false);
        gameObject.SetActive(false);
    }
}
