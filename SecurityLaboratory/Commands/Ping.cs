using Discord;
using Discord.WebSocket;
using SecurityLaboratory.API.Interfaces;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SecurityLaboratory.Commands
{
    public class PingCommand : ISlashCommand
    {
        public SlashCommandBuilder Data { get; } = new()
        {
            Name = "ping",
            Description = "Responds with Pong and latency in ms.",
            DefaultMemberPermissions = GuildPermission.UseApplicationCommands,
        };

        public ulong GuildId { get; set; } = SecurityLaboratory.Instance.Config.GuildId;

        public async Task Run(SocketSlashCommand command)
        {
            // Check the latency of the bot
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            await command.RespondAsync("Calculating latency...");

            stopwatch.Stop();
            var latency = stopwatch.ElapsedMilliseconds;
            
            await command.ModifyOriginalResponseAsync(msg => msg.Content = $"Pong! The Bot Latency Is: {latency} ms");
        }
    }
}