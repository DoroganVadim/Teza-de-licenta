namespace DoctorApplication.Models.Home
{
    public class PageViewModel
    {
        public int pageNumber { get; private set; }
        public int totalPages { get; private set; }
        public int pageSize { get; private set; }
        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            this.pageSize = pageSize;
            this.pageNumber = pageNumber;
            totalPages = (int)Math.Ceiling(count / (double)this.pageSize);
        }
        public bool HasPreviousPage
        {
            get
            {
                return (pageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (pageNumber < totalPages);
            }
        }
    }
}
