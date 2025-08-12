using DataAccessLayer1;
using System.Data;

namespace BusinessLayer
{   
    public class clsUsers
    {
        public enum enModeUsers { AddMode = 0, UpdateMode = 1 }

        public enModeUsers Mode = enModeUsers.AddMode;
        public int UserID { set; get; }
        public int PersonID { set; get; }

        public clsPeople PersonInfo;
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }

        public clsUsers()
        {
            UserID = -1;
            PersonID = -1;
            UserName = "";
            Password = "";
            IsActive = false;
            Mode = enModeUsers.AddMode;
        }
        public clsUsers(int userID, int personID, string username, string password, bool isactive)
        {
            UserID = userID;
            PersonID = personID;
            UserName = username;
            Password = password;
            IsActive = isactive;
            this.PersonInfo=clsPeople.FindByID(personID);
            Mode = enModeUsers.UpdateMode;
        }

        public static bool IsUserExistAndActive(string username, string password, ref bool IsActive)
        {
            return clsUsersData.IsUserExistAndActive(username, password, ref IsActive);
        }

        public static DataTable GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }

        public static bool IsPersonUser(int personid)
        {
            return clsUsersData.IsPersonUser(personid);
        }

        public static bool isUserExist(string UserName)
        {
            return clsUsersData.IsUserExist(UserName);
        }

        public  bool AddNewUser()
        {
            this.UserID= clsUsersData.AddNewUser(this.PersonID,this.UserName, this.Password,this. IsActive);
            return(this.UserID !=-1);
        }

        public static clsUsers FindUser(int UserID)
        {
            string username = "", password = "";
            bool isactive = false;
            int PersonID = -1;

            if (clsUsersData.FindUser(UserID, ref PersonID, ref username, ref password, ref isactive))
                return new clsUsers(UserID, PersonID, username, password, isactive);
            else
                return null;
        }

        public static clsUsers FindUserByUserNameAndPassword(string username, string password)
        {
            int UserID = -1, PersonID = -1;
            bool isactive = false;

            if (clsUsersData.FindUserByUserNameAndPassword(ref UserID, ref PersonID, username, password, ref isactive))
                return new clsUsers(UserID, PersonID, username, password, isactive);
            else
                return null;
        }

        public  bool UpdateUser()
        {
            return clsUsersData.UpdateUser(this.UserID, this.UserName, this.Password,this. IsActive);
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUsersData.DeleteUser(UserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enModeUsers.AddMode:
                    {
                        if (AddNewUser())
                        {
                            Mode = enModeUsers.UpdateMode;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                case enModeUsers.UpdateMode:
                    {
                        if (UpdateUser())
                        {
                            //Mode = enModeUsers.AddMode;
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
