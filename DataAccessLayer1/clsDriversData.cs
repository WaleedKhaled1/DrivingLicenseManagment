using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer1
{
    public class clsDriversData
    {
        public static DataTable GetAllDriversInfo()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from Drivers_View";

            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                connection.Close();
                reader.Close();
            }
            return dt;
        }
        public static int _AddNewDriver(int PersonID,int CreatedByUserID,DateTime CreatedDate)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "Insert into Drivers Values (@PersonID,@CreatedByUserID,@CreatedDate)" +
                "Select Scope_Identity()";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@PersonID", PersonID);
            cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);

            int _InsertedID = -1;
            try
            {
                connection.Open();

                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    _InsertedID = InsertedID;
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }

            return _InsertedID;
        }

        public static bool FindDriverByID(int DriverID,ref int personID,ref int CreatedByUserID,ref DateTime createdDate)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    personID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    createdDate = (DateTime)reader["CreatedDate"];
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return isFound;
        }

        public static bool FindDriverByPersonID(ref int DriverID, int personID, ref int CreatedByUserID, ref DateTime createdDate)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@PersonID", personID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    DriverID = (int)reader["DriverID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    createdDate = (DateTime)reader["CreatedDate"];
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return isFound;
        }



    }
}



