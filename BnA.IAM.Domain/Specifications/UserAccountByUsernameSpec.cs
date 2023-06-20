using BnA.IAM.Domain.Entities;
using BnA.PM.SharedKernel.Specification;
using System;
using System.Linq.Expressions;

namespace BnA.IAM.Domain.Specifications;

public sealed class UserAccountByUsernameSpec : Specification<ApplicationUser>
{
    private readonly string _username;

    public UserAccountByUsernameSpec(string username) =>
        _username = username.Trim();

    public override Expression<Func<ApplicationUser, bool>> ToExpression() =>
        e => e.UserName == _username;
}
