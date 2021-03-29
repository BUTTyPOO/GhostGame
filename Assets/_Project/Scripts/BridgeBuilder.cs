using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BridgeBuilder : MonoBehaviour
{
    const float bridgeDeleteDist = 100.0f;
    const float bridgeHumanYOffset = 0.48f;

    public List<GameObject> bridges;
    GameObject cam;
    public GameObject bridgePrefab;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.gameObject;
        InvokeRepeating("DeleteDistantBridges", 5.0f, 5.0f);
        Enforcer.Instance.HumanSpawned += OnHumanSpawned;
    }

    void OnEnable()
    {
        //Enforcer.Instance.HumanSpawned += OnHumanSpawned; //BUGGED FOR SOME REASON!??!
    }

    void OnDisable()
    {
        Enforcer.Instance.HumanSpawned -= OnHumanSpawned;
    }

    void DeleteDistantBridges()
    {
        List<GameObject> newBridgeList = bridges;
        for (int i = newBridgeList.Count-1; i >= 0 ; --i)
        {
            DeleteBridgeIfFar(bridges[i]);
        }
        bridges = newBridgeList;
    }

    void DeleteBridgeIfFar(GameObject bridge)
    {
        if (IsBridgeFar(bridge))
        {
            bridges.Remove(bridge);
            Destroy(bridge);
        }
    }

    bool IsBridgeFar(GameObject bridge)
    {
        if (Vector3.Distance(cam.transform.position, bridge.transform.position) > bridgeDeleteDist)
            return true;
        return false;
    }

    void OnHumanSpawned(GameObject human)   //FIX RETARDED FUNCTION, WHAT DOES IT DO?!?!??!
    {
        Vector3 bridgeSpawnLoc = human.transform.position;
        bridgeSpawnLoc.y -= bridgeHumanYOffset;
        InstantiateBridge(bridgeSpawnLoc);
    }

    void InstantiateBridge(Vector3 pos)
    {
        GameObject bridge = Instantiate(bridgePrefab, pos, Quaternion.identity, transform);
        bridges.Add(bridge);
        StartCoroutine(WaitThenAnimateSpawning(bridge));
    }

    IEnumerator WaitThenAnimateSpawning(GameObject bridge)
    {
        bridge.transform.localScale = new Vector3(1, 0, 1);
        yield return new WaitForSeconds(0.5f);
        bridge.transform.DOScaleY(1, 0.7f).SetEase(Ease.OutElastic);
    }
}
