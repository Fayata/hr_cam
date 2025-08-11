using System;

namespace hr_cam
{
    public partial class CheckSession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["email"] == null)
                { }
                else
                {
                    Label1.Text = Session["email"].ToString();
                }
                if (Session["name"] == null)
                { }
                else
                {
                    Label2.Text = Session["name"].ToString();
                }
                if (Session["role"] == null)
                { }
                else
                {
                    Label3.Text = Session["role"].ToString();
                }
            }

        }
    }
}