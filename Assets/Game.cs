using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour {

    private const string __APPID = "6479714";  //ID приложения
    private const string __VKAPIURL = "https://api.vk.com/method/";
    private string TOKEN;
    private string USER_ID;
    public Text text;
    private List<string> idFriends;
    public Image ava, avaHelper;
    public string currentDown;
    private List<string> quest = new List<string>();
    public Text[] questText;
    public GameObject[] buttonAnswer;
    public string currentAnswer;
    public bool answerTrue;
    public GameObject panelAnswerCorrect;
    public Text currentName;
    public GameObject buttonStart;
    public int score;
    public GameObject endGamePanel;
    public Text scorEnd;
    public Text questTextName;
    public Text currentScore;
    public GameObject like;
    public bool chekAds;

    void Start()
    {
        score = 0;
        answerTrue = false;
        currentDown = "no";
        TOKEN = PlayerPrefs.GetString("token").Split('\n')[0];
        USER_ID = PlayerPrefs.GetString("token").Split('\n')[1];
        text.text = USER_ID;
        StartCoroutine(StartZapros());
        foreach (var e in buttonAnswer)
        {
            e.SetActive(false);
        }
        ava.sprite = new Sprite();
    }

    public string RandomID()
    {
        return idFriends[Random.Range(0, idFriends.Count)];
    }

    public void StartGame()
    {
        try
        {
            ava.gameObject.SetActive(true);
            avaHelper.gameObject.SetActive(true);
            currentName.text = "";
            like.SetActive(true);
            currentScore.text = score + "";
            switch (Random.Range(0, 3))
            {
                case 0:
                    {
                        StartCoroutine(ZaprosPhoto());
                        break;
                    }
                case 1:
                    {
                        StartCoroutine(ZaprosBirthday());
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(Publics());
                        break;
                    }
            }
            foreach (var e in buttonAnswer)
            {
                e.SetActive(true);
            }
        }
        catch (Exception)
        {
            StartGame();
        }
    }

    public IEnumerator ZaprosBirthday()
    {
        var bDay = new Friend();
        WWW www;
        string str = "";
        questTextName.text = "Когда День Рождения вашего друга?";
        while (bDay.bdate == null)
        {
            var x = Random.Range(0, idFriends.Count);
            var id = idFriends[x];
            idFriends.RemoveAt(x);
            quest = new List<string>();
            www = new WWW(__VKAPIURL + "users.get?user_ids=" + id + "&fields=" + "bdate,photo_100" + "&access_token=" + TOKEN + "&v=5.74");
            yield return www;
            //text.text = www.text;
            str = www.text.Substring(13, www.text.Length - 15);
            if (str.Contains("6,\"error_msg\""))
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
            bDay = JsonConvert.DeserializeObject<Friend>(str);
            text.text = id;
        }
        www = new WWW(bDay.photo_100);
        yield return www;

        Texture2D texture = www.texture;
        ava.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0f, 0f));

        currentName.text = bDay.first_name + " " + bDay.last_name;
        quest.Add(bDay.bdate.Split('.')[0] + " " + IntToStr(bDay.bdate.Split('.')[1]));
        currentAnswer = quest[0];
        string help;
        string help2;
        while (true)
        {
            help = Random.Range(1, 31) + " " + IntToStr(Random.Range(1, 13).ToString());
            if (help == currentAnswer) continue;
            help2 = Random.Range(1, 31) + " " + IntToStr(Random.Range(1, 13).ToString());
            if (help2 != currentAnswer && help2 != help)
                break;
        }
        quest.Add(help);
        quest.Add(help2);
        quest.Sort();

        for (int i = 0; i < 3; i++)
        {
            buttonAnswer[i].name = quest[i];
            questText[i].text = quest[i];
        }
        panelAnswerCorrect.SetActive(false);
    }

    public IEnumerator ZaprosPhoto()
    {
        var friend = new Friend();
        var str = "";
        WWW www;
        var id = "";
        var sex = "";
        questTextName.text = "Кто это?";
        while (true)
        {
            id = RandomID();
            quest = new List<string>();
            www =
                new WWW(__VKAPIURL + "users.get?user_ids=" + id + "&fields=" + "sex,photo_100" + "&access_token=" +
                        TOKEN + "&v=5.74");
            yield return www;
            text.text = id;
            str = www.text.Substring(13, www.text.Length - 15);
            //text.text = str;
            if (str.Contains("6,\"error_msg\""))
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
            if (str.Split('/')[4] == "camera_100.png\"}" || str.Split('/')[4] == "deactivated_100.png\"}")
            {
                continue;               
            }
            break;
        }

        friend = JsonConvert.DeserializeObject<Friend>(str);      
        sex = friend.sex;       
        www = new WWW(friend.photo_100);
        yield return www;
        Texture2D texture = www.texture;
        ava.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0f, 0f));       
        quest.Add(friend.first_name + " " + friend.last_name);
        currentAnswer = quest[0];
        var dopFriend = new Friend();
        string helpId = "-1";
        for (int i = 0; i < 2; i++)
        {
            while (true)
            { 
                id = RandomID();
                if (id == helpId) continue;
                www =
                    new WWW(__VKAPIURL + "users.get?user_ids=" + id + "&fields=sex" + "&access_token=" + TOKEN +
                            "&v=5.74");
                yield return www;
                helpId = id;
                str = www.text.Substring(13, www.text.Length - 15);

                if (str.Contains("6,\"error_msg\""))
                {
                    yield return new WaitForSeconds(1f);
                    continue;
                }
                dopFriend = JsonConvert.DeserializeObject<Friend>(str);
                if (dopFriend.sex == sex && friend.first_name+friend.last_name != dopFriend.first_name+dopFriend.last_name)
                    break;
            }
            quest.Add(dopFriend.first_name + " " + dopFriend.last_name);
        }
        quest.Sort();
        for (int i = 0; i < 3; i++)
        {
            buttonAnswer[i].name = quest[i];
            questText[i].text = quest[i];
        }
        panelAnswerCorrect.SetActive(false);  
    }

    public void EndGame()
    {       
        score = 0;
        chekAds = false;
        buttonStart.SetActive(true);
        endGamePanel.SetActive(false);
    }

    public IEnumerator Publics()
    {
        var groups = new RootObject();
        string id = RandomID();
        questTextName.text = "Самый любимый паблик вашего друга?";
        WWW www;
        string str = "[";
        quest = new List<string>();
        while (true)
        {
            id = RandomID();
            www =
                new WWW(__VKAPIURL + "users.getSubscriptions?user_id=" + id + "&extended=0" + "&count=3" +
                        "&access_token=" + TOKEN + "&v=5.74");
            yield return www;

            var strX = www.text.Substring(12, www.text.Length - 13);

            if (strX.Contains("6,\"error_msg\""))
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
            if (strX.Split('"')[0] == "rror_code")
                continue;
            groups = JsonConvert.DeserializeObject<RootObject>(strX);

            var count = groups.groups.count;

            if (count > 2)
            break;
        }

        while (true)
        {
            www =
                new WWW(__VKAPIURL + "users.get?user_ids=" + id + "&fields=" + "photo_100" + "&access_token=" +
                        TOKEN + "&v=5.74");
            yield return www;
            text.text = id;
            var stre = www.text.Substring(13, www.text.Length - 15);

            if (stre.Contains("6,\"error_msg\""))
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
            if (stre.Contains("deactivated") || stre.Split('"')[0] == "rror_code")
                continue;
            var friend = JsonConvert.DeserializeObject<Friend>(stre);
            www = new WWW(friend.photo_100);
            yield return www;
            Texture2D texture = www.texture;
            ava.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0f, 0f));
            currentName.text = friend.first_name + " " + friend.last_name;
            break;
        }

        var threeGroups = groups.groups.items;
        while (true)
        {
            www =
                new WWW(__VKAPIURL + "groups.getById?group_ids=" + threeGroups[0] + "," + threeGroups[1] + "," +
                        threeGroups[2] + "&access_token=" + TOKEN + "&v=5.74");
            yield return www;
            if (www.text.Contains("6,\"error_msg\""))
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
            break;
        }
        //text.text = www.text;
        str = www.text;
        var topGroups = JsonConvert.DeserializeObject<RootObjectForGroup>(str);

        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
                currentAnswer = topGroups.response[i].name;
            quest.Add(topGroups.response[i].name);
        }

    quest.Sort();
        for (int i = 0; i< 3; i++)
        {
            buttonAnswer[i].name = quest[i];
            questText[i].text = quest[i];
        }
        panelAnswerCorrect.SetActive(false);
    }

    public IEnumerator StartZapros()
    {
        idFriends = new List<string>();
        WWW zapros = new WWW(__VKAPIURL + "friends.get?user_ids=" + USER_ID + "&access_token=" + TOKEN + "&v=5.74");
        yield return zapros;
        var str = zapros.text.Remove(0, 12);
        str = str.Remove(str.Length - 1);
        var myFriends = JsonConvert.DeserializeObject<MyFriends>(str);
        //text.text = str;
        idFriends = myFriends.items;               
        zapros.Dispose();
    }

    public string IntToStr(string n)
    {
        switch (n)
        {
            case "1":
                return "Января";
            case "2":
                return "Февраля";
            case "3":
                return "Марта";
            case "4":
                return "Апреля";
            case "5":
                return "Мая";
            case "6":
                return "Июня";
            case "7":
                return "Июля";
            case "8":
                return "Августа";
            case "9":
                return "Сентября";
            case "10":
                return "Октября";
            case "11":
                return "Ноября";
            case "12":
                return "Декабря";                
        }
        return "error";
    }
}

public class MyFriends
{
    public int count { get; set; }
    public List<string> items { get; set; }
}

public class Friend
{
    public int id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string bdate { get; set; }
    public string sex { get; set; }
    public string photo_100 { get; set; }
}

public class Users
{
    public int count { get; set; }
    public List<object> items { get; set; }
}

public class Groups
{
    public int count { get; set; }
    public List<string> items { get; set; }
}

public class RootObject
{
    public Users users { get; set; }
    public Groups groups { get; set; }
}

public class ForGroup
{
    public int id { get; set; }
    public string name { get; set; }
    public string screen_name { get; set; }
    public int is_closed { get; set; }
    public string type { get; set; }
    public string photo_50 { get; set; }
    public string photo_100 { get; set; }
    public string photo_200 { get; set; }
}

public class RootObjectForGroup
{
    public List<ForGroup> response { get; set; }
}