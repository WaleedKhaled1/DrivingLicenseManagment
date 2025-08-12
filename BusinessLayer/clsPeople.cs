using DataAccessLayer1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace BusinessLayer
{
    public class clsPeople
    {

        public enum enMode { AddMode = 0, UpdateMode = 1 };
        enMode Mode = enMode.AddMode;

        public int PersonID { set; get; }
        public string NationalNo { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }
        public DateTime DateOfBirth { set; get; }
        public int Gendor { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int CountryID { set; get; }
        public string ImagePath { set; get; }


            

        public clsPeople() 
        {
            PersonID = -1;
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = new DateTime(2000, 1, 1);
            Gendor = 0; // 0 = unspecified, 1 = male, 2 = female, etc.
            Address = " ";
            Phone = "";
            Email = "";
            CountryID = 0;
            ImagePath = "";
            Mode = enMode.AddMode;
        }

        public clsPeople(int personID, string nationalNo, string firstName, string secondName, string thirdName, string lastName,
                  DateTime dateOfBirth, int gendor, string address, string phone, string email, int countryID, string imagePath)
        {
            PersonID = personID;
            NationalNo = nationalNo;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gendor = gendor;
            Address = address;
            Phone = phone;
            Email = email;
            CountryID = countryID;
            ImagePath = imagePath;
            Mode=enMode.UpdateMode;
        }


        
        public static DataTable GetAllPeople()
        {
            return clsPersonsData.GetAllPeople();
        }

        public static DataTable GetAllCountries()
        {
            return clsPersonsData.GetAllCountries();
        }

        public static bool IsExist(string NationalNo)
        {
            return clsPersonsData.IsPersonExistByNN(NationalNo);
        }

        public static clsPeople FindByID(int PersonID)
        {
           // System.Diagnostics.Debug.WriteLine("دخلنا على DVLDBusiness.Find");

            string NationalNo="",FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int Gendor = -1, NationalityCountryID = -1;
     

            if (clsPersonsData.FindByID(PersonID,ref NationalNo,ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email,ref NationalityCountryID,ref ImagePath))
            {
                return new clsPeople(PersonID,NationalNo,FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public static clsPeople FindByNationalNo(string NationalNo)
        {
            // System.Diagnostics.Debug.WriteLine("دخلنا على DVLDBusiness.Find");

            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int Gendor = -1, NationalityCountryID = -1,PersonID=-1;


            if (clsPersonsData.FindByNationalNo(ref PersonID, NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            {
                return new clsPeople(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }
        private bool AddPerson()
        {
            this.PersonID = clsPersonsData.AddPerson(this.NationalNo, this.FirstName,this.SecondName,this.ThirdName,this.LastName,this.DateOfBirth,this.Gendor,this.Address,this.Phone,this.Email,this.CountryID, this.ImagePath);

            return (this.PersonID != -1);
        }
        private bool UpdatePerson()
        {
            return clsPersonsData.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email, this.CountryID, this.ImagePath);
        }
        public static string GetCountryNameByID(int CountryID)
        {
            return clsPersonsData.GetCountryNameByID(CountryID);    
        }
        public static bool DeletePersonByID(int PersonID)
        {
            return clsPersonsData.DeletePersonByID(PersonID);
        }
        public bool Save()
        {
           switch(Mode)
            {
                case enMode.AddMode:    
                    {
                        if (AddPerson())
                        {
                            Mode = enMode.UpdateMode;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    case enMode.UpdateMode:
                    {
                        if(UpdatePerson())
                        {
                            Mode = enMode.AddMode;
                            return true;
                        }

                        else
                            { return false; }
                    }
            }

            return false;
        }

    }
}
