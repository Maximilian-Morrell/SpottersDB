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
            sqlcontroller.ConnectToDB("Maximilian_Morrell_3IT_2024_25");


            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}