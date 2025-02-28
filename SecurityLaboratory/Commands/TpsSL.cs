using Discord;
using Discord.WebSocket;
using SecurityLaboratory.API.Interfaces;
using Exiled.API.Features;
using System;
using System.Threading.Tasks;

namespace SecurityLaboratory.Commands
{
    public class TpsSLCommand : ISlashCommand
    {
        public SlashCommandBuilder Data { get; } = new()
        {
            Name = "tps",
            Description = "Check the SCP:SL TPS on the server.",
            DefaultMemberPermissions = GuildPermission.UseApplicationCommands,
        };

        public ulong GuildId { get; set; } = SecurityLaboratory.Instance.Config.GuildId;

        public async Task Run(SocketSlashCommand command)
        {
            try
            {
                await command.RespondAsync($"Server TPS: {Exiled.API.Features.Server.Tps} TPS.", ephemeral: false);;
            }
            catch (Exception ex)
            {
                await command.FollowupAsync($"An error occurred while trying to check the TPS of the server: {ex.Message}", ephemeral: false);
                Log.Error($"An error occurred while trying to check the TPS of the server: {ex.Message}");
            }
        }
    }
}