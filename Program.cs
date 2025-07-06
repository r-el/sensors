using sensors.src.Services;

namespace sensors
{
    /// <summary>
    /// Application entry point - contains only startup logic
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize and start the application
            var applicationManager = new ApplicationManager();
            applicationManager.Run();
        }
    }
}
