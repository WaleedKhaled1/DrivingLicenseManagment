using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer1
{
    public class clsDetainedLicenseData
    {
        static string SourceNameInEventViewer = "clsDetainedLicenseData";
        public static bool FindLicenseDetainedInfoByLicenseID(ref int detainID,int licenseID,ref DateTime detainDate,ref float fineFees,ref int createdByUserID,ref bool isReleased,
                                                              ref DateTime? releaseDate,ref int? releasedByUserID,ref int? releaseApplicationID)

        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM DetainedLicenses WHERE LicenseID = @LicenseID And IsReleased=0";

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

                    detainID = (int)reader["DetainID"];
                    licenseID = (int)reader["LicenseID"];
                    detainDate = (DateTime)reader["DetainDate"];
                    fineFees = Convert.ToSingle(reader["FineFees"]);
                    createdByUserID = (int)reader["CreatedByUserID"];
                    isReleased = (bool)reader["IsReleased"];

                    releaseDate = reader["ReleaseDate"] != DBNull.Value ? (DateTime?)reader["ReleaseDate"] : null;
                    releasedByUserID = reader["ReleasedByUserID"] != DBNull.Value ? (int?)reader["ReleasedByUserID"] : null;
                    releaseApplicationID = reader["ReleaseApplicationID"] != DBNull.Value ? (int?)reader["ReleaseApplicationID"] : null;
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

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from DetainedLicenses_View";

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

        public static int AddNewLicense(int licenseID,DateTime detainDate,float fineFees,int createdByUserID,bool isReleased)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            
                string query = @"
            INSERT INTO DetainedLicenses 
            (LicenseID,DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID)
            VALUES
            (@LicenseID,@DetainDate, @FineFees, @CreatedByUserID, @IsReleased, @ReleaseDate, @ReleasedByUserID, @ReleaseApplicationID)
            Select Scope_Identity()";


            SqlCommand command = new SqlCommand(query, connection);
                
                    command.Parameters.AddWithValue("@DetainDate",detainDate);
            command.Parameters.AddWithValue("@LicenseID", licenseID);
            command.Parameters.AddWithValue("@FineFees",fineFees);
                    command.Parameters.AddWithValue("@CreatedByUserID",createdByUserID);
                    command.Parameters.AddWithValue("@IsReleased",isReleased);

                    command.Parameters.AddWithValue("@ReleaseDate",System.DBNull.Value);
                    command.Parameters.AddWithValue("@ReleasedByUserID", System.DBNull.Value);
                    command.Parameters.AddWithValue("@ReleaseApplicationID", System.DBNull.Value);

            int _InsertedID = -1;
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();
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
        public static bool IsDetained(int LicenseID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select Found=1 from DetainedLicenses where LicenseID=@LicenseID AND IsReleased=0";

            SqlCommand sqlCommand = new SqlCommand(query, connection);

            sqlCommand.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                connection.Open();

                object result = sqlCommand.ExecuteScalar();

                if (result != null)
                {
                    isFound = true;
                }
                else
                {
                    isFound = false;
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

            return isFound;
        }

        public static bool UpdateDetainedLicenseToReleased(int LicenseID,DateTime? releaseDate, int? releasedByUserID, int? releaseApplicationID)
        {
            int rowsaffected = 0;
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "Update DetainedLicenses set IsReleased=1,ReleaseDate=@ReleaseDate,ReleasedByUserID=@ReleasedByUserID,ReleaseApplicationID=@ReleaseApplicationID " +
                " where LicenseID=@LicenseID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ReleaseDate", releaseDate);
            cmd.Parameters.AddWithValue("@ReleasedByUserID", releasedByUserID);
            cmd.Parameters.AddWithValue("@ReleaseApplicationID", releaseApplicationID);
            cmd.Parameters.AddWithValue("@LicenseID", LicenseID);
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
    
        
    
