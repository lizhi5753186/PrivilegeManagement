using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using LH.Application.Dtos;
using LH.Application.ServiceContract;
using LH.Domain.Model;
using LH.Domain.Repositories;

namespace LH.Application.ServiceImp
{
    public class PermissionService : BaseService, IPermissionService
    {

        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public PermissionService(IPermissionRepository permissionRepository,
            IRolePermissionRepository rolePermissionRepository)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public GetResults<BaseEntityDto> GetPermissions(PageInput input)
        {
            var result = GetDefault<GetResults<BaseEntityDto>>();
            Expression<Func<Permission, bool>> filterExp = x => true;
            if (!string.IsNullOrWhiteSpace(input.Name))
                filterExp = p => p.Name.Contains(input.Name);

            var query = _permissionRepository.Find(filterExp, p => p.Id, SortOrder.Descending, input.Current, input.Size);

            result.Total = _permissionRepository.Find(filterExp).Count();
            result.Data = query.Select(x => new BaseEntityDto()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return result;
        }

        public UpdateResult UpdatePermission(BaseEntityDto permissionDto)
        {
            var result = GetDefault<UpdateResult>();
            var permission = _permissionRepository.FindSingle(x => x.Id == permissionDto.Id);
            if (permission == null)
            {
                result.Message = "FUNC_NOT_EXIST";
                return result;
            }
            if (IsExisted(permissionDto.Name))
            {
                result.Message = "FUNC_NAME_HAS_EXIST";
                return result;
            }

            permission.Name = permissionDto.Name;
            _permissionRepository.Commit();
            result.IsSaved = true;
            return result;
        }

        public CreateResult<int> CreatePermission(BaseEntityDto permissionDto)
        {
            var result = GetDefault<CreateResult<int>>();
            if (IsExisted(permissionDto.Name))
            {
                result.Message = "FUNC_NAME_HAS_EXIST";
                return result;
            }
            var permission = new Permission()
            {
                Name = permissionDto.Name
            };

            _permissionRepository.Add(permission);
            _permissionRepository.Commit();
            result.IsCreated = true;
            result.Id = permission.Id;
            return result;
        }

        public DeleteResult DeletePermission(int actionId)
        {
            var result = GetDefault<DeleteResult>();
            var permission = _permissionRepository.FindSingle(p => p.Id == actionId);
            if (permission != null)
            {
                _rolePermissionRepository.Delete(permission.RolePermissions);
                _permissionRepository.Delete(permission);
            }

            _permissionRepository.Commit();
            result.IsDeleted = true;
            return result;
        }

        private bool IsExisted(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && _permissionRepository.Find(x => x.Name == name).Any();
        }
    }
}