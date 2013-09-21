using System;

namespace Lander_Craft_JibLibX
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LanderGame game = new LanderGame())
            {
                game.Run();
            }
        }
    }
#endif
}

