using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer1
{
    public class clsApplicationTypesData
    {
        public static DataTable GetAllApplicationsTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from ApplicationTypes";
                           

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

        public static bool FindApplicationTypeInfo(int AppTypeID,ref string AppTitle,ref float AppTypeFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ID", AppTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    AppTitle = reader["ApplicationTypeTitle"].ToString();
                    AppTypeFees =((float)Convert.ToDouble( reader["ApplicationFees"].ToString()));
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

        public static bool UpdateApplicationType(int AppTypeID,string AppTitle,float AppTypeFees)
        {
            int rowsaffected = 0;
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "update ApplicationTypes set ApplicationTypeTitle=@ApplicationTypeTitle,ApplicationFees=@ApplicationFees " +
                "where ApplicationTypeID=@ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeTitle", AppTitle);
            command.Parameters.AddWithValue("@ApplicationFees", AppTypeFees);
            command.Parameters.AddWithValue("@ApplicationTypeID", AppTypeID);

         

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

            return (rowsaffected>0);
        }
    }
        


}
