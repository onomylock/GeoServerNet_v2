namespace Shared.Common.Models.DTO.Base;

public class GenericResponseBase<T> 
{
    public bool IsSucccess { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
    public IEnumerable<ErrorBase> Errors { get; set; }
}