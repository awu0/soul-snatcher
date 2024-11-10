using UnityEngine;
using System;
using System.Collections.Generic;

// Base context class that all ability contexts will inherit from
public abstract class AbilityContext
{
    public Grids Grids;
    public int Damage { get; set; }
}

// For abilities that need target and damage info
public class TargetedContext : AbilityContext {
  public Entity Target { get; set; }
  public Vector2Int Direction { get; set; }
}

// For movement/directional abilities
public class DirectionalContext : AbilityContext {
  public Vector2Int Direction { get; set; }
}

// For abilities that only affect player attributes
public class BuffContext : AbilityContext {
  // these abilities simply need activation
}



