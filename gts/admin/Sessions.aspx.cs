﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PkmnFoundations.GTS.admin
{
    public partial class Sessions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GtsSessionManager manager = GtsSessionManager.FromContext(Context);

            StringBuilder builder = new StringBuilder();
            builder.Append("Active sessions (");
            builder.Append(manager.Sessions4.Count);
            builder.Append("):<br />");
            foreach (KeyValuePair<String, GtsSession4> session in manager.Sessions4)
            {
                builder.Append("PID: ");
                builder.Append(session.Value.PID);
                builder.Append("<br />Token: ");
                builder.Append(session.Value.Token);
                builder.Append("<br />Hash: ");
                builder.Append(session.Value.Hash);
                builder.Append("<br />URL: ");
                builder.Append(session.Value.URL);
                builder.Append("<br />Expires: ");
                builder.Append(session.Value.ExpiryDate);
                builder.Append("<br /><br />");
            }

            builder.Append("Active GenV sessions (");
            builder.Append(manager.Sessions5.Count);
            builder.Append("):<br />");
            foreach (KeyValuePair<String, GtsSession5> session in manager.Sessions5)
            {
                builder.Append("PID: ");
                builder.Append(session.Value.PID);
                builder.Append("<br />Token: ");
                builder.Append(session.Value.Token);
                builder.Append("<br />Hash: ");
                builder.Append(session.Value.Hash);
                builder.Append("<br />URL: ");
                builder.Append(session.Value.URL);
                builder.Append("<br />Expires: ");
                builder.Append(session.Value.ExpiryDate);
                builder.Append("<br /><br />");
            }

            if (Request.QueryString["data"] != null)
            {
                byte[] data = GtsSession4.DecryptData(Request.QueryString["data"]);
                builder.Append("Data:<br />");
                builder.Append(RenderHex(data.ToHexStringLower()));
                builder.Append("<br />");
            }
            if (Request.QueryString["data5"] != null)
            {
                byte[] data = GtsSession5.DecryptData(Request.QueryString["data5"]);
                builder.Append("Data:<br />");
                builder.Append(RenderHex(data.ToHexStringLower()));
                builder.Append("<br />");
            }

            litDebug.Text = builder.ToString();
        }

        private String RenderHex(String hex)
        {
            StringBuilder builder = new StringBuilder();
            for (int x = 0; x < hex.Length; x += 16)
            {
                builder.Append(hex.Substring(x, Math.Min(16, hex.Length - x)));
                builder.Append("<br />");
            }
            return builder.ToString();
        }
    }
}