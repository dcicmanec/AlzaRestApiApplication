CREATE TABLE [dbo].[ShoppingItemsProfile](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[ImgUri] [nvarchar](50) NULL,
	[Price] [decimal](10, 2) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_ShoppingItemsProfile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]