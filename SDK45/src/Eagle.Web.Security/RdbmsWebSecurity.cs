using Eagle.Common.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Eagle.Web.Security
{
    public class RdbmsWebSecurity
    {

        /// <summary>
        /// Gets the user name for the current user.
        /// </summary>
        /// <value>The name of the current user.</value>
        public static string CurrentUserName
        {
            get
            {
                return Context.User.Identity.Name;
            }
        }

        /// <summary>
        /// Gets the authentication status of the current user.
        /// </summary>
        /// <value><c>true</c> if the current user is authenticated; otherwise, false. The default is <c>false</c>.</value>
        public static bool IsAuthenticated
        {
            get
            {
                return Request.IsAuthenticated;
            }
        }

        internal static HttpContextBase Context
        {
            get
            {
                return new HttpContextWrapper(HttpContext.Current);
            }
        }

        internal static HttpRequestBase Request
        {
            get
            {
                return Context.Request;
            }
        }

        internal static HttpResponseBase Response
        {
            get
            {
                return Context.Response;
            }
        }


        /// <summary>
        /// Creates a new user entry and a new membership account.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <returns>A token that can be sent to the user to confirm the user account.</returns>
        public static string CreateUserAndAccount(string userName, string password, string emailOrCellPhoneNo)
        {
            var provider = VerifyProvider();

            MembershipCreateStatus status;

            MembershipUser membershipUser = null;

            LoginIdentityType identityType = GetLoginIdentityType(emailOrCellPhoneNo);

            if (identityType == LoginIdentityType.Email)
            {
                membershipUser = provider.CreateUser(userName, password, emailOrCellPhoneNo, null, null, true, null, out status);
            }
            else
            {
                membershipUser = provider.CreateUser(userName, password, emailOrCellPhoneNo, out status);
            }

            if (membershipUser == null ||
                status != MembershipCreateStatus.Success)
            {
                return null;
            }

            return membershipUser.ProviderUserKey.ToString();
        }


        /// <summary>
        /// Logins the specified user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password for the user.</param>
        /// <param name="persistCookie">(Optional) true to specify that the authentication token in the cookie should be persisted beyond the current session; otherwise false. The default is false.</param>
        /// <returns><c>true</c> if the user was logged in; otherwise, <c>false</c>.</returns>
        public static bool Login(string userNameOrEmailOrCellPhoneNo, string password, bool persistCookie = false)
        {
            var provider = VerifyProvider();

            bool success = false;

            LoginIdentityType identityType = GetLoginIdentityType(userNameOrEmailOrCellPhoneNo);

            if (identityType == LoginIdentityType.UserName)
            {
                success = provider.ValidateUser(userNameOrEmailOrCellPhoneNo, password);
            }
            else if (identityType == LoginIdentityType.Email)
            {
                success = provider.ValidateUserByEmail(userNameOrEmailOrCellPhoneNo, password);
            }
            else
            {
                success = provider.ValdateUserByCellPhoneNo(userNameOrEmailOrCellPhoneNo, password);
            }

            if (success)
            {
                //FormsAuthentication.SetAuthCookie(userNameOrEmailOrCellPhoneNo, persistCookie, FormsAuthentication.FormsCookiePath);

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, userNameOrEmailOrCellPhoneNo, DateTime.Now,
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                persistCookie,
                Request.UserHostAddress);

                //FormsIdentity identity = new FormsIdentity(ticket);
                //HttpContext.Current.User = new GenericPrincipal(identity, null);

                string authTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie ticketCookie = new HttpCookie(FormsAuthentication.FormsCookieName, authTicket);
                ticketCookie.Domain = FormsAuthentication.CookieDomain;
                Response.Cookies.Add(ticketCookie);
            }

            return success;
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }

        private static RdbmsMembershipProvider VerifyProvider()
        {
            RdbmsMembershipProvider provider = System.Web.Security.Membership.Provider as RdbmsMembershipProvider;

            if (provider == null)
            {
                throw new InvalidOperationException();
            }

            return provider;
        }

        private static LoginIdentityType GetLoginIdentityType(string UserNameOrEmailOrCellPhoneNo)
        {
            if (RegexUtils.IsEmail(UserNameOrEmailOrCellPhoneNo))
            {
                return LoginIdentityType.Email;
            }
            else if (RegexUtils.IsMobilePhone(UserNameOrEmailOrCellPhoneNo))
            {
                return LoginIdentityType.CellPhoneNo;
            }
            else
            {
                return LoginIdentityType.UserName;
            }
        }

    }
}
