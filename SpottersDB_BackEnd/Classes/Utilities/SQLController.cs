using Microsoft.Data.SqlClient;

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
            if(con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            con.ConnectionString = "server = (localdb)\\MSSQLLocalDB; integrated security = false;";
            con.Open();
            cmd = new SqlCommand("CREATE DATABASE " +  DatabaseName, con);
            cmd.ExecuteNonQuery();
            con.Close();
            ConnectToDB(DatabaseName);
        }


    }
}
