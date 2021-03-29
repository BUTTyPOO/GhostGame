using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    const float idealOrthoSize = 5.0f;
    const float maxOrthoSize = 6.5f;
    const float camZoomSpeed = 8.0f;

    public GameObject player;
    public GameObject human;

    [SerializeField] BoxCollider2D gameOverBoxCol;

    Vector3 offset;
    Camera cam;

    float followSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void OnEnable()
    {
        Enforcer.Instance.HumanSpawned += AssignNewHuman;
        offset = transform.position - player.transform.position;
    }

    void OnDisable()
    {
        Enforcer.Instance.HumanSpawned -= AssignNewHuman;
    }

    void LateUpdate()
    {
        FollowPlayer();
        //CompensateZoomForHuman();
    }

    void FollowPlayer()
    {
        if (!player)
        {
            print("no player");
            return;
        }
        Vector3 newPosition = player.transform.position + offset;
        newPosition.z = -10;    // don't change Z pos
        transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed * Time.deltaTime);
    }

    void CompensateZoomForHuman()
    {
        if (!human) return;
        if (InCamView(human.transform))
            ZoomIn();
        else
            ZoomOut();
    }

    bool InCamView(Transform t)
    {
        Vector3 viewPortPos = cam.WorldToViewportPoint(t.position);
        if (viewPortPos.x < 0f || viewPortPos.x > 0.95f) return false;   //view port is between 0-1.0 so if it's outta those bounds it's not in view
        return true;
    }

    void ZoomIn()
    {
        if (cam.orthographicSize > idealOrthoSize)
            cam.orthographicSize -= (camZoomSpeed * Time.deltaTime);
    }

    void ZoomOut()
    {
        if (cam.orthographicSize < maxOrthoSize)
            cam.orthographicSize += (camZoomSpeed * Time.deltaTime);
    }


    void AssignNewHuman(GameObject newHuman)
    {
        human = newHuman;
    }
}
