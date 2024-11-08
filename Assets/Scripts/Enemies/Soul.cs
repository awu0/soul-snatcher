using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
  public EntityType Type { get; private set; }

  public void Initialize(EntityType type) 
  {
    Type = type;
  }
}