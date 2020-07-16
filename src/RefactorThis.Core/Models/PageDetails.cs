using System;
namespace RefactorThis.Core.Models
{
    public class PageDetails
    {
        const int MaxPageSize = 100;

        public int CurrentPage { get; set; }

        private int _pageSize;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }

    }
}
