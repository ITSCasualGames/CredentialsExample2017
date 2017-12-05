using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using GameData;

namespace CredentialSignalRGameServer
{
    public class CredentialHub : Hub
    {
        
        public void Hello()
        {
            Clients.All.hello();
        }

        public PlayerData checkCredentials(string name, string password)
        {
            return GameDataObjects.RegisteredPlayers
                                  .FirstOrDefault(p => p.PlayerName == name.ToLower() 
                                   && p.Password == password.ToLower());
            
            
        }
    }
}