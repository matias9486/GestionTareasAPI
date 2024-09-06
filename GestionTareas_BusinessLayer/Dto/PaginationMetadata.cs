namespace GestionTareas_BusinessLayer.Dto
{
    public class PaginationMetadata
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public bool HasNext { get; set; }
        public string? NextPageUrl { get; set; }
        public bool HasPrevious { get; set; }
        public string? PreviousPageUrl { get; set; }
    }
}
