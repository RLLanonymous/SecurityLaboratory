using Exiled.Events.EventArgs.Player;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace SecurityLaboratory.EventHandlers.Moderation_Logs
{
    public class OnKicked
    {
        private readonly DiscordSocketClient _client;
        
        public OnKicked(DiscordSocketClient client)
        {
            _client = client;
        }

        public async void OnPlayerKicked(KickedEventArgs ev)
        {
            var channel = _client.GetChannel(SecurityLaboratory.Instance.Config.ModerationLogsChannelId) as IMessageChannel;
            if (channel != null)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("🚨 A player has been kicked ! 🚨")
                    .WithColor(Color.Red)
                    .AddField("👤 Player:", $"{ev.Player.Nickname} ({ev.Player.UserId})", true)
                    .AddField("📜 Reason:", ev.Reason, false)
                    .WithFooter($"Kick registered on: {System.DateTime.UtcNow}")
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