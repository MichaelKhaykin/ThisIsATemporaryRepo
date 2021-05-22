using System;

namespace SingalRServer
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var gameServer = new GameServer();
            gameServer.Run();
        }
    }
#endif
}
