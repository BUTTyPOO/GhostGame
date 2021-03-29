using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    [SerializeField] float posAmp = 0.003f;
    [SerializeField] float posSpeed = 3.0f;

    [SerializeField] float rotAmp = 5.0f;
    [SerializeField] float rotSpeed = 3.0f;

    void Update()
    {
        Vector3 newPos;
        newPos = transform.position;
        newPos.y += ReturnSin(posAmp, posSpeed);
        transform.position = newPos;

        Vector3 newRot = transform.localRotation.eulerAngles;
        newRot.z = ReturnSin(rotAmp, rotSpeed);
        transform.localRotation = Quaternion.Euler(newRot);
    }

    float ReturnSin(float _amplitute, float _speed)
    {
        return _amplitute * Mathf.Sin(Time.time * _speed);
    }
}
