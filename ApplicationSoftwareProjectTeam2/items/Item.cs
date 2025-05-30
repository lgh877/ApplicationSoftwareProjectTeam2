namespace ApplicationSoftwareProjectTeam2.items
{
    public class Item
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public ItemType Type { get; set; }
        public string Description { get; set; }

        public float AttackBonus { get; set; }
        public float DefenseBonus { get; set; }
        public float SpeedBonus { get; set; }
        public float HealthBonus { get; set; }
        public float KnockbackPowerBonus { get; set; }
        public float WeightBonus { get; set; }
        public float ElasticityBonus { get; set; }


        public Item(string name, int price, ItemType type, string description)
        {
            Name = name;
            Price = price;
            Type = type;
            Description = description;
        }

        public void ApplyTo(Unit unit)
        {
            unit.Attack += AttackBonus;
            unit.Defense += DefenseBonus;
            unit.Speed += SpeedBonus;
            unit.Health += HealthBonus;
            unit.KnockbackPower += KnockbackPowerBonus;
            unit.Weight += WeightBonus;
            unit.Elasticity += ElasticityBonus;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}