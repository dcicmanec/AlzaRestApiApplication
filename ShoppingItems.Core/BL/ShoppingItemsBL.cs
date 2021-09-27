using ShoppingItems.Core.DAL;
using ShoppingItems.Models;
using System;
using System.Collections.Generic;

namespace ShoppingItems.Core.BL
{
    public class ShoppingItemsBL
    {
        ///<summary>  
        /// This method is used to get the Shopping Item list  
        ///</summary>  
        ///<returns></returns>  
        public List<ShoppingItemsProfile> GetShoppingList()
        {
            List<ShoppingItemsProfile> ObjShoppingItems = null;
            try
            {
                ObjShoppingItems = new ShoppingItemsDAL().GetShoppingList();
            }
            catch (Exception)
            {
                throw;
            }
            return ObjShoppingItems;
        }
        ///<summary>  
        /// This method is used to get Shopping list details by id    
        ///</summary>  
        ///<returns></returns>  
        public List<ShoppingItemsProfile> GetShoppingItemDetailsById(int Id)
        {
            List<ShoppingItemsProfile> ObjShoppingListDetails = null;
            try
            {
                ObjShoppingListDetails = new ShoppingItemsDAL().GetShoppingListDetailsById(Id);
            }
            catch (Exception)
            {
                throw;
            }
            return ObjShoppingListDetails;
        }
        ///<summary>  
        /// This method is used to add update Shopping list info  
        ///</summary>  
        ///<param name="shoppingItem"></param>  
        ///<returns></returns>  
        public int UpdateShoppingItemsInfo(ShoppingItemsProfile shoppingItem)
        {
            int result = 0;
            try
            {
                result = new ShoppingItemsDAL().UpdateShoppingItemsInfo(shoppingItem);
            }
            catch (Exception)
            {
                return 0;
            }
            return result;
        }
    }
}
