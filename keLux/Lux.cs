#region LICENSE

// This file is part of keLux.
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
// along with LeagueSharp.Common.  If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using LeagueSharp;

#endregion

namespace keLux
{
    internal sealed class Lux
    {
        public Lux()
        {
            if (ObjectManager.Player == null ||
                ObjectManager.Player != null && !"Lux".Equals(ObjectManager.Player.ChampionName))
            {
                return;
            }
        }
    }
}
