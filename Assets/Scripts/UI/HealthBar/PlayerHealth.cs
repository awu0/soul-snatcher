using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{

    public Player playerScript;
    public Image healthBar;
    public Image damagedBar;
    public float cutBarOffset;
    public Transform cutBarTemplate;
    public TextMeshProUGUI healthText;
    public GameObject HitTextPrefab;

    public bool takeDamage;
    public bool heal;

    private float currentHealth;
    private float currentMax;
    public float barWidth = 332.5f;
    private float damagedHealthShrinkTimer;
    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 1f;
    private float damageTaken = 0;
    private float healTaken = 0;

    private void Awake()
    {
        cutBarTemplate = transform.Find("CutBarTemplate");
        if (playerScript != null)
        {
            healthText.text = $"{playerScript.health}";
        }
    }

    private void Start()
    {
        playerScript = FindObjectOfType<Player>();
        currentHealth = playerScript.health;
        currentMax = playerScript.maxHealth;
        damagedBar.fillAmount = healthBar.fillAmount;
        Debug.Log(playerScript.health);
        Debug.Log(playerScript.maxHealth);
    }

    private void Update()
    {
        if (playerScript != null) {
            if (healthText.text == "0")
            {
                healthText.text = $"{playerScript.health}";
            }

            if (playerScript.health < currentHealth)
            {
                damageTaken = currentHealth - playerScript.health;
                SetDamage();
                currentHealth = playerScript.health;
                healthText.text = $"{playerScript.health}";
            }
            else if (playerScript.health > currentHealth) {
                healTaken = playerScript.health - currentHealth;
                SetHeal();
                currentHealth = playerScript.health;
                healthText.text = $"{playerScript.health}";
            }

            if (takeDamage)
            {
                playerScript.health -= 1;
                takeDamage = false;
            }
            if (heal)
            {
                playerScript.health += 1;
                heal = false;
            }
        }

        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0) {
            if (healthBar.fillAmount < damagedBar.fillAmount) {
                float shrinkSpeed = .1f;
                damagedBar.fillAmount -= shrinkSpeed * Time.deltaTime;
            }  
        }

    }

    private void SetDamage() {
        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
        float beforeDamagedFillAmount = healthBar.fillAmount;
        healthBar.fillAmount = Mathf.Clamp((float)playerScript.health / playerScript.maxHealth, 0, 1);
        Transform cutBar = Instantiate(cutBarTemplate, transform);
        cutBar.gameObject.SetActive(true);
        cutBar.transform.localScale = new Vector3(1.4f, 1.4f, 1);
        cutBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(healthBar.fillAmount * barWidth + barWidth/2 + cutBarOffset,
            cutBar.GetComponent<RectTransform>().anchoredPosition.y);
        cutBar.GetComponent<Image>().fillAmount = beforeDamagedFillAmount - healthBar.fillAmount;
        Debug.Log(cutBar.GetComponent<Image>().fillAmount);
        cutBar.gameObject.AddComponent<HealthBarCutFallDown>();

        if (HitTextPrefab != null) {
            showHitText(true);
        }

    }

    void showHitText(bool damage)
    {
        var hitText = Instantiate(HitTextPrefab, playerScript.transform.position, Quaternion.identity);
        hitText.GetComponent<TextMeshPro>().text = damageTaken.ToString();

        if (!damage) {
            hitText.GetComponent<TextMeshPro>().color = Color.green;
            hitText.GetComponent<TextMeshPro>().text = healTaken.ToString();
        }
    }

    private void SetHeal()
    {
        healthBar.fillAmount = Mathf.Clamp((float)playerScript.health / playerScript.maxHealth, 0, 1);
        damagedBar.fillAmount = healthBar.fillAmount;

        if (HitTextPrefab != null && playerScript.health != playerScript.maxHealth)
        {
            showHitText(false);
        }
    }
}
