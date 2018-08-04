using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CardShow : MonoBehaviour {
    public Flavor Flavor = Flavor.Copper;
    public Categories Categories = Categories.Neutral;
    public int Strong = 0;

    private RectTransform icon;
    private RectTransform strong;
    private RectTransform frame;
    //private RectTransform body;
    public RectTransform back;
    // Use this for initialization
    void Start()
    {
        foreach (RectTransform item in gameObject.GetComponentsInChildren<RectTransform>())
        {
            Debug.Log(item.name);
            if (item.name == "CardIcon")
            {
                icon = item;
            }
            if (item.name == "CardStrong")
            {
                strong = item;
            }
            if (item.name == "CardFrame")
            {
                frame = item;
            }
            if (item.name == "CardBody")
            {
                //body = item;
            }
        }
        strong.gameObject.GetComponent<Text>().text = Strong.ToString();
        frame.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/Frames/border" + (Flavor == Flavor.Copper ? 1 : (Flavor == Flavor.Silver ? 2 : 3)));
        if (Categories == Categories.Neutral)
        {
            icon.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FactionIcon/factionIcon1" + (Flavor == Flavor.Gold ? "l" : ""));
            back.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FlippedFaction/flippedFaction1");
        }
        if (Categories == Categories.Monster)
        {
            icon.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FactionIcon/factionIcon2" + (Flavor == Flavor.Gold ? "l" : ""));
            back.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FlippedFaction/flippedFaction2");
        }
        if (Categories == Categories.Neutral)
        {
            icon.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FactionIcon/factionIcon3" + (Flavor == Flavor.Gold ? "l" : ""));
            back.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FlippedFaction/flippedFaction3");
        }
        if (Categories == Categories.Scoiatael)
        {
            icon.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FactionIcon/factionIcon4" + (Flavor == Flavor.Gold ? "l" : ""));
            back.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FlippedFaction/flippedFaction4");
        }
        if (Categories == Categories.Skellige)
        {
            icon.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FactionIcon/factionIcon5" + (Flavor == Flavor.Gold ? "l" : ""));
            back.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FlippedFaction/flippedFaction5");
        }
        if (Categories == Categories.Nilfgaard)
        {
            icon.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FactionIcon/factionIcon6" + (Flavor == Flavor.Gold ? "l" : ""));
            back.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardSprotes/FlippedFaction/flippedFaction6");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
public enum Flavor//品质
{
    Copper = 1,
    Silver = 2,
    Gold = 3
}
public enum Categories//势力
{
    Neutral = 1,    //中立
    Monster = 2,    //怪物
    Northern = 3,   //北方
    Scoiatael = 4,  //松鼠党
    Skellige = 5,   //群岛
    Nilfgaard = 6   //帝国
}