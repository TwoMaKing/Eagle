using Dappers;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Eagle.Web.Security
{
    public abstract class AccountDbProvider
    {
        private const string SQL_INSERT_MEMBERSHIP = @"INSERT INTO [webapp_membership]
                                                    ( [UserId],
                                                      [Email],
                                                      [CellPhoneNo],
                                                      [Password],
                                                      [PasswordQuestion],
                                                      [PasswordAnswer],
                                                      [ConfirmationToken],
                                                      [IsApproved],
                                                      [LastActivityDate],
                                                      [LastLoginDate],
                                                      [LastPasswordChangedDate],
                                                      [CreationDate],
                                                      [IsLockedOut],
                                                      [LastLockedOutDate],
                                                      [FailedPasswordAttemptCount],
                                                      [FailedPasswordAttemptWindowStart],
                                                      [PasswordVerificationToken],
                                                      [PasswordVerificationTokenExpirationDate])
                                                      VALUES (@UserId, @Email, @CellPhoneNo, @Password, 
                                                              @PasswordQuestion, @PasswordAnswer, @ConfirmationToken, @IsApproved, 
                                                              {0}, {0}, NULL, {0}, 0, NULL, NULL, NULL, NULL, NULL);";

        protected AccountDbProvider(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.UserInsertSql = this.ReplaceDatabaseTokens(string.Format("INSERT INTO [WEBAPP_USERS] ([NAME], [LASTACTIVITYDATE]) VALUES (@Name, {0}); SELECT {1} AS NewUserId;", this.CurrentDateSqlStatement, this.SelectLastInsertedRowAutoIDStatement));
            this.MembershipInsertSql = this.ReplaceDatabaseTokens(string.Format(SQL_INSERT_MEMBERSHIP, this.CurrentDateSqlStatement));
            this.LastActivityDateUpdateSql = this.ReplaceDatabaseTokens(string.Format("UPDATE [WEBAPP_USERS] SET [LastActivityDate] = {0} WHERE [Name] = @Name;", this.CurrentDateSqlStatement));
            this.LastLoginDateUpdateSql = this.ReplaceDatabaseTokens(string.Format("UPDATE [WEBAPP_MEMBERSHIP] SET [LASTLOGINDATE] = {0} WHERE [USERID] = @UserId;", this.CurrentDateSqlStatement));
            this.InitialFailureUpdateSql = this.ReplaceDatabaseTokens(string.Format("UPDATE [WEBAPP_MEMBERSHIP] SET [FAILEDPASSWORDATTEMPTCOUNT] = '1', [FAILEDPASSWORDATTEMPTWINDOWSTART] = {0} WHERE [USERID] = @UserId;", this.CurrentDateSqlStatement));
            this.FailureCountUpdateSql = this.ReplaceDatabaseTokens("UPDATE [WEBAPP_MEMBERSHIP] SET [FAILEDPASSWORDATTEMPTCOUNT] = @Count WHERE [USERID] = @UserId;");
            this.LockoutUserUpdateSql = this.ReplaceDatabaseTokens(string.Format("UPDATE [WEBAPP_MEMBERSHIP] SET [ISLOCKEDOUT] = '1', [LASTLOCKEDOUTDATE] = {0} WHERE [USERID] = @UserId;", this.CurrentDateSqlStatement));
        }

        protected string ConnectionString
        {
            get;
            set;
        }

        protected virtual string UserInsertSql
        {
            get;
            private set;
        }

        protected virtual string MembershipInsertSql
        {
            get;
            private set;
        }
        
        protected string LastActivityDateUpdateSql
        {
            get;
            private set;
        }

        protected string LastLoginDateUpdateSql
        {
            get;
            private set;
        }

        protected string InitialFailureUpdateSql
        {
            get;
            private set;
        }

        protected string FailureCountUpdateSql
        {
            get;
            private set;
        }

        protected string LockoutUserUpdateSql
        {
            get;
            private set;
        }

        public abstract string SelectLastInsertedRowAutoIDStatement { get; }

        public abstract char ParameterPrefix { get; }

        public abstract char ParameterLeftToken { get; }

        public abstract char ParameterRightToken { get; }

        public abstract char WildCharToken { get; }

        public abstract char WildSingleCharToken { get; }

        public abstract string CurrentDateSqlStatement { get; }

        protected abstract IDbConnection CreateDbConnection();

        /// <summary>
        /// Get User Id by user name
        /// </summary>
        public int GetUserId(string userName)
        {
            string querySql = this.ReplaceDatabaseTokens("SELECT USERID FROM WEBAPP_USERS WHERE NAME = @Name;");

            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                return dbConnection.ExecuteScalar<int>(querySql, new { Name = userName });
            }
        }

        public string GetUserNameByEmail(string email)
        {
            string querySql = this.ReplaceDatabaseTokens(@"SELECT WEBAPP_USERS.NAME
                                                           FROM WEBAPP_USERS INNER JOIN WEBAPP_MEMBERSHIP ON WEBAPP_USERS.USERID = WEBAPP_MEMBERSHIP.USERID
                                                           WHERE WEBAPP_MEMBERSHIP.EMAIL = @Email");

            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                return dbConnection.ExecuteScalar<string>(querySql, new { Email = email });
            }
        }

        public string GetUserNameByCellPhoneNo(string cellPhoneNo)
        {
            string querySql = this.ReplaceDatabaseTokens(@"SELECT WEBAPP_USERS.NAME
                                                           FROM WEBAPP_USERS INNER JOIN WEBAPP_MEMBERSHIP ON WEBAPP_USERS.USERID = WEBAPP_MEMBERSHIP.USERID
                                                           WHERE WEBAPP_MEMBERSHIP.CELLPHONENO = @CellPhoneNo");

            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                return dbConnection.ExecuteScalar<string>(querySql, new { CellPhoneNo = cellPhoneNo });
            }
        }

        /// <summary>
        /// Get whether the current user id has existed in Membership table.
        /// </summary>
        public bool ExistsMembershipByUserId(int userId)
        {
            string querySql = this.ReplaceDatabaseTokens("SELECT COUNT(*) FROM WEBAPP_MEMBERSHIP WHERE USERID = @UserId");
            
            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                int count = dbConnection.ExecuteScalar<int>(querySql, new { UserId = userId });

                return count != default(int);
            }
        }

        /// <summary>
        /// Add a new account in membership
        /// </summary>
        public virtual int CreateUser(User user, Membership membership)
        {
            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                dbConnection.Open();

                using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        IEnumerable<dynamic> insertedUserIds = dbConnection.Query(this.UserInsertSql, new { user.Name }, dbTransaction);

                        var newUserIdEntity = insertedUserIds.SingleOrDefault();

                        if (newUserIdEntity != null)
                        {
                            int newUserId = Convert.ToInt32(newUserIdEntity.NewUserId);

                            int insertedMembership = dbConnection.Execute(this.MembershipInsertSql,
                                                                                new
                                                                                {
                                                                                    UserId = newUserId,
                                                                                    Email = membership.Email,
                                                                                    CellPhoneNo = membership.CellPhoneNo,
                                                                                    Password = membership.Password,
                                                                                    PasswordQuestion = membership.PasswordQuestion,
                                                                                    PasswordAnswer = membership.PasswordAnswer,
                                                                                    ConfirmationToken = membership.ConfirmationToken,
                                                                                    IsApproved = membership.IsApproved
                                                                                },
                                                                                dbTransaction);
                        }

                        dbTransaction.Commit();

                        return 1;
                    }
                    catch
                    {
                        dbTransaction.Rollback();

                        return -1;
                    }
                    finally
                    {
                        if (dbConnection.State != ConnectionState.Closed)
                        {
                            dbConnection.Close();
                        }
                    }
                }
            }
        }

        public MembershipUser GetUser(string userName, bool userIsOnline)
        {
            string userJoinMembershipQuery = this.ReplaceDatabaseTokens(@"SELECT WEBAPP_USERS.UserId AS Id,
                                                                                 WEBAPP_USERS.Name AS Name, 
                                                                                 WEBAPP_USERS.LastActivityDate AS LastActivityDate, 
                                                                                 WEBAPP_MEMBERSHIP.UserId AS UserId,
                                                                                 WEBAPP_MEMBERSHIP.Email AS Email,
                                                                                 WEBAPP_MEMBERSHIP.CellPhoneNo AS CellPhoneNo,
                                                                                 WEBAPP_MEMBERSHIP.Password AS Password,
                                                                                 WEBAPP_MEMBERSHIP.PasswordQuestion AS PasswordQuestion,
                                                                                 WEBAPP_MEMBERSHIP.PasswordAnswer AS PasswordAnswer,
                                                                                 WEBAPP_MEMBERSHIP.ConfirmationToken AS ConfirmationToken,
                                                                                 WEBAPP_MEMBERSHIP.IsApproved AS IsApproved,
                                                                                 WEBAPP_MEMBERSHIP.LastActivityDate AS LastActivityDate,
                                                                                 WEBAPP_MEMBERSHIP.LastLoginDate AS LastLoginDate,
                                                                                 WEBAPP_MEMBERSHIP.LastPasswordChangedDate AS LastPasswordChangedDate,
                                                                                 WEBAPP_MEMBERSHIP.CreationDate AS CreationDate,
                                                                                 WEBAPP_MEMBERSHIP.IsLockedOut AS IsLockedOut,
                                                                                 WEBAPP_MEMBERSHIP.LastLockedOutDate AS LastLockedOutDate,
                                                                                 WEBAPP_MEMBERSHIP.FailedPasswordAttemptCount AS FailedPasswordAttemptCount,
                                                                                 WEBAPP_MEMBERSHIP.FailedPasswordAttemptWindowStart AS FailedPasswordAttemptWindowStart,
                                                                                 WEBAPP_MEMBERSHIP.PasswordVerificationToken AS PasswordVerificationToken,
                                                                                 WEBAPP_MEMBERSHIP.PasswordVerificationTokenExpirationDate AS PasswordVerificationTokenExpirationDate
                                                                          FROM WEBAPP_USERS INNER JOIN WEBAPP_MEMBERSHIP ON WEBAPP_USERS.USERID = WEBAPP_MEMBERSHIP.USERID
                                                                          WHERE WEBAPP_USERS.NAME = @Name");

            Func<User, Membership, Membership> membershipBuilder = (u, m) =>
            {
                m.User = u;
                return m;
            };

            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                var memberships = dbConnection.Query<User, Membership, Membership>(userJoinMembershipQuery, membershipBuilder, new { Name = userName }, null, true, "UserId");

                if (memberships != null && memberships.Count().Equals(1))
                {
                    if (userIsOnline)
                    {
                        dbConnection.Execute(this.LastActivityDateUpdateSql,
                                             new { userName });
                    }

                    var membership = memberships.FirstOrDefault();

                    return new MembershipUser("MySqlMembershipProvider", userName, membership.ConfirmationToken, membership.Email, membership.PasswordQuestion, null,
                                              membership.IsApproved, membership.IsLockedOut, membership.CreateDate, membership.LastLoginDate,
                                              membership.LastActivityDate,
                                              membership.LastPasswordChangedDate.HasValue ? membership.LastPasswordChangedDate.Value : DateTime.MinValue,
                                              membership.LastLockedOutDate.HasValue ? membership.LastLockedOutDate.Value : DateTime.MinValue);
                }
            }

            return null;
        }

        public string GetPasswordByUserName(string userName, out bool isApproved, out int userId)
        {
            string queryString = this.ReplaceDatabaseTokens(@"SELECT WEBAPP_MEMBERSHIP.UserId, Password, IsApproved FROM WEBAPP_MEMBERSHIP 
                                                              INNER JOIN WEBAPP_USERS ON WEBAPP_MEMBERSHIP.USERID = WEBAPP_USERS.USERID
                                                              WHERE WEBAPP_USERS.NAME = @Name;");

            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                dynamic membershipInfo = dbConnection.Query(queryString, new { Name = userName }).SingleOrDefault();

                if (membershipInfo == null)
                {
                    isApproved = false;
                    userId = -1;
                    return null;
                }

                isApproved = Convert.ToBoolean(membershipInfo.IsApproved);
                userId = Convert.ToInt32(membershipInfo.UserId);
                return Convert.ToString(membershipInfo.Password);
            }
        }

        public string GetPasswordByEmail(string email, out bool isApproved, out int userId)
        {
            string queryString = this.ReplaceDatabaseTokens(@"SELECT UserId, Password, IsApproved FROM WEBAPP_MEMBERSHIP 
                                                              WHERE EMAIL = @Email;");

            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                dynamic membershipInfo = dbConnection.Query(queryString, new { Email = email }).SingleOrDefault();

                if (membershipInfo == null)
                {
                    isApproved = false;
                    userId = -1;
                    return null;
                }

                isApproved = Convert.ToBoolean(membershipInfo.IsApproved);
                userId = Convert.ToInt32(membershipInfo.UserId);
                return Convert.ToString(membershipInfo.Password);
            }
        }

        public string GetPasswordByCellPhoneNo(string email, out bool isApproved, out int userId)
        {
            string queryString = this.ReplaceDatabaseTokens(@"SELECT UserId, Password, IsApproved FROM WEBAPP_MEMBERSHIP 
                                                              WHERE CELLPHONENO = @CellPhoneNo;");

            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                dynamic membershipInfo = dbConnection.Query(queryString).SingleOrDefault();

                if (membershipInfo == null)
                {
                    isApproved = false;
                    userId = -1;
                    return null;
                }

                isApproved = Convert.ToBoolean(membershipInfo.IsApproved);
                userId = Convert.ToInt32(membershipInfo.UserId);
                return Convert.ToString(membershipInfo.Password);
            }
        }

        public void UpdateLastLoginDate(int userId)
        {
            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                dbConnection.Execute(this.LastLoginDateUpdateSql, new { UserId = userId });
            }
        }

        public void UpdateFailureCount(int userId, int passwordAttemptWindow, int maxInvalidPasswordAttempts)
        {
            int failedPswAttemptCount = 0;
            DateTime failedPswAttemptsStart = new DateTime();

            string failedQuerySql = this.ReplaceDatabaseTokens("SELECT FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart FROM [WEBAPP_MEMBERSHIP] WHERE [USERID] = @UserId;");

            using (IDbConnection dbConnection = this.CreateDbConnection())
            {
                dynamic failedInfo = dbConnection.Query(failedQuerySql, new { UserId = userId }).SingleOrDefault();

                if (failedInfo != null)
                {
                    failedPswAttemptCount = Convert.ToInt32(failedInfo.FailedPasswordAttemptCount);
                    failedPswAttemptsStart = Convert.ToDateTime(failedInfo.FailedPasswordAttemptWindowStart);
                }

                DateTime windowEnd = failedPswAttemptsStart.AddMinutes(passwordAttemptWindow);

                if (failedPswAttemptCount == 0 || DateTime.Now > windowEnd)
                {
                    // First password failure or outside of PasswordAttemptWindow.
                    // Start a new password failure count from 1 and a new window starting now.
                    if (dbConnection.Execute(this.InitialFailureUpdateSql, new { UserId = userId }) < 0)
                    {
                        throw new ProviderException("Unable to update failure count and window start.");
                    }
                }
                else
                {
                    if (failedPswAttemptCount++ >= maxInvalidPasswordAttempts)
                    {
                        // Password attempts have exceeded the failure threshold. Lock out the user.
                        if (dbConnection.Execute(this.LockoutUserUpdateSql, new { UserId = userId }) < 0)
                        {
                            throw new ProviderException("Unable to lock out user.");
                        }
                    }
                    else
                    {
                        // Password attempts have not exceeded the failure threshold. Update
                        // the failure counts. Leave the window the same.
                        if (dbConnection.Execute(this.FailureCountUpdateSql, new { Count = failedPswAttemptCount, UserId = userId }) < 0)
                        {
                            throw new ProviderException("Unable to update failure count.");
                        }
                    }
                }
            }
        }

        #region Helper methods

        protected string ReplaceDatabaseTokens(string sql)
        {
            return ReplaceDatabaseTokens(sql, this.ParameterLeftToken, this.ParameterRightToken, this.ParameterPrefix, this.WildCharToken, this.WildSingleCharToken);
        }

        private static string ReplaceDatabaseTokens(string sql, char leftToken, char rightToken, char paramPrefixToken, char wildcharToken, char wildsinglecharToken)
        {
            string retQuerySql = sql;

            if (leftToken != '[')
            {
                retQuerySql = retQuerySql.Replace('[', leftToken);
            }
            if (rightToken != ']')
            {
                retQuerySql = retQuerySql.Replace(']', rightToken);
            }
            if (paramPrefixToken != '@')
            {
                retQuerySql = retQuerySql.Replace('@', paramPrefixToken);
            }
            if (wildcharToken != '%')
            {
                retQuerySql = retQuerySql.Replace('%', wildcharToken);
            }
            if (wildsinglecharToken != '_')
            {
                retQuerySql = retQuerySql.Replace('_', wildsinglecharToken);
            }

            return retQuerySql;
        }

        #endregion

    }
}
