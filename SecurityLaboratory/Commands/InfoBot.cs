using Discord;
using Discord.WebSocket;
using SecurityLaboratory.API.Interfaces;
using System.Threading.Tasks;

namespace SecurityLaboratory.Commands
{
    public class InfoBotCommand : ISlashCommand
    {
        public SlashCommandBuilder Data { get; } = new()
        {
            Name = "infobot",
            Description = "Displays information about the bot, such as version and creator.",
            DefaultMemberPermissions = GuildPermission.UseApplicationCommands,
        };

        public ulong GuildId { get; set; } = SecurityLaboratory.Instance.Config.GuildId;

        public async Task Run(SocketSlashCommand command)
        {
            // Embed
            var embed = new EmbedBuilder()
                .WithTitle("Bot Information")
                .WithDescription("Here are some details about this bot and plugin instance:")
                .AddField("Plugin:", SecurityLaboratory.Instance.Name, true)
                .AddField("Version:", SecurityLaboratory.Instance.Version, true)
                .AddField("Created By:", SecurityLaboratory.Instance.Author, true)
                .AddField("Guild ID:", SecurityLaboratory.Instance.Config.GuildId, true)
                .AddField("EXILED Required Version:", SecurityLaboratory.Instance.RequiredExiledVersion, true)
                .AddField("Plugin Debug:", SecurityLaboratory.Instance.Config.Debug, true)
                .WithColor(Color.Blue)
                .Build();
            
            // Buttons
            var components = new ComponentBuilder()
                .WithButton(ButtonBuilder.CreateLinkButton("Visit GitHub","https://github.com/RLLanonymous/SecurityLaboratory", new Emoji("\ud83d\udd17")))
                .WithButton(ButtonBuilder.CreateLinkButton("Join Discord","https://discord.gg/fqvMufQkmk", new Emoji("\ud83d\udcac")))
                .Build();
            
            await command.RespondAsync(embed: embed, components: components);
        }
    }
}