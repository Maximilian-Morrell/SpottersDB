namespace SpottersDB_BackEnd.Classes.API
{
    public class API_Get : API_Base
    {
        public API_Get(WebApplication app) : base(app)
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
