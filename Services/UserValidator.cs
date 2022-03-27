using FluentValidation;
using Services.Models;

namespace Services
{
    internal class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(it => it.Name)
                .NotNull()
                .Must(v => v.Length > 5).WithMessage("账号长度必须大于5位");
        }
    }
}
