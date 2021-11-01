using System;
using System.Xml;
using System.Windows.Forms;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.KulaWorld
{
    class Component : LogicComponent
    {
        public override string ComponentName => "Kula World";
        private Settings settings { get; set; }
        private TimerModel timer;
        private Timer update_timer;
        private SplitLogic SplitLogic;

        public Component(LiveSplitState state)
        {
            timer = new TimerModel { CurrentState = state };
            update_timer = new Timer() { Interval = 15, Enabled = true };
            settings = new Settings();
            update_timer.Tick += UpdateTimer_Tick;

            SplitLogic = new SplitLogic();
            SplitLogic.OnStartTrigger += OnStartTrigger;
            SplitLogic.OnGameTimeTrigger += OnGameTimeTrigger;
            SplitLogic.OnSplitTrigger += OnSplitTrigger;
        }

        public override void Dispose()
        {
            settings.Dispose();
            update_timer?.Dispose();
        }

        void UpdateTimer_Tick(object sender, EventArgs eventArgs)
        {
            try { SplitLogic.Update(timer); } catch { return; }
        }

        void OnStartTrigger(object sender, EventArgs e)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.NotRunning) return;
            if (!settings.RunStart) return;
            timer.Start();
        }

        void OnGameTimeTrigger(object sender, double value)
        {
            timer.CurrentState.SetGameTime(TimeSpan.FromSeconds(value));
        }

        void OnSplitTrigger(object sender, SplitLogic.SplitTrigger type)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.Running) return;
            switch (type)
            {
                case SplitLogic.SplitTrigger.Level:
                    if (!settings.WorldSplit) timer.Split();
                    break;
                case SplitLogic.SplitTrigger.World:
                    if (settings.WorldSplit) timer.Split();
                    break;
            }
        }
        public override XmlNode GetSettings(XmlDocument document) { return this.settings.GetSettings(document); }

        public override Control GetSettingsControl(LayoutMode mode) { return this.settings; }

        public override void SetSettings(XmlNode settings) { this.settings.SetSettings(settings); }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
    }
}
