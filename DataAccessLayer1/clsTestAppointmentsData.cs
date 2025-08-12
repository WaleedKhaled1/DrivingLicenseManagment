using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer1
{
    public class clsTestAppointmentsData
    {
        public static DataTable GetAllTestAppointments(int LocalDrivingLicenseApplicationID, int testType)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from TestAppointments where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID And TestTypeID=@TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = LocalDrivingLicenseApplicationID;
            command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = testType;


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
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();
                reader.Close();
            }
            return dt;
        }

        public static bool FindTestAppointmentInfoByID(int appintmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate,
                                                       ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int? RetakeTestAppID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@TestAppointmentID", appintmentID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestTypeID = Convert.ToInt32( reader["TestTypeID"]);
                    LocalDrivingLicenseApplicationID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    IsLocked = Convert.ToBoolean( reader["IsLocked"]);
                    if (reader["RetakeTestApplicationID"]!=DBNull.Value)
                    RetakeTestAppID = Convert.ToInt32(reader["RetakeTestApplicationID"]);
                    else
                        RetakeTestAppID = -1;
                }

                else
                {
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return isFound;

        }
        public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
                                                        float PaidFees, int CreatedByUserID, bool IsLocked, int?RetakeTestAppID)
        {

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @"INSERT INTO TestAppointments VALUES
    (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestAppID) "+
     "SELECT SCOPE_IDENTITY();";


            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            cmd.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
            cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            cmd.Parameters.AddWithValue("@IsLocked", IsLocked);

            if (RetakeTestAppID != -1)
            {
                cmd.Parameters.AddWithValue("@RetakeTestAppID", RetakeTestAppID);
            }
            else
                cmd.Parameters.AddWithValue("@RetakeTestAppID", System.DBNull.Value);

            int InsertedID = -1;
            try
            {
                connection.Open();
               InsertedID = Convert.ToInt32(cmd.ExecuteScalar());
            }

            catch (Exception ex) 
            { 
                throw new Exception(ex.Message, ex); 
            }

            finally { connection.Close(); }

            return InsertedID;
        }
        public static bool _UpdateTestAppointment(int AppointmentID,int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
                                                    float PaidFees, int CreatedByUserID, bool IsLocked, int ?RetakeTestAppID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @"
        UPDATE TestAppointments
        SET  AppointmentDate = @AppointmentDate
        WHERE 
            TestAppointmentID = @AppointmentID;";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
           
            cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            int rowsAffected = -1;
            try
            {
                connection.Open();
                 rowsAffected= cmd.ExecuteNonQuery();
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
              
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
        public static bool UpdateAppointmentStatusToLocked(int testAppointmentID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @"UPDATE TestAppointments SET IsLocked=1 where TestAppointmentID=@TestAppointmentID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);

            int rowsAffected = -1;
            try
            {
                connection.Open();
                rowsAffected = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteAllAppointmentsForThisLocalLicenseAppID(int LocalDrivingLicenseID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "delete FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseID);


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

        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @"select TestID from Tests where TestAppointmentID=@TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
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


            return TestID;

        }
    }
}
