﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LacunaExpanseAPIWrapper;

namespace LacunaExpress.AccountManagement
{
    public class AccountModel
    {
        public AccountModel() { }
        public AccountModel(string empireName, string password, string server, string sessionID, bool? SetAsActiveAccount)
        {
            EmpireName = empireName;
            Password = password;
            Server = server;
            SessionID = sessionID;
            DisplayName = empireName + " " + server;
        }
        public string DisplayName { get; set; }
        public string EmpireName { get; set; }
        public string Password { get; set; }
        public string SessionID { get; set; }
        public string Server { get; set; }
        public string Capital { get; set; }
        public bool ActiveAccount { get; set; }
        public DateTime CaptchaLastRenewed { get; set; }
        public DateTime SessionRenewed { get; set; }
        public Dictionary<string, string> Colonies { get; set; }
        public Dictionary<string, string> Stations { get; set; }
        public Dictionary<string, string> AllBodies { get; set; }
        public Dictionary<string, List<BuildingCacheList>> Shipyards { get; set; }
        public List<BuildingCache> Parliaments { get; set; }
        public List<BuildingCache> SSLabAs { get; set; }
        public List<BuildingCache> Archmins { get; set; }
        public List<BuildingCache> Intelmins { get; set; }
        public List<BuildingCache> Trademins { get; set; }
    }

}
