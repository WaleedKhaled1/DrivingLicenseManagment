using DataAccessLayer1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsTestAppointments
    {
        public enum enMode { AddMode=1,UpdateMode=2};
        public static enMode _Mode= enMode.AddMode;
        public int TestAppointmentID { get; set; } 
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestAppID { get; set; }

        public int TestID
        {
            get
            {
              return GetTestID();
            }
        }

        public clsTestAppointments()
        {
            TestAppointmentID = -1;
            TestTypeID = -1;
            LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.Now;
            PaidFees = 0.0f;
            CreatedByUserID = -1;
            IsLocked = false;
            RetakeTestAppID = -1;
            _Mode = enMode.AddMode;
        }

        public clsTestAppointments(int testAppointmentID,int testTypeID,int localDrivingLicenseApplicationID,DateTime appointmentDate,float paidFees,int createdByUserID,bool isLocked,int?retakeTestAppID)
        {
            TestAppointmentID = testAppointmentID;
            TestTypeID = testTypeID;
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            IsLocked = isLocked;
            RetakeTestAppID = retakeTestAppID;
            _Mode= enMode.UpdateMode;
        }

        public static DataTable GetAllTestAppointments(int LocalDrivingLicenseApplicationID,int testType)
        {
            return clsTestAppointmentsData.GetAllTestAppointments(LocalDrivingLicenseApplicationID,testType);
        }

        public static clsTestAppointments FindTestAppointmentInfoByID(int appointmentID)
        {
            int TestTypeID = -1,LocalDrivingLicenseApplicationID = -1,CreatedByUserID = -1;
            int? RetakeTestAppID = -1;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0.0f;
            bool IsLocked = false;

            if (clsTestAppointmentsData.FindTestAppointmentInfoByID(appointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID, ref AppointmentDate, ref PaidFees,
                ref CreatedByUserID, ref IsLocked, ref RetakeTestAppID))

                return new clsTestAppointments(appointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestAppID);
            else
            {
                return null;
            }
            
        }

        private bool _AddNewTestAppointment()
        {
            this.TestAppointmentID = clsTestAppointmentsData.AddNewTestAppointment(this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate, this.PaidFees,
                this.CreatedByUserID,this.IsLocked,this.RetakeTestAppID);

            return (this.TestAppointmentID!=-1);
                
        }

        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentsData._UpdateTestAppointment(this.TestAppointmentID, this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestAppID);
        }

        public static bool UpdateAppointmentStatusToLocked(int testAppointmentID)
        {
            return clsTestAppointmentsData.UpdateAppointmentStatusToLocked(testAppointmentID);
        }

        public static bool DeleteAllAppointmentsForThisLocalLicenseAppID(int LocalDrivingLicenseID)
        {
            return clsTestAppointmentsData.DeleteAllAppointmentsForThisLocalLicenseAppID(LocalDrivingLicenseID);
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddMode:
                    {
                        if(_AddNewTestAppointment())
                        {
                            _Mode=enMode.UpdateMode;
                            return true;
                        }

                        return false;
                    }

                    case enMode.UpdateMode:
                    {
                        return (_UpdateTestAppointment());
                    }

                    
            }

            return false;
        }

        public int GetTestID()
        {
            return clsTestAppointmentsData.GetTestID(TestAppointmentID);
        }


    }
}
