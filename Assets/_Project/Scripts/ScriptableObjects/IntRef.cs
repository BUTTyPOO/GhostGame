using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IntRef
{
    public bool isConst = true;
    public int constVal;
    public IntVariable var;

    public int value
    {
        get { return isConst ? constVal : var.value; }
        set
        {
            if (isConst)
                constVal = value;
            else
                var.value = value;
        }
    }
}
