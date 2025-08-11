using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace hr_cam
{
    public partial class add_nvr : System.Web.UI.Page
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
                    if (!IsPostBack)
                    {
                        //TextBox3.TextMode = TextBoxMode.Password;
                        TextBox3.TextMode = TextBoxMode.Password;
                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            if (CheckIfNVRExists())
            {
                Response.Write("<script>alert('URL / IP Address already Exist. You cannot add another Network Video Recorder with the same URL/IP Address');</script>");
            }
            else
            {
                AddNewNVR();
                    //Response.Write("<script>alert('Bikin fungsi buat API');</script>");
            }
        }

        void AddNewNVR()
        {
            //string url = TextBox1.Text;
            //    string user = TextBox2.Text;
            //    string pass=TextBox3.Text;
            //Response.Write("<script>alert('Bikin fungsi add');</script>");
            //Response.Redirect("nvr.aspx");
            string url = TextBox1.Text;
            if (IsValidUrl(url))
            { }
            else
                {
                    url = "http://" + TextBox1.Text;
                }
                // Lakukan sesuatu jika URL valid
                //Response.Write("URL valid.");
                try
                {
                    //string url = TextBox1.Text;
                    string username = TextBox2.Text;
                    string password = TextBox3.Text;
                    using (MySqlConnection con = new MySqlConnection(strcon))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        using (MySqlCommand cmd = new MySqlCommand("INSERT INTO network_video_recorders (url,username,password) values(@url,@username,@password)", con))
                        {
                            cmd.Parameters.AddWithValue("@url", url);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            //Response.Redirect("user_admin.aspx");
                            //Response.Write("<script>alert('User Admin added Successfully');</script>");
                            Session["success"] = "Network Video Recorder have been succesfully added.";
                            Response.Redirect("nvr.aspx");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            
            
        }



        bool CheckIfNVRExists()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT * from network_video_recorders where url='" + TextBox1.Text.Trim() + "'", con))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }

        }

        protected void TogglePasswordVisibility(string inputId, string iconId)
        {
            var passwordInput = FindControl(inputId) as TextBox;
            var visibilityIcon = FindControl(iconId) as HtmlGenericControl;

            if (passwordInput.TextMode == TextBoxMode.Password)
            {
                passwordInput.TextMode = TextBoxMode.SingleLine;
                visibilityIcon.InnerText = "visibility_off";
            }
            else
            {
                passwordInput.TextMode = TextBoxMode.Password;
                visibilityIcon.InnerText = "visibility";
            }
        }

        private bool IsValidUrl(string url)
        {
            string pattern = @"^(http|https):\/\/[^\s$.?#].[^\s]*$";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.IsMatch(url);
        }

    }
}