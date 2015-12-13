using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using LH.Application.Dtos;
using LH.Application.ServiceContract;
using LH.Domain.Model;
using LH.Domain.Repositories;
using LH.Repository;
using Microsoft.SqlServer.Server;

namespace LH.Application.ServiceImp
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public GetResults<UserDto> GetUsers(PageInput input)
        {
            var result = GetDefault<GetResults<UserDto>>();
            var filterExp = BuildExpression(input);
            var query = _userRepository.Find(filterExp, user => user.Id, SortOrder.Descending, input.Current, input.Size);
            result.Total = _userRepository.Find(filterExp).Count();
            result.Data = query.Select(user => new UserDto()
            {
                Id = user.Id,
                CreateTime = user.CreationTime,
                Email = user.Email,
                State = user.State,
                Name = user.Name,
                RealName = user.RealName,
                Password = "*******",
                Roles = user.UserRoles.Take(4).Select(z => new BaseEntityDto()
                {
                    Id = z.Role.Id,
                    Name = z.Role.RoleName
                }).ToList(),

                TotalRole = user.UserRoles.Count()
            }).ToList();

            return result;
        }

        public UpdateResult UpdateUser(UserDto user)
        {
            var result = GetDefault<UpdateResult>();
            var existUser = _userRepository.FindSingle(u => u.Id == user.Id);
            if (existUser == null)
            {
                result.Message = "USER_NOT_EXIST";
                result.StateCode = 0x00303;
                return result;
            }
            if (IsHasSameName(existUser.Name, existUser.Id))
            {
                result.Message = "USER_NAME_HAS_EXIST";
                result.StateCode = 0x00302;
                return result;
            }

            existUser.RealName = user.RealName;
            existUser.Name = user.Name;
            existUser.State = user.State;
            existUser.Email = user.Email;
            _userRepository.Commit();
            result.IsSaved = true;
            return result;
        }

        public CreateResult<int> AddUser(UserDto userDto)
        {
            var result = GetDefault<CreateResult<int>>();
            if (IsHasSameName(userDto.Name, userDto.Id))
            {
                result.Message = "USER_NAME_HAS_EXIST";
                result.StateCode = 0x00302;
                return result;
            }
            var user = new User()
            {
                CreationTime = DateTime.Now,
                Password = "",
                Email = userDto.Email,
                State = userDto.State,
                RealName = userDto.RealName,
                Name = userDto.Name
            };

            _userRepository.Add(user);
            _userRepository.Commit();
            result.Id = user.Id;
            result.IsCreated = true;
            return result;
        }

        public DeleteResult DeleteUser(int userId)
        {
            var result = GetDefault<DeleteResult>();
            var user = _userRepository.FindSingle(x => x.Id == userId);
            if (user != null)
            {
                _userRepository.Delete(user);
                _userRepository.Commit();
            }
            result.IsDeleted = true;
            return result;
        }

        public UpdateResult UpdatePwd(UserDto user)
        {
            var result = GetDefault<UpdateResult>();
            var userEntity =_userRepository.FindSingle(x => x.Id == user.Id);
            if (userEntity == null)
            {
                result.Message = string.Format("当前编辑的用户“{0}”已经不存在", user.Name);
                return result;
            }
            userEntity.Password = user.Password;
            _userRepository.Commit();
            result.IsSaved = true;
            return result;
        }

        public GetResult<UserDto> GetUser(int userId)
        {
            var result = GetDefault<GetResult<UserDto>>();
            var model = _userRepository.FindSingle(x => x.Id == userId);
            if (model == null)
            {
                result.Message = "USE_NOT_EXIST";
                result.StateCode = 0x00402;
                return result;
            }
            result.Data = new UserDto()
            {
                CreateTime = model.CreationTime,
                Email = model.Email,
                Id = model.Id,
                RealName = model.RealName,
                State = model.State,
                Name = model.Name,
                Password = "*******"
            };
            return result;
        }

        public UpdateResult UpdateRoles(UserDto user)
        {
            var result = GetDefault<UpdateResult>();
            var model = _userRepository.FindSingle(x => x.Id == user.Id);
            if (model == null)
            {
                result.Message = "USE_NOT_EXIST";
                result.StateCode = 0x00402;
                return result;
            }

            var list = model.UserRoles.ToList();
            if (user.Roles != null)
            {
                foreach (var item in user.Roles)
                {
                    if (!list.Exists(x => x.Role.Id == item.Id))
                    {
                        _userRoleRepository.Add(new UserRole { RoleId = item.Id, UserId = model.Id });
                    }
                }

                foreach (var item in list)
                {
                    if (!user.Roles.Exists(x => x.Id == item.Id))
                    {
                        _userRoleRepository.Delete(item);
                    }
                }

                _userRoleRepository.Commit();
                _userRepository.Commit();
            }

            result.IsSaved = true;
            return result;
        }

        public DeleteResult DeleteRole(int userId, int roleId)
        {
            var result = GetDefault<DeleteResult>();
            var model = _userRoleRepository.FindSingle(x => x.UserId == userId && x.RoleId == roleId);
            if (model != null)
            {
                _userRoleRepository.Delete(model);
                _userRoleRepository.Commit();
            }

            result.IsDeleted = true;
            return result;
        }

        public bool Exist(string username, string password)
        {
            return _userRepository.FindSingle(u => u.Name == username && u.Password == password) != null;
        }

        private bool IsHasSameName(string name, int userId)
        {
            return !string.IsNullOrWhiteSpace(name) && _userRepository.Find(u=>u.Name ==name && u.Id != userId).Any();
        }

        private Expression<Func<User, bool>> BuildExpression(PageInput pageInput)
        {
            Expression<Func<User, bool>> filterExp = user => true;
            if (string.IsNullOrWhiteSpace(pageInput.Name))
                return filterExp;
            
            switch (pageInput.Type)
            {
                case 0:
                    filterExp = user => user.Name.Contains(pageInput.Name) || user.Email.Contains(pageInput.Name);
                    break;
                case 1:
                    filterExp = user => user.Name.Contains(pageInput.Name);
                    break;
                case 2:
                    filterExp = user => user.Email.Contains(pageInput.Name);
                    break;
            }

            return filterExp;
        }
    }
}