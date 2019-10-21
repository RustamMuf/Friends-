using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour {

    Game game;
    private Image colorButton;
    private GameObject ads;

    void Awake()
    {
        game = FindObjectOfType<Game>();
        colorButton = gameObject.GetComponent<Image>();
    }

void OnMouseDown()
    {
    if (!game.panelAnswerCorrect.activeSelf)
    {
        game.currentDown = gameObject.name;
        gameObject.GetComponent<Image>().color = new Color(0.44f, 0.36f, 0.44f, 0.7f);

            if (game.currentDown == game.currentAnswer)
            {
                game.score++;
                game.currentScore.text = game.score + ""; 
                game.panelAnswerCorrect.SetActive(true);      
                game.StartGame();
            }
        else
        {
            colorButton.color = Color.white;
            for (int i = 0; i < 3; i++)
            {
                game.questText[i].text = "";
            }
            game.ava.sprite = new Sprite();
            foreach (var e in game.buttonAnswer)
            {
                e.SetActive(false);
            }
            game.ava.gameObject.SetActive(false);
            game.avaHelper.gameObject.SetActive(false);
            game.currentName.text = "";
            game.scorEnd.text = "ВАШ РЕЗУЛЬТАТ\n" + game.score;
            game.currentScore.text = "";
            game.like.SetActive(false);
            game.questTextName.text = "";
            game.endGamePanel.SetActive(true);
            if (game.chekAds)
            {
                ads = FindObjectOfType<Ads>().gameObject;
                ads.SetActive(false);
            }
        }
    }

    }
    void OnMouseUp()
    {
        gameObject.GetComponent<Image>().color = Color.white;
    }
}
