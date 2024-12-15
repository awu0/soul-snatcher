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
    Hooker,
}

public class EntityBaseStats
{
    public int Attack { get; set; }
    public int MaxHealth { get; set; }
    public int Range { get; set; }

    // simply stores a readable ability for UI
    public string Ability { get; set; }

    public EntityBaseStats(int attack, int maxHealth, int range=1, string ability="None")
    {
        Attack = attack;
        MaxHealth = maxHealth;
        Range = range;
        Ability = ability;
    }
}

public static class EntityData
{
  public static readonly Dictionary<EntityType, EntityBaseStats> EntityBaseStatMap = new Dictionary<EntityType, EntityBaseStats>
  {
      [EntityType.Obstacle] = new EntityBaseStats(attack: 0, maxHealth: 50, range: 0, ability: "None"),
      [EntityType.Slime] = new EntityBaseStats(attack: 5, maxHealth: 13, range: 1, ability: "None"), //PLAYER
      [EntityType.GiantPillbug] = new EntityBaseStats(attack: 4, maxHealth: 12, range: 1, ability: "Roll"),
      [EntityType.Weakling] = new EntityBaseStats(attack: 1, maxHealth: 1, range: 1, ability: "None"),
      [EntityType.EvilEye] = new EntityBaseStats(attack: 4, maxHealth: 6, ability: "Laser"),
      [EntityType.Snake] = new EntityBaseStats(attack: 4, maxHealth: 8, range: 1, ability: "Poison"),
      [EntityType.StoneGolem] = new EntityBaseStats(attack: 5, maxHealth: 14, ability: "Guard"),
      [EntityType.Hooker] = new EntityBaseStats(attack: 3, maxHealth: 11, ability: "Hook"),
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
      [EntityType.Hooker] = typeof(Hook),
  };

  public static readonly Dictionary<EntityType, String> EntityStringMap = new Dictionary<EntityType, String>
  {
      [EntityType.Obstacle] = "Wall",
      [EntityType.Slime] = "Slime", //PLAYER
      [EntityType.GiantPillbug] = "Giant Pillbug",
      [EntityType.Weakling] = "Weakling",
      [EntityType.EvilEye] = "Evil Eye",
      [EntityType.Snake] = "Snake",
      [EntityType.StoneGolem] = "Stone Golem",
      [EntityType.Hooker] = "Hooker",
  };
}