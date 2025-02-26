using Exiled.API.Features;
using System;
using Exiled.API.Enums;

namespace SecurityLaboratory
{
    public class SecurityLaboratory : Plugin<Config, Translations>
    {
        public override string Name { get; } = "Security Laboratory";
        public override string Author { get; } = "Sexy Lanonymous";
        public override string Prefix { get; } = "SecurityLaboratory";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override PluginPriority Priority { get; } = PluginPriority.Highest;
        public override Version RequiredExiledVersion { get; } = new Version(9,5,0);

        public static SecurityLaboratory Instance { get; private set; }
        
        public override void OnEnabled()
        {
            Instance = this;
            base.OnEnabled();
        }
        
        public override void OnDisabled()
        {
            Instance = null;
            base.OnDisabled();
        }
    }
}