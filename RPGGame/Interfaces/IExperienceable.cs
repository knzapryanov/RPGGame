﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame.Interfaces
{
    public interface IExperienceable
    {
        int Experiance { get; }

        void LevelUp();

        void GainExperience(ICharacter enemy);
    }
}
