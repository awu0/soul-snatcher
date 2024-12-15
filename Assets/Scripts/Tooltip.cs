using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Tooltip : MonoBehaviour
{
    private Dictionary<EntityType, string> enemyTooltips;
    private Grids grids;
    public TextMeshProUGUI tooltipText;
    public Image backdrop;
    public Vector3 offset;

    private void Awake()
    {
        enemyTooltips = new Dictionary<EntityType, string>();

        foreach (var entity in EntityData.EntityBaseStatMap)
        {
            EntityType type = entity.Key;
            EntityBaseStats stats = entity.Value;

            string tooltip = $"Atk: {stats.Attack}\n" +
                            $"Abi: {stats.Ability}";

            enemyTooltips[type] = tooltip;
        }

        var gridObject = GameObject.FindGameObjectWithTag("Game Board");
        if (gridObject != null)
        {
            grids = gridObject.GetComponent<Grids>();
        }
    }

    void Update()
    {
        Entity enemy = grids.GetEntityAtMouse(Input.mousePosition);
        if (enemy != null) 
        {
            tooltipText.gameObject.SetActive(true);
            backdrop.gameObject.SetActive(true);
            tooltipText.text = $"Hp: {enemy.health}/{enemy.maxHealth}\n";
            tooltipText.text += GetTooltip(enemy.type);
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(enemy.transform.position);
            transform.position = screenPosition + offset;
        }
        else {
            tooltipText.gameObject.SetActive(false);
            backdrop.gameObject.SetActive(false);
        }
    }
    public string GetTooltip(EntityType type)
    {
        if (enemyTooltips.ContainsKey(type))
        {
            return enemyTooltips[type];
        }
        else 
        {
            return "no tooltip";
        }
    }
}