using SpottersDB_BackEnd.Classes.Structure;
using SpottersDB_BackEnd.Classes.Utilities;

namespace SpottersDB_BackEnd.Classes.API
{
    public class API_PATCH : API_Base
    {
        public API_PATCH(WebApplication app, SQLController sQLController) : base(app, sQLController)
        {

        }

        protected override void MainAPI()
        {
            // Post Country Route
            app.MapPost("/Patch/Country", (HttpRequest req) => PATCH_Country(req));
        }

        private async void PATCH_Country(HttpRequest req)
        {
            try
            {
                IFormCollection form = await req.ReadFormAsync();
                Country country = new Country(Convert.ToInt32(form["ID"]), form["ICAO"], form["Name"]);
                sqlcontroller.UpdateCountry(country);
            }
            catch (Exception e)
            {

            }
        }
    }
}
