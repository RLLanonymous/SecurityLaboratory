using Discord;
using Discord.WebSocket;
using SecurityLaboratory.API.Interfaces;
using Exiled.API.Features;
using System;
using System.Threading.Tasks;

namespace SecurityLaboratory.Commands
{
    public class RestartSLCommand : ISlashCommand
    {
        public SlashCommandBuilder Data { get; } = new()
        {
            Name = "restart",
            Description = "Restarts the SCP:SL server.",
            DefaultMemberPermissions = GuildPermission.Administrator,
        };

        public ulong GuildId { get; set; } = SecurityLaboratory.Instance.Config.GuildId;

        public async Task Run(SocketSlashCommand command)
        {
            await command.RespondAsync("Restarting the server... Please wait.", ephemeral: false);
            
            // Try to restart the server
            try
            {
                Exiled.API.Features.Server.Restart();
            }
            
            // In case of an exception, send a message to the user
            catch (Exception ex)
            {
                await command.FollowupAsync($"An error occurred while trying to restart the server: {ex.Message}", ephemeral: false);
                Log.Debug($"An error occurred while trying to restart the server: {ex.Message}");
            }
        }
    }
}