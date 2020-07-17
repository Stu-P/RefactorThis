using System;
using Newtonsoft.Json;

namespace RefactorThis.Core.Models
{
    public class PagingRequest
    {
        private int _pageNumber = 0;
        public int PageNumber
        {
            get { return _pageNumber; }

            set
            { _pageNumber = value - 1; }
        }


        public int PageSize { get; set; } = 100;


        public int StartingPosition()
        {
            return PageSize * PageNumber;
        }
    }
}
