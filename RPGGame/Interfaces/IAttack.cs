using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Interfaces
{
    using Characters;

    public interface IAttack
    {
        int Damage { get; set; }

        void Attack(ICharacter enemy);
    }
}
