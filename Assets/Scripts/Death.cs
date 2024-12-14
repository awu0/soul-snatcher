using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Death : MonoBehaviour
{
    public Button startButton;
    public Button menuButton;
    public GameManager gameManager;
    public TextMeshProUGUI tipText;
    private Dictionary<EntityType, string> enemyTips;

    private void Awake()
    {
        enemyTips = new Dictionary<EntityType, string>
        {
            { EntityType.GiantPillbug, "Died to: Giant Pillbug\nDon't get hit by its roll!\nIt will stun itself when rolling into a wall." },
            { EntityType.EvilEye, "Died to: Evil Eye\nWatch out for its ranged attack!\nUse obstacles and abilities to your advantage." },
            { EntityType.Snake, "Died to: Snake\nBe wary of its poison attack!" },
            { EntityType.StoneGolem, "Died to: Golem\nIt is a slow enemy...\nuntil it isn't." },
            { EntityType.Hooker, "Died to: Hooker\nAvoid getting locked down by its hook when there are multiple enemies." }
        };
    }
    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(MenuOpen);
        }
        tipText.alignment = TextAlignmentOptions.Center;
    }

    void Update()
    {   
        if (gameManager.lastEnemyTurn != null)
        {
            EntityType lastEnemyType = gameManager.lastEnemyTurn.type;
            enemyTips.TryGetValue(lastEnemyType, out string tip);
            tipText.text = tip;
        }
        else {
            tipText.text = "Died to: Poison\nCareful, you take damage every turn when poisoned by the Snake!";
        }
    }

    void StartGame()
    {
        gameManager.ResetGame();
        gameManager.ToggleDeathScreen(false);
    }

    void MenuOpen()
    {
        SceneManager.LoadScene("StartScreen");
        gameManager.ToggleDeathScreen(false);
    }
}
