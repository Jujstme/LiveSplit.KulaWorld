using System.Reflection;
using LiveSplit.KulaWorld;
using LiveSplit.UI.Components;
using System;
using LiveSplit.Model;

[assembly: ComponentFactory(typeof(KulaWorldFactory))]

namespace LiveSplit.KulaWorld
{
    public class KulaWorldFactory : IComponentFactory
    {
        public string ComponentName => "Team Sonic Racing Autosplitter";
        public string Description => "Automatic splitting and IGT calculation";
        public ComponentCategory Category => ComponentCategory.Control;
        public string UpdateName => this.ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/Jujstme/LiveSplit.TeamSonicRacing/master/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => this.UpdateURL + "Components/update.LiveSplit.TeamSonicRacing.xml";
        public IComponent Create(LiveSplitState state) { return new Component(state); }
    }
}
