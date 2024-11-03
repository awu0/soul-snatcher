using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
  public EnemyType Type { get; private set; }

  public void Initialize(EnemyType type) 
  {
    Type = type;
  }
}