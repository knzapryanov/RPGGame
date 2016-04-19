using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Items
{
    public class HealthPotion : Item
    {
        private const char HealthPotionSymbol = 'H';

        public HealthPotion(Position position, HealthPotionSize healthPotionSize)
            : base(position, HealthPotionSymbol)
        {
            this.HealthPotionSize = healthPotionSize;
        }

        public HealthPotionSize HealthPotionSize { get; set; }

        public int HealthRestoreAmount 
        {
            get
            {
                return (int)this.HealthPotionSize;
            }
        }
    }
}
