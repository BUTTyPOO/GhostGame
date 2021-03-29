using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIDialog : MonoBehaviour
{
    void OnEnable()
    {
        transform.localScale = new Vector3(0, 1, 1);
        transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBounce);
    }

    public void CloseDialog()
    {
        transform.DOScale(new Vector3(0, 1, 1), 0.15f).SetEase(Ease.InOutCirc).OnComplete(DisableDialog);
        
    }

    void DisableDialog()
    {
        gameObject.SetActive(false);
    }

}
