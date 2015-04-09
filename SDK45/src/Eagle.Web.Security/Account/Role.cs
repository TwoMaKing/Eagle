/*
CREATE TABLE `webapp_roles` (
  `RoleId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
*/


namespace Eagle.Web.Security
{
    public class Role
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role() { }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
