using Interview_Test.Infrastructure;
using Interview_Test.Models;
using Interview_Test.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Interview_Test.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InterviewTestDbContext _context;
    public UserRepository(InterviewTestDbContext context)
    {
        _context = context;
    }

    public dynamic GetUsers()
    {
        var users = _context.UserTb
            .AsNoTracking()
            .Select(u => new
            {
                Id = u.Id,
                UserId = u.UserId,
                Username = u.Username,
                FirstName = u.UserProfile.FirstName,
                LastName = u.UserProfile.LastName,
                Age = u.UserProfile.Age,
                Roles = u.UserRoleMappings
                    .Select(urm => new
                    {
                        RoleId = urm.Role.RoleId
                    })
                    .ToList(),
                Permissions = u.UserRoleMappings
                    .SelectMany(urm => urm.Role.Permissions)
                    .Select(p => p.Permission)
                    .ToList()
            })
            .ToList();

        if (!users.Any())
            return null;

        var result = users.Select(u => new
        {
            u.Id,
            u.UserId,
            u.Username,
            u.FirstName,
            u.LastName,
            u.Age,
            Roles = u.Roles
                .Distinct()
                .Count(),
            Permissions = u.Permissions
                .Distinct()
                .Count()
        })
        .ToList();

        return result;
    }

    public dynamic GetUserById(string id)
    {
        var user = _context.UserTb
        .AsNoTracking()
        .Where(u => u.UserId == id)
        .Select(u => new
        {
            Id = u.Id,
            UserId = u.UserId,
            Username = u.Username,
            FirstName = u.UserProfile.FirstName,
            LastName = u.UserProfile.LastName,
            Age = u.UserProfile.Age,
            Roles = u.UserRoleMappings
                .Select(urm => new
                {
                    RoleId = urm.Role.RoleId,
                    RoleName = urm.Role.RoleName
                })
                .ToList(),
            Permissions = u.UserRoleMappings
                .SelectMany(urm => urm.Role.Permissions)
                .Select(p => p.Permission)
                .ToList()
        })
        .FirstOrDefault();

        if (user == null)
            return null;

        var permissions = user.Permissions
            .Distinct()
            .OrderBy(p => p)
            .ToList();


        return new
        {
            Id = user.Id.ToString().ToUpper(),
            UserId = user.UserId,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Age = user.Age,
            Roles = user.Roles,
            Permissions = permissions
        };
    }

    public int CreateUser()
    {
        var users = Data.Users;

        var roleCache = new Dictionary<int, RoleModel>();
        var existUserId = _context.UserTb.Select(u => u.UserId).ToHashSet();

        foreach (var user in users)
        {
            if (existUserId.Contains(user.UserId))
                continue;

            var newUser = new UserModel
            {
                Id = user.Id,
                UserId = user.UserId,
                Username = user.Username,
                UserProfile = new UserProfileModel
                {
                    FirstName = user.UserProfile.FirstName,
                    LastName = user.UserProfile.LastName,
                    Age = user.UserProfile.Age
                }
            };

            var mappings = new List<UserRoleMappingModel>();

            foreach (var mapping in user.UserRoleMappings)
            {
                var role = mapping.Role;

                if (!roleCache.TryGetValue(role.RoleId, out var dbRole))
                {
                    dbRole = _context.RoleTb.FirstOrDefault(r => r.RoleName == role.RoleName);
                    if (dbRole == null)
                    {
                        dbRole = new RoleModel
                        {
                            RoleId = role.RoleId,
                            RoleName = role.RoleName,
                            Permissions = role.Permissions
                                .GroupBy(p => p.Permission)
                                .Select(g => new PermissionModel
                                {
                                    Permission = g.Key
                                })
                                .ToList()
                        };
                        _context.RoleTb.Add(dbRole);
                    }
                    roleCache[role.RoleId] = dbRole;
                }

                mappings.Add(new UserRoleMappingModel
                {
                    User = newUser,
                    Role = dbRole
                });
            }

            newUser.UserRoleMappings = mappings;
            _context.UserTb.Add(newUser);

            existUserId.Add(user.UserId);
        }
        return _context.SaveChanges();
    }
}