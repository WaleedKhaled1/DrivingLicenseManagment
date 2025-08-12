using DataAccessLayer1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsLicenses:clsDrivers
    {

        public enum enIssue { FirstTime = 1, Renew = 2, ReplacmentForDamaged = 3, ReplacmentForLost = 4 };
        public int LicenseID { set; get; }
        public int ApplicationID     { set; get; }
        public int LicenseClass { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
        public float PaidFees { set; get; }
        public bool IsActive { set; get; }
        public enIssue IssueReason { set; get; }
        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }

        public clsPeople PesonInfo { set; get; }

        public clsLicenses()
        {
            LicenseID = 0;
            ApplicationID = 0;
            DriverID = 0;
            LicenseClass = 0;
            IssueDate = DateTime.MinValue;
            ExpirationDate = DateTime.MinValue;
            Notes = string.Empty;
            PaidFees = 0.0f;
            IsActive = true; 
            IssueReason = enIssue.FirstTime;
            CreatedByUserID = 0;

            DriverID = 0;
            PersonID = 0;
            CreatedByUserID = 0;
            DCreatedDate = DateTime.Now;
        }

        public clsLicenses(int licenseID, int applicationID,int licenseClass, DateTime issueDate,
                  DateTime expirationDate, string notes, float paidFees, bool isActive, enIssue issueReason,int driverID,int personID,int createdByUserID,DateTime dcreatedDate)
        {
            LicenseID = licenseID;
            ApplicationID = applicationID;
            LicenseClass = licenseClass;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = issueReason;

            DriverID=driverID;
            PersonID = personID;
            clsPeople PesonInfo=clsPeople.FindByID(PersonID);
            CreatedByUserID = createdByUserID;
            DCreatedDate = dcreatedDate;
        }


        public static clsLicenses FindLicenseInfoByAppID(int AppID)
        {
            int licenseID = 0, licenseClass = 0, driverID = -1;
          DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
          string notes = "";
          float paidFees = 0.0f;
          bool isActive = false;
          int issueReason =(int) enIssue.FirstTime;

            clsDrivers Drivers=null;

            bool IsFound = false;

            IsFound=clsLicensesData.FindLicenseInfoByAppID(ref licenseID,AppID,ref driverID,ref licenseClass, ref issueDate, ref expirationDate, 
                ref notes, ref paidFees, ref isActive, ref issueReason);

            if (IsFound)
            {
                Drivers = clsDrivers.FindDriverByID(driverID);
            }

            if(Drivers!=null)
             {
                    return new clsLicenses(licenseID,AppID,licenseClass, issueDate, expirationDate, notes, paidFees, isActive,(enIssue) issueReason,driverID,Drivers.PersonID,
                        Drivers.CreatedByUserID,Drivers.DCreatedDate);
             }
             else
              return null;
            
        }

        public static clsLicenses FindLicenseInfoByLicenseID(int LicenseID)
        {
            int AppID = 0, licenseClass = 0, driverID = -1;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            string notes = "";
            float paidFees = 0.0f;
            bool isActive = false;
            int issueReason = (int)enIssue.FirstTime;

            clsDrivers Drivers = null;

            bool IsFound = false;

            IsFound = clsLicensesData.FindLicenseInfoByLicenseID(LicenseID,ref AppID, ref driverID, ref licenseClass, ref issueDate, ref expirationDate,
                ref notes, ref paidFees, ref isActive, ref issueReason);

            if (IsFound)
            {
                Drivers = clsDrivers.FindDriverByID(driverID);
            }

            if (Drivers != null)
            {
                return new clsLicenses(LicenseID, AppID, licenseClass, issueDate, expirationDate, notes, paidFees, isActive, (enIssue)issueReason, driverID, Drivers.PersonID,
                    Drivers.CreatedByUserID, Drivers.DCreatedDate);
            }
            else
                return null;
        }
        public static DataTable GetAllLocalLicensesForThisPerson(int PersonID)
        {
            return clsLicensesData.GetAllLocalLicensesForThisPerson(PersonID);
        }

        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicensesData.AddNewLicenseData(this.ApplicationID,this.DriverID,this.LicenseClass,this.IssueDate,this.ExpirationDate,
                this.Notes,this.PaidFees,this.IsActive,(int)this.IssueReason,this.CreatedByUserID);

            return (this.LicenseID != -1);
        }

        public bool Save()
        {
            clsDrivers driver=clsDrivers.FindDriverByPersonID(this.PersonID);

            if (driver != null)
            {
                this.DriverID = driver.DriverID;
            }

            else
            {
                if (!base.Save())
                {
                    return false;
                }
            }


            return _AddNewLicense();

        }


        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicensesData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);

        }

        private string GetIssueReasonText(enIssue enissue)
        {
            switch(enissue)
            {
                case enIssue.FirstTime:
                    return "First Time";

                case enIssue.Renew:
                    return "Renew";

                case enIssue.ReplacmentForDamaged:
                    return "Replacment For Damaged";

                case enIssue.ReplacmentForLost:
                    return "Replacment For Lost";
            }

            return "";
        }

        public static bool UpdateStatusLocalLicenseToActiveOrInActive(int LicenseID,int Status)
        {
            return clsLicensesData.UpdateStatusLocalLicenseToActiveOrInActive(LicenseID,Status);
        }


    }
}
