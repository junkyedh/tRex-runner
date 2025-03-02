using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrexRunner.Entities
{
    //Contact chung cho cac class muon xac dinh collision box
    public interface ICollidable
    {
        Rectangle CollisionBox { get; }
    }
}

