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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using KeMinimap;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace keMinimapMod
{
    internal sealed class MinimapMod
    {
        private readonly int initialHeight;
        private readonly int initialLeft;
        private readonly int initialTop;
        private readonly int initialWidth;

        private readonly Dictionary<Dimension, int> maxValues = new Dictionary<Dimension, int>
        {
            { Dimension.Horizontal, Drawing.Width },
            { Dimension.Vertical, Drawing.Height }
        };

        private readonly MenuWrapper menu = new MenuWrapper("keMinimapMod", false, false);
        private readonly Dictionary<string, MenuWrapper.SliderLink> values;
        private int height;
        private int left;
        private int top;
        private int width;

        internal MinimapMod()
        {
            values = new Dictionary<string, MenuWrapper.SliderLink>
            {
                { "left", Slider("Left", Minimap.Left, Dimension.Horizontal) },
                { "top", Slider("Top", Minimap.Top, Dimension.Vertical) },
                { "width", Slider("Width", Minimap.Width, Dimension.Horizontal) },
                { "height", Slider("Height", Minimap.Height, Dimension.Vertical) }
            };
            Debug.Assert(values != null, "values != null");
            // ReSharper disable once PossibleNullReferenceException
            Minimap.Left = initialLeft = left = values["left"].Value.Value;
            // ReSharper disable once PossibleNullReferenceException
            Minimap.Top = initialTop = top = values["top"].Value.Value;
            // ReSharper disable once PossibleNullReferenceException
            Minimap.Width = initialWidth = width = values["width"].Value.Value;
            // ReSharper disable once PossibleNullReferenceException
            Minimap.Height = initialHeight = height = values["height"].Value.Value;
        }

        ~MinimapMod()
        {
            Minimap.Left = initialLeft;
            Minimap.Top = initialTop;
            Minimap.Width = initialWidth;
            Minimap.Height = initialHeight;
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        internal void Update()
        {
            Debug.Assert(values != null, "values != null");
            if (left == values["left"].Value.Value && top == values["top"].Value.Value &&
                width == values["width"].Value.Value && height == values["height"].Value.Value)
            {
                return;
            }
            Debug.Assert(values != null, "values != null");
            // ReSharper disable once PossibleNullReferenceException
            Minimap.Left = left = values["left"].Value.Value;
            // ReSharper disable once PossibleNullReferenceException
            Minimap.Top = top = values["top"].Value.Value;
            // ReSharper disable once PossibleNullReferenceException
            Minimap.Width = width = values["width"].Value.Value;
            // ReSharper disable once PossibleNullReferenceException
            Minimap.Height = height = values["height"].Value.Value;
        }

        private MenuWrapper.SliderLink Slider(string property, float value, Dimension dimension)
        {
            Debug.Assert(menu != null, "menu != null");
            Debug.Assert(menu.MainMenu != null, "menu.MainMenu != null");
            Debug.Assert(maxValues != null, "maxValues != null");
            return menu.MainMenu.AddLinkedSlider(property, Convert.ToInt32(value), 0, maxValues[dimension]);
        }

        private enum Dimension
        {
            Horizontal,
            Vertical
        }
    }
}
