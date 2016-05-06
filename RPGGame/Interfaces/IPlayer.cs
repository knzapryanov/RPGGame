using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Interfaces
{
    using Characters;

    public interface IPlayer : ICharacter, IMoveable, ICollect, IHeal, IExperienceable
    {
        PlayerRace Race { get; set; }

        string GetPlayerCurrentStatus();

        int GetPlayerMaximumHealth();        
    }
}
