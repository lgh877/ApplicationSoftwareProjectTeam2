using ApplicationSoftwareProjectTeam2.entities;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int Price { get; set; }
        public ItemType Type { get; set; }
        public string Description { get; set; }

        public float AttackBonus { get; set; }
        //public float DefenseBonus { get; set; }
        public int SpeedBonus { get; set; }
        public float HealthBonus { get; set; }
        public int KnockbackPowerBonus { get; set; }
        public int WeightBonus { get; set; }
        public float ElasticityBonus { get; set; }


        public Item(string name, int price, ItemType type, string description)
        {
            Name = name;
            Price = price;
            Type = type;
            Description = description;
        }

        public virtual void ApplyTo(LivingEntity unit)
        {
            unit.finalAttackDamage += AttackBonus;
            //unit.Defense += DefenseBonus;
            unit.moveSpeed += SpeedBonus;
            unit.finalMaxHealth += HealthBonus;
            unit.pushPower += KnockbackPowerBonus;
            unit.weight += WeightBonus;
            unit.elasticForce += ElasticityBonus;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}