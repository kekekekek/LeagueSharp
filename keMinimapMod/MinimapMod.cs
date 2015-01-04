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
using KeMinimap;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace keMinimapMod
{
    internal sealed class MinimapMod
    {
        private readonly MenuWrapper.BoolLink fogOfWarValue;
        private readonly bool initialFogOfWar;
        private readonly int initialHeight;
        private readonly int initialLeft;
        private readonly int initialTop;
        private readonly float initialTransparency;
        private readonly int initialWidth;

        private readonly Dictionary<Dimension, int> maxValues = new Dictionary<Dimension, int>
        {
            { Dimension.Horizontal, Drawing.Width },
            { Dimension.Vertical, Drawing.Height }
        };

        private readonly MenuWrapper menu = new MenuWrapper("keMinimapMod", false, false);
        private readonly Dictionary<string, MenuWrapper.SliderLink> rangingValues;
        private bool fogOfWar;
        private int height;
        private int left;
        private int top;
        private float transparency;
        private int width;

        internal MinimapMod()
        {
            Debug.Assert(menu != null, "MinimapMod(): menu = null");
            Debug.Assert(menu.MainMenu != null, "MinimapMod(): menu.MainMenu == null");
            rangingValues = new Dictionary<string, MenuWrapper.SliderLink>
            {
                { "transparency", menu.MainMenu.AddLinkedSlider("Transparency", 100) },
                { "left", Slider("Left", Minimap.Left, Dimension.Horizontal) },
                { "top", Slider("Top", Minimap.Top, Dimension.Vertical) },
                { "width", Slider("Width", Minimap.Width, Dimension.Horizontal) },
                { "height", Slider("Height", Minimap.Height, Dimension.Vertical) }
            };
            fogOfWarValue = menu.MainMenu.AddLinkedBool("Fog of War");
            Minimap.Transparency = initialTransparency = transparency = Transparency;
            Minimap.FogOfWar = initialFogOfWar = fogOfWar = FogOfWar;
            Minimap.Left = initialLeft = left = Left;
            Minimap.Top = initialTop = top = Top;
            Minimap.Width = initialWidth = width = Width;
            Minimap.Height = initialHeight = height = Height;
        }

        private float Transparency
        {
            get
            {
                Debug.Assert(rangingValues != null, "Transparency: rangingValues = null");
                Debug.Assert(
                    rangingValues["transparency"] != null, "Transparency: rangingValues[\"transparency\"] = null");
                return rangingValues["transparency"].Value.Value / 100f;
            }
        }

        private bool FogOfWar
        {
            get
            {
                Debug.Assert(fogOfWarValue != null, "FogOfWar: fogOfWar = null");
                return fogOfWarValue.Value;
            }
        }

        private int Left
        {
            get
            {
                Debug.Assert(rangingValues != null, "Left: rangingValues = null");
                Debug.Assert(rangingValues["left"] != null, "Left: rangingValues[\"left\"] = null");
                return rangingValues["left"].Value.Value;
            }
        }

        private int Top
        {
            get
            {
                Debug.Assert(rangingValues != null, "Top: rangingValues = null");
                Debug.Assert(rangingValues["top"] != null, "Top: rangingValues[\"top\"] = null");
                return rangingValues["top"].Value.Value;
            }
        }

        private int Width
        {
            get
            {
                Debug.Assert(rangingValues != null, "Width: rangingValues = null");
                Debug.Assert(rangingValues["width"] != null, "Width: rangingValues[\"width\"] = null");
                return rangingValues["width"].Value.Value;
            }
        }

        private int Height
        {
            get
            {
                Debug.Assert(rangingValues != null, "Height: rangingValues = null");
                Debug.Assert(rangingValues["height"] != null, "Height: rangingValues[\"height\"] = null");
                return rangingValues["height"].Value.Value;
            }
        }

        ~MinimapMod()
        {
            Minimap.Transparency = initialTransparency;
            Minimap.FogOfWar = initialFogOfWar;
            Minimap.Left = initialLeft;
            Minimap.Top = initialTop;
            Minimap.Width = initialWidth;
            Minimap.Height = initialHeight;
        }

        internal void Update()
        {
            if (Math.Abs(transparency - Transparency) >= 0.01)
            {
                Minimap.Transparency = transparency = Transparency;
            }
            if (fogOfWar != FogOfWar)
            {
                Minimap.FogOfWar = fogOfWar = FogOfWar;
            }
            if (left == Left && top == Top && width == Width && height == Height)
            {
                return;
            }
            Minimap.Left = left = Left;
            Minimap.Top = top = Top;
            Minimap.Width = width = Width;
            Minimap.Height = height = Height;
        }

        private MenuWrapper.SliderLink Slider(string property, float value, Dimension dimension)
        {
            Debug.Assert(menu != null, "Slider(string, float, Dimension): menu = null");
            Debug.Assert(menu.MainMenu != null, "Slider(string, float, Dimension): menu.MainMenu = null");
            Debug.Assert(maxValues != null, "Slider(string, float, Dimension): maxValues = null");
            return menu.MainMenu.AddLinkedSlider(property, Convert.ToInt32(value), 0, maxValues[dimension]);
        }

        private enum Dimension
        {
            Horizontal,
            Vertical
        }
    }
}
