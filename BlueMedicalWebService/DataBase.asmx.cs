using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;

namespace BlueMedicalWebService
{
    /// <summary>
    /// Summary description for DataBase
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DataBase : System.Web.Services.WebService
    {
        SqlConnection conn = new SqlConnection();

        #region Gets
        [WebMethod]
        public DataSet GetFixedAssets()
        {
            conn.ConnectionString = "Data Source=DESKTOP-6PS3SV3; Initial Catalog='WSActivosFijos'; Trusted_Connection = True";

            string query = "SELECT AssetID, AssetName, DepartmentName, Amount, DateCreated, LastUsed " +
                "FROM FixedAssets A " +
                "INNER JOIN Departments D ON A.DepartmentID = D.DepartmentID;";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetFixedAssetsByID(string id)
        {
            conn.ConnectionString = "Data Source=DESKTOP-6PS3SV3; Initial Catalog='WSActivosFijos'; Trusted_Connection = True";

            string query = "SELECT * FROM FixedAssets WHERE AssetID = " + id;

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetDepartments()
        {
            conn.ConnectionString = "Data Source=DESKTOP-6PS3SV3; Initial Catalog='WSActivosFijos'; Trusted_Connection = True";

            string query = "SELECT * FROM Departments";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetDepartmentsByID(string id)
        {
            conn.ConnectionString = "Data Source=DESKTOP-6PS3SV3; Initial Catalog='WSActivosFijos'; Trusted_Connection = True";

            string query = "SELECT * FROM Departments WHERE DepartmentID = " + id;

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);
            return ds;
        }
        #endregion

        #region Inserts
        [WebMethod]
        public void InsertFixedAsset(string assetName, string departmentID, string amount)
        {
            conn.ConnectionString = "Data Source=DESKTOP-6PS3SV3; Initial Catalog='WSActivosFijos'; Trusted_Connection = True";

            DateTime dateCreated = System.DateTime.Today;
            DateTime lastUsed = System.DateTime.Today;

            string query = "INSERT INTO FixedAssets (AssetName, DepartmentID, Amount, DateCreated, LastUsed) VALUES (@AssetName, @DepartmentID, @Amount, '" + dateCreated + "', '" + lastUsed + "' )";

            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@AssetName", assetName);
                cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                cmd.Parameters.AddWithValue("@Amount", amount);

                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        [WebMethod]
        public void InsertDepartment(string departmentName)
        {
            conn.ConnectionString = "Data Source=DESKTOP-6PS3SV3; Initial Catalog='WSActivosFijos'; Trusted_Connection = True";

            string query = "INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName)";

            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@DepartmentName", departmentName);

                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        [WebMethod]
        public void InsertUsers(string userName, string password, string status)
        {
            conn.ConnectionString = "Data Source=DESKTOP-6PS3SV3; Initial Catalog='WSActivosFijos'; Trusted_Connection = True";

            string query = "INSERT INTO Users (UserName, Password, Status) VALUES (@UserName, @Password, @Status)";

            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Status", status);

                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion
    }
}
