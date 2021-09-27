-- === === === === === === === === === === === === === === ===  
--Author: Nimit Joshi  
    --Create date: 31 / 01 / 2016  
    --Description: This sp is used to insert and update data in Cricketer Profile  
    -- === === === === === === === === === === === === === === ===  
    CREATE PROCEDURE [dbo].[sp_UpdateShoppingItemsDetails]  
    --Add the parameters  for the stored procedure here  
@Id INT = NULL,  
@Name VARCHAR(50) = NULL,  
@ImgUri VARCHAR(50) = NULL,  
@Price Decimal = NULL,  
@Description VARCHAR(MAX) = NULL,   
@Status INT OUT  
AS  
BEGIN  
--SET NOCOUNT ON added to prevent extra result sets from  
--interfering with SELECT statements.  
  
IF EXISTS(SELECT ccp.ID FROM [dbo].[ShoppingItemsProfile] ccp(NOLOCK) WHERE ccp.ID = @Id)  
BEGIN  
UPDATE [dbo].[ShoppingItemsProfile]  
SET Name = ISNULL(@Name, Name),  
    ImgUri = ISNULL(@ImgUri, ImgUri),  
    Price = ISNULL(@Price, Price),  
    Description = ISNULL(@Description, Description)  
WHERE ID = @Id;  
IF(@@ROWCOUNT > 0)  
BEGIN  
SET @Status = 3;  
--Record Updated Successfully!!  
    END;  
END;  
ELSE  
BEGIN  
SET @Status = 4;  
--Record does not exists!!  
    END;  
END