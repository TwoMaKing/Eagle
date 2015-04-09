CREATE TABLE `webapp_users` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` nvarchar(256) NOT NULL,
  `LastActivityDate` datetime DEFAULT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

CREATE TABLE `webapp_profiles` (
  `UserId` int(11) NOT NULL,
  `LastUpdatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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
  UNIQUE KEY `ConfirmationToken_UNIQUE` (`ConfirmationToken`),
  UNIQUE KEY `CellPhoneNo_UNIQUE` (`CellPhoneNo`),
  UNIQUE KEY `idx_membership_Email_Password` (`Email`,`Password`),
  UNIQUE KEY `idx_membership_CellPhoneNo_Password` (`CellPhoneNo`,`Password`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `webapp_roles` (
  `RoleId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` nvarchar(255) NOT NULL,
  PRIMARY KEY (`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `webapp_usersinroles` (
  `UserId` int(11) NOT NULL DEFAULT '0',
  `RoleId` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserId`,`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
