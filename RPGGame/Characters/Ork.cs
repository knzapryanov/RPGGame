using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Characters
{
    using Attributes;

    [Enemy]
    public class Ork : Character
    {
        private const int OrkDamage = 90;
        private const int OrkHealth = 280;
        private const char OrkSymbol = 'O';

        public Ork(Position position, string name)
            : base(position, OrkSymbol, name, OrkDamage, OrkHealth)
        {

        }        
    }
}
