using System;
using LeagueSharp.Common;

namespace keLux
{
    internal class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arguments")]
        private static void Main(string[] arguments)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "keLux.Lux")]
        private static void OnGameLoad(EventArgs arguments)
        {
            new Lux();
        }
    }
}
