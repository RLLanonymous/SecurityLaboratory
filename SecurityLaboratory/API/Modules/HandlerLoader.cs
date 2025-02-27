using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using SecurityLaboratory.API.Interfaces;
using Exiled.API.Features;
using PluginAPI.Commands;

namespace SecurityLaboratory.API.Modules
{
    public class HandlerLoader
    {
        private readonly List<IRegisterable> _inits = new();

        /// <summary>
        /// Once you run this, it will grab all the <see cref="IRegisterable"/> classes from your plugin's <see cref="Assembly"/> and run their <see cref="IRegisterable.Init"/> method.
        /// It also grabs your <see cref="ISlashCommand"/> classes and registers them. You will need to do no command handling on your side. DiscordLab does it all.
        /// </summary>
        /// <param name="assembly">
        /// Your plugin's <see cref="Assembly"/>. Defaults to <see cref="Assembly.GetCallingAssembly"/>.
        /// </param>
        /// <remarks>
        /// If you use this function, you are required to call <see cref="Unload"/> when your plugin is about to be disabled. No need to pass in any params though.
        /// </remarks>
        public bool Load(Assembly assembly)
        {
            assembly ??= Assembly.GetCallingAssembly();
            if (SecurityLaboratory.Instance.Config.Token is "token" or "")
            {
                Log.Error($"Could not load {assembly.GetName().Name} because the bot token is not set in the config file.");
                return false;
            }
            Type registerType = typeof(IRegisterable);
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || !registerType.IsAssignableFrom(type))
                    continue;

                IRegisterable init = Activator.CreateInstance(type) as IRegisterable;
                if(SecurityLaboratory.Instance.Config.Debug) Log.Debug($"Loading {type.Name}...");
                _inits.Add(init);
                init!.Init();
            }

            SlashCommandLoader.LoadCommands(assembly);
            return true;
        }
        
        /// <summary>
        /// Unloads all IRegisterable classes that were loaded.
        /// </summary>
        public void Unload()
        {
            foreach (IRegisterable init in _inits)
                init.Unregister();
            SlashCommandLoader.ClearCommands();
        }
    }
}