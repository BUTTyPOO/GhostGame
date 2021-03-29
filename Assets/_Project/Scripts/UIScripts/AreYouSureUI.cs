using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AreYouSureUI : MonoBehaviour
{
    public delegate void YesFunc();
    YesFunc yesPushed;

    public void BindYesButton(YesFunc func)
    {
        yesPushed = func;
    }

    public void YesPressed()
    {
        if (yesPushed != null)
            yesPushed();
        CloseDialog();
    }

    public void NoPushed()
    {
        CloseDialog();
    }

    void OnEnable()
    {
        transform.localScale = new Vector3(0, 1, 1);
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutCirc);
    }

    void CloseDialog()
    {
        transform.DOScale(new Vector3(0, 1, 1), 0.15f).SetEase(Ease.InOutCirc).OnComplete(DisableDialog);
    }

    void DisableDialog()
    {
        gameObject.SetActive(false);
    }
}
