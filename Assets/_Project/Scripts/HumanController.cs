using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ExtenMeths;

public class HumanController : MonoBehaviour
{
    public delegate void LookHandler();
    public event LookHandler HumanLooked;

    public enum States
    {
        MOVING,
        LOOKING,
    }

    [SerializeField] List<GameObject> sprites;

    public States state { get; private set; }

    SpriteRenderer spriteRenderer;
    GameObject sprite;
    public Transform ghostTargetLoc;
    
    float speed = 0.3f;
    float lookRate = 3.0f;
    float firstLookTimer = 1.0f;
    Vector3 scale;

    void Awake()
    {
        GetAndSetSprite();
    }

    void Start()
    {
        speed = Random.Range(0.3f, 0.6f);
        lookRate = Random.Range(0.6f, 3.0f); 
        firstLookTimer = Random.Range(0.4f, 3.0f);

        InvokeRepeating("Think", firstLookTimer, lookRate);
        Move();
    }

    void GetAndSetSprite()
    {
        sprite = GetRandSprite();
        scale = sprite.transform.localScale;
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        ghostTargetLoc = sprite.transform.GetChild(0);
        spriteRenderer.enabled = true;
    }

    GameObject GetRandSprite()
    {
        GameObject chosenSprite = sprites[1/*Random.Range(0, sprites.Count)*/]; // remove to get rand human skins
        return chosenSprite;
    }

    void OnEnable()
    {
        HumanLooked += Enforcer.Instance.EnforceLook;   // Maybe have the enforcer do this when he spawns a Human?
        Enforcer.Instance.HumanHasDied += Die;
        Enforcer.Instance.HumanSpawned += OnSpawned;
    }

    void OnDisable()
    {
        HumanLooked -= Enforcer.Instance.EnforceLook;   // Maybe have the enforcer do this when he spawns a Human?
        Enforcer.Instance.HumanHasDied -= Die;
        Enforcer.Instance.HumanSpawned -= OnSpawned;
    }

    void Think()
    {
        switch (state)
        {
            case States.MOVING: // If Moving, LOOK
                Look();
                break;
                
            case States.LOOKING:
                Move();
                break;
        }
    }

    void Update()
    {
        if (state == States.MOVING)
        {
            Vector3 movepos = transform.position;
            movepos.x += (speed * Time.deltaTime);
            transform.position = movepos;
        }
    }

    void Look()
    {
        transform.DOKill(); //Kill current tweens
        state = States.LOOKING;
        spriteRenderer.flipX = true;
        HumanLooked?.Invoke();
        //SoundMan.instance.PlaySound(8);
    }

    void Move()
    {
        state = States.MOVING;
        spriteRenderer.flipX = false;
        StartCoroutine(WalkTweenCycle());
        //SoundMan.instance.PlaySound(9);
    }

    void Die()
    {
        transform.DOKill();
        Destroy(gameObject);
    }

    void OnSpawned(GameObject humanSpawned)
    {
        if (humanSpawned != this.gameObject)
            return;
        
        StartCoroutine(WaitThenAnimateSpawning());

    }

    IEnumerator WaitThenAnimateSpawning()
    {
        scale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, -90f);
        yield return new WaitForSeconds(0.5f);
        transform.DOScale(scale, 0.2f).SetEase(Ease.InOutElastic);
        transform.DORotate(Vector3.zero, 1.0f).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(0.2f);
        transform.localScale = scale;   // HOPEFULLY THIS FIXES SMALL BOY
    }
    
    IEnumerator WalkTweenCycle()
    {
        while (state == States.MOVING)
        {
            Tween tweeny = transform.DORotate(new Vector3(0, 0, -10f), 0.2f).SetEase(Ease.InOutCirc);
            yield return tweeny.WaitForCompletion();
            tweeny = transform.DORotate(new Vector3(0, 0, 10f), 0.2f).SetEase(Ease.InOutCirc);
            yield return tweeny.WaitForCompletion();
        }
        transform.DORotate(new Vector3(0, 0, 0), 0.2f);
    }
}
