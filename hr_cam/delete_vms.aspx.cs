using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class delete_vms : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("login.aspx");
            }
            else
            {
                if (Session["role"].ToString() == "Admin")
                {
                    string id = Request.QueryString["id"];
                    if (id != "")
                    {
                        DeleteNVR(id);
                    }
                    else { }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
        }

        void DeleteNVR(string id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (MySqlCommand cmd = new MySqlCommand("DELETE from video_management_systems where id='" + id + "'", con))
                    {

                        cmd.ExecuteNonQuery();
                        con.Close();
                        Session["fail"] = "Video Management System have been succesfully removed.";
                        Response.Redirect("vms.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}