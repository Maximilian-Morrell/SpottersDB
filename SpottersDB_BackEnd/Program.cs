using SpottersDB_BackEnd.Classes.API;
using SpottersDB_BackEnd.Classes.Utilities;

namespace SpottersDB_BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            // Creating an object from the SQLController Class & tests the connection (Checks if DB exists)
            SQLController sqlcontroller = new SQLController();
            sqlcontroller.ConnectToDB("Maximilian_Morrell_3IT_2024_25", app);

            // Running all of the API Handler
            API_GET getAPI = new API_GET(app, sqlcontroller);
            API_POST postAPI = new API_POST(app, sqlcontroller);


            app.MapGet("/", () => "Hello World!");
            // Fallback if route is not found
            app.MapFallback(() => Results.NotFound(StatusCodes.Status404NotFound + " - API Route Not Found"));

            app.Run();
        }
    }
}