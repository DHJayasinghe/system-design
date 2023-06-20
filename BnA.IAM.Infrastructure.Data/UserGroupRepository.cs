using BnA.IAM.Application.Common.Interfaces;
using BnA.IAM.Domain.Entities;
using BnA.PM.Persistence;
using BnA.PM.SharedKernel;
using BnA.PM.SharedKernel.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BnA.IAM.Infrastructure.Data;

public sealed class UserGroupRepository : IUserGroupRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserGroupRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<int> CountAsync(Specification<ApplicationRole> specification, CancellationToken cancellationToken = default) =>
        await _dbContext.Roles.CountAsync(specification.ToExpression(), cancellationToken);

    public async Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.Roles.CountAsync(cancellationToken);

    public async Task<Maybe<ApplicationRole>> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        if (id.Equals(default(TId))) return null;

        return await _dbContext.Roles
            .SingleOrDefaultAsync(d => id.Equals(d.Id), cancellationToken);
    }

    public async Task<List<ApplicationRole>> ListAsync(Specification<ApplicationRole> specification, CancellationToken cancellationToken = default) =>
        await _dbContext.Roles
            .Where(specification.ToExpression())
            .ToListAsync(cancellationToken);

    public async Task<ApplicationRole> SingleAsync(Specification<ApplicationRole> specification, CancellationToken cancellationToken = default) =>
        await _dbContext.Roles.Where(specification.ToExpression()).SingleOrDefaultAsync(cancellationToken);
}
