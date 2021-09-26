using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using ShoppingItems.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace ShoppingItems.Core.DAL
{
    public class ShoppingItemsDAL
    {
        #region Variable  
        ///<summary>  
        /// Specify the Database variable    
        ///</summary>  
        Database objDB;
        ///<summary>  
        /// Specify the static variable    
        ///</summary>  
        static string ConnectionString;
        #endregion
        #region Constructor  
        ///<summary>  
        /// This constructor is used to get the connectionstring from the config file      
        ///</summary>  
        public ShoppingItemsDAL()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["ShoppingItemsConnectionString"].ToString();
        }
        #endregion
        #region Database Method  
        public List<T> ConvertTo<T>(DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return Temp;
            }
            catch
            {
                return Temp;
            }
        }
        public T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString();
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                return obj;
            }
        }
        #endregion
        #region ShoppingItemsProfile  
        ///<summary>  
        /// This method is used to get the Shopping Items data      
        ///</summary>  
        ///<returns></returns>  
        public List<ShoppingItemsProfile> GetShoppingList()
        {
            List<ShoppingItemsProfile> objGetShoppingItems = null;
            objDB = new SqlDatabase(ConnectionString);
            using (DbCommand objcmd = objDB.GetStoredProcCommand("sp_GetShoppingItemsList"))
            {
                try
                {
                    using (DataTable dataTable = objDB.ExecuteDataSet(objcmd).Tables[0])
                    {
                        objGetShoppingItems = ConvertTo<ShoppingItemsProfile>(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                    return null;
                }
            }
            return objGetShoppingItems;
        }

        ///<summary>  
        /// This method is used to get Shopping item details by id    
        ///</summary>  
        ///<returns></returns>  
        public List<ShoppingItemsProfile> GetShoppingListDetailsById(int Id)
        {
            List<ShoppingItemsProfile> objShoppingListDetails = null;
            objDB = new SqlDatabase(ConnectionString);
            using (DbCommand objcmd = objDB.GetStoredProcCommand("sp_GetShoppingItemsDetailsById"))
            {
                try
                {
                    objDB.AddInParameter(objcmd, "@ID", DbType.Int32, Id);
                    using (DataTable dataTable = objDB.ExecuteDataSet(objcmd).Tables[0])
                    {
                        objShoppingListDetails = ConvertTo<ShoppingItemsProfile>(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                    return null;
                }
            }
            return objShoppingListDetails;
        }
        ///<summary>  
        /// This method is used to add update Shopping item info  
        ///</summary>  
        ///<param name="shoppingItem"></param>  
        ///<returns></returns>  
        public int UpdateShoppingItemsInfo(ShoppingItemsProfile shoppingItem)
        {
            int result = 0;
            objDB = new SqlDatabase(ConnectionString);
            using (DbCommand objCMD = objDB.GetStoredProcCommand("sp_UpdateShoppingItemsDetails"))
            {
                objDB.AddInParameter(objCMD, "@Id", DbType.Int32, shoppingItem.Id);
                if (string.IsNullOrEmpty(shoppingItem.Name)) objDB.AddInParameter(objCMD, "@Name", DbType.String, DBNull.Value);
                else objDB.AddInParameter(objCMD, "@Name", DbType.String, shoppingItem.Name);
                if (string.IsNullOrEmpty(shoppingItem.ImgUri)) objDB.AddInParameter(objCMD, "@ImgUri", DbType.String, DBNull.Value);
                else objDB.AddInParameter(objCMD, "@ImgUri", DbType.String, shoppingItem.ImgUri);
                if (shoppingItem.Price == 0) objDB.AddInParameter(objCMD, "@Price", DbType.Decimal, DBNull.Value);
                else objDB.AddInParameter(objCMD, "@Price", DbType.Decimal, shoppingItem.Price);
                if (string.IsNullOrEmpty(shoppingItem.Description)) objDB.AddInParameter(objCMD, "@Descpription", DbType.String, DBNull.Value);
                else objDB.AddInParameter(objCMD, "@Description", DbType.String, shoppingItem.Description);
                objDB.AddInParameter(objCMD, "@Status", DbType.Int32, 3);
                try
                {
                    objDB.ExecuteNonQuery(objCMD);
                    result = Convert.ToInt32(objDB.GetParameterValue(objCMD, "@Status"));
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return result;
        }
        ///<summary>  
        /// This method is used to delete Shopping Item info  
        ///</summary>  
        ///<param name="shoppingItem"></param>  
        ///<returns></returns>  
        public int DeleteShoppingListInfo(ShoppingItemsProfile shoppingList)
        {
            int result = 0;
            objDB = new SqlDatabase(ConnectionString);
            using (DbCommand objCMD = objDB.GetStoredProcCommand("CC_DeleteShoppingItemsProfile"))
            {
                objDB.AddInParameter(objCMD, "@Id", DbType.Int32, shoppingList.Id);
                objDB.AddOutParameter(objCMD, "@Status", DbType.Int16, 0);
                try
                {
                    objDB.ExecuteNonQuery(objCMD);
                    result = Convert.ToInt32(objDB.GetParameterValue(objCMD, "@Status"));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }
        #endregion
    }
}