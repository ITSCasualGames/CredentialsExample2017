﻿using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredentialSignalRGameServer
{
    public static class GameDataObjects
    {
        static public List<PlayerData> RegisteredPlayers = new List<PlayerData>()
            {
                new PlayerData {
                playerID = Guid.NewGuid().ToString(),
                CharacterImage = "Player 1",
                GamerTag ="High Flyer",  Password = "plrx",
                PlayerName= "paul", XP = 2000},

                new PlayerData {
                playerID = Guid.NewGuid().ToString(),
                CharacterImage = "Player 2",
                GamerTag ="Bug Hunter",  Password = "plrxx",
                PlayerName= "fred", XP = 200},

            };

    }
}