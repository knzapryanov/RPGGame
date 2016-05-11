using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Interfaces
{
    using Characters;
    using Items;

    public interface IPlayer : ICharacter, IMoveable, ICollect, IHeal, IExperienceable
    {
        PlayerRace Race { get; set; }

        string GetPlayerPotionsInfo();

        int GetPlayerMaximumHealth();

        HealthPotionSize AddHealthPotionToInventory(Item item);

        string AddSwordToInventory(Item item);

        bool HaveSword();
    }
}
