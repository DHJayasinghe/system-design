
using BnA.IAM.Domain.Entities;
using BnA.PM.SharedKernel.Specification;
using System;
using System.Linq.Expressions;

namespace BnA.IAM.Domain.Specifications;


public sealed class UserGroupByNameSpec : Specification<ApplicationRole>
{
    public string Name { get; set; }
    public UserGroupByNameSpec(string name) 
    {
        Name = name;
    }

    public override Expression<Func<ApplicationRole, bool>> ToExpression() =>
        e => e.Name == Name;
}

