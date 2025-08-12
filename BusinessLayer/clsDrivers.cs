using DataAccessLayer1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsDrivers
    {
        public int DriverID { set; get; }
        public int PersonID { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime DCreatedDate{ set; get; }

        public clsDrivers()
        {
            DriverID = 0;
            PersonID = 0;
            CreatedByUserID = 0;
            DCreatedDate = DateTime.MinValue;
        }

        public clsDrivers(int driverID, int personID, int createdByUserID, DateTime dCreatedDate)
        {
            DriverID = driverID;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            DCreatedDate = dCreatedDate;
        }

        public static DataTable GetAllDriversInfo()
        {
            return clsDriversData.GetAllDriversInfo();
        }

        public static clsDrivers FindDriverByID(int DriverID)
        {
            int personID = -1, createdbyuserID = -1;
            DateTime dcreatedDate= DateTime.MinValue;

            if (clsDriversData.FindDriverByID(DriverID, ref personID, ref createdbyuserID, ref dcreatedDate))
            {
                return new clsDrivers(DriverID, personID, createdbyuserID, dcreatedDate);
            }
            else
                return null;
        }

        public static clsDrivers FindDriverByPersonID(int PersonID)
        {
            int DriverID = -1, createdbyuserID = -1;
            DateTime dcreatedDate = DateTime.MinValue;

            if (clsDriversData.FindDriverByPersonID(ref DriverID, PersonID, ref createdbyuserID, ref dcreatedDate))
            {
                return new clsDrivers(DriverID, PersonID, createdbyuserID, dcreatedDate);
            }
            else
                return null;
        }

        private bool _AddNewDriver()
        {
            this.DriverID=clsDriversData._AddNewDriver(this.PersonID, this.CreatedByUserID,this.DCreatedDate);
            return (this.DriverID!=-1);
        }

        public bool Save()
        {
            return _AddNewDriver();
        }

    }
}
