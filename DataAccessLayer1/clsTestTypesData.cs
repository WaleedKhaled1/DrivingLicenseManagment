using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer1
{
    public class clsTestTypesData
    {
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from TestTypes";


            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                if (reader != null && reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                connection.Close();
            }

            return dt;
        }
        public static bool FindTestTypeInfo(int TestTypeID,ref string TestTypeTitle,ref string TestTypeDescription,ref float TestTypeFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestTypeTitle = reader["TestTypeTitle"].ToString();
                    TestTypeFees = ((float)Convert.ToDouble(reader["TestTypeFees"].ToString()));
                    TestTypeDescription = reader["TestTypeDescription"].ToString();
                }

                else
                {
                    isFound = false;
                }

                connection.Close();
            }

            catch (Exception ex)
            {
                isFound = false;
            }

            finally { connection.Close(); }

            return isFound;
        }
        public static bool UpdateTestType(int TestTypeID,  string TestTypeTitle,  string TestTypeDescription,  float TestTypeFees)
        {
            int rowsaffected = 0;
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "update TestTypes set TestTypeTitle=@TestTypeTitle,TestTypeDescription=@TestTypeDescription ,TestTypeFees=@TestTypeFees " +
                "where TestTypeID=@TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);



            try
            {
                connection.Open();
                rowsaffected = command.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                //IsUpdated = false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsaffected > 0);
        }
    }
}
