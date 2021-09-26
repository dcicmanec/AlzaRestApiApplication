using System.ComponentModel.DataAnnotations;

namespace AlzaRestApiApplication.Models
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }
        public string ImgUri { get; set; }
        [Required, Range(1, 10000)]
        public string Price { get; set; }
        [StringLength(4000, MinimumLength = 3)]
        public string Description { get; set; }
        public int MaxPageSize { get; set; }
    }

    public class PageViewModel
    {
        public int PageSize { get; set; }
        public int PageNumberNext { get; set; }
        public int PageNumberPrev { get; set; }
    }

    public class DetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}