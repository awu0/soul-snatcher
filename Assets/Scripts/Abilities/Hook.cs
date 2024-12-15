using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damages every entity in a straight line starting from the Caster.
/// </summary>
public class Hook : Ability
{
    public override AbilityType Type => AbilityType.Directional;
    private GameObject chainTilePrefab;
    private GameObject hookTilePrefab;
    private GameObject hookGameObject;
    private Entity hookedEntity;
    private List<Vector2Int> chainTileLocations = new List<Vector2Int>(); 
    private List<GameObject> chainTiles = new List<GameObject>(); 

    protected override void ActivateInternal(AbilityContext context) {
        chainTiles.Clear();
        chainTileLocations.Clear();
        chainTilePrefab = Resources.Load<GameObject>("Prefabs/ChainTile");
        if (chainTilePrefab == null)
        {
            Debug.LogWarning("chain tile prefab not found!");
        }

        hookTilePrefab = Resources.Load<GameObject>("Prefabs/HookTile");
        if (chainTilePrefab == null)
        {
            Debug.LogWarning("hook tile prefab not found!");
        }

        Debug.Log($"{gameObject} used Hook");
        
        audioManager.playHookAbility();
        
        var directionalContext = (DirectionalContext)context;
        var (startX, startY) = Caster.GetCurrentPosition();

        Vector2Int current = new Vector2Int(startX, startY) + directionalContext.Direction;
        Grids grids = directionalContext.Grids;
        
        // calculate the position directly in front of the caster
        Vector2Int hookedEntityNewPosition = new Vector2Int(startX, startY) + directionalContext.Direction;

        while (grids.IsPositionWithinBounds(current.x, current.y))
        {
            Entity entity = grids.GetEntityAt(current.x, current.y);
            if (entity != null)
            {
                hookedEntity = entity;
                DrawChainSprites(directionalContext);
                DrawHookSprite(current.x, current.y, directionalContext);

                entity.TakeDamage(directionalContext.Damage);
                
                // pull the entity towards the caster
                entity.MoveTo(hookedEntityNewPosition.x, hookedEntityNewPosition.y);

                if (hookGameObject != null) {
                    hookGameObject.transform.position = new Vector3(entity.gameObject.transform.position.x, entity.gameObject.transform.position.y, 0);
                }

                // targets one entity
                break;
            }
            chainTileLocations.Add(new Vector2Int(current.x, current.y));
            current += directionalContext.Direction;
        }
    }

    private void DrawChainSprites(DirectionalContext directionalContext) {
      foreach (Vector2Int chainTileLocation in chainTileLocations) {
        GameObject chainTile = Instantiate(chainTilePrefab, new Vector3(chainTileLocation.x, chainTileLocation.y, 0), Quaternion.identity);
        chainTiles.Add(chainTile);

        if (directionalContext.Direction.x == 0 && Mathf.Abs(directionalContext.Direction.y) > 0) {
            chainTile.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
      }
    }

    private void DrawHookSprite(int x, int y, DirectionalContext directionalContext) {
      hookGameObject = Instantiate(hookTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
 
      if (directionalContext.Direction == Vector2Int.up) {
          hookGameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
      }
      else if (directionalContext.Direction == Vector2Int.down) {
          hookGameObject.transform.rotation = Quaternion.Euler(270, 0, 0);
      }
      else if (directionalContext.Direction == Vector2Int.left) {
          hookGameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
      }
      else if (directionalContext.Direction == Vector2Int.right) {
          hookGameObject.transform.rotation = Quaternion.identity;
      }
    }

    private void Update() {
      if (hookGameObject != null && hookedEntity != null) {
        hookGameObject.transform.position = new Vector3(hookedEntity.gameObject.transform.position.x, hookedEntity.gameObject.transform.position.y, 0);

        // Check if the entity crosses a chain tile and remove it
        for (int i = 0; i < chainTiles.Count; i++) {
            Vector2Int chainTilePosition = chainTileLocations[i];

          if (Vector2.Distance(hookedEntity.gameObject.transform.position, new Vector3(chainTilePosition.x, chainTilePosition.y, 0)) < 0.1f) {
            Destroy(chainTiles[i]);
            chainTileLocations.RemoveAt(i);
            
            break;
          }
        }
      }
    }
}
