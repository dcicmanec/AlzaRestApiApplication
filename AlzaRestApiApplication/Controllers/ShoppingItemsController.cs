using ShoppingItems.Core.BL;
using ShoppingItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AlzaRestApiApplication.Controllers
{
    public class ShoppingItemsController : ApiController
    {
        #region Variable  
        HttpResponseMessage response;
        ShoppingItemsBL shoppingItemsBL;
        #endregion #region Public Method  
        ///<summary>  
        /// This method is used to get Shopping list  
        ///</summary>  
        ///<returns></returns>  
        [HttpGet, ActionName("GetShoppingList")]
        public HttpResponseMessage GetShoppingList(int pageNumber = 0, int pageSize = 0)
        {
            Result result;
            shoppingItemsBL = new ShoppingItemsBL();
            try
            {
                var shoppingList = shoppingItemsBL.GetShoppingList();

                if (!object.Equals(shoppingList, null))
                {
                    var maxPageSize = shoppingList.Count();
                    if (pageNumber != 0 && pageSize != 0)
                    {
                        shoppingList = shoppingList
                                .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
                    }

                    shoppingList[0].MaxPageSize = maxPageSize;

                    response = Request.CreateResponse<List<ShoppingItemsProfile>>(HttpStatusCode.OK, shoppingList);
                }
            }
            catch (Exception ex)
            {
                result = new Result();
                result.Status = 0;
                result.Message = ex.Message;
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, result);
            }
            return response;
        }
        ///<summary>  
        /// This method is used to get Shopping list by id  
        ///</summary>  
        ///<returns></returns>  
        [HttpGet, ActionName("GetShoppingListInfoById")]
        public HttpResponseMessage GetShoppingListInfoById(int ShoppingListId)
        {
            Result result;
            shoppingItemsBL = new ShoppingItemsBL();
            try
            {
                var shoppingList = shoppingItemsBL.GetShoppingItemDetailsById(ShoppingListId);
                if (!object.Equals(shoppingList, null))
                {
                    response = Request.CreateResponse<List<ShoppingItemsProfile>>(HttpStatusCode.OK, shoppingList);
                }
            }
            catch (Exception ex)
            {
                result = new Result();
                result.Status = 0;
                result.Message = ex.Message;
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, result);
            }
            return response;
        }
        ///<summary>  
        /// This method is used to update Shopping item info in the database.  
        ///</summary>  
        ///<param name="ShoppingItem"></param>  
        ///<returns></returns>  
        [HttpPost, ActionName("UpdateShoppingItemInfo")]
        public HttpResponseMessage UpdateShoppingItemInfo(ShoppingItemsProfile ShoppingItem)
        {
            Result ObjResult;
            int result;
            shoppingItemsBL = new ShoppingItemsBL();
            try
            {
                result = shoppingItemsBL.UpdateShoppingItemsInfo(ShoppingItem);

                if (result > 0)
                {
                    if (result == 3)
                    {
                        ObjResult = new Result();
                        ObjResult.Status = result;
                        ObjResult.Message = "Record Updated Successfully!!";
                        response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
                    }
                    else if (result == 2)
                    {
                        ObjResult = new Result();
                        ObjResult.Status = result;
                        ObjResult.Message = "Record does not Exists!!";
                        response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
                    }
                    else
                    {
                        ObjResult = new Result();
                        ObjResult.Status = result;
                        ObjResult.Message = "Record Not Added!!";
                        response = Request.CreateResponse<Result>(HttpStatusCode.OK, ObjResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ObjResult = new Result();
                ObjResult.Status = 0;
                ObjResult.Message = ex.Message;
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ObjResult);
            }
            return response;
        }

    }

}