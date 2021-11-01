using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace LiveSplit.KulaWorld
{
    class Watchers : MemoryWatcherList
    {
        // General variables
        private MemoryWatcher<uint> GameVersionCode { get; set; }
        private MemoryWatcher<int> LevelIGT_ntsc { get; set; }
        private MemoryWatcher<byte> LevelNo_ntsc { get; set; }
        private MemoryWatcher<byte> WorldNo_ntsc { get; set; }
        private MemoryWatcher<bool> DemoMode_ntsc { get; set; }
        private MemoryWatcher<int> LevelIGT_pal { get; set; }
        private MemoryWatcher<byte> LevelNo_pal { get; set; }
        private MemoryWatcher<byte> WorldNo_pal { get; set; }
        private MemoryWatcher<bool> DemoMode_pal { get; set; }
        private MemoryWatcher<int> LevelIGT_ntscj { get; set; }
        private MemoryWatcher<byte> LevelNo_ntscj { get; set; }
        private MemoryWatcher<byte> WorldNo_ntscj { get; set; }
        private MemoryWatcher<bool> DemoMode_ntscj { get; set; }

        // Fake Watchers
        private GameVersion GameVersion => (GameVersion)GameVersionCode.Current;
        public int RefreshRate => GameVersion == GameVersion.PAL ? 50 : 60;
        public FakeMemoryWatcher<int> LevelIGT => new FakeMemoryWatcher<int>(
            GameVersion == GameVersion.NTSC ? LevelIGT_ntsc.Old : GameVersion == GameVersion.PAL ? LevelIGT_pal.Old : LevelIGT_ntscj.Old,
            GameVersion == GameVersion.NTSC ? LevelIGT_ntsc.Current : GameVersion == GameVersion.PAL ? LevelIGT_pal.Current : LevelIGT_ntscj.Current);
        public FakeMemoryWatcher<byte> LevelNo => new FakeMemoryWatcher<byte>(
            GameVersion == GameVersion.NTSC ? LevelNo_ntsc.Old : GameVersion == GameVersion.PAL ? LevelNo_pal.Old : LevelNo_ntscj.Old,
            GameVersion == GameVersion.NTSC ? LevelNo_ntsc.Current : GameVersion == GameVersion.PAL ? LevelNo_pal.Current : LevelNo_ntscj.Current);
        public FakeMemoryWatcher<byte> WorldNo => new FakeMemoryWatcher<byte>(
            GameVersion == GameVersion.NTSC ? WorldNo_ntsc.Old : GameVersion == GameVersion.PAL ? WorldNo_pal.Old : WorldNo_ntscj.Old,
            GameVersion == GameVersion.NTSC ? WorldNo_ntsc.Current : GameVersion == GameVersion.PAL ? WorldNo_pal.Current : WorldNo_ntscj.Current);
        public FakeMemoryWatcher<bool> DemoMode => new FakeMemoryWatcher<bool>(
            GameVersion == GameVersion.NTSC ? DemoMode_ntsc.Old : GameVersion == GameVersion.PAL ? DemoMode_pal.Old : DemoMode_ntscj.Old,
            GameVersion == GameVersion.NTSC ? DemoMode_ntsc.Current : GameVersion == GameVersion.PAL ? DemoMode_pal.Current : DemoMode_ntscj.Current);

        // Autospltiter variables
        public int TotalIGT = 0;

        public Watchers(Process game)
        {
            IntPtr baseRAMAddress = IntPtr.Zero;
            switch (game.ProcessName.ToLower())
            {
                case "epsxe":
                    // Listing ePSXe supported versions with the offsets needed to identify the starting point of emulated RAM
                    var epsxeversions = new Dictionary<int, int>{
                        { 0x182A000, 0xA82020 },   // ePSXe v2.0.5
                        { 0x1553000, 0x825140 },   // ePSXe v2.0.2-1
                        { 0x1359000, 0x81A020 },   // ePSXe v2.0.0
                        { 0xA08000, 0x68B6A0 },    // ePSXe v1.9.25
                        { 0x9D3000, 0x6579A0 },    // ePSXe v1.9.0
                        { 0x9C2000, 0x652EA0 },    // ePSXe v1.8.0
                        { 0x8B7000, 0x54C020 },    // ePSXe v1.7.0
                        { 0x4E2000, 0x1B6E40 },    // ePSXe v1.6.0
                    };
                    if (epsxeversions.TryGetValue(game.MainModuleWow64Safe().ModuleMemorySize, out int wramOffsetepsxe))
                    {
                        baseRAMAddress = game.MainModuleWow64Safe().BaseAddress + wramOffsetepsxe;
                    } else {
                        throw new Exception();
                    }
                    break;
                case "retroarch":
                    ProcessModuleWow64Safe libretromodule = game.ModulesWow64Safe().Where(m => m.ModuleName == "mednafen_psx_hw_libretro.dll" || m.ModuleName == "mednafen_psx_libretro.dll" || m.ModuleName == "pcsx_rearmed_libretro.dll" || m.ModuleName == "duckstation_libretro.dll").First();
                    switch (libretromodule.ModuleName)
                    {
                        case "mednafen_psx_hw_libretro.dll":
                        case "mednafen_psx_libretro.dll":
                            baseRAMAddress = (IntPtr)0x40000000;
                            break;
                        case "pcsx_rearmed_libretro.dll":
                            baseRAMAddress = (IntPtr)0x30000000;
                            break;
                        case "duckstation_libretro.dll":
                            var DuckstationVersions = new Dictionary<int, int>{
                                { 0x4B0A000, 0x2D4030 },   // Duckstation 64bit
                                { 0x55B000, 0x22CF88 },    // Duckstation 32bit
                            };
                            if (DuckstationVersions.TryGetValue(libretromodule.ModuleMemorySize, out int wramOffsetduckstation))
                            {
                                baseRAMAddress = libretromodule.BaseAddress + wramOffsetduckstation;
                            } else
                            {
                                throw new Exception();
                            }
                            break;
                    }
                    break;
            }

            if (baseRAMAddress == IntPtr.Zero) throw new Exception();

            this.GameVersionCode = new MemoryWatcher<uint>(baseRAMAddress + 0xA350E);
            this.LevelIGT_ntsc = new MemoryWatcher<int>(baseRAMAddress + 0xA5584);
            this.LevelNo_ntsc = new MemoryWatcher<byte>(baseRAMAddress + 0xA3408);
            this.WorldNo_ntsc = new MemoryWatcher<byte>(baseRAMAddress + 0xA340C);
            this.DemoMode_ntsc = new MemoryWatcher<bool>(baseRAMAddress + 0xA342C);
            this.LevelIGT_pal = new MemoryWatcher<int>(baseRAMAddress + 0xA5110);
            this.LevelNo_pal = new MemoryWatcher<byte>(baseRAMAddress + 0xA2EA0);
            this.WorldNo_pal = new MemoryWatcher<byte>(baseRAMAddress + 0xA2EA4);
            this.DemoMode_pal = new MemoryWatcher<bool>(baseRAMAddress + 0xA566C);
            this.LevelIGT_ntscj = new MemoryWatcher<int>(baseRAMAddress + 0xA1ED0);
            this.LevelNo_ntscj = new MemoryWatcher<byte>(baseRAMAddress + 0x9F50C);
            this.WorldNo_ntscj = new MemoryWatcher<byte>(baseRAMAddress + 0x9F510);
            this.DemoMode_ntscj = new MemoryWatcher<bool>(baseRAMAddress + 0x9F534);

            this.AddRange(this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => !p.GetIndexParameters().Any()).Select(p => p.GetValue(this, null) as MemoryWatcher).Where(p => p != null));
        }

    }

    class FakeMemoryWatcher<T>
    {
        public T Current { get; set; }
        public T Old { get; set; }
        public bool Changed { get; }
        public FakeMemoryWatcher(T old, T current)
        {
            this.Old = old;
            this.Current = current;
            this.Changed = !old.Equals(current);
        }
    }

    enum GameVersion : uint
    {
        NTSC = 0x53554C53,
        PAL = 0x53454353,
        NTSCJ = 0x53504353,
    }
}
