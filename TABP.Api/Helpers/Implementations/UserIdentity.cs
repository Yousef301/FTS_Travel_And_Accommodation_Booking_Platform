﻿using System.Security.Claims;
using TABP.Shared.Enums;
using TABP.Web.Helpers.Interfaces;

namespace TABP.Web.Helpers.Implementations;

public class UserIdentity : IUserIdentity
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdentity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetClaimValue(string claimType)
    {
        var claimValue = _httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);
        if (claimValue == null)
        {
            throw new UnauthorizedAccessException();
        }

        return claimValue;
    }

    public Guid Id => Guid.Parse(GetClaimValue("id"));

    public string Username => GetClaimValue("username");

    public string FirstName => GetClaimValue("firstName");

    public string LastName => GetClaimValue("lastName");

    public string Email => GetClaimValue(ClaimTypes.Email);

    public Role Role
    {
        get
        {
            var roleClaim = GetClaimValue(ClaimTypes.Role);
            if (!Enum.TryParse(roleClaim, out Role role))
            {
                throw new UnauthorizedAccessException();
            }

            return role;
        }
    }
}