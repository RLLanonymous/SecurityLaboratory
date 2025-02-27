﻿using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SecurityLaboratory
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        
        public bool Debug { get; set; } = false;
        
        [Description("The token of the bot.")] 
        public string Token { get; set; } = "token";
        
        [Description("The default guild where the bot will be used. You can set this individually for each module, but if a module doesn't have a guild id set, it will use this one.")]
        public ulong GuildId { get; set; } = new();
        
        
    }
}