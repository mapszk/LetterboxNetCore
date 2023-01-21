namespace LetterboxNetCore.DTOs
{
    public class PaginationDTO<T> where T : class
    {
        public PaginationDTO(List<T> data, int totalCount, int pages)
        {
            this.Data = data;
            this.TotalCount = totalCount;
            this.Pages = pages;
        }

        public int TotalCount { get; set; }
        public int Pages { get; set; }
        public List<T> Data { get; set; }
    }
}