using System.Collections.Generic;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class Unit
    {
        public string Name { get; set; }
        public float Attack { get; set; }
        public float Defense { get; set; }
        public float Speed { get; set; }
        public float Health { get; set; }

        public int MaxItemSlots { get; set; } = 3;
        public List<Item> EquippedItems { get; set; } = new List<Item>();

        public bool EquipItem(Item item)
        {
            if (EquippedItems.Count >= MaxItemSlots)
                return false;

            item.ApplyTo(this);
            EquippedItems.Add(item);
            return true;
        }
    }
}