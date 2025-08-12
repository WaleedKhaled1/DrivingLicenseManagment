using DataAccessLayer1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsTestTypes
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        public enTestType TestTypeID { set; get; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public float TestTypeFees { get; set; }
        public clsTestTypes()
        {
            TestTypeID = enTestType.VisionTest;
            TestTypeTitle = "";
            TestTypeDescription = "";
            TestTypeFees = -1;
        }

        private clsTestTypes(enTestType TestTypeID,string TestTypeTitle,string TestTypeDescription,float TestTypeFees)
        {
            this.TestTypeID =TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;
        }


        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }
        public static clsTestTypes FindTestTypeInfo(enTestType TestTypeID)
        {
            string Title = "", Description = "";
            float Fees = -1;

            if (clsTestTypesData.FindTestTypeInfo((int)TestTypeID,ref Title, ref Description, ref Fees))
                return new clsTestTypes(TestTypeID, Title, Description, Fees);
            else
                return null;
        }
        public bool UpdateTestType()
        {
            return clsTestTypesData.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        }
    }
}
