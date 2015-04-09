/*
CREATE TABLE `webapp_membership` (
  `UserId` int(11) NOT NULL DEFAULT '0',
  `Email` nvarchar(128) DEFAULT NULL,
  `CellPhoneNo` nvarchar(12) DEFAULT NULL,
  `Password` nvarchar(128) NOT NULL,
  `PasswordQuestion` nvarchar(255) DEFAULT NULL,
  `PasswordAnswer` nvarchar(255) DEFAULT NULL,
  `ConfirmationToken` nvarchar(128) DEFAULT NULL,
  `IsApproved` tinyint(1) DEFAULT NULL,
  `LastActivityDate` datetime DEFAULT NULL,
  `LastLoginDate` datetime DEFAULT NULL,
  `LastPasswordChangedDate` datetime DEFAULT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `IsLockedOut` tinyint(1) DEFAULT NULL,
  `LastLockedOutDate` datetime DEFAULT NULL,
  `FailedPasswordAttemptCount` int(10) unsigned DEFAULT NULL,
  `FailedPasswordAttemptWindowStart` datetime DEFAULT NULL,
  `PasswordVerificationToken` nvarchar(128) DEFAULT NULL,
  `PasswordVerificationTokenExpirationDate` datetime DEFAULT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `Email_UNIQUE` (`Email`),
  UNIQUE KEY `CellPhoneNo_UNIQUE` (`CellPhoneNo`),
  UNIQUE KEY `idx_membership_Email_Password` (`Email`,`Password`),
  UNIQUE KEY `idx_membership_CellPhoneNo_Password` (`CellPhoneNo`,`Password`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Web.Security
{
	public class Membership
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Membership"/> class.
		/// </summary>
		public Membership()
		{
		}

		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		public int UserId
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the mobile phone of the user.
        /// </summary>
        public string CellPhoneNo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password question.
        /// </summary>
        public string PasswordQuestion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password answer.
        /// </summary>
        public string PasswordAnswer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the confirmation token.
        /// </summary>
        public string ConfirmationToken
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the create date.
		/// </summary>
		public DateTime CreateDate
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is approved.
		/// </summary>
		public bool IsApproved
		{
			get;
			set;
		}

        public DateTime LastActivityDate
        {
            get;
            set;
        }

        public DateTime LastLoginDate 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the password changed date.
        /// </summary>
        public DateTime? LastPasswordChangedDate
        {
            get;
            set;
        }

        public bool IsLockedOut
        {
            get;
            set;
        }

        public DateTime? LastLockedOutDate
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the password failures count.
		/// </summary>
        public int? FailedPasswordAttemptCount
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the last password failure date.
        /// </summary>
        public DateTime? FailedPasswordAttemptWindowStart
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the password verification token.
		/// </summary>
		public string PasswordVerificationToken
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the password verification token expiration date.
		/// </summary>
		public DateTime? PasswordVerificationTokenExpirationDate
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual User User
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the user profile.
		/// </summary>
		public virtual UserProfile UserProfile
		{
			get;
			set;
		}
	}
}