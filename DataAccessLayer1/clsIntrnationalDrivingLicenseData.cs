using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer1
{
    public class clsIntrnationalDrivingLicenseData
    {

        public static DataTable GetAllInternationalLicensesForThisPerson(int PersonID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select InternationalLicenseID,ApplicationID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive from InternationalLicenses " +
                "join Drivers on InternationalLicenses.DriverID=Drivers.DriverID where PersonID=@PersonID";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static DataTable GetAllInternationalLicensesApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select InternationalLicenseID,ApplicationID,DriverID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive from InternationalLicenses";


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

        public static bool UpdateStatusInternationalLicenseToInActive(int InternationalLicenseID)
        {
            int rowsaffected = 0;
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "Update InternationalLicenses set IsActive=0 where InternationalLicenseID=@InternationalLicenseID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            try
            {
                connection.Open();
                rowsaffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowsaffected > 0);
        }
        public static int AddNewIntrnationalDrivingLicense(int AppID, int driverID, int issuedUsingLocalLicenseID, DateTime issueDate, DateTime expirationDate,
            bool isActive, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @"
                INSERT INTO InternationalLicenses 
                ( ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID)
                VALUES
                ( @ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID)
                Select Scope_Identity()";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ApplicationID", AppID);
            cmd.Parameters.AddWithValue("@DriverID", driverID);
            cmd.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", issuedUsingLocalLicenseID);
            cmd.Parameters.AddWithValue("@IssueDate", issueDate);
            cmd.Parameters.AddWithValue("@ExpirationDate", expirationDate);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            bool isFound = false;
            int InternationalLicenseID = -1;
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT Top 1 InternationalLicenseID FROM InternationalLicenses where DriverID=@DriverID AND GetDate() between IssueDate and ExpirationDate ";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return InternationalLicenseID;
        }

        public static bool FindInternationalLicenseInfoByDriverID
            (
            ref int internationalLicenseID,
            ref int AppID,
             int driverID,
            ref int issuedUsingLocalLicenseID,
            ref DateTime issueDate,
            ref DateTime expirationDate,
            ref bool isActive
            )

        {
            bool isFound = false;

            string query = @"SELECT InternationalLicenseID,ApplicationID,IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive 
                     FROM InternationalLicenses 
                     WHERE DriverID = @DriverID";

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@DriverID", driverID);

            SqlDataReader reader = null;
                try
                {
                connection.Open();
                 reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;

                        internationalLicenseID = (int)reader["InternationalLicenseID"];
                        AppID = (int)reader["ApplicationID"];
                    issuedUsingLocalLicenseID= (int)reader["IssuedUsingLocalLicenseID"];
                    issueDate = (DateTime)reader["IssueDate"];
                        expirationDate = (DateTime)reader["ExpirationDate"];
                        isActive = (bool)reader["IsActive"];
                    }

                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while retrieving international license info", ex);
            }

            finally
            {
                reader.Close();
                connection.Close();
            }

            return isFound;
        }


        public static bool FindInternationalLicenseInfoByInternationalLicenseID
           (
            int internationalLicenseID,
           ref int AppID,
           ref int driverID,
           ref int issuedUsingLocalLicenseID,
           ref DateTime issueDate,
           ref DateTime expirationDate,
           ref bool isActive
           )

        {
            bool isFound = false;

            string query = @"SELECT ApplicationID,DriverID,IssuedUsingLocalLicenseID,IssueDate, ExpirationDate, IsActive 
                     FROM InternationalLicenses 
                     WHERE  InternationalLicenseID = @InternationalLicenseID";

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseID);

            SqlDataReader reader = null;
            try
            {
                connection.Open();
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    AppID = (int)reader["ApplicationID"];
                    driverID = (int)reader["DriverID"];
                    issuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    isActive = (bool)reader["IsActive"];
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving international license info", ex);
            }

            finally
            {
                reader.Close();
                connection.Close();
            }

            return isFound;
        }


    }
    }

