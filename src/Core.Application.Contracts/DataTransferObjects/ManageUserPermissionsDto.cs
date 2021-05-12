using Core.Application.Contracts.Response;

namespace Core.Application.Contracts.DataTransferObjects
{
    public class ManageUserPermissionsDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PermissionValue { get; set; }
        public PaginatedList<ManageClaimDto> ManagePermissionsDto { get; set; }
    }

    
}
