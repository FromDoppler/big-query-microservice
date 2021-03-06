using Dapper;
using Doppler.BigQueryMicroservice.Entitites;
using Doppler.BigQueryMicroservice.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Repository.Implementation
{
    /// <summary>
    /// UserAccessByUserrepository for access to sql database
    /// </summary>
    public class UserAccessByUserRepository : BaseRepository<UserAccessByUser>, IUserAccessByUserRepository
    {
        public UserAccessByUserRepository(IDatabaseConnectionFactory connectionFactory, ILogger<UserAccessByUser> bigQueryLogger) : base(connectionFactory, bigQueryLogger)
        {
            TableName = "[datastudio].[UserAccessByUser]";
        }

        #region IUserAccessByUserRepository methods implementations
        public async Task<IReadOnlyList<UserAccessByUser>> GetAllByUserIdAsync(int id)
        {
            var builder = new SqlBuilder();
            builder.Select("*").
                Where($"IdUser = @Id and validTo > GETUTCDATE()");

            var builderTemplate = builder.AddTemplate("Select /**select**/ from datastudio.UserAccessByUser /**where**/");

            using (var connection = await base.CreateConnectionAsync())
            {
                try
                {
                    var result = await connection.QueryAsync<UserAccessByUser>(builderTemplate.RawSql, new { Id = id });
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    base.BigQueryLogger.LogError(ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<MergeEmailResult> MergeEmailsAsync(int userId, List<string> emails)
        {
            using (var connection = await base.CreateConnectionAsync())
            {
                try
                {
                    var dt = new DataTable("[dbo].[TypeEmail]");
                    dt.Columns.Add("Email", typeof(string));

                    foreach (var email in emails)
                    {
                        dt.Rows.Add(email);
                    }

                    dt.SetTypeName("[dbo].[TypeEmail]");
                    var actions = new[] { "C", "U" };
                    string sql = "exec [dbo].[SaveEmailsByAccounName] @Emails, @UserId";
                    var data = await connection.QueryAsync<MergeResponse>(sql, new { Emails = dt, UserId = userId });
                    var result = new MergeEmailResult(data.Where(a => actions.Contains(a.Action)).Select(e => e.Email).ToList());
                    return result;
                }
                catch (Exception ex)
                {
                    base.BigQueryLogger.LogError(ex, "Error merging emails for user: {userId}", userId);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        #endregion
    }
}
