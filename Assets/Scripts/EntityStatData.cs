using System;
using System.Collections.Generic;

public enum EntityType
{
    Obstacle,
    Slime,
    Weakling,
    GiantPillbug,
    EvilEye,
    Snake,
    StoneGolem,
}

public class EntityBaseStats
{
    public int Attack { get; set; }
    public int MaxHealth { get; set; }
    public int Range { get; set; }

    public EntityBaseStats(int attack, int maxHealth, int range=1)
    {
        Attack = attack;
        MaxHealth = maxHealth;
        Range = range;
    }
}

public static class EntityData
{
  public static readonly Dictionary<EntityType, EntityBaseStats> EntityBaseStatMap = new Dictionary<EntityType, EntityBaseStats>
  {
      [EntityType.Obstacle] = new EntityBaseStats(attack: 0, maxHealth: 50, range: 0),
      [EntityType.Slime] = new EntityBaseStats(attack: 6, maxHealth: 16, range: 1), //PLAYER
      [EntityType.GiantPillbug] = new EntityBaseStats(attack: 5, maxHealth: 12, range: 1),
      [EntityType.Weakling] = new EntityBaseStats(attack: 1, maxHealth: 1, range: 1),
      [EntityType.EvilEye] = new EntityBaseStats(attack: 4, maxHealth: 6),
      [EntityType.Snake] = new EntityBaseStats(attack: 5, maxHealth: 8, range: 1),
      [EntityType.StoneGolem] = new EntityBaseStats(attack: 7, maxHealth: 15),
  };

  public static readonly Dictionary<EntityType, Type> EntityAbilityMap = new Dictionary<EntityType, Type>
  {
      [EntityType.Obstacle] = null,
      [EntityType.Slime] = null, //PLAYER
      [EntityType.GiantPillbug] = typeof(PillbugRoll),
      [EntityType.Weakling] = null,
      [EntityType.EvilEye] = typeof(EyeLaser),
      [EntityType.Snake] = typeof(SnakeBite),
      [EntityType.StoneGolem] = typeof(Guard),
  };
}