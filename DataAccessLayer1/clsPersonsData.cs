using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer1
{
    public class clsPersonsData
    {
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "SELECT P.*, CASE P.Gendor WHEN 0 THEN 'Male'  WHEN 1 THEN 'Female' ELSE 'Unknown' END As GendorCaption,CountryName " +
                "FROM People P inner JOIN Countries C ON P.NationalityCountryID = C.CountryID;";

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

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select * from Countries";

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
                Console.WriteLine(ex.Message);
            }

            finally
            {
                connection.Close();
                reader.Close();
            }

            return dt;
        }

        public static bool IsPersonExistByNN( string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "select Found=1 from People where NationalNo=@nationalno";

            SqlCommand sqlCommand = new SqlCommand(query, connection);

            sqlCommand.Parameters.AddWithValue("@nationalno", NationalNo);
    

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
                throw new Exception(ex.Message);
            }

            finally
            {
                connection.Close();

            }

            return isFound;
        }

        public static int AddPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, int Gendor, string Address, string Phone, string Email,
            int CountryID, string ImagePath)
        {


            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = @"insert into People  (NationalNo,FirstName, SecondName,ThirdName,LastName,DateOfBirth,Gendor,Address, Phone,Email, NationalityCountryID,ImagePath)
                            values  (@NationalNo,@FirstName,@SecondName,@ThirdName,@LastName,@DateOfBirth,@Gendor,@Address,@Phone,@Email,@CountryID,@ImagePath)" +
                "select Scope_identity()";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@SecondName", SecondName);
            if (ThirdName != "")
            {
                cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            cmd.Parameters.AddWithValue("@Gendor", Gendor);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            if (Email != "")
            {
                cmd.Parameters.AddWithValue("@Email", Email);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Email", System.DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@CountryID", CountryID);

            if (ImagePath != "")
            {
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            }

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
                Console.WriteLine(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return _InsertedID;


        }

        public static bool FindByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName,
          ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref int Gendor,
           ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    NationalNo = reader["NationalNo"].ToString();
                    FirstName = reader["FirstName"].ToString();
                    SecondName = reader["SecondName"].ToString();
                    ThirdName = reader["ThirdName"].ToString();
                    LastName = reader["LastName"].ToString();
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    Gendor = Convert.ToInt32(reader["Gendor"]);
                    Address = reader["Address"].ToString();
                    Email = reader["Email"].ToString();
                    Phone = reader["Phone"].ToString();
                    NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);
                    ImagePath = reader["ImagePath"] == DBNull.Value ? "" : reader["ImagePath"].ToString();
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

        public static bool FindByNationalNo(ref int PersonID, string NationalNo, ref string FirstName, ref string SecondName,
          ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref int Gendor,
           ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "SELECT * FROM People WHERE NationalNo = @nationalno";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nationalno", NationalNo);

            try
            {
                //System.Diagnostics.Debug.WriteLine("Trying to open connection...");
                connection.Open();
                // System.Diagnostics.Debug.WriteLine("Connection opened.");

                SqlDataReader reader = cmd.ExecuteReader();
                System.Diagnostics.Debug.WriteLine("Reader executed. Has rows: " + reader.HasRows);

                if (reader.Read())
                {
                    isFound = true;
                    //System.Diagnostics.Debug.WriteLine("Reader found a row. Filling values...");

                    PersonID = (int)reader["PersonID"];
                    FirstName = reader["FirstName"].ToString();
                    SecondName = reader["SecondName"].ToString();
                    ThirdName = reader["ThirdName"].ToString();
                    LastName = reader["LastName"].ToString();
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    Gendor = Convert.ToInt32(reader["Gendor"]);
                    Address = reader["Address"].ToString();
                    Email = reader["Email"].ToString();
                    Phone = reader["Phone"].ToString();
                    NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);
                    ImagePath = reader["ImagePath"] == DBNull.Value ? "" : reader["ImagePath"].ToString();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No row found in reader.");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
     DateTime DateOfBirth, int Gendor, string Address, string Phone, string Email,
     int NationalityCountryID, string ImagePath)
        {
            int rowsaffected = 0;
            
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = @"
        UPDATE People  
        SET NationalNo = @NationalNo,
            FirstName = @FirstName, 
            SecondName = @SecondName,
            ThirdName = @ThirdName,
            LastName = @LastName, 
            DateOfBirth = @DateOfBirth,
            Gendor = @Gendor,
            Address = @Address, 
            Phone = @Phone, 
            Email = @Email, 
            NationalityCountryID = @NationalityCountryID,
            ImagePath = @ImagePath
        WHERE PersonID = @PersonID";


            SqlCommand cmd = new SqlCommand(query, connection);
            
                cmd.Parameters.AddWithValue("@PersonID", PersonID);
                cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@SecondName", SecondName);
                if (ThirdName != null)
                    cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
                else
                    cmd.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                cmd.Parameters.AddWithValue("@Gendor", Gendor);
                cmd.Parameters.AddWithValue("@Address", Address);
                cmd.Parameters.AddWithValue("@Phone", Phone);
                cmd.Parameters.AddWithValue("@Email", Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath ?? (object)DBNull.Value);

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
        

        public static string GetCountryNameByID(int countryID)
        {
            string countryName = "";

            SqlConnection connection = new SqlConnection(StringDataAccess.connection);

            string query = "SELECT CountryName FROM Countries WHERE CountryID = @CountryID";
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@CountryID", countryID);

            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                    countryName = result.ToString();
            }
            catch (Exception ex)
            {
                // تقدر تطبع الخطأ أو تسجله
                throw new Exception("Error fetching country name: " + ex.Message);
            }


            return countryName;
        }

        public static bool DeletePersonByID(int PersonID)
        {
            SqlConnection connection = new SqlConnection(StringDataAccess.connection);
            string query = "delete FROM People WHERE PersonID = @PersonID";
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            bool IsDeleted=false;

            try
            {
                connection.Open();
                int result = cmd.ExecuteNonQuery();

                if (result != 0)
                {
                    IsDeleted = true;
                }

                else
                { IsDeleted = false; }
            }

            catch (Exception ex)
            {
                IsDeleted = false;
            }

            finally
            { connection.Close(); }

            return IsDeleted;
        }

      

    }

    }

