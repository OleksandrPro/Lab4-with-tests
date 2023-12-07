﻿using System.Collections.Generic;

namespace Lab4
{
    public class Level1 : Level
    {
        public Level1()
        {            
            player = new Player(500, 150);
            PlayerStateMachine psm = new PlayerStateMachine();
            psm.InitPlayer(player);
            player.InitPSM(psm);
            platforms = new List<Platform> ();
            barrier = new List<SFML.Graphics.FloatRect> ();
            AddPlatform(0, 350, 1300, 50);
            AddPlatform(0, 600, 1300, 50);
            AddPlatform(0, 850, 1300, 50);
        }
    }
}
