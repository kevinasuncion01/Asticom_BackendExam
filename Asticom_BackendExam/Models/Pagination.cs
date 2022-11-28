namespace Asticom_BackendExam.Models
{
    public class Pagination
    {
        private const int _maxPageSize = 50;
        private const int _defaultSize = 10;
        private int _pageSize = 0;

        public int PageNumber { get; set; } = 1;
        public int PageSize {
            get { return _pageSize; }
            set
            {
                if (value < _defaultSize) _pageSize = _defaultSize;
                else if (value > _maxPageSize) _pageSize = _maxPageSize;
                else _pageSize = value;
            }
        }
    }
    public class PaginationRequest: Pagination
    {

    }

    public class PaginationResponse: Pagination
    {
        public int TotalItems { get; set; }
    }
}
