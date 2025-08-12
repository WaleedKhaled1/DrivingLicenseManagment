using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer1
{
    public class clsApplicationsData
    {
        public static bool FindAllApplicationInfoByID(int ApplicationID, ref int ApplicantPersonID, ref int ApplicationTypeID, ref int CreatedByUserID,
          ref DateTime ApplicationDate, ref DateTime ApplicationLastStatusDate, ref int ApplicationStatus, ref float PaidFees)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from Applications where ApplicationID=@ApplicationID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;

                    ApplicantPersonID = (int)reader["ApplicantPersonID"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationStatus = Convert.ToInt32(reader ["ApplicationStatus"]);
                    ApplicationLastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees =Convert.ToSingle( reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }
                else
                {
                    IsFound = false;
                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return IsFound;
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int AppType,int LicenseClass)
        {

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select A. ApplicationID from Applications A join LocalDrivingLicenseApplications L on A.ApplicationID=L.ApplicationID " +
                "where ApplicantPersonID=@ApplicantPersonID AND ApplicationTypeID=@ApplicationTypeID " +
                "AND ApplicationStatus=1 And LicenseClassID=@LicenseClassID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            cmd.Parameters.AddWithValue("@ApplicationTypeID", AppType);
            cmd.Parameters.AddWithValue("@LicenseClassID", LicenseClass);

            SqlDataReader reader = null;
            int AppID = -1;
            try
            {
                connection.Open();

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    AppID =(int) reader["ApplicationID"];
                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
                reader.Close();
            }

            return AppID;
        }

        public static int AddNewApplication(int ApplicantPersonID, int ApplicationTypeID, int CreatedByUserID,
             DateTime ApplicationDate, DateTime ApplicationLastStatusDate, int ApplicationStatus,  float PaidFees)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "Insert into Applications Values(@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,@ApplicationStatus,@LastStatusDate," +
                "@PaidFees,@CreatedByUser)" +
                "Select Scope_Identity()";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ApplicantPersonID",ApplicantPersonID);
            cmd.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            cmd.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            cmd.Parameters.AddWithValue("@LastStatusDate", ApplicationLastStatusDate);
            cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
            cmd.Parameters.AddWithValue("@CreatedByUser", CreatedByUserID);

            int _InsertedID = -1;
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if(result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    _InsertedID = InsertedID;
                }
            }

            catch(Exception ex) 
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }

            return _InsertedID;
        }

        public static bool UpdateApplicationStatus(int AppID, int status)
        {
         

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "Update Applications set ApplicationStatus=@Status,set LastStatusDate=@now where ApplicationID=@ApplicationID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@ApplicationID", AppID);
            cmd.Parameters.AddWithValue("@now", DateTime.Now);

            bool _IsFound = false;
            try

            {
                connection.Open();
                int result = cmd.ExecuteNonQuery();
                if (result != 0)
                {
                    _IsFound = true;
                }
                else
                    _IsFound = false;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally 
            { connection.Close(); }

            return _IsFound;


        }

        public static bool DeleteApplication(int AppID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "delete FROM Applications WHERE ApplicationID = @ApplicationID";
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@ApplicationID", AppID);


            int result = 0;
            try
            {
                connection.Open();
                result = cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            { connection.Close(); }

            return (result != 0);
        }


    }
}


    

