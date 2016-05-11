using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Items
{
    public class Sword : Item
    {
        private const char SwordSymbol = 'S';

        public Sword(Position position, int swordDamage, int swordDurability, string swordName)
            : base(position, SwordSymbol)
        {
            this.SwordDamage = swordDamage;
            this.SwordDurability = swordDurability;
            this.SwordName = swordName;
        }

        public int SwordDamage { get; set; }

        public int SwordDurability { get; set; }

        public string SwordName { get; set; }
    }
}
