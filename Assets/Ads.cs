using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour {

    private Game game;

    void Awake()
    {
        game = FindObjectOfType<Game>();
    }

    void Start () {
        if (Advertisement.isSupported)
            Advertisement.Initialize("2589246", false);
    }

    public void ADs()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo", new ShowOptions
            {
                resultCallback = result =>
                {
                    if (result == ShowResult.Finished)
                    {
                        game.chekAds = true;
                        game.StartGame();
                        game.endGamePanel.SetActive(false);
                    }
                    gameObject.SetActive(false);
                }
            }
                );
        }
    }
}
