using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Business.Requests;

public record UserRegisterRequest(string FirstName,
    string LastName,
    string Email,
    string Password
);
public record AssignLecturerRequest(string Email);
public record UserLoginRequest(string Email, string Password);   
