using System;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.KulaWorld
{
    public partial class Settings : UserControl
    {
        public bool RunStart { get; set; }
        public bool WorldSplit { get; set; }
        
        public Settings()
        {
            InitializeComponent();

            // General settings
            chkrunStart.DataBindings.Add("Checked", this, "RunStart", false, DataSourceUpdateMode.OnPropertyChanged);
            chkWorld.DataBindings.Add("Checked", this, "WorldSplit", false, DataSourceUpdateMode.OnPropertyChanged);

            // Default Values
            RunStart = true;
            WorldSplit = false;
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("settings");
            settingsNode.AppendChild(ToElement(doc, "RunStart", RunStart));
            settingsNode.AppendChild(ToElement(doc, "WorldSplit", WorldSplit));
            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            RunStart = ParseBool(settings, "RunStart", true);
            WorldSplit = ParseBool(settings, "WorldSplit", true);
        }

        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            bool val;
            return settings[setting] != null ? (Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_) : default_;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }
    }
}
