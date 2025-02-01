namespace Shared.Common.Models.DTO.Base;

public class PaginationResponseBase<T> : GenericResponseBase<T> where T : EntityResponseBase
{
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}