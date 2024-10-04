using SpottersDB_BackEnd.Classes.Utilities;

namespace SpottersDB_BackEnd.Classes.API
{
    public class API_GET : API_Base
    {
        public API_GET(WebApplication app, SQLController sqlcontroller) : base(app, sqlcontroller)
        {

        }

        protected override void MainAPI()
        {
            app.MapGet("/get/test", GetTest);
        }

        private string GetTest()
        {
            return "Hello, MORLOL.cc";
        }
    }
}
