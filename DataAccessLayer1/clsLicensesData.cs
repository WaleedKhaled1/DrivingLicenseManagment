using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer1
{
    public class clsLicensesData
    {
        static string SourceNameInEventViewer = "clsLicensesData";
        public static DataTable GetAllLocalLicensesForThisPerson(int PersonID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select LicenseID,ApplicationID,ClassName,IssueDate,ExpirationDate,IsActive from Licenses join LicenseClasses " +
                "on Licenses.LicenseClass=LicenseClasses.LicenseClassID join Drivers on Licenses.DriverID=Drivers.DriverID where PersonID=@PersonID";
              

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
                if (!EventLog.SourceExists(SourceNameInEventViewer))
                {
                    EventLog.CreateEventSource(SourceNameInEventViewer, "Application");
                }

                EventLog.WriteEntry(SourceNameInEventViewer, ex.Message, EventLogEntryType.Error);
            }

            finally
            {
                connection.Close();
                reader.Close();
            }
            return dt;
        }
        public static int AddNewLicenseData(int applicationID, int driverID, int licenseClass, DateTime issueDate, DateTime expirationDate, string notes,
        float paidFees, bool isActive, int issueReason, int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @"
            INSERT INTO Licenses
            (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
            VALUES
            (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID)
          Select Scope_Identity()
        ";


            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
            cmd.Parameters.AddWithValue("@DriverID", driverID);
            cmd.Parameters.AddWithValue("@LicenseClass", licenseClass);
            cmd.Parameters.AddWithValue("@IssueDate", issueDate);
            cmd.Parameters.AddWithValue("@ExpirationDate", expirationDate);
            if(notes!="")
            cmd.Parameters.AddWithValue("@Notes", notes);
            else
                cmd.Parameters.AddWithValue("@Notes", System.DBNull.Value);

            cmd.Parameters.AddWithValue("@PaidFees", paidFees);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            cmd.Parameters.AddWithValue("@IssueReason", issueReason);
            cmd.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

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
                if (!EventLog.SourceExists(SourceNameInEventViewer))
                {
                    EventLog.CreateEventSource(SourceNameInEventViewer, "Application");
                }

                EventLog.WriteEntry(SourceNameInEventViewer, ex.Message, EventLogEntryType.Error);
            }

            finally
            {
                connection.Close();
            }

            return _InsertedID;
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @"SELECT        Licenses.LicenseID
                            FROM Licenses INNER JOIN
                                                  Drivers ON Licenses.DriverID = Drivers.DriverID
                            WHERE  
                             
                             Licenses.LicenseClass = @LicenseClass 
                              AND Drivers.PersonID = @PersonID
                              And IsActive=1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
            }

            catch (Exception ex)
            {
                if (!EventLog.SourceExists(SourceNameInEventViewer))
                {
                    EventLog.CreateEventSource(SourceNameInEventViewer, "Application");
                }

                EventLog.WriteEntry(SourceNameInEventViewer, ex.Message, EventLogEntryType.Error);

            }

            finally
            {
                connection.Close();
            }


            return LicenseID;
        }

        public static bool FindLicenseInfoByAppID(ref int licenseID,int applicationID,ref int driverID,ref int licenseClass,ref DateTime issueDate,ref DateTime expirationDate,ref string notes,
        ref float paidFees,ref bool isActive,ref int issueReason)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM Licenses WHERE ApplicationID = @ApplicationID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
            SqlDataReader reader=null;

            try
            {
                connection.Open();
               reader= cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    licenseID = Convert.ToInt32(reader["LicenseID"]);
                    applicationID = Convert.ToInt32(reader["ApplicationID"]);
                    driverID = Convert.ToInt32(reader["DriverID"]);
                    licenseClass = Convert.ToInt32( reader["LicenseClass"]);
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : "";
                    paidFees = Convert.ToSingle(reader["PaidFees"]);
                    isActive = Convert.ToBoolean(reader["IsActive"]);
                    issueReason =Convert.ToInt32(reader["IssueReason"]);
                }

              
            }
            catch (Exception ex)
            {
                if (!EventLog.SourceExists(SourceNameInEventViewer))
                {
                    EventLog.CreateEventSource(SourceNameInEventViewer, "Application");
                }

                EventLog.WriteEntry(SourceNameInEventViewer, ex.Message, EventLogEntryType.Error);
            }

            finally { connection.Close();
                reader.Close();
            }

            return isFound;
        }

        public static bool FindLicenseInfoByLicenseID(int licenseID,ref int applicationID, ref int driverID, ref int licenseClass, ref DateTime issueDate, ref DateTime expirationDate, ref string notes,
       ref float paidFees, ref bool isActive, ref int issueReason)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@LicenseID", licenseID);
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    licenseID = Convert.ToInt32(reader["LicenseID"]);
                    applicationID = Convert.ToInt32(reader["ApplicationID"]);
                    driverID = Convert.ToInt32(reader["DriverID"]);
                    licenseClass = Convert.ToInt32(reader["LicenseClass"]);
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : "";
                    paidFees = Convert.ToSingle(reader["PaidFees"]);
                    isActive = Convert.ToBoolean(reader["IsActive"]);
                    issueReason = Convert.ToInt32(reader["IssueReason"]);
                }


            }
            catch (Exception ex)
            {
                if (!EventLog.SourceExists(SourceNameInEventViewer))
                {
                    EventLog.CreateEventSource(SourceNameInEventViewer, "Application");
                }

                EventLog.WriteEntry(SourceNameInEventViewer, ex.Message, EventLogEntryType.Error);
            }

            finally
            {
                connection.Close();
                reader.Close();
            }

            return isFound;
        }

        public static bool UpdateStatusLocalLicenseToActiveOrInActive(int LicenseID,int Status)
        {
            int rowsaffected = 0;
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "Update Licenses set IsActive=@Status where LicenseID=@LicenseID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@LicenseID", LicenseID);
            cmd.Parameters.AddWithValue("@Status", Status);
            try
            {
                connection.Open();
                rowsaffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (!EventLog.SourceExists(SourceNameInEventViewer))
                {
                    EventLog.CreateEventSource(SourceNameInEventViewer, "Application");
                }

                EventLog.WriteEntry(SourceNameInEventViewer, ex.Message, EventLogEntryType.Error);
            }

            finally
            {
                connection.Close();
            }

            return (rowsaffected > 0);
        }
    }
}
