using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using LH.Application.Dtos;
using LH.Application.ServiceContract;
using LH.Common;
using LH.Domain.Model;
using LH.Domain.Repositories;
using LH.Repository;

namespace LH.Application.ServiceImp
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public RoleService(IRoleRepository roleRepository, 
            IUserRoleRepository userRoleRepository,
            IRolePermissionRepository rolePermissionRepository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public GetResults<RoleDto> GetRoles(RoleInput input)
        {
            var result = GetDefault<GetResults<RoleDto>>();
            var filterExp = BuildExpression(input);

            var query = _roleRepository.Find(filterExp, r => r.Id, SortOrder.Descending, input.Current, input.Size);

            if (input.UserId > 0)
                query = query.Where(x => x.UserRoles.Any((z => z.UserId == input.UserId)));

            result.Total = _roleRepository.Find(filterExp).Count();
            result.Data = query.Select(roleDto => new RoleDto()
            {
                Id = roleDto.Id,
                Name = roleDto.RoleName,
                Permissions = roleDto.RolePermissions.Select(p=>new BaseEntityDto()
                {
                    Id = p.Permission.Id,
                    Name = p.Permission.Name
                }).ToList()
            }).ToList();

            return result;
        }

        public CreateResult<int> CreateRole(RoleDto roleDto)
        {
            var result = GetDefault<CreateResult<int>>();
            if (IsExisted(roleDto.Name))
            {
                result.Message = "ROLE_NAME_HAS_EXIST";
                return result;
            }

            var role = new Role()
            {
                RoleName = roleDto.Name,
                CreationTime = DateTime.Now
            };

            if (roleDto.Permissions != null && roleDto.Permissions.Any())
            {
                roleDto.Permissions.ForEach(p => role.RolePermissions.Add(new RolePermission()
                {
                    PermissionId = p.Id
                }));
            }

            _roleRepository.Add(role);
            _roleRepository.Commit();
            result.Id = role.Id;
            result.IsCreated = true;
            return result;
        }

        public UpdateResult UpdateRole(RoleDto roleDto)
        {
            var result = GetDefault<UpdateResult>();
            var role = _roleRepository.FindSingle(r => r.Id == roleDto.Id);
            if (role == null)
            {
                result.Message = "ROLE_NOT_EXIST";
                return result;
            }
            if (IsExisted(roleDto.Name))
            {
                result.Message = "ROLE_NAME_HAS_EXIST";
                return result;
            }

            if (roleDto.Permissions != null && roleDto.Permissions.Any())
            {
                var list = role.RolePermissions.ToList();
                foreach (var item in list)
                {
                    if (!roleDto.Permissions.Exists((x => x.Id == item.PermissionId)))
                    {
                        _rolePermissionRepository.Delete(item);
                    }
                }

                foreach (var item in role.RolePermissions)
                {
                    if (!list.Exists(x => x.PermissionId == item.Id))
                    {
                        role.RolePermissions.Add(new RolePermission() { PermissionId = item.Id, RoleId = role.Id });
                    }
                }
            }

            role.RoleName = roleDto.Name;
            _roleRepository.Commit();
            result.IsSaved = true;
            return result;
        }

        public DeleteResult DeleteRole(int roleId)
        {
            var result = GetDefault<DeleteResult>();
            var role = _roleRepository.FindSingle(r => r.Id == roleId);
            if (role != null)
            {
                _roleRepository.Delete(role);
            }

            _roleRepository.Commit();
            result.IsDeleted = true;
            return result;
        }

        private bool IsExisted(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && _roleRepository.Find(x => x.RoleName == name).Any();
        }

        private Expression<Func<Role, bool>> BuildExpression(RoleInput input)
        {
            Expression<Func<Role, bool>> filterExp = x => true;
            if (!string.IsNullOrWhiteSpace(input.Name))
                filterExp = (x => x.RoleName.Contains(input.Name));

            return filterExp;
        }
    }
}