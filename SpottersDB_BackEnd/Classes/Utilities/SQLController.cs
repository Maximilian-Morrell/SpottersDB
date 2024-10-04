using Microsoft.Data.SqlClient;
using SpottersDB_BackEnd.Classes.Structure;

namespace SpottersDB_BackEnd.Classes.Utilities
{
    public class SQLController
    {
        // Instances for reuse
        private SqlConnection con = new SqlConnection("server = (localdb)\\MSSQLLocalDB; integrated security = false;");
        private SqlCommand cmd = null;

        // Checks if DB Exists
        public void ConnectToDB(string DatabaseName)
        {
            cmd = new SqlCommand("", con);
            try
            {
                con.Open();
                con.ChangeDatabase(DatabaseName);
                con.Close();
                con.ConnectionString = "server = (localdb)\\MSSQLLocalDB; integrated security = false; database = " + DatabaseName;
            }
            catch (Exception e)
            {
                // DB does not Exists
                CreateDatabase(DatabaseName);
            }
            con.Close();
        }

        // Creates the DB
        private void CreateDatabase(string DatabaseName)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                con.ConnectionString = "server = (localdb)\\MSSQLLocalDB; integrated security = false;";
                con.Open();
                cmd.CommandText = "CREATE DATABASE " + DatabaseName;
                cmd.ExecuteNonQuery();
                con.ChangeDatabase(DatabaseName);
                cmd.CommandText = "CREATE TABLE Countries (CountryID INT NOT NULL IDENTITY, ICAOCode char(10), CountryName char(255), PRIMARY KEY(CountryID))";
                cmd.ExecuteNonQuery();
                con.Close();
                ConnectToDB(DatabaseName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AddCountry(Country country)
        {
            try
            {
                con.Open();
                cmd.CommandText = @$"INSERT INTO Countries (ICAOCode, CountryName) VALUES ('{country.ICAO_Code}', '{country.Name}')";
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            con.Close();
        }
    }
}
