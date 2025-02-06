using Shared.Common.Models.DTO.Base;

namespace MasterServer.Application.Models.Dto.UserGroup;

public class UserGroupReadResultDto : EntityResponseBase
{
    public string Alias { get; set; }
}