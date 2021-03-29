using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public interface IGameState
{
    void OnExit();
    void OnEnter();
}

public class Enforcer : MonoBehaviour   // Enforces rules. Decides if you failed, died, won, whatever!
{
    // Delegates / Events:
    public delegate void ChangedGameStateHandler(IGameState newState);
    public event ChangedGameStateHandler GameStateChanged;

    public delegate void DeadPlayerHandler();
    public event DeadPlayerHandler PlayerDied;

    public delegate void DeadHumanHandler();
    public event DeadHumanHandler HumanHasDied;

    public delegate void HumanSpawnedHandler(GameObject newHuman);
    public event HumanSpawnedHandler HumanSpawned;


    [SerializeField] public PlayerController player;
    [SerializeField] HumanController curHuman;
    [SerializeField] GameObject spawnPoint1;
    [SerializeField] GameObject humanPrefab;

    const float humanLookGracePeriod = 0.35f;

    public IntRef currentPoints;
    [SerializeField] IntRef highScore;
    public bool beatHighScore;
    bool isGhostDead => (player.state == PlayerController.States.DEAD);

    // Audio:
    public AudioClip mainMenuMusic;
    public AudioClip gamePlayMusic;

    // States:
    public static MainMenuState mainMenuState = new MainMenuState();
    public static PlayingState playingState = new PlayingState();
    public static GameOverState gameOverState = new GameOverState();
    public IGameState gameState;

    [SerializeField] GameObject game;

    [SerializeField] SettingsVars settingsVars;

    // Singleton Enforcement
    private static Enforcer _instance;
    public static Enforcer Instance
    {
        get { return _instance; }
    }
    
    public class MainMenuState : IGameState
    {
        public void OnExit()
        {
        }

        public void OnEnter()
        {
        }
    }

    public class PlayingState : IGameState
    {
        public void OnExit()
        {

        }

        public void OnEnter()
        {
            Enforcer.Instance.beatHighScore = false;
            Enforcer.Instance.currentPoints.value = 0;
        }
    }

    public class GameOverState : IGameState
    {
        public void OnExit()
        {
            return;
        }

        public void OnEnter()
        {
            return;
        }
    }
    

    void Awake() 
    { 
        if (_instance != null && _instance != this) 
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        InitCurState();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameState == mainMenuState) return;
        GetObjects();
    }

    void Start()
    {
        currentPoints.value = 0;
        highScore.value = Saver.LoadHighScore();    //load highscore from prefs
        Saver.LoadSettings(settingsVars);
        Application.targetFrameRate = 60;
        GetObjects();
    }

    void GetObjects()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        curHuman = GameObject.Find("Human").GetComponent<HumanController>();
        spawnPoint1 = GameObject.Find("SpawnPoint1");
    }

    void InitCurState() // Guess current state based on scene index
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (sceneIndex)
        {
            case 0:
                ChangeGameState(mainMenuState);
                break;
            case 1:
                ChangeGameState(playingState);
                break;
            default:
                ChangeGameState(mainMenuState);
                break;      
        }
    }

    void ChangeGameState(IGameState newState)
    {
        if (gameState == newState) return;
        gameState?.OnExit();
        gameState = newState;
        gameState.OnEnter();

        GameStateChanged?.Invoke(gameState);
    }

    public void EnforceLook()
    {
        if (!isGhostDead)
            StartCoroutine(EnforceLookEnum());
    }

    IEnumerator EnforceLookEnum()
    {
        yield return new WaitForSeconds(humanLookGracePeriod);
        while (curHuman.state == HumanController.States.LOOKING)    //human looking
        {
            if (player.state == PlayerController.States.MOVING)
            {
                MakeGhostDie();
                break;
            }
            yield return null;
        }
    }

    void MakeGhostDie()
    {
        if (player.state != PlayerController.States.DEAD)
        {
            PlayerDied?.Invoke();
            GameOver();
        }
        else print("Ghost state already DEAD");
    }

    void GameOver()
    {
        UpdateHighScoreIfNeeded();
        ChangeGameState(gameOverState);
        Saver.SaveHighScore(highScore.value);
    }

    public void EnforceKill()
    {
        ++currentPoints.value;
        HumanHasDied?.Invoke();
        SpawnAnotherHuman();
    }

    void UpdateHighScoreIfNeeded()
    {
        if (currentPoints.value > highScore.value)
        {
            highScore.value = currentPoints.value;
            beatHighScore = true;
        }
    }

    void SpawnAnotherHuman()
    {
        Vector3 spawnPoint = spawnPoint1.transform.position;
        GameObject newHuman = Instantiate(humanPrefab, spawnPoint, Quaternion.identity);
        curHuman = newHuman.GetComponent<HumanController>();
        HumanSpawned?.Invoke(newHuman);
    }

    public void HumanOutRanGhost()
    {
        MakeGhostDie();
    }

    // Button Functions (that are binded to UI functions)
    public void RestartGame()
    {
        ChangeGameState(playingState);
        SceneManager.LoadScene(1);
    }

    public void GoToMainMenu()
    {
        ChangeGameState(mainMenuState);
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        ChangeGameState(playingState);
    }
}