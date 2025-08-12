using DataAccessLayer1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsApplications
    {
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7

        };

        public enum enMode { AddMode = 0, UpdateMode = 1 }
        public enMode _Mode = enMode.AddMode;
        public enum enStatus { New = 1, Cancelled = 2, Completed = 3 };

        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }

        public DVLDBusiness PersonInfo { set; get; }
        public DateTime ApplicationDate { set; get; }
        public int ApplicationTypeID { set; get; }
        public enStatus ApplicationStatus { set; get; }
        public DateTime ApplicationLastStatusDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }

        public clsApplications(int applicationID, int applicantPersonID, DateTime applicationDate,
        int applicationTypeID, enStatus applicationStatus, DateTime applicationLastStatusDate, float paidFees, int createdByUserID)
        {
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            PersonInfo=DVLDBusiness.FindByID(ApplicantPersonID);
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            ApplicationLastStatusDate = applicationLastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            _Mode = enMode.UpdateMode;
        }

        public clsApplications()
        {

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

        public static clsApplications FindAllApplicationInfoByID(int ApplicationID)
        {
            int ApplicantPersonID = -1, CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now, ApplicationLastStatusDate = DateTime.Now;

            int ApplicationStatus = -1;
            int ApplicationTypeID = -1;
            float PaidFees = -1;



            if (clsApplicationsData.FindAllApplicationInfoByID(ApplicationID,ref ApplicantPersonID, ref ApplicationTypeID,  
              ref CreatedByUserID, ref ApplicationDate, ref ApplicationLastStatusDate, ref ApplicationStatus, ref PaidFees))
            {
                return new clsApplications(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, (enStatus)ApplicationStatus, ApplicationLastStatusDate
                    , PaidFees, CreatedByUserID);
            }

            else
                return null;

        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID,int AppType,int LicenseClass)
        {
            return clsApplicationsData.GetActiveApplicationIDForLicenseClass(PersonID, AppType,LicenseClass);
        }

        public bool Cancel()

        {
            return clsApplicationsData.UpdateApplicationStatus(ApplicationID, 2);
        }

        public bool SetComplete()

        {
            return clsApplicationsData.UpdateApplicationStatus(ApplicationID, 3);
        }
        public static bool UpdateApplicationStat(int AppID,int status)
        {
            return clsApplicationsData.UpdateApplicationStatus(AppID, status);
        }

        private bool _AddNewApplication()
        {
            this.ApplicationID=clsApplicationsData.AddNewApplication(this.ApplicantPersonID,this.ApplicationTypeID,this.CreatedByUserID,
                this.ApplicationDate,this.ApplicationLastStatusDate,(int)this.ApplicationStatus,this.PaidFees);

            return (this.ApplicationID != -1);
        }

        protected static bool DeleteApplication(int AppID)
        {
            return clsApplicationsData.DeleteApplication(AppID);
        }

        public bool Save()
        {
            
            switch(_Mode)
            {
                case enMode.AddMode:
                    {
                        if (_AddNewApplication())
                            _Mode = enMode.UpdateMode;
                        return true;
                    }
            }

            return false;
        }


    }
}
