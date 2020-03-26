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
        public Boolean ValidationUser(string username, string password)
        {
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string query = "SELECT COUNT(Username) from Users where Username='"
                + username + "' and Password='" + password + "'";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);
            return (ds.Tables[0].Rows[0][0].ToString() == "1");
        }

        [WebMethod]
        public Boolean GetStatus(string username)
        {
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            string query = "SELECT COUNT(Username) from Users where Username='"
                + username + "' and Status = 'Active'";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);
            return (ds.Tables[0].Rows[0][0].ToString() == "1");

        }

        [WebMethod]
        public DataSet GetFixedAssets()
        {
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            string query = "SELECT AssetID, AssetName, DepartmentName, DateCreated, LastUsed, UsefulLife, Price, DepreciationRate, DepreciatedAmount FROM FixedAssets A INNER JOIN Departments D ON A.DepartmentID = D.DepartmentID ORDER BY AssetID;";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetFixedAssetsByID(string id)
        {
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            string query = "SELECT * FROM FixedAssets WHERE AssetID = " + id;

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetDepartments()
        {
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            string query = "SELECT * FROM Departments";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);
            return ds;
        }

        [WebMethod]
        public DataSet GetDepartmentsByID(string id)
        {
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            string query = "SELECT * FROM Departments WHERE DepartmentID = " + id;

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);
            return ds;
        }
        #endregion

        #region Inserts
        [WebMethod]
        public void InsertFixedAsset(string assetName, string departmentID, string usefulLife, string price)
        {
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            DateTime dateCreated = System.DateTime.Today;
            DateTime lastUsed = System.DateTime.Today;

            var vidaUtil = Convert.ToDouble(usefulLife);
            var precio = Convert.ToDouble(price);

            //no sirve formula de depreciationRate y depreciatedAmount
            double depreciationRate =  1 / vidaUtil;
            depreciationRate = depreciationRate * 2;
            var depreciatedAmount = precio * depreciationRate;

             string query = "INSERT INTO FixedAssets (AssetName, DepartmentID, DateCreated, LastUsed, UsefulLife, Price, DepreciationRate, DepreciatedAmount) VALUES (@AssetName, @DepartmentID, '" + dateCreated + "', '" + lastUsed + "', @UsefulLife, @Price, @DepreciationRate, @DepreciatedAmount)";

            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@AssetName", assetName);
                cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                cmd.Parameters.AddWithValue("@UsefulLife", usefulLife);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@DepreciationRate", depreciationRate);
                cmd.Parameters.AddWithValue("@DepreciatedAmount", depreciatedAmount);

                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        [WebMethod]
        public void InsertDepartment(string departmentName)
        {
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


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
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


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

        #region Updates
        [WebMethod]
        public void UpdateFixedAsset(string assetId, string assetName, string departmentID, string amount)
        {
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            DateTime lastUsed = System.DateTime.Today;

            string query = "UPDATE FixedAssets SET AssetName = @AssetName, DepartmentID = @DepartmentID, LastUsed = '" + lastUsed + "',  WHERE AssetID = " + assetId + "";

            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@AssetName", assetName);
                cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region Delete
        [WebMethod]
        public void DeleteFixedAsset(string assetId)
        {
            conn.ConnectionString = "Server=tcp:ulacitws.database.windows.net,1433;Initial Catalog=WSActivosFijos;Persist Security Info=False;User ID=adminulacit;Password=Ulacit2019.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            DateTime lastUsed = System.DateTime.Today;

            string query = "DELETE FROM FixedAssets WHERE AssetID = @AssetID";

            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@AssetID", assetId);

                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion
    }
}
