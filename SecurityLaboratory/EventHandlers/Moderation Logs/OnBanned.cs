using Exiled.Events.EventArgs.Player;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace SecurityLaboratory.EventHandlers.Moderation_Logs
{
    public class OnBanned
    {
        private readonly DiscordSocketClient _client;
        
        public OnBanned(DiscordSocketClient client)
        {
            _client = client;
        }

        public async void OnPlayerBanned(BannedEventArgs ev)
        {
            var channel = _client.GetChannel(SecurityLaboratory.Instance.Config.ModerationLogsChannelId) as IMessageChannel;
            if (channel != null)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("🚨 A player has been banned ! 🚨")
                    .WithColor(Color.Red)
                    .AddField("👤 Player:", $"{ev.Player.Nickname} ({ev.Player.UserId})", true)
                    .AddField("🕒 Expires:", $"{ev.Details.Expires}", true)
                    .AddField("📜 Reason:", ev.Details.Reason, false)
                    .WithFooter($"Ban registered on: {System.DateTime.UtcNow}")
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