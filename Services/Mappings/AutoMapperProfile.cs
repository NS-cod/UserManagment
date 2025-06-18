using AutoMapper;
using Models;
using Models.Dtos.Permission;
using Models.Dtos.Role;
using Models.Dtos.RolePermissionDto;
using Models.Dtos.User;
using Models.Dtos.UserRoleDto;
using Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<User, UserDto>()
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
               .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles
                   .Where(ur => ur.IsActive)
                   .Select(ur => ur.Role)))
               .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));

            CreateMap<User, UserSummaryDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.IsEmailVerified, opt => opt.MapFrom(src => src.IsEmailVerified))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Se maneja en el servicio
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.LastLoginDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsLocked, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.FailedLoginAttempts, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore()); // Se maneja por separado

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastLoginDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsLocked, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.FailedLoginAttempts, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            // ============= ROLE MAPPINGS =============

            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions
                    .Where(rp => rp.IsActive)
                    .Select(rp => rp.Permission)))
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.UserRoles
                    .Count(ur => ur.IsActive)));

            CreateMap<Role, RoleSummaryDto>()
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.UserRoles
                    .Count(ur => ur.IsActive)))
                .ForMember(dest => dest.PermissionCount, opt => opt.MapFrom(src => src.RolePermissions
                    .Count(rp => rp.IsActive)));

            CreateMap<CreateRoleDto, Role>()
                .ForMember(dest => dest.IsSystemRole, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

            CreateMap<UpdateRoleDto, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsSystemRole, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

            // ============= PERMISSION MAPPINGS =============

            CreateMap<Permission, PermissionDto>();

            CreateMap<Permission, PermissionSummaryDto>();

            CreateMap<CreatePermissionDto, Permission>()
                .ForMember(dest => dest.IsSystemPermission, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

            CreateMap<UpdatePermissionDto, Permission>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsSystemPermission, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

            // ============= USER ROLE MAPPINGS =============

            CreateMap<UserRole, UserRoleDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            CreateMap<AssignUserRoleDto, UserRole>()
                .ForMember(dest => dest.AssignedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedBy, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            // ============= ROLE PERMISSION MAPPINGS =============

            CreateMap<RolePermission, RolePermissionDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.Permission));

            CreateMap<AssignRolePermissionDto, RolePermission>()
                .ForMember(dest => dest.AssignedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.Permission, opt => opt.Ignore());

            // ============= AUTHENTICATION MAPPINGS =============

            CreateMap<User, LoginResponseDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore());
        
    }
    }
}
