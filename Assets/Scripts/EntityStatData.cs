using System.Collections.Generic;

public enum EntityType
{
    Slime,
    Weakling,
    GiantPillbug
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
      [EntityType.Slime] = new EntityBaseStats(attack: 5, maxHealth: 20, range: 1),
      [EntityType.GiantPillbug] = new EntityBaseStats(attack: 6, maxHealth: 20, range: 1),
      [EntityType.Weakling] = new EntityBaseStats(attack: 3, maxHealth: 10, range: 1),
  };
}