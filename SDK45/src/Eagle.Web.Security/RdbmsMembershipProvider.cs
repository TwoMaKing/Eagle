using Dappers;
using Eagle.Common.Utils;
using Eagle.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using System.Web.WebPages;

namespace Eagle.Web.Security
{
    public abstract class RdbmsMembershipProvider : MembershipProvider
    {
        private int maxInvalidPasswordAttempts;
        private int passwordAttemptWindow;
        private int minRequiredNonAlphanumericCharacters;
        private int minRequiredPasswordLength;
        private string passwordStrengthRegularExpression;
        private bool enablePasswordReset;
        private bool enablePasswordRetrieval;
        private bool requiresUniqueEmail;
        private bool requiresUniqueCellPhoneNo;
        private bool requiresQuestionAndAnswer;
        private bool requireConfirmationToken;

        public RdbmsMembershipProvider() 
        {
            
        }

        internal string ConnectionString
        {
            get;
            set;
        }

        protected AccountDbProvider DbProvider
        {
            get;
            set;
        }

        public override string ApplicationName
        {
            get;
            set;
        }

        public override bool EnablePasswordReset
        {
            get
            {
                return this.enablePasswordReset;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return this.enablePasswordRetrieval;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return this.maxInvalidPasswordAttempts;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return this.minRequiredNonAlphanumericCharacters;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return this.minRequiredPasswordLength;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return this.passwordAttemptWindow;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return MembershipPasswordFormat.Encrypted;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return this.passwordStrengthRegularExpression;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return this.requiresQuestionAndAnswer;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return this.requiresUniqueEmail;
            }
        }

        public virtual bool RequiresUniqueCellPhoneNo 
        {
            get 
            {
                return this.requiresUniqueCellPhoneNo;
            }
        }

        public virtual bool RequireConfirmationToken
        {
            get 
            {
                return this.requireConfirmationToken;
            }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (name.IsEmpty())
            {
                name = "RdbmsMembershipProvider";
            }

            if (config["requiresUniqueCellPhoneNo"].IsEmpty())
            {
                config.Add("requiresUniqueCellPhoneNo", "true");
            }

            if (config["requireConfirmationToken"].IsEmpty())
            {
                config.Add("requireConfirmationToken", "true");
            }

            base.Initialize(name, config);

            this.maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigurationValue(config["maxInvalidPasswordAttempts"], "5"));
            this.passwordAttemptWindow = Convert.ToInt32(GetConfigurationValue(config["passwordAttemptWindow"], "10"));
            this.minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigurationValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            this.minRequiredPasswordLength = Convert.ToInt32(GetConfigurationValue(config["minRequiredPasswordLength"], "7"));
            this.passwordStrengthRegularExpression = Convert.ToString(GetConfigurationValue(config["passwordStrengthRegularExpression"], string.Empty));
            this.enablePasswordReset = Convert.ToBoolean(GetConfigurationValue(config["enablePasswordReset"], "true"));
            this.enablePasswordRetrieval = Convert.ToBoolean(GetConfigurationValue(config["enablePasswordRetrieval"], "false"));
            this.requiresUniqueEmail = Convert.ToBoolean(GetConfigurationValue(config["requiresUniqueEmail"], "true"));
            this.requiresUniqueCellPhoneNo = Convert.ToBoolean(GetConfigurationValue(config["requiresUniqueCellPhoneNo"], "true"));
            this.requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigurationValue(config["requiresQuestionAndAnswer"], "false"));
            this.requireConfirmationToken = Convert.ToBoolean(GetConfigurationValue(config["requireConfirmationToken"], "true"));

            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (connectionStringSettings == null ||
                connectionStringSettings.ConnectionString.IsEmpty())
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            this.ConnectionString = connectionStringSettings.ConnectionString;

            this.DbProvider = this.CreateAccountDbProvider();

            config.Remove("connectionStringName");
            config.Remove("enablePasswordRetrieval");
            config.Remove("enablePasswordReset");
            config.Remove("requiresQuestionAndAnswer");
            config.Remove("applicationName");
            config.Remove("requiresUniqueEmail");
            config.Remove("maxInvalidPasswordAttempts");
            config.Remove("passwordAttemptWindow");
            config.Remove("passwordFormat");
            config.Remove("name");
            config.Remove("description");
            config.Remove("minRequiredPasswordLength");
            config.Remove("minRequiredNonalphanumericCharacters");
            config.Remove("passwordStrengthRegularExpression");
            config.Remove("hashAlgorithmType");
            config.Remove("requiresUniqueCellPhoneNo");
            config.Remove("requireConfirmationToken");

