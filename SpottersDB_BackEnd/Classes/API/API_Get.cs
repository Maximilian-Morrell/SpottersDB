using SpottersDB_BackEnd.Classes.Structure;
using SpottersDB_BackEnd.Classes.Utilities;
using System.Reflection;

namespace SpottersDB_BackEnd.Classes.API
{
    public class API_GET : API_Base
    {
        public API_GET(WebApplication app, SQLController sqlcontroller) : base(app, sqlcontroller)
        {

        }

        protected override void MainAPI()
        {
            // Get Countries Route
            app.MapGet("/Get/Countries", GET_Countries);
            // Get Country by ID Route
            app.MapGet("/Get/Country", (int ID) => GET_Country(ID));
        }

        private List<Country> GET_Countries()
        {
            List<Country> countries = sqlcontroller.GetCountries();
            foreach(Country country in countries)
            {
                foreach(PropertyInfo propertyInfo in typeof(Country).GetProperties())
                {
                    app.Logger.LogInformation(propertyInfo.Name + ": " + propertyInfo.GetValue(country, null));
                }
            }
            return countries;
        }

        private Country GET_Country(int ID)
        {
            Country country = sqlcontroller.GetCountryByID(ID);
            foreach (PropertyInfo propertyInfo in typeof(Country).GetProperties())
            {
                app.Logger.LogInformation(propertyInfo.Name + ": " + propertyInfo.GetValue(country, null));
            }
            return country;
        }
    }
}
