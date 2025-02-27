using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SecurityLaboratory.API.Interfaces;
using SecurityLaboratory.API.Modules;
using Exiled.API.Features;

namespace SecurityLaboratory.Handlers
{
    public class DiscordBot : IRegisterable
    {
        public static DiscordBot Instance { get; private set; }
        
        public DiscordSocketClient Client { get; private set; }

        private SocketGuild _guild;

        public void Init()
        {
            Instance = this;
            DiscordSocketConfig config = new()
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages,
                LogLevel = SecurityLaboratory.Instance.Config.Debug ? LogSeverity.Debug : LogSeverity.Warning
            };
            Client = new(config);
            Client.Log += DiscLog;
            Client.Ready += Ready;
            Client.SlashCommandExecuted += SlashCommandHandler;
            Task.Run(StartClient);
        }

        public void Unregister()
        {
            Task.Run(StopClient);
        }

        private Task DiscLog(LogMessage msg)
        {
            if (msg.Exception is WebSocketException or GatewayReconnectException)
            {
                return Task.CompletedTask;
            }
            switch (msg.Severity)
            {
                case LogSeverity.Error or LogSeverity.Critical:
                    Log.Error(msg);
                    break;
                case LogSeverity.Warning:
                    Log.Warn(msg);
                    break;
                default:
                    Log.Info(msg);
                    break;
            }

            return Task.CompletedTask;
        }

        private async Task StartClient()
        {
            Log.Info("Starting Discord bot...");
    
            try
            {
                await Client.LoginAsync(TokenType.Bot, SecurityLaboratory.Instance.Config.Token);
                Log.Info("Bot successfully logged in.");
                await Client.StartAsync();
                Log.Info("Bot started successfully.");
            }
            catch (Exception ex)
            {
                Log.Error($"Error while starting Discord bot: {ex.Message}");
            }
        }

        private async Task StopClient()
        {
            await Client.Rest.DeleteAllGlobalCommandsAsync();
            Log.Debug("All global commands have been deleted.");
            await Client.LogoutAsync();
            await Client.StopAsync();
        }

        public SocketGuild GetGuild(ulong id)
        {
            return id == 0 ? _guild : Client.GetGuild(id);
        }

        private async Task Ready()
        {
            _guild = Client.GetGuild(SecurityLaboratory.Instance.Config.GuildId);
            foreach (ISlashCommand command in SlashCommandLoader.Commands)
            {
                try
                {
                    SocketGuild guild = GetGuild(command.GuildId);
                    if (guild == null)
                    {
                        Log.Warn($"Command {command.Data.Name} failed to register, couldn't find guild {command.GuildId} (from module) nor {SecurityLaboratory.Instance.Config.GuildId} (from the bot). Make sure your guild IDs are correct.");
                        continue;
                    }
                    await guild.CreateApplicationCommandAsync(command.Data.Build());
                }
                catch (Exception exception)
                {
                    Log.Error($"Failed to create guild command '{command.Data.Name}': {exception}");
                }
            }
            await Task.CompletedTask;
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            List<ISlashCommand> commands = SlashCommandLoader.Commands;
            ISlashCommand cmd = commands.FirstOrDefault(c => c.Data.Name == command.Data.Name);
            if (cmd == null) return;
            await cmd.Run(command);
        }
    }
}