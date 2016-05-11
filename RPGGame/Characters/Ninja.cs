using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Characters
{
    using Attributes;

    [Enemy]
    public class Ninja : Character
    {
        private const int NinjaDamage = 120;
        private const int NinjaHealth = 350;
        private const char NinjaSymbol = 'N';

        public Ninja(Position position, string name)
            : base(position, NinjaSymbol, name, NinjaDamage, NinjaHealth)
        {

        }
    }
}
