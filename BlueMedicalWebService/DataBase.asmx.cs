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

        [WebMethod]
        public string ValidacionNumTarjeta (string Num_Tarjeta, string Mes_Exp, string Anio_Exp, string CVV) 
        {
            conn.ConnectionString = "Data Source= localhost;Initial Catalog=dbEfoodMetodosPago;Trusted_Connection=True";

            string query = "SELECT COUNT(Num_Tarjeta) FROM DBO.tbInfoTarjetas WHERE Num_Tarjeta = '" + Num_Tarjeta + "'AND Mes_Exp = " + Mes_Exp + " AND Anio_Exp = " + Anio_Exp + " AND CVV = " + CVV + ";";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);

            var validar = ds.Tables[0].Rows[0][0].ToString();  

            if (validar == "1")
            {
                return "0 - Tarjeta Existente";
            }
            else
            {
                return "1 - Tarjeta No Existente";
            }
        }

        [WebMethod]
        public string ValidacionFecha(string Num_Tarjeta, int Mes_Exp, int Anio_Exp)
        {
            conn.ConnectionString = "Data Source= localhost;Initial Catalog=dbEfoodMetodosPago;Trusted_Connection=True";

            string query = "SELECT COUNT(Num_Tarjeta) FROM DBO.tbInfoTarjetas WHERE Num_Tarjeta = '" + Num_Tarjeta + "'AND Mes_Exp = " + Mes_Exp + " AND Anio_Exp = " + Anio_Exp + ";";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);

            var validar = ds.Tables[0].Rows[0][0].ToString();

            if (validar == "1")
            {
                int mes = 8;
                int year = 20;

                if (Anio_Exp >= year) 
                {
                    if (Mes_Exp >= mes)
                    {
                        return "0 - Tarjeta Existente (Fecha)";
                    }
                    else 
                    {
                        return "2 - Tarjeta Vencida";
                    }
                } else 
                {
                    return "2 - Tarjeta Vencida";
                }
            }
            else
            {
                return "2 - Tarjeta No Encontrada";
            }
        }


        [WebMethod]
        public string ValidacionCVV(string Num_Tarjeta, string CVV)
        {
            conn.ConnectionString = "Data Source= localhost;Initial Catalog=dbEfoodMetodosPago;Trusted_Connection=True";

            string query = "SELECT COUNT(Num_Tarjeta) FROM DBO.tbInfoTarjetas WHERE Num_Tarjeta = '" + Num_Tarjeta + " AND CVV = " + CVV + ";";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);

            var validar = ds.Tables[0].Rows[0][0].ToString();

            if (validar == "1")
            {
                return "0 - Tarjeta Existente (CVV)";
            }
            else
            {
                return "3 - CVV Incorrecto";
            }
        }

        [WebMethod]
        public string ValidacionTipo(string Tipo)
        {
            conn.ConnectionString = "Data Source= localhost;Initial Catalog=dbEfoodMetodosPago;Trusted_Connection=True";

            string query = "SELECT COUNT(Tipo) FROM DBO.tbInfoTarjetas WHERE Tipo = " + Tipo + ";";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();

            da.Fill(ds);

            var validar = ds.Tables[0].Rows[0][0].ToString();

            if (validar == "1")
            {
                return "0 - Tarjeta Existente (Tipo)";
            }
            else
            {
                return "4 - Tipo de Tarjeta No Existente";
            }
        }

        [WebMethod]
        public string validacionMonto(string Num_Tarjeta, string Monto)
        {
            conn.ConnectionString = "Data Source= localhost;Initial Catalog=dbEfoodMetodosPago;Trusted_Connection=True";

            string query = "SELECT COUNT(Monto) FROM DBO.tbInfoTarjetas WHERE Num_Tarjeta = " + Num_Tarjeta + " AND Monto > " + Monto + ";";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);

            DataSet ds = new DataSet();

            da.Fill(ds);

            var validar = ds.Tables[0].Rows[0][0].ToString();

            if (validar == "1")
            {
                string montoDB = "SELECT Monto FROM DBO.tbInfoTarjetas WHERE Num_Tarjeta = '" + Num_Tarjeta + "';";
                SqlDataAdapter daMonto = new SqlDataAdapter(montoDB, conn);
                DataSet dsMonto = new DataSet();
                daMonto.Fill(dsMonto);
                var montoString = dsMonto.Tables[0].Rows[0][0].ToString();

                float monto = float.Parse(montoString);
                float montoDescontado = float.Parse(Monto);

                float total = monto - montoDescontado;

                string update = "UPDATE dbo.tbInfoTarjetas SET Monto = " + total + "WHERE Num_Tarjeta = '" + Num_Tarjeta + "';";

                using (SqlCommand cmd = new SqlCommand(update))
                {
                    cmd.Parameters.AddWithValue("@Monto", total);

                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                return "Total: " + total;
            }
            else
            {
                return "5 - No Hay Fondos Suficiente";
            }
        }


        #region Gets
        [WebMethod]
        public Boolean ValidationUser(string username, string password)
        {
            conn.ConnectionString = "Data Source= localhost;Initial Catalog=dbEfoodMetodosPago;Trusted_Connection=True";

            string query = "SELECT from dbo.tbConsecutivos where Username='"
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
        public DataSet GetConsecutivos()
        {
            conn.ConnectionString = "Data Source= localhost;Initial Catalog=dbEfoodMetodosPago;Trusted_Connection=True";


            string query = "SELECT * FROM dbo.tbConsecutivos";

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
