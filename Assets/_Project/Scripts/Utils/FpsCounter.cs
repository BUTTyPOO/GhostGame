using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    Text txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponentInChildren<Text>();
        InvokeRepeating("UpdateFPSTxt", 0.1f, 0.1f);
        DontDestroyOnLoad(gameObject);
    }

    void UpdateFPSTxt()
    {
        txt.text = (1f / Time.unscaledDeltaTime).ToString();
    }

}
