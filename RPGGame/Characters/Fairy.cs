using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Characters
{
    using Attributes;

    [Enemy]
    public class Fairy : Character
    {
        private const int FairyDamage = 80;
        private const int FairyHealth = 120;
        private const char FairySymbol = 'F';

        public Fairy(Position position, string name)
            : base(position, FairySymbol, name, FairyDamage, FairyHealth)
        {

        }
    }
}
