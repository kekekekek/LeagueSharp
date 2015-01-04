#region LICENSE

// This file is part of keMinimapMod.
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
using LeagueSharp.Common;

#endregion

// ReSharper disable ConvertToExpressionBodyWhenPossible

namespace keMinimapMod
{
    internal static class Game
    {
        private static MinimapMod _mod;
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad(EventArgs arguments)
        {
            _mod = new MinimapMod();
            LeagueSharp.Game.OnGameUpdate += OnGameUpdate;
        }

        private static void OnGameUpdate(EventArgs arguments)
        {
            Debug.Assert(_mod != null, "OnGameUpdate(arguments): _mod = null");
            _mod.Update();
        }
    }
}
