using DataAccessLayer1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsDetainedLicense
    {
        public int DetainID {  get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public float FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleasedByUserID { get; set; }
        public int? ReleaseApplicationID { get; set; }

        public clsDetainedLicense()
        {
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.Now;
            FineFees = 0.0f;
            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = DateTime.MinValue;
            ReleasedByUserID = -1;
            ReleaseApplicationID = -1;
        }

        public clsDetainedLicense(int detainID, int licenseID, DateTime detainDate, float fineFees,
                      int createdByUserID, bool isReleased, DateTime? releaseDate,
                      int ?releasedByUserID, int ?releaseApplicationID)
        {
            DetainID = detainID;
            LicenseID = licenseID;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserID = createdByUserID;
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUserID = releasedByUserID;
            ReleaseApplicationID = releaseApplicationID;
        }

        public static clsDetainedLicense FindLicenseDetainedInfoByLicenseID(int LicenseID)
        {
            int detainID=0;
            DateTime detainDate=DateTime.Now;
            float fineFees=0;
            int createdByUserID=0;
            bool isReleased=false;
            DateTime? releaseDate=DateTime.Now;
            int? releasedByUserID=0;
            int? releaseApplicationID=0;


            if (clsDetainedLicenseData.FindLicenseDetainedInfoByLicenseID(ref detainID, LicenseID,ref detainDate,ref fineFees, ref createdByUserID, ref isReleased, ref releaseDate,
                ref releasedByUserID, ref releaseApplicationID))
            {
                return new clsDetainedLicense(detainID,LicenseID,detainDate,fineFees,createdByUserID,isReleased,releaseDate,releasedByUserID,releaseApplicationID);
            }

            else
                return null;

        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }

        public bool UpdateDetainedLicenseToReleased()
        {
            return clsDetainedLicenseData.UpdateDetainedLicenseToReleased(this.LicenseID,this.ReleaseDate,this.ReleasedByUserID,this.ReleaseApplicationID);
        }

        public static bool IsDetained(int LicenseiD)
        {
            return clsDetainedLicenseData.IsDetained(LicenseiD);
        }

        private bool _AddNewLicense()
        {
            this.DetainID = clsDetainedLicenseData.AddNewLicense(this.LicenseID, this.DetainDate, this.FineFees,this.CreatedByUserID,this.IsReleased);

            return (this.DetainID != -1);
        }

        public bool Save()
        {
          return _AddNewLicense();
        }




    }
}
