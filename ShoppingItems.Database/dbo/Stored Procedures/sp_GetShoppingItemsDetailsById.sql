CREATE Proc [dbo].[sp_GetShoppingItemsDetailsById]  
@ID int  
AS  
Begin  
select * from ShoppingItemsProfile(NOLOCK) where ID = @Id  
End