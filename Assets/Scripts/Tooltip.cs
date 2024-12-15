using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tooltip : MonoBehaviour
{
    private Dictionary<EntityType, string> enemyTooltips;
    private Grids grids;

    private void Awake()
    {
        enemyTooltips = new Dictionary<EntityType, string>();

        foreach (var entity in EntityData.EntityBaseStatMap)
        {
            EntityType type = entity.Key;
            EntityBaseStats stats = entity.Value;

            string tooltip = $"{EntityData.EntityStringMap[type]}\n" +
                             $"Health: {stats.MaxHealth}\n" +
                             $"Attack: {stats.Attack}\n" +
                             $"Range: {stats.Range}\n" +
                             $"Ability: {stats.Ability}";

            enemyTooltips[type] = tooltip;
        }

        var gridObject = GameObject.FindGameObjectWithTag("Game Board");
        if (gridObject != null)
        {
            grids = gridObject.GetComponent<Grids>();
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
            return "No tooltip available.";
        }
    }
}