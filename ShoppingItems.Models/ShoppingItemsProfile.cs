namespace ShoppingItems.Models
{
    public class ShoppingItemsProfile : Result
    {
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string ImgUri
        {
            get;
            set;
        }
        public decimal Price
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }

    }

    public class Result
    {
        public int Status
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }

        public int MaxPageSize
        {
            get;
            set;
        }

    }
}