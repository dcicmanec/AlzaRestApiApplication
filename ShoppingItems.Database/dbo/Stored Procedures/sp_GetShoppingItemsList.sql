CREATE Proc[dbo].[sp_GetShoppingItemsList]  
AS  
Begin  
select ID, Name, ImgUri, Price, Description from ShoppingItemsProfile(NOLOCK)  
End