using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Interfaces
{
    using Items;

    public interface ICollect
    {
        IEnumerable<Item> Inventory { get; }

        
    }
}
