using Micro.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Micro.Authentication
{
    public class UserRoleProvider : RoleProvider
    {
        private AppDbContext _appDbContext;
        public UserRoleProvider()
        {
            _appDbContext = new AppDbContext();
        }

        public override string ApplicationName
        { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {

            var result=(from users in _appDbContext.Users
                    join
rolemapper in _appDbContext.RoleMappings on users.UserId equals rolemapper.UserId
                    join
                    roles in _appDbContext.Roles on rolemapper.RoleId equals roles.RoleId
                    where users.Username == username
                    select roles.RoleName).ToArray();
            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}