using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public List<GameObject> UIs = new List<GameObject>();

    public int activeUIindex = -1;

    public int defaultPageIndex = 0;

    public TMP_Text remainingTimeDisplay;

    public TMP_Text gameName;

    public TMP_Text startButtonText;

    public GameObject icon1;

    public GameObject icon2;

    public GameObject watches;

    private List<SpriteRenderer> HealthBarRenderers = new List<SpriteRenderer>();

    public List<Sprite> watchesSprites = new List<Sprite>();

    public List<GameObject> HealthBarHearts = new List<GameObject>();

    public Sprite fullHeart;

    public Sprite halfHeart;

    public Sprite emptyHeart;

    public GameObject interactionIndicator;

    public bool initialized = false;


    public GameObject startButton;

    private SpriteRenderer watchRenderer;
    // Start is called before the first frame update
    void Start()
    {
        watchRenderer = watches.GetComponent<SpriteRenderer>();

        foreach(var heart in HealthBarHearts) {
            HealthBarRenderers.Add(heart.GetComponent<SpriteRenderer>());
        }

        int i = 0;
        foreach(var ui in UIs) {
            if(i == defaultPageIndex) {
                ui.SetActive(true);
                activeUIindex = i;
            }
            else {
                ui.SetActive(false);
            }
            
            i++;
        }

        watchRenderer.sprite = watchesSprites[0];
        remainingTimeDisplay.text = "0:00";

        initialized = true;
    }
    

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        UpdateHealthBar();
        UpdateIntInidicator();
        UpdateButton();
        UpdateTitle();
    }

    void UpdateIntInidicator() {
        if(GameManager.Instance.GetPrimaryPlayer() != null) {
            if(GameManager.Instance.GetPrimaryPlayer().GetComponent<Player>().objectsToInteract.Count > 0) {
                interactionIndicator.SetActive(true);
            }
            else {
                interactionIndicator.SetActive(false);
            }
        }
    }

    public void UpdateTitle() {
        if(GameManager.Instance.gameLost) {
            gameName.text = "Game over";
            icon1.SetActive(false);
            icon2.SetActive(false);
        }
        else if(GameManager.Instance.gameWon) {
            gameName.text = "You are not stuck!";
            icon1.SetActive(false);
            icon2.SetActive(false);
        }
        else {
            gameName.text = "Stuck";
            icon1.SetActive(true);
            icon2.SetActive(true);
        }
    }

    public void UpdateButton() {
        if(GameManager.Instance.gameLost) {
            startButton.SetActive(false);
        }
        else if(GameManager.Instance.gameWon) {
            startButton.SetActive(false);
        }
        else if(!GameManager.Instance.gameLost && GameManager.Instance.gameStarted) {
            startButtonText.text = "Resume";
        }
        else {
            startButtonText.text = "Start game";
        }
    }

    public void OnHelp() {
        setUIPage(2);
    }

    public void OnBack() {
        setUIPage(0);
    }

    public void setUIPage(int index) {
        if(index == activeUIindex) {
            return;
        }
        else {
            if(activeUIindex >= 0) {
                UIs[activeUIindex].SetActive(false);
            }

            activeUIindex = index;
            if(index >= 0) {
                UIs[index].SetActive(true);
                //Debug.Log(index);
            }
        }
    }

    void UpdateTime() {
        if(GameManager.Instance.GetPrimaryPlayer() != null) {
            float totalTime = GameManager.Instance.GetPrimaryPlayer().GetComponent<Player>().timeLoopLength;
            float current = GameManager.Instance.GetPrimaryPlayer().GetComponent<Player>().remainingTime;

            float perc = current/totalTime;
            int newSpriteIndex = (watchesSprites.Count - 1) - Mathf.RoundToInt(perc*(watchesSprites.Count-1));

            if(newSpriteIndex < 0) {
                newSpriteIndex = 0;
            }
            else if(newSpriteIndex > watchesSprites.Count - 1) {
                newSpriteIndex = watchesSprites.Count-1;
            }

            watchRenderer.sprite = watchesSprites[newSpriteIndex];
            
            int seconds = Mathf.RoundToInt(current) % 60;
            int minutes = Mathf.RoundToInt(current) / 60;

            seconds = seconds < 0 ? 0 : seconds;

            string secondsStr = seconds.ToString();
            secondsStr = secondsStr.Length == 1 ? "0" + secondsStr : secondsStr ;

            remainingTimeDisplay.text = minutes.ToString() + ":" + secondsStr;
        }
    }


    void UpdateHealthBar() {
        if(GameManager.Instance.GetPrimaryPlayer() != null) {
            float totalHP = GameManager.Instance.GetPrimaryPlayer().GetComponent<Player>().totalHP;
            float current = GameManager.Instance.GetPrimaryPlayer().GetComponent<Player>().HP;

            float perc = current/totalHP;
            float percStep = 1.0f/HealthBarRenderers.Count;
            bool empty = false;
            for(int i = 0; i < HealthBarRenderers.Count; i++) {
                if(empty || perc == 0.0f) {
                    HealthBarRenderers[i].sprite = emptyHeart;
                    continue;
                }

                perc = perc - percStep;
                
                if(perc >= 0.0f) {
                    HealthBarRenderers[i].sprite = fullHeart;
                }
                else {
                    if((perc < 0.0f && perc > -percStep) || i == 0) {
                        HealthBarRenderers[i].sprite = halfHeart;
                    }
                    else {
                        HealthBarRenderers[i].sprite = emptyHeart;
                    }

                    empty = true;
                }
            }
        }
    }
}
