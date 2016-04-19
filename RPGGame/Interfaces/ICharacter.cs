using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Interfaces
{
    public interface ICharacter : IAttack, IDestroyable
    {
        string Name { get; }

        Position Position { get; }
    }
}
