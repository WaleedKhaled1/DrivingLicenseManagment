using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer1
{
    public class clsLicenseClassData
    {
        static string SourceNameInEventViewer = "clsLicenseClassData";
        public static DataTable GetAllClasses()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from LicenseClasses";

            SqlCommand sqlCommand = new SqlCommand(query, connection);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();
                connection.Close();
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
        public static bool FindLicenseClassInfoByID(int licenseClassID, ref string className, ref string classDescription,
                                                ref int minimumAllowedAge, ref int defaultValidityLength, ref float classFees)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from LicenseClasses where LicenseClassID=@LicenseClassID ";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@LicenseClassID", licenseClassID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;

                    className = reader["ClassName"].ToString();
                    classDescription= reader["ClassDescription"].ToString();
                    minimumAllowedAge=Convert.ToInt32( reader["MinimumAllowedAge"]);   
                    defaultValidityLength= Convert.ToInt32(reader["DefaultValidityLength"]);
                    classFees = Convert.ToSingle( reader["ClassFees"]);

                }
                else
                {
                    IsFound = false;
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

            return IsFound;
        }


        public static bool FindLicenseClassInfoByName(ref int licenseClassID, string className, ref string classDescription,
                                               ref int minimumAllowedAge, ref int defaultValidityLength, ref float classFees)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from LicenseClasses where ClassName=@ClassName";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ClassName", className);

            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;
                    licenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
                    classDescription = reader["ClassDescription"].ToString();
                    minimumAllowedAge = Convert.ToInt32(reader["MinimumAllowedAge"]);
                    defaultValidityLength = Convert.ToInt32(reader["DefaultValidityLength"]);
                    classFees = Convert.ToSingle(reader["ClassFees"]);

                }
                else
                {
                    IsFound = false;
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

            return IsFound;
        }
    }
}
