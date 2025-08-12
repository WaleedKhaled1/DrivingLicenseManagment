using DataAccessLayer1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsInternationalDrivingLicensens : clsApplications
    {
        public int InternationalLicenseID { set; get; }
        public int DriverID { set; get; }
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }


        public clsInternationalDrivingLicensens()
        {
            InternationalLicenseID = 0;
            DriverID = 0;
            IssuedUsingLocalLicenseID = 0;
            IssueDate = default;
            ExpirationDate = default;
            IsActive = false;
            ApplicationID = 0;
            ApplicantPersonID = 0;
            ApplicationDate = default;
            ApplicationTypeID = 0;
            ApplicationStatus = enStatus.New;
            ApplicationLastStatusDate = default;
            PaidFees = 0f;
            CreatedByUserID = 0;
        }

        public clsInternationalDrivingLicensens(
       int internationalLicenseID,
       int driverID,
       int issuedUsingLocalLicenseID,
       DateTime issueDate,
       DateTime expirationDate,
       bool isActive,
       int applicationID,
       int applicantPersonID,
       DateTime applicationDate,
       int applicationTypeID,
       enStatus applicationStatus,
       DateTime applicationLastStatusDate,
       float paidFees,
       int createdByUserID)
        {
            InternationalLicenseID = internationalLicenseID;
            DriverID = driverID;
            IssuedUsingLocalLicenseID = issuedUsingLocalLicenseID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;

            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            PersonInfo = DVLDBusiness.FindByID(ApplicantPersonID);
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            ApplicationLastStatusDate = applicationLastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
        }

        public static DataTable GetAllInternationalLicensesForThisPerson(int PersonID)
        {
            return clsIntrnationalDrivingLicenseData.GetAllInternationalLicensesForThisPerson(PersonID);
        }

        public static DataTable GetAllInternationalLicensesApplications()
        {
            return clsIntrnationalDrivingLicenseData.GetAllInternationalLicensesApplications();
        }

        public static bool UpdateStatusInternationalLicenseToInActive(int InternationalLicenseID)
        {
            return clsIntrnationalDrivingLicenseData.UpdateStatusInternationalLicenseToInActive(InternationalLicenseID);
        }
       
        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsIntrnationalDrivingLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        public static clsInternationalDrivingLicensens FindInternationalLicenseInfoByDriverID(int DriverID)
        {
            int internationalLicenseID = 0;
            int AppID = 0;
            int LocalLicenseID = 0;
            DateTime issueDate = DateTime.Now;
            DateTime expirationDate = DateTime.Now;
            bool isActive = false;

            clsApplications Application = null;

            bool IsFound = clsIntrnationalDrivingLicenseData.FindInternationalLicenseInfoByDriverID(ref internationalLicenseID,ref AppID,DriverID,ref LocalLicenseID,
                ref issueDate,ref expirationDate,ref isActive);

            if (IsFound)
            {
                Application = clsApplications.FindAllApplicationInfoByID(AppID);
            }

            if (Application != null)
            {
                return new clsInternationalDrivingLicensens(internationalLicenseID,DriverID,LocalLicenseID,issueDate,expirationDate,isActive
                    ,Application.ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                    Application.ApplicationTypeID, Application.ApplicationStatus, Application.ApplicationLastStatusDate, Application.PaidFees, Application.CreatedByUserID);
            }

            else
                return null;
        }

        public static clsInternationalDrivingLicensens FindInternationalLicenseInfoByInternationalLicenseID(int InternationalLicenseID)
        {
            int LocalLicenseID = 0;
            int AppID = 0;
            int driverID = 0;
            DateTime issueDate = DateTime.Now;
            DateTime expirationDate = DateTime.Now;
            bool isActive = false;

            clsApplications Application = null;

            bool IsFound = clsIntrnationalDrivingLicenseData.FindInternationalLicenseInfoByInternationalLicenseID(InternationalLicenseID, ref AppID, ref driverID,ref LocalLicenseID,
                ref issueDate, ref expirationDate, ref isActive);

            if (IsFound)
            {
                Application = clsApplications.FindAllApplicationInfoByID(AppID);
            }

            if (Application != null)
            {
                return new clsInternationalDrivingLicensens(InternationalLicenseID, driverID, LocalLicenseID, issueDate, expirationDate, isActive
                    , Application.ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                    Application.ApplicationTypeID, Application.ApplicationStatus, Application.ApplicationLastStatusDate, Application.PaidFees, Application.CreatedByUserID);
            }

            else
                return null;
        }

        private bool _AddNewIntrnationalDrivingLicense()
        {
            this.InternationalLicenseID=clsIntrnationalDrivingLicenseData.AddNewIntrnationalDrivingLicense(this.ApplicationID,this.DriverID,this.IssuedUsingLocalLicenseID
                ,this.IssueDate,this.ExpirationDate,this.IsActive,this.CreatedByUserID);

            return (this.InternationalLicenseID != -1);
        }

        public bool Save()
        {
            base._Mode = enMode.AddMode;

            if (!base.Save())
            {
                return false;
            }

            return _AddNewIntrnationalDrivingLicense();
        }


    }
}
