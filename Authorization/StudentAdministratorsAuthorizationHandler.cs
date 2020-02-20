using authorizationRoles.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authorizationRoles.Authorization
{
    public class StudentAdministratorsAuthorizationHandler
                    : AuthorizationHandler<OperationAuthorizationRequirement, Student>
    {
        protected override Task HandleRequirementAsync(
                                              AuthorizationHandlerContext context,
                                     OperationAuthorizationRequirement requirement,
                                     Student resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(Constants.StudentAdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}