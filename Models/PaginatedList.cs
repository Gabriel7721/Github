namespace MyEx1.Models
{
    internal class PaginatedList<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public List<News> Items { get; set; }

        //kiểm tra xem có trang trước đó hay không
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        //kiểm tra xem có trang kế tiếp hay không
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}