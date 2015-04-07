/*	
CREATE TABLE `webapp_profiles` (
  `UserId` int(11) NOT NULL,
  `LastUpdatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Security
{
    public class UserProfile
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId
        {
            get;
            set;
        }

        public DateTime LastUpdatedDate
        {
            get;
            set;
        }

    }
}
