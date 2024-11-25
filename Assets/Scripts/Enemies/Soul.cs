using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
  public EntityType Type { get; private set; }

  private Transform playerTransform;
  private SpriteRenderer playerSpriteRenderer;
    private SpriteFlasher spriteFlasher;
  private float moveSpeed = 6f;


  public void Initialize(EntityType type) 
  {
    Type = type;
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    spriteFlasher = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteFlasher>();
  }

  private void Start() {
    StartCoroutine(MoveTowardsPlayer());
  }

  private IEnumerator MoveTowardsPlayer() {
    while (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) > 0.1f)
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        yield return null;
    }

    AbsorbSoul();
    spriteFlasher.CallTransformSpriteFlasher();
  }

  private void AbsorbSoul() {
    Player player = playerTransform.GetComponent<Player>();
    if (player != null) {
      player.AbsorbSoul(this);
    }

    Destroy(gameObject);
  }
}