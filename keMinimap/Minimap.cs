#region LICENSE

// This file is part of keMinimap.
// Copyright (C) 2014  kekek
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using LeagueSharp.Common;
using SharpDX;

#endregion

// ReSharper disable ConvertMethodToExpressionBody

[assembly: CLSCompliant(true)]

namespace KeMinimap
{
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Minimap")]
    public static class Minimap
    {
        public static float Transparency
        {
            get { return Read(Offsets.Transparency); }
            set { Write(Offsets.Transparency, value); }
        }

        public static bool FogOfWar
        {
            get
            {
                Debug.Assert(OriginalFogOfWarCall != null, "FogOfWar: OriginalFogOfWarCall = null");
                return Marshal.ReadByte(FogOfWarCall) == OriginalFogOfWarCall[0];
            }
            set { Patch(value ? OriginalFogOfWarCall : NopNearCall); }
        }

        public static float Left
        {
            get { return Read(Offsets.Left); }
            set { Write(Offsets.Left, value); }
        }

        public static float Top
        {
            get { return Read(Offsets.Top); }
            set { Write(Offsets.Top, value); }
        }

        public static float Width
        {
            get { return Read(Offsets.Width); }
            set { Write(Offsets.Width, value); }
        }

        public static float Height
        {
            get { return Read(Offsets.Height); }
            set { Write(Offsets.Height, value); }
        }

        public static bool MouseOver
        {
            get { return Mouse.X > Left && Mouse.X < Left + Width && Mouse.Y > Top && Mouse.Y < Top + Height; }
        }

        private static IntPtr ModuleBase
        {
            get { return Process.GetCurrentProcess().MainModule.BaseAddress; }
        }

        private static IntPtr FogOfWarCall
        {
            get { return ModuleBase + 0xB32E5F; }
        }

        private static Vector2 Mouse
        {
            get { return Utils.GetCursorPos(); }
        }

        private static IntPtr InitializeBaseAddress()
        {
            return Marshal.ReadIntPtr(Marshal.ReadIntPtr(ModuleBase + 0x14C901C), 0xD4);
        }

        private static byte[] InitializeOriginalFogOfWarCall()
        {
            var originalFogOfWarCall = new byte[5];
            Marshal.Copy(FogOfWarCall, originalFogOfWarCall, 0, originalFogOfWarCall.Length);
            return originalFogOfWarCall;
        }

        private static byte[] InitializeNopNearCall()
        {
            Debug.Assert(OriginalFogOfWarCall != null, "InitializeNopNearCall(): OriginalFogOfWarCall = null");
            return Enumerable.Repeat((byte) 0x90, OriginalFogOfWarCall.Length).ToArray();
        }

        private static float Read(Offsets property)
        {
            var value = Marshal.PtrToStructure(BaseAddress + (int) property, typeof(float));
            Debug.Assert(value != null, "Read(Offset): Value at offset " + (int) property + " = null");
            return (float) value;
        }

        private static void Write(Offsets property, float value)
        {
            Marshal.StructureToPtr(value, BaseAddress + (int) property, false);
        }

        private static uint Access(uint protection, out uint oldProtection)
        {
            Debug.Assert(OriginalFogOfWarCall != null, "Access(uint, out uint): OriginalFogOfWarCall = null");
            NativeMethods.VirtualProtect(FogOfWarCall, new IntPtr(OriginalFogOfWarCall.Length), protection, out oldProtection);
            return oldProtection;
        }

        private static void Patch(byte[] values)
        {
            uint protection;
            protection = Access((uint)NativeMethods.Protection.PageExecuteReadWrite, out protection);
            Debug.Assert(values != null, "Patch(byte[]): values = null");
            Marshal.Copy(values, 0, FogOfWarCall, values.Length);
            Access(protection, out protection);
        }

        private enum Offsets
        {
            Transparency = 0x04,
            Left = 0xAC,
            Top = 0xB0,
            Width = 0xB4,
            Height = 0xB8
        }

        private static readonly IntPtr BaseAddress = InitializeBaseAddress();
        private static readonly byte[] OriginalFogOfWarCall = InitializeOriginalFogOfWarCall();
        private static readonly byte[] NopNearCall = InitializeNopNearCall();
    }
}
