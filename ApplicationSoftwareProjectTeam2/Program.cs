namespace ApplicationSoftwareProjectTeam2
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
<<<<<<< HEAD
<<<<<<< Updated upstream
            Application.Run(new GamePanel());
=======
            Application.Run(new Form1());
>>>>>>> Stashed changes
=======
            Application.Run(new GameMenu());
>>>>>>> 5862acffa1a5bd0a8da6360d73e7a68e086c31f6
        }
    }
}