            if (config.Count > 0)
            {
                string attribUnrecognized = config.GetKey(0);
                if (!attribUnrecognized.IsEmpty())
                {
                    throw new ConfigException(string.Format("Provider unrecognized attribute: {0}.", attribUnrecognized));
                }
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (this.RequiresUniqueEmail &&
                !this.GetUserNameByEmail(email).IsEmpty())
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            var membershipUser = this.GetUser(username, false);

            if (membershipUser == null)
            {
                User user = new User
                {
                    Name = username
                };

                string token = null;
                object dbtoken = DBNull.Value;

                if (this.RequireConfirmationToken)
                {
                    token = SecurityHelper.GenerateToken();
                    dbtoken = token;
                }

                Membership membership = new Membership
                {
                    Email = email,
                    IsApproved = isApproved,
                    Password = SecurityHelper.EncodePassword(password),
                    PasswordQuestion = passwordQuestion,
                    PasswordAnswer = passwordAnswer,
                    ConfirmationToken = dbtoken.ToString()
                };

                int createdResult = this.DbProvider.CreateUser(user, membership);
                
                status = createdResult > 0 ? MembershipCreateStatus.Success : MembershipCreateStatus.UserRejected;

                return this.GetUser(username, false);
            }

            status = MembershipCreateStatus.DuplicateUserName;

            return null;
        }

        public virtual MembershipUser CreateUser(string username, string password, string cellPhoneNo, out MembershipCreateStatus status)
        {
            var args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);
            
            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (this.RequiresUniqueCellPhoneNo &&
                !this.GetUserNameByCellPhoneNo(cellPhoneNo).IsEmpty())
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            var membershipUser = this.GetUser(username, false);

            if (membershipUser == null)
            {
                User user = new User
                {
                    Name = username
                };

                string token = null;
                object dbtoken = DBNull.Value;

                if (this.RequireConfirmationToken)
                {
                    token = SecurityHelper.GenerateToken();
                    dbtoken = token;
                }

                Membership membership = new Membership
                {
                    CellPhoneNo = cellPhoneNo,
                    IsApproved = true,
                    Password = SecurityHelper.EncodePassword(password),
                    ConfirmationToken = dbtoken.ToString()
                };

                int createdResult = this.DbProvider.CreateUser(user, membership);

                status = createdResult > 0 ? MembershipCreateStatus.Success : MembershipCreateStatus.UserRejected;

                return this.GetUser(username, false);
            }

            status = MembershipCreateStatus.DuplicateUserName;

            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return this.DbProvider.GetUser(username, userIsOnline);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            return this.DbProvider.GetUserNameByEmail(email);
        }

        public virtual string GetUserNameByCellPhoneNo(string cellPhoneNo)
        {
            return this.DbProvider.GetUserNameByCellPhoneNo(cellPhoneNo);
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            bool isApproved = false;
            int userId;

            string dbPassword = this.DbProvider.GetPasswordByUserName(username, out isApproved, out userId);

            if (SecurityHelper.CheckPassword(password, dbPassword))
            {
                if (isApproved)
                {
                    isValid = true;
                    this.DbProvider.UpdateLastLoginDate(userId);
                }
            }
            else
            {
                this.DbProvider.UpdateFailureCount(userId, this.PasswordAttemptWindow, this.MaxInvalidPasswordAttempts);
            }

            return isValid;
        }

        public virtual bool ValidateUserByEmail(string email, string password)
        {
            bool isValid = false;
            bool isApproved = false;
            int userId;

            string dbPassword = this.DbProvider.GetPasswordByEmail(email, out isApproved, out userId);

            if (SecurityHelper.CheckPassword(password, dbPassword))
            {
                if (isApproved)
                {
                    isValid = true;
                    this.DbProvider.UpdateLastLoginDate(userId);
                }
            }
            else
            {
                this.DbProvider.UpdateFailureCount(userId, this.PasswordAttemptWindow, this.MaxInvalidPasswordAttempts);
            }

            return isValid;
        }

        public virtual bool ValdateUserByCellPhoneNo(string cellPhoneNo, string password)
        {
            bool isValid = false;
            bool isApproved = false;
            int userId;

            string dbPassword = this.DbProvider.GetPasswordByCellPhoneNo(cellPhoneNo, out isApproved, out userId);

            if (SecurityHelper.CheckPassword(password, dbPassword))
            {
                if (isApproved)
                {
                    isValid = true;
                    this.DbProvider.UpdateLastLoginDate(userId);
                }
            }
            else
            {
                this.DbProvider.UpdateFailureCount(userId, this.PasswordAttemptWindow, this.MaxInvalidPasswordAttempts);
            }

            return isValid;
        }

        public abstract AccountDbProvider CreateAccountDbProvider();

        #region Helper methods

        private static string GetConfigurationValue(string configValue, string defaultValue)
        {
            return configValue.IsEmpty() ? defaultValue : configValue;
        }

        #endregion

    }
}
