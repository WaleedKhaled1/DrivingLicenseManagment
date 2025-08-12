using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace DataAccessLayer1
{
    public class clsLocalDrivingLicenseData
    {
        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from LocalDrivingLicenseApplications_View";

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

        public static bool FindLocalDrivingLicenseInfoData(int LocalLicenseID,ref int LocalClassID,ref int ApplicationID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplications";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplications", LocalLicenseID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    IsFound=true;
                    LocalClassID =(int) reader["LicenseClassID"];
                    ApplicationID = (int)reader["ApplicationID"];
                }

                else
                {
                    IsFound=false;
                }
            }

            catch(Exception ex) 
            {
              throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close ();
                reader.Close();
            }

            return IsFound;
        }

        public static bool FindLocalDrivingLicenseInfoData(ref int LocalLicenseID, ref int LocalClassID, int ApplicationID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from LocalDrivingLicenseApplications where ApplicationID=@ApplicationID";

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
                    LocalClassID = (int)reader["LicenseClassID"];
                    LocalLicenseID = (int)reader["LocalDrivingLicenseApplicationID"];
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

            finally
            {
                connection.Close();
                reader.Close();
            }

            return IsFound;
        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)

        {


            bool IsFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @" SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    IsFound = true;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }

            return IsFound;

        }

        public static bool IsExistApplicationForThisLicenseClass(int ApplicationID,int LicenseClassID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select Found=1 from LocalDrivingLicenseApplications where LicenseClassID=@LicenseClassID AND ApplicationID=@ApplicationID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
             cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);


            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    IsFound = true;
                }
                else
                    IsFound = false;
            }

            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }

            finally { connection.Close (); }

            return IsFound;

        }

        public static int AddNewLocalDrivingLicense( int LocalClassID, int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "Insert into LocalDrivingLicenseApplications Values(@ApplicationID,@LocalClassID)" +
                "Select Scope_Identity()";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@LocalClassID", LocalClassID);
            cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            int _InsertedID = -1;
            try
            {
                connection.Open ();

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

        public static bool UpdateLocalDrivingLicense(int LocalDrivingLicenseID, int LocalClassID)
        {
            int rowsaffected =0;
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "Update LocalDrivingLicenseApplications set LicenseClassID=@LocalClassID where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplications";
               
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplications", LocalDrivingLicenseID);
            cmd.Parameters.AddWithValue("@LocalClassID", LocalClassID);

            try
            {
                connection.Open();
                rowsaffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowsaffected>0);

        }

        public static bool DeleteLocalDrivingLicenseApplication(int _LocalDrivingLicenseID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "delete FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", _LocalDrivingLicenseID);


            int result = 0;
            try
            {
                connection.Open();
                 result= cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            { connection.Close(); }

            return (result!=0);
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)

        {

            bool Result = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @" SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)  
                            AND(TestAppointments.TestTypeID = @TestTypeID) and isLocked=0
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null)
                {
                    Result = true;
                }

            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }

            return Result;

        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)

        {


            bool Result = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @" SELECT top 1 TestResult
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && bool.TryParse(result.ToString(), out bool returnedResult))
                {
                    Result = returnedResult;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }

            return Result;

        }

        public static int GetNumberOfPassedTests(int localdrivinglicenseID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @"select Count(*) from Tests join TestAppointments on Tests.TestAppointmentID=TestAppointments.TestAppointmentID
                             where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplications AND TestResult=1";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplications", localdrivinglicenseID);

            int _Number = -1;
            try
            {
                connection.Open();
                object Result = cmd.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int value))
                {
                    _Number = value;
                }


            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            finally { connection.Close(); }

            return _Number;
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)

        {


            byte TotalTrialsPerTest = 0;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @" SELECT TotalTrialsPerTest = count(TestID) 
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                       ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(), out byte Trials))
                {
                    TotalTrialsPerTest = Trials;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }

            return TotalTrialsPerTest;

        }

    }
}
