using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Eagle.Tests.Domain.Models;

namespace Eagle.Tests.Application
{
    public static class GlobalApplication 
    {
        public static User LoginUser 
        {
            get 
            { 
                object loginUser = HttpContext.Current.Session["user"];

                if (loginUser != null)
                {
                    return (User)loginUser;
                }

                return null;
            }
        }

    }
}
