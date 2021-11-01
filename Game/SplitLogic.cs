using System;
using System.Diagnostics;
using System.Linq;
using LiveSplit.Model;

namespace LiveSplit.KulaWorld
{
    class SplitLogic
    {
        private Process game;
        private Watchers watchers;

        public event EventHandler OnStartTrigger;

        public delegate void GameTimeTriggerEventHandler(object sender, double value);
        public event GameTimeTriggerEventHandler OnGameTimeTrigger;

        public delegate void SplitTriggerEventHandler(object sender, SplitTrigger type);
        public event SplitTriggerEventHandler OnSplitTrigger;

        public void Update(TimerModel timer)
        {
            if (game == null || game.HasExited) { if (!HookGameProcess()) return; }
            if (timer.CurrentState.IsGameTimePaused == false) timer.CurrentState.IsGameTimePaused = true;
            watchers.UpdateAll(game);
            if (timer.CurrentState.CurrentPhase == TimerPhase.NotRunning) ResetInternalVars();
            UpdateParams();
            Start();
            GameTime();
            Split();
        }

        void ResetInternalVars()
        {
            watchers.TotalIGT = 0;
        }

        void UpdateParams()
        {
            // Update the IGT
            if (watchers.LevelIGT.Current == 0 && watchers.LevelIGT.Old != 0) watchers.TotalIGT += watchers.LevelIGT.Old;
        }

        void Start()
        {
            if (watchers.LevelIGT.Current != 0 && watchers.LevelIGT.Old == 0 && !watchers.DemoMode.Current)
            {
                this.OnStartTrigger?.Invoke(this, EventArgs.Empty);
            }
        }

        void GameTime()
        {
            this.OnGameTimeTrigger?.Invoke(this, (double)(watchers.TotalIGT + watchers.LevelIGT.Current) / watchers.RefreshRate);
        }

        void Split()
        {
            if (watchers.LevelNo.Changed && watchers.LevelNo.Current < 15 && !(watchers.LevelNo.Current == 0 && watchers.WorldNo.Current == 0))
            {
                this.OnSplitTrigger?.Invoke(this, SplitTrigger.Level);
            } else if (watchers.WorldNo.Current > watchers.WorldNo.Old)
            {
                this.OnSplitTrigger?.Invoke(this, SplitTrigger.World);
            }
        }

        public enum SplitTrigger
        {
            World,
            Level
        }

        bool HookGameProcess()
        {
            foreach (var process in new string[] { "retroarch", "ePSXe" })
            {
                game = Process.GetProcessesByName(process).OrderByDescending(x => x.StartTime).FirstOrDefault(x => !x.HasExited);
                if (game == null) continue;
                try { watchers = new Watchers(game); } catch { game = null; return false; }
                return true;
            }
            return false;
        }

    }
}
