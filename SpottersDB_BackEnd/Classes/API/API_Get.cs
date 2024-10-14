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

            // Get Airports Route
            app.MapGet("/Get/Airports", GET_Airports);
            // Get Airport by ID Route
            app.MapGet("/Get/Airport", (int ID) => GET_Airport(ID));

            // Get Airlines Route
            app.MapGet("/Get/Airlines", GET_Airlines);
            // Get Airline By ID Route
            app.MapGet("/Get/Airline", (int ID) => GET_Airline(ID));
        }

        private List<Country> GET_Countries()
        {
            return sqlcontroller.GetCountries();
        }

        private Country GET_Country(int ID)
        {
            return sqlcontroller.GetCountryByID(ID);
        }

        private List<Airport> GET_Airports()
        {
            return sqlcontroller.GetAirports();
        }

        private Airport GET_Airport(int ID)
        {
            return sqlcontroller.GetAirportByID(ID);
        }

        private List<Airline> GET_Airlines()
        {
            return sqlcontroller.GetAirlines();
        }

        private Airline GET_Airline(int ID)
        {
            return sqlcontroller.GetAirlineByID(ID);
        }
    }
}
