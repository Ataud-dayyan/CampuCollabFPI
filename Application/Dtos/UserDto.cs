using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record UserDto(
        string Id,
        string Email,
        string UserName);

    public record CreateUserDto(
        string Email,
        string UserName,
        string Password
        );
    public record ChangePasswordDto(
       string Email,
       string UserName,
       string Password
       );
}
