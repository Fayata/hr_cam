using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class add_camera : System.Web.UI.Page
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
                        TextBox3.TextMode = TextBoxMode.Password;
                        using (MySqlConnection con = new MySqlConnection(strcon))
                        {
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }

                            using (MySqlCommand cmd = new MySqlCommand("SELECT * from camera_sites", con))
                            {
                                using (MySqlDataReader dr = cmd.ExecuteReader())
                                {
                                    if (dr.HasRows)
                                    {
                                        while (dr.Read())
                                        {
                                            DropDownList1.Items.Add(new ListItem(dr.GetValue(1).ToString(), dr.GetValue(0).ToString()));
                                        }
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            string name = TextBox1.Text;
            string location = TextBox2.Text;
            if (CheckIfCameraExists())
            {
                Response.Write("<script>alert('Camera with this name and location already Exist. You cannot add another camera with the same Name and Location');</script>");
            }
            else
            {
                //Response.Write("<script>alert('Masuk');</script>");
                AddNewCamera();
            }
        }

        void AddNewCamera()
        {
            try
            {
                string camera_site_id = DropDownList1.SelectedValue;
                string name = TextBox1.Text;
                string location = TextBox2.Text;
                string url = TextBox4.Text;
                string username = TextBox5.Text;
                string password = TextBox3.Text;
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO cameras(camera_site_id,name,location,url,username,password) values(@camera_site_id,@name,@location,@url,@username,@password)", con))
                    {
                        cmd.Parameters.AddWithValue("@camera_site_id", camera_site_id);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@location", location);
                        cmd.Parameters.AddWithValue("@url", url);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        cmd.ExecuteNonQuery();
                        con.Close();
                        Session["success"] = "Camera have been succesfully added.";
                        Response.Redirect("camera.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }



        bool CheckIfCameraExists()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where name='" + TextBox1.Text.Trim() + "' and location='" + TextBox2.Text.Trim() + "'", con))
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
    }
}