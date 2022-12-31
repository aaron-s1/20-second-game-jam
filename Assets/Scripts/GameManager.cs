using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
// using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance { get; private set; }

    [SerializeField] GameObject cowToSpawn;
    [SerializeField] int scoreNeededToWin;

    [Space(10)]
    [SerializeField] GameObject gameOverScreens;
    [SerializeField] GameObject startScreen;
    [Space(5)]
    [SerializeField] TextMeshProUGUI gameOutcomeScreenText;
    [SerializeField] TextMeshProUGUI textScore;
    [SerializeField] TextMeshProUGUI countdownText;


    [Space(10)]
    [SerializeField] Image totalMilkedImageBar;      // rename to something better later

    [Space(5)]
    [SerializeField] AudioSource audioSource;


    [HideInInspector] public bool gameHasStarted;
    [HideInInspector] public bool gameHasEnded;


    Coroutine countdownToGameEnd;

    int gameSecondsRemaining;

    int score;

    bool cowCanMoo;


    void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        cowCanMoo = true;

        gameSecondsRemaining = 20;
        countdownText.text = gameSecondsRemaining.ToString();
    }


    public void SpawnNewMooMoo()
    {
        if (cowCanMoo)
            audioSource.PlayOneShot(audioSource.clip);

        int x_randomPos = Random.Range(-7, 7);
        int y_RandomPos = Random.Range(-3, 3);

        var newCow = Instantiate(cowToSpawn, new Vector3 (x_randomPos, y_RandomPos, 0), RandomizeLookDirection());
        newCow.SetActive(true);
    }


    // makes cow sometimes face right, sometimes face left
    Quaternion RandomizeLookDirection()
    {
        var decideRotation = Random.Range(1, 3);

        if (decideRotation == 1)
            return cowToSpawn.transform.rotation;   // -90f by default
        return Quaternion.Euler(0, 90f, 0);
    }


    IEnumerator CountdownToGameEnd()
    {
        while (true) {
            yield return new WaitForSeconds(1f);
            gameSecondsRemaining--;
            countdownText.text = gameSecondsRemaining.ToString();

            if (gameSecondsRemaining == 0)
                EndGame();
        }
    }


    void EndGame()
    {
        StopCoroutine(countdownToGameEnd);
        countdownToGameEnd = null;

        gameHasEnded = true;

        gameOverScreens.SetActive(true);

        if (score >= scoreNeededToWin)
            gameOutcomeScreenText.text = "Game won!";
        else gameOutcomeScreenText.text = "Game lost :(";
    }



    void Update() {
        SpaceKeyStartsGame();
        I_KeyReloadsGameAfterGameEnds();
        M_KeyTogglesMooSound();
    }


    void SpaceKeyStartsGame()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !gameHasStarted) {
            if (!gameHasEnded) {
                gameHasStarted = true;
                startScreen.SetActive(false);
                countdownToGameEnd = StartCoroutine(CountdownToGameEnd());
            }
        }
    }


    void M_KeyTogglesMooSound()
    {
        if (Input.GetKeyDown(KeyCode.M))
            cowCanMoo = !cowCanMoo;
    }


    void I_KeyReloadsGameAfterGameEnds()
    {
        if (!gameHasEnded)
            return;
        
        if (Input.GetKeyDown(KeyCode.I)) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }


    public void IncrementScore(int addAmount = 1) 
    {        
        score += addAmount;
        textScore.text = score.ToString();

        // Fade in homielander's clips when Score is 10 and 20.
        if ((score % 10) == 0)
            VideoPlayerManager.Instance.AssignAndPlayClip();

        IncreaseMilk_In_UI(addAmount);
    }


    void IncreaseMilk_In_UI(int multiplier)
    {
        var currentFill = totalMilkedImageBar.fillAmount;

        if (currentFill >= 1f)
            return;
            
        var milkToAdd = (1f/scoreNeededToWin) * multiplier;
        var combinedMilkAmount = currentFill + milkToAdd;

        if (combinedMilkAmount >= 1f)
            totalMilkedImageBar.fillAmount = 1f;
        else totalMilkedImageBar.fillAmount = combinedMilkAmount;
    }
}
