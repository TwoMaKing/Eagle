/*
CREATE TABLE `webapp_usersinroles` (
  `UserId` int(11) NOT NULL DEFAULT '0',
  `RoleId` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserId`,`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Security
{
    public class UsersInRoles
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        public virtual User User
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public virtual Role Role
        {
            get;
            set;
        }
    }
}
