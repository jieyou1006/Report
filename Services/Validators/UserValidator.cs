using FluentValidation;
using Services.Models;

namespace Services
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(it => it.Name)
                .NotNull()
                .Must(v => v.Length > 5).WithMessage("账号长度必须大于5位");

            RuleFor(it => it.Id)
                .NotNull()
                .Must(v => v > 100).WithMessage("Id必须大于100");
        }
    }
}
