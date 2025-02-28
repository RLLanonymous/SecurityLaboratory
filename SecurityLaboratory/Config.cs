using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SecurityLaboratory
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        
        public bool Debug { get; set; } = false;
        
        [Description("The token of the bot.")] 
        public string Token { get; set; } = "token";
        
        [Description("The guild Id where the bot will be used.")]
        public ulong GuildId { get; set; } = new();
        
        [Description("The channel Id of the Moderations Logs.")] 
        public ulong ModerationLogsChannelId { get; set; } = new();
                
        [Description("The channel Id of the Logs.")] 
        public ulong LogsChannelId { get; set; } = new();
    }
}