using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggerler : MonoBehaviour
{
    [System.Serializable]
    class Wig
    {
        public float amp;
        public float speed;
        public float vertShift = 0;
        public bool isApplied = true;
    }

    [SerializeField] Wig yPosWigler;
    [SerializeField] Wig rotWiggler;
    [SerializeField] Wig scaleWiggler;

    float elapTime = 0.0f;

    void Update()
    {
        YPos();
        Rot();
        Scale();
        elapTime += Time.deltaTime;
    }

    void YPos()
    {
        if (!yPosWigler.isApplied) return;
        Vector3 newPos;
        newPos = transform.position;
        newPos.y += ReturnSin(yPosWigler);
        transform.position = newPos;
    }

    void Rot()
    {
        if (!rotWiggler.isApplied) return;
        Vector3 newRot = transform.localRotation.eulerAngles;
        newRot.z = ReturnSin(rotWiggler);
        transform.localRotation = Quaternion.Euler(newRot);
    }

    void Scale()
    {
        if (!scaleWiggler.isApplied) return;
        Vector3 newScale = transform.localScale;
        newScale.x = ReturnSin(scaleWiggler);
        newScale.y = ReturnSin(scaleWiggler);
        transform.localScale = newScale;
    }

    float ReturnSin(Wig wiggler)
    {
        return (wiggler.amp * Mathf.Sin(wiggler.speed * elapTime)) + wiggler.vertShift;
    }
}
