using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgTiler : MonoBehaviour    // Constantly teleports bg, making it infinate.
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        // float dist = Vector2.Distance(cam.transform.position, transform.position);
        float dist = cam.transform.position.x - transform.position.x;
        // print(dist);
        if (Mathf.Abs(dist) >= 5f)
        {
            Vector3 newPos = transform.position;
            newPos.x += 5.0f;
            transform.position = newPos;
        }
        dist = cam.transform.position.y - transform.position.y;
        if (Mathf.Abs(dist) >= 5f)
        {
            Vector3 newPos = transform.position;
            newPos.y -= 5.0f;
            transform.position = newPos;
        }
    }
}
