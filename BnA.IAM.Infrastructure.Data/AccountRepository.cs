using BnA.IAM.Application.Common.Interfaces;
using BnA.IAM.Domain.Entities;
using BnA.PM.Persistence;
using BnA.PM.Persistence.TableSplittings;
using BnA.PM.SharedKernel;
using BnA.PM.SharedKernel.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BnA.IAM.Infrastructure.Data;

public sealed class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _dbContext;
    public AccountRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<int> CountAsync(Specification<ApplicationUser> specification, CancellationToken cancellationToken = default) =>
        await _dbContext.Users.CountAsync(specification.ToExpression(), cancellationToken);

    public async Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.Users.CountAsync(cancellationToken);

    public async Task<Maybe<ApplicationUser>> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull =>
        !id.Equals(default(TId))
            ? await _dbContext.Set<ApplicationUser>()
                .Include(e => e.Roles)
                    .ThenInclude(e => e.ApplicationRole)
                .Include(e => e.PreferredCountry)
            .SingleOrDefaultAsync(d => id.Equals(d.Id), cancellationToken)
        : null;

    public async Task<Maybe<ApplicationUser>> GetByExternalIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        var sqlQuery = $"SELECT TOP 1 B.ApplicationUserId FROM dbo.BusinessUsers B WHERE B.ExternalRef='{id}'";

        var userId = _dbContext.RawSqlQuery(sqlQuery, account => account[0]).FirstOrDefault();

        return !userId.Equals(default(TId))
            ? await _dbContext.Set<ApplicationUser>()
                .Include(e => e.Roles)
                .ThenInclude(e => e.ApplicationRole)
                .Include(e => e.PreferredCountry)
                .SingleOrDefaultAsync(d => Guid.Parse(userId.ToString()).Equals(d.Id), cancellationToken)
            : null;
    }


    public async Task<List<ApplicationUser>> ListAsync(Specification<ApplicationUser> specification, CancellationToken cancellationToken = default) =>
        await _dbContext.Users
            .Include(e => e.Roles)
            .Where(specification.ToExpression())
            .ToListAsync(cancellationToken);

    public async Task<Maybe<UserAccount>> FindByExternalRef(string externalOrganizationRef = null, string externalUserRef = null, Guid? userId = null, Guid? organizationId = null)
    {
        if ((string.IsNullOrEmpty(externalUserRef) && !userId.HasValue) || (string.IsNullOrEmpty(externalOrganizationRef) && !organizationId.HasValue)) return null;

        var sqlQuery = $"SELECT TOP 1 U.Id,Org.Id OrganizationId, B.Id UserReference FROM dbo.AspNetUsers U INNER JOIN dbo.BusinessUsers B " +
                "ON U.Id=B.ApplicationUserId INNER JOIN dbo.Organizations Org " +
                (organizationId.HasValue
                    ? $"ON B.OrganizationId=Org.Id AND Org.Id='{organizationId}'"
                    : $"ON B.OrganizationId=Org.Id AND Org.ExternalId='{externalOrganizationRef}' ") +
                (userId.HasValue
                    ? $"AND B.ApplicationUserId='{userId.Value}'"
                    : $"AND B.ExternalRef='{externalUserRef}'");
        var result = _dbContext
            .RawSqlQuery(sqlQuery, account => new { Id = (string)account[0], OrganizationId = (Guid)account[1], UserReference = (Guid)account[2] })
            .FirstOrDefault();
        if (result is null) return null;

        return await Task.FromResult(new UserAccount(Guid.Parse(result.Id), result.OrganizationId, result.UserReference));
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.SaveChangesAsync(cancellationToken);

    public void Add(ApplicationUser entity)
    {
        SplittedApplicationUser applicationUser = new SplittedApplicationUser(entity);
        _dbContext.Set<SplittedApplicationUser>().Add(applicationUser);
    
    }
    public void Remove(ApplicationUser entity)
    {
        SplittedApplicationUser applicationUser = _dbContext.Set<SplittedApplicationUser>().Find(entity.Id);
        _dbContext.Set<SplittedApplicationUser>().Remove(applicationUser);
    }

    public async Task<ApplicationUser> SingleAsync(Specification<ApplicationUser> specification, CancellationToken cancellationToken = default) =>
       await _dbContext.Users.Where(specification.ToExpression()).SingleOrDefaultAsync(cancellationToken);
}


internal static class AccountRepositoryHelper
{
    public static List<T> RawSqlQuery<T>(this ApplicationDbContext context, string query, Func<DbDataReader, T> map)
    {
        using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;

        context.Database.OpenConnection();

        using var result = command.ExecuteReader();
        var entities = new List<T>();

        while (result.Read())
        {
            entities.Add(map(result));
        }

        return entities;
    }
}