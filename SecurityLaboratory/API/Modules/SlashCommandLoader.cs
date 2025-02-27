﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Discord;
using SecurityLaboratory.API.Interfaces;
using SecurityLaboratory.Handlers;
using SecurityLaboratory.API.Interfaces;

namespace SecurityLaboratory.API.Modules
{
    public static class SlashCommandLoader
    {
        public static List<ISlashCommand> Commands = new();
        
        /// <summary>
        /// Adds all commands in a <see cref="Assembly"/> from the <see cref="ISlashCommand"/> classes to a list.
        /// </summary>
        /// <param name="assembly">
        /// Your plugin's <see cref="Assembly"/>.
        /// </param>
        public static void LoadCommands(Assembly assembly)
        {
            Type registerType = typeof(ISlashCommand);
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || !registerType.IsAssignableFrom(type))
                    continue;

                ISlashCommand init = Activator.CreateInstance(type) as ISlashCommand;
                Commands.Add(init);
            }
        }

        /// <summary>
        /// This clears all commands from the list.
        /// </summary>
        /// <remarks>
        /// This should only be used by the main bot, you should have no reason to run this.
        /// </remarks>
        public static void ClearCommands()
        {
            Commands = new();
        }
    }
}