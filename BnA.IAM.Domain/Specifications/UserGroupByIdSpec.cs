using BnA.IAM.Domain.Entities;
using BnA.PM.SharedKernel.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BnA.IAM.Domain.Specifications;

public sealed class UserGroupByIdSpec : Specification<ApplicationRole>
{
    private readonly IEnumerable<Guid> _groupIds;
    public UserGroupByIdSpec(IEnumerable<Guid> groupIds) => _groupIds = groupIds.Distinct().ToList();

    public override Expression<Func<ApplicationRole, bool>> ToExpression() =>
        e => _groupIds.Contains(e.Id);
}
