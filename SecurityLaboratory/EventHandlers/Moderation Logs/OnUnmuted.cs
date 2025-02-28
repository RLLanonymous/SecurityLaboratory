using Exiled.Events.EventArgs.Player;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace SecurityLaboratory.EventHandlers.Moderation_Logs
{
    public class OnUnmuted
    {
        private readonly DiscordSocketClient _client;
        
        public OnUnmuted(DiscordSocketClient client)
        {
            _client = client;
        }

        public async void OnPlayerUnmute(RevokingMuteEventArgs ev)
        {
            var channel = _client.GetChannel(SecurityLaboratory.Instance.Config.ModerationLogsChannelId) as IMessageChannel;
            if (channel != null)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("🚨 A player has been Unmute ! 🚨")
                    .WithColor(Color.Red)
                    .AddField("👤 Player:", $"{ev.Player.Nickname} ({ev.Player.UserId})", true)
                    .WithFooter($"Unmute registered on: {System.DateTime.UtcNow}")
                    .Build();

                await channel.SendMessageAsync(embed: embed);
            }
            else
            {
                Log.Warn("Discord ModerationLogsChannelId is not set in the config file or isn't working.");
            }
        }
    }
}