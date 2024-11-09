using System.Collections.Generic;

public enum EntityType
{
    Obstacle,
    Slime,
    Weakling,
    GiantPillbug,
    EvilEye,
    Snake,
}

public class EntityBaseStats
{
    public int Attack { get; set; }
    public int MaxHealth { get; set; }
    public int Range { get; set; }

    public EntityBaseStats(int attack, int maxHealth, int range)
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
      [EntityType.Slime] = new EntityBaseStats(attack: 8, maxHealth: 20, range: 1), //PLAYER
      [EntityType.GiantPillbug] = new EntityBaseStats(attack: 5, maxHealth: 12, range: 1),
      [EntityType.Weakling] = new EntityBaseStats(attack: 1, maxHealth: 1, range: 1),
      [EntityType.EvilEye] = new EntityBaseStats(attack: 5, maxHealth: 6, range: 1),
      [EntityType.Snake] = new EntityBaseStats(attack: 5, maxHealth: 8, range: 1),
  };
}