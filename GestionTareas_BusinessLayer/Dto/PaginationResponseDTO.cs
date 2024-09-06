namespace GestionTareas_BusinessLayer.Dto
{
    public class PaginationResponseDTO<T>
    {
        public List<T> Data { get; set; }
        public PaginationMetadata Pagination { get; set; }
    }
}
