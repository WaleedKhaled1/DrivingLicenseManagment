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
    public class clsApplicationTypes
    {
        public int AppTypeID { get; set; }
        public string AppTitle { get; set; }
        public float AppTypeFees { get; set; }

        clsApplicationTypes() 
        { 
            AppTypeFees = -1;
            AppTypeID = -1;
            AppTitle ="";
        }

        clsApplicationTypes(int ID,string Type,float Fees)
        {
            AppTypeFees = Fees;
            AppTypeID = ID;
            AppTitle = Type;
        }
        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationsTypes();
        }

        public static clsApplicationTypes FindApplicationTypeInfo(int AppTypeID)
        {
            string Type = "";
            float Fees = -1;

            if(clsApplicationTypesData.FindApplicationTypeInfo(AppTypeID,ref Type,ref Fees))
                return new clsApplicationTypes(AppTypeID,Type,Fees);
            else
                return null;
        }

        public bool UpdateApplicationType()
        {
            return clsApplicationTypesData.UpdateApplicationType(this.AppTypeID, this.AppTitle,this.AppTypeFees);
        }
    }
}
