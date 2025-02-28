using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.Loader;
using GameCore;
using PluginAPI.Events;
using SecurityLaboratory.API.Modules;
using SecurityLaboratory.EventHandlers.Moderation_Logs;
using SecurityLaboratory.Handlers;
using Log = Exiled.API.Features.Log;
using Player = Exiled.Events.Handlers.Player;
using UpdateStatus = System.Data.UpdateStatus;
using Version = System.Version;

namespace SecurityLaboratory
{
    public class SecurityLaboratory : Plugin<Config, Translations>
    {
        public override string Name { get; } = "Security Laboratory";
        public override string Author { get; } = "Sexy Lanonymous | Forked from DiscordLab";
        public override string Prefix { get; } = "SecurityLaboratory";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override PluginPriority Priority { get; } = PluginPriority.Highest;
        public override Version RequiredExiledVersion { get; } = new Version(9,5,0);
        
        public static SecurityLaboratory Instance { get; private set; }
        private HandlerLoader _handlerLoader;
        private OnBanned _banHandler;
        private OnKicked _kickHandler;
        private OnMuted _muteHandler;
        private OnUnmuted _unmuteHandler;

        public override void OnEnabled()
        {
            Instance = this;
            if(Config.Token is "token" or "")
            {
                Log.Error("Please set the bot token in the config file.");
                return;
            }

            if (Config.GuildId is 0)
            {
                Log.Warn("You have no guild ID set in the config file, you might get errors until you set it. ");
            }

            string restartAfterRoundsConfig = ConfigFile.ServerConfig.GetString("restart_after_rounds", "0");

            if (int.TryParse(restartAfterRoundsConfig, out int restartAfterRounds) &&
                restartAfterRounds is >= 1 and < 10)
            {
                Log.Warn("You have a restart_after_rounds value set between 1 and 9, which isn't recommended. DiscordLab restarts every time your server restarts, so it's recommended" +
                         "to set a high number, or 0, for this value to avoid potential Discord rate limits. This is just a warning.");
            }
            _handlerLoader = new ();
            _handlerLoader.Load(Assembly);
            
            if (DiscordBot.Instance.Client != null)
            {
                _banHandler = new OnBanned(DiscordBot.Instance.Client);
                _kickHandler = new OnKicked(DiscordBot.Instance.Client);
                _muteHandler = new OnMuted(DiscordBot.Instance.Client);
                _unmuteHandler = new OnUnmuted(DiscordBot.Instance.Client);
                
                Player.Banned += _banHandler.OnPlayerBanned;
                Player.Kicked += _kickHandler.OnPlayerKicked;
                Player.IssuingMute += _muteHandler.OnPlayerMute;
                Player.RevokingMute += _unmuteHandler.OnPlayerUnmute;
            }
            else
            {
                Log.Warn("Discord isn't initialized properly to subscribe to exiled events!");
            }
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _handlerLoader.Unload();
            _handlerLoader = null;
            
            if (_banHandler != null)
            {
                Player.Banned -= _banHandler.OnPlayerBanned;
            }
            if (_kickHandler != null)
            {
                Player.Kicked -= _kickHandler.OnPlayerKicked;
            }

            if (_muteHandler != null)
            {
                Player.IssuingMute -= _muteHandler.OnPlayerMute;
            }
            
            if (_unmuteHandler != null)
            {
                Player.RevokingMute -= _unmuteHandler.OnPlayerUnmute;
            }
            base.OnDisabled();
        }
    }
}