using DataAccessLayer1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsLocalDrivingLicense : clsApplications
    {
        public enum enMode { AddMode = 0, UpdateMode = 1 }
        enMode _Mode = enMode.AddMode;
        public int LocalDrivingLicenseID { get; set; }
        public int LocalLicenseClassID { get; set; }

        public clsLocalDrivingLicense()
        {
            LocalDrivingLicenseID = 0;
            LocalLicenseClassID = 0;

            ApplicationID = 0;
            ApplicantPersonID = 0;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = 0;
            ApplicationStatus = enStatus.New;
            ApplicationLastStatusDate = DateTime.Now;
            PaidFees = 0.0f;
            CreatedByUserID = 0;
            _Mode = enMode.AddMode;
        }

        public clsLocalDrivingLicense(int localDrivingLicenseID, int localLicenseClassID, int applicationID, int applicantPersonID, DateTime applicationDate,
        int applicationTypeID, enStatus applicationStatus, DateTime applicationLastStatusDate, float paidFees, int createdByUserID)
        {
            LocalDrivingLicenseID = localDrivingLicenseID;
            LocalLicenseClassID = localLicenseClassID;
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            PersonInfo = DVLDBusiness.FindByID(ApplicantPersonID);
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            ApplicationLastStatusDate = applicationLastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            _Mode = enMode.UpdateMode;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseData.GetAllLocalDrivingLicenseApplications();
        }

        public static clsLocalDrivingLicense FindLocalDrivingLicenseInfoByID(int LocalLicenseID)
        {
            int LocalClassID = -1, ApplicationID = -1;
            clsApplications Application = null;

            bool IsFound = clsLocalDrivingLicenseData.FindLocalDrivingLicenseInfoData(LocalLicenseID, ref LocalClassID, ref ApplicationID);

            if (IsFound)
            {
                Application = clsApplications.FindAllApplicationInfoByID(ApplicationID);
            }

            if (Application != null)
            {
                return new clsLocalDrivingLicense(LocalLicenseID, LocalClassID, Application.ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                    Application.ApplicationTypeID, Application.ApplicationStatus, Application.ApplicationLastStatusDate, Application.PaidFees, Application.CreatedByUserID);
            }

            else
                return null;


        }

        public static clsLocalDrivingLicense FindLocalDrivingLicenseInfoByAppID(int AppID)
        {
            int LocalClassID = -1, LocalDrivingID = -1;
            clsApplications Application = null;

            bool IsFound = clsLocalDrivingLicenseData.FindLocalDrivingLicenseInfoData(ref LocalDrivingID, ref LocalClassID, AppID);

            if (IsFound)
            {
                Application = clsApplications.FindAllApplicationInfoByID(AppID);
            }

            if (Application != null)
            {
                return new clsLocalDrivingLicense(LocalDrivingID, LocalClassID, Application.ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                    Application.ApplicationTypeID, Application.ApplicationStatus, Application.ApplicationLastStatusDate, Application.PaidFees, Application.CreatedByUserID);
            }

            else
                return null;


        }

        public static bool IsExistApplicationForThisLicenseClass(int AppID,int LicenseClassID)
        {
            return clsLocalDrivingLicenseData.IsExistApplicationForThisLicenseClass(AppID, LicenseClassID);
        }
        private bool _AddNewLocalDrivingLicense()
        {
            this.LocalDrivingLicenseID=clsLocalDrivingLicenseData.AddNewLocalDrivingLicense(this.LocalLicenseClassID,this.ApplicationID);

            return(this.LocalDrivingLicenseID!=-1);
        }

        private bool _UpdateLocalDrivingLicense()
        {
            return clsLocalDrivingLicenseData.UpdateLocalDrivingLicense(this.LocalDrivingLicenseID, this.LocalLicenseClassID);
        }
            
        public bool DoesAttendTestType(clsTestTypes.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseData.DoesAttendTestType(this.LocalDrivingLicenseID, (int)TestTypeID);
        }

        public bool DoesPassTestType(clsTestTypes.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseData.DoesPassTestType(this.LocalDrivingLicenseID, (int)TestTypeID);
        }

        public bool IsLicenseIssued()
        {
            return clsLicenses.GetActiveLicenseIDByPersonID(this.ApplicantPersonID,this.LocalLicenseClassID)!=-1;
        }

        public int GetActiveLicenseID()
        {
            return clsLicenses.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LocalLicenseClassID);
        }

        public bool IsThereAnActiveScheduledTest(clsTestTypes.enTestType TestTypeID)
        {

            return clsLocalDrivingLicenseData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseID, (int)TestTypeID);
        }

        public static bool _DeleteLocalDrivingLicenseApplication(int _LocalDrivingLicenseID)
        {
            clsLocalDrivingLicense obj=FindLocalDrivingLicenseInfoByID(_LocalDrivingLicenseID);


            clsTests.DeleteAllTestsOfferedForThisLocalLiceneseAppIDIfExists(_LocalDrivingLicenseID);

            clsTestAppointments.DeleteAllAppointmentsForThisLocalLicenseAppID(_LocalDrivingLicenseID);
            

                    if (clsLocalDrivingLicenseData.DeleteLocalDrivingLicenseApplication(_LocalDrivingLicenseID))
                    {
                        return DeleteApplication(obj.ApplicationID);
                    }
                
            

            return false;
        }

        public clsTests GetLastTestPerTestType(clsTestTypes.enTestType TestTypeID)
        {
            return clsTests.FindLastTestPerPersonAndLicenseClass(this.ApplicantPersonID, this.LocalLicenseClassID, TestTypeID);
        }

        public int GetNumberOfPassedTests()
        {
            return clsLocalDrivingLicenseData.GetNumberOfPassedTests(this.LocalDrivingLicenseID);
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID,int TestTypeID)

        {
            return clsLocalDrivingLicenseData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID,TestTypeID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddMode:
                    {
                        base._Mode = (clsApplications.enMode)_Mode;

                        if (!base.Save())
                        {
                            return false;
                        }

                        if (_AddNewLocalDrivingLicense())
                            _Mode = enMode.UpdateMode;
                        return true;
                    }

                    case enMode.UpdateMode:
                    {
                        return _UpdateLocalDrivingLicense();
                    }


            }

            return false;
        }
    }
}
