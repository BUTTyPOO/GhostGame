using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulsCount : MonoBehaviour
{
    Text txt;
    PlayerController plrScript;
    Enforcer enforcer;
    [SerializeField] IntRef souls;
    [SerializeField] IntRef curPoints;

    void Start()
    {
        txt = GetComponentInChildren<Text>();
        enforcer = GameObject.Find("Enforcer").GetComponent<Enforcer>();
        plrScript = enforcer.player;
        print(plrScript);
        plrScript.HumanKilled += OnHumanKilled;
    }

    void OnEnable()
    {
        //plrScript.HumanKilled += OnHumanKilled;
    }

    void OnDisable()
    {
        //plrScript.HumanKilled -= OnHumanKilled;
    }

    void OnHumanKilled()    // all points come from earning "souls"
    {
        txt.text = (souls.value + curPoints.value).ToString();
    }
}
