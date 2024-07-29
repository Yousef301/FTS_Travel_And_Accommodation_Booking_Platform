﻿using TABP.DAL.Enums;

namespace TABP.Web.Services.Interfaces;

public interface IUserContext
{
    Guid Id { get; }
    string Username { get; }
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
    Role Role { get; }
}