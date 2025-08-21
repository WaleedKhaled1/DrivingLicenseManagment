using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DataAccessLayer1
{
    public class clsUsersData
    {
        static string SourceNameInEventViewer = "clsUsersData";
        public static bool FindUser(int UserID, ref int PersonID, ref string username, ref string password, ref bool isactive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM Users WHERE UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    username = reader["UserName"].ToString();
                    password = reader["Password"].ToString();
                    PersonID = (int)reader["PersonID"];
                    isactive = Convert.ToBoolean(reader["IsActive"]);


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

                if (!EventLog.SourceExists(SourceNameInEventViewer))
                {
                    EventLog.CreateEventSource(SourceNameInEventViewer, "Application");
                }

                EventLog.WriteEntry(SourceNameInEventViewer, ex.Message, EventLogEntryType.Error);
            }

            finally { connection.Close(); }

            return isFound;

        }

        public static bool FindUserByUserNameAndPassword(ref int UserID, ref int PersonID, string username, string password, ref bool isactive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM Users WHERE UserName = @UserName and Password=@Password";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@Password", password);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    UserID = (int)reader["UserID"];
                    isactive = Convert.ToBoolean(reader["IsActive"]);


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

                if (!EventLog.SourceExists(SourceNameInEventViewer))
                {
                    EventLog.CreateEventSource(SourceNameInEventViewer, "Application");
                }

                EventLog.WriteEntry(SourceNameInEventViewer, ex.Message, EventLogEntryType.Error);
            }

            finally { connection.Close(); }

            return isFound;
        }
        public static bool IsUserExistAndActive(string username, string password, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select IsActive from Users where UserName=@username And Password=@password";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", username);
            command.Parameters.AddWithValue("@Password", password);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null)
                {
                    isFound = true;
                    IsActive = (bool)result;
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

        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "SELECT Found=1 FROM Users WHERE UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
               
                isFound = false;

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

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "SELECT P.*, U.*, FullName = FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName " +
                           "FROM People P INNER JOIN Users U ON P.PersonID = U.PersonID;";

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
                if (!EventLog.SourceExists(SourceNameInEventViewer))
                {
                    EventLog.CreateEventSource(SourceNameInEventViewer, "Application");
                }

                EventLog.WriteEntry(SourceNameInEventViewer, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                connection.Close();
            }

            return dt;

        }

        public static bool IsPersonUser(int perosnid)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = $"select Found=1 from Users where Users.PersonID=@personid";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personid", perosnid);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
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

                isFound = false;
            }

            finally
            {
                connection.Close();

            }

            return isFound;
        }

        public static int AddNewUser(int PersonID, string Username, string Password, bool IsActive)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "insert into Users values(@PersonID,@UserName,@Password,@IsActive)" +
                "select Scope_identity()";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@PersonID", PersonID);
            cmd.Parameters.AddWithValue("@UserName", Username);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@IsActive", IsActive);
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
            { connection.Close(); }

            return _InsertedID;
        }
 
        public static bool UpdateUser(int userID, string username, string password, bool isactive)
        {
            int rowsaffected = 0;
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "update Users set UserName=@username,Password=@password,IsActive=@isactive " +
                "where UserID=@userID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@userID", userID);
            command.Parameters.AddWithValue("@isactive", isactive);

            bool IsUpdated = false;


            try
            {
                connection.Open();
                rowsaffected = command.ExecuteNonQuery();
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

        public static bool DeleteUser(int UserID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "delete FROM Users WHERE UserID = @userid";
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@userid", UserID);


            int result = 0;
            try
            {
                connection.Open();
                result = cmd.ExecuteNonQuery();
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
            { connection.Close(); }

            return (result != 0);
        }

    }
}
