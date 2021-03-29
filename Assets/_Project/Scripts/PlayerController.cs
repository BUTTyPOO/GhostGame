using UnityEngine;
using System.Collections;

using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public delegate void KillHandler();
    public event KillHandler HumanKilled;

    public enum States
    {
        PAUSED,
        NULL,
        HIDING,
        MOVING,
        DEAD,
        KILLING,
    }

    [SerializeField] public States state { get; private set; }

    Animator animator;
    public GameObject curHuman;
    public HumanController curHumanScript;
    Transform moveTarget;

    ParticleSystem bloodParts;

    const float MouthOpenDist = 1.5f;
    const float killRange = 1.0f;
    const float lungeSpeed = 0.4f;
    [SerializeField] float speed = 1.5f;

    bool mouthOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(1).GetComponent<Animator>();
        bloodParts = transform.GetChild(0).GetComponent<ParticleSystem>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        Think();
        HandleInput();
    }

    void OnEnable()
    {
        HumanKilled += Enforcer.Instance.EnforceKill;
        Enforcer.Instance.HumanSpawned += UpdateCurHuman;
        Enforcer.Instance.GameStateChanged += GameStateChanged;
        Enforcer.Instance.PlayerDied += Die;
        UpdateCurHuman(curHuman);
    }

    void OnDisable()
    {
        HumanKilled += Enforcer.Instance.EnforceKill;
        Enforcer.Instance.HumanSpawned -= UpdateCurHuman;
        Enforcer.Instance.GameStateChanged -= GameStateChanged;
        Enforcer.Instance.PlayerDied -= Die;
    }

    void GameStateChanged(IGameState newState)
    {
        if (newState == Enforcer.mainMenuState)
            state = States.PAUSED;
        else if (newState == Enforcer.playingState)
            state = States.NULL;
    }

    void Think()
    {
        OpenOrCloseMyMouthIfIShould();
        KillIfIcan();
    }

    void OpenOrCloseMyMouthIfIShould()
    {
        if (!curHuman || state == States.KILLING) return;

        float distance = Vector2.Distance(transform.position, curHuman.transform.position);
        if (distance <= MouthOpenDist)
            OpenMouth();
        else
            CloseMouth();
    }

    void OpenMouth()
    {
        if (mouthOpen || state == States.HIDING) return;
        animator.SetBool("mouthOpen", true);
        SoundMan.instance.PlaySound(7);
        mouthOpen = true;
    }

    void CloseMouth()
    {
        if (!mouthOpen) return;
        animator.SetBool("mouthOpen", false);
        mouthOpen = false;
    }

    void KillIfIcan()
    {
        if (IsCloseEnoughToKill())
            Kill();
    }

    bool IsCloseEnoughToKill()
    {
        if (!curHuman) return false;
        float distance = Vector2.Distance(transform.position, curHuman.transform.position);
        if (distance <= killRange)
            return true;
        return false;
    }

    void Kill()
    {
        state = States.KILLING;
        LungeTo(curHuman.transform.position);
        UpdateAnimViaState();
        bloodParts.Play();
        HumanKilled?.Invoke();
        StartCoroutine(WaitThenChangeState());
        SoundMan.instance.PlaySound(2, 0.25f);
    }

    void LungeTo(Vector2 pos)
    {
        transform.DOMove(pos, lungeSpeed).SetEase(Ease.OutExpo);
    }

    IEnumerator WaitThenChangeState()
    {
        yield return new WaitForSeconds(lungeSpeed);
        state = States.NULL;
        UpdateAnimViaState();
    }

    void StartMoving()
    {
        state = States.MOVING;
        UpdateAnimViaState();
    }
    
    void Hide()
    {
        state = States.HIDING;
        UpdateAnimViaState();
        mouthOpen = false;
    }

    void Die()
    {
        if (state == States.DEAD) return;
        state = States.DEAD;
        UpdateAnimViaState();
        SoundMan.instance.PlaySound(6, 1.0f);
    }

    void HandleInput()
    {
        if (state == States.DEAD || state == States.KILLING || state == States.PAUSED)
            return;

        if (Input.GetMouseButton(0))
        {
            MoveTowardsEnemy();
        }
        else
        {
            Hide();
        }
    }

    void MoveTowardsEnemy()
    {
        if (!curHuman) return;
        if (state != States.MOVING) StartMoving();
        transform.position = Vector2.MoveTowards(transform.position, moveTarget.position, speed * Time.deltaTime);
    }

    void UpdateAnimViaState()
    {
        MakeAllAnimBoolsFalse();
        switch (state)
        {
            case States.MOVING:
                animator.SetBool("hiding", false);
                break;
            case States.HIDING:
                animator.SetBool("hiding", true);
                break;
            case States.DEAD:
                animator.SetBool("dead", true);
                break;
            case States.KILLING:
                animator.SetBool("kill", true);
                animator.SetBool("mouthOpen", true);
                break;
        }
    }

    void MakeAllAnimBoolsFalse()
    {
        animator.SetBool("hiding", false);
        animator.SetBool("dead", false);
        animator.SetBool("kill", false);
        animator.SetBool("mouthOpen", false);
    }

    void UpdateCurHuman(GameObject newHuman)
    {
        curHuman = newHuman;
        curHumanScript = curHuman.GetComponent<HumanController>();
        moveTarget = curHumanScript.ghostTargetLoc;
    }
}