using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Enter : MonoBehaviour {

    public GameObject menu;
    private SampleWebView swv;
    public Text URL;
    private string helpUrl;
    public GameObject destroiElement;

    void Start()
    {
        swv = FindObjectOfType<SampleWebView>().GetComponent<SampleWebView>();
    }

    void Update()
    {
        if (URL.text != swv.status.text)
            URL.text = swv.status.text;
        if (URL.text.Contains("access_token"))
        {
            helpUrl = URL.text;
            char[] Symbols = { '=', '&' };
            var s = helpUrl.Split(Symbols);
            PlayerPrefs.SetString("token", s[1] + "\n" + s[5]);
            Destroy(FindObjectOfType<WebViewObject>().gameObject);
            Destroy(FindObjectOfType<SampleWebView>().gameObject);
            Destroy(destroiElement);
            menu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
