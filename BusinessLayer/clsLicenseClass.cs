using DataAccessLayer1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsLicenseClass
    {
        public int LicenseClassID {  get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public int MinimumAllowedAge { get; set; }
        public int DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }

        public clsLicenseClass()
        {
            LicenseClassID = 0;
            ClassName = string.Empty;
            ClassDescription = string.Empty;
            MinimumAllowedAge = 18;
            DefaultValidityLength = 10;
            ClassFees = 0.0f;
        }

        public clsLicenseClass(int licenseClassID, string className, string classDescription,
                           int minimumAllowedAge, int defaultValidityLength, float classFees)
        {
            LicenseClassID = licenseClassID;
            ClassName = className;
            ClassDescription = classDescription;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFees = classFees;
        }


        public enum enMode { AddMode = 0, UpdateMode = 1 }
        enMode Mode = enMode.AddMode;

        public static DataTable GetAllClasses()
        {
            return clsLicenseClassData.GetAllClasses();
        }


        public static clsLicenseClass FindLicenseClassInfoByID(int LicenseClassID)
        {
            int minimumAllowedAge = 18, defaultValidityLength = 10;
            string className = string.Empty, classDescription = string.Empty;
            float classFees = 0.0f;

            if (clsLicenseClassData.FindLicenseClassInfoByID(LicenseClassID, ref className, ref classDescription,
                                                ref minimumAllowedAge, ref defaultValidityLength, ref classFees))
                return new clsLicenseClass(LicenseClassID, className, classDescription, minimumAllowedAge, defaultValidityLength, classFees);

                    else
                return null;


        }

        public static clsLicenseClass FindLicenseClassInfoByName(string classname)
        {
            int LicenseClassID=-1,minimumAllowedAge = 18, defaultValidityLength = 10;
         string classDescription = string.Empty;
            float classFees = 0.0f;

            if (clsLicenseClassData.FindLicenseClassInfoByName(ref LicenseClassID, classname, ref classDescription,
                                                ref minimumAllowedAge, ref defaultValidityLength, ref classFees))
                return new clsLicenseClass(LicenseClassID, classname, classDescription, minimumAllowedAge, defaultValidityLength, classFees);

            else
                return null;


        }
    }
}
