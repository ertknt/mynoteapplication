using MyNote.Common;
using MyNote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyNote.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            if(HttpContext.Current.Session["login"] != null)
            {
                MyNoteUser user = HttpContext.Current.Session["login"] as MyNoteUser;

                return user.Username;

            }

            return "system";
        }
    }
}