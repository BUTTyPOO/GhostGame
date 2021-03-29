using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCol : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Enforcer.Instance.HumanOutRanGhost();
    }
}
