﻿using MediatR;

namespace TABP.Application.Commands.Users.Register;

public class RegisterCommand : IRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateOnly BirthDate { get; set; }
}