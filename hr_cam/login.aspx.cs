using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class Login : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    Response.Write("User is authenticated");
            //}
            //else
            //{
            //    Response.Write("User is not authenticated");
            //}
        }


        protected void Button1_Click1(object sender, EventArgs e)
        {
            //string pwAsli = "kosong";
            //string hashPw = BCrypt.Net.BCrypt.HashPassword("kosong");

            //string pw = "kosong";

            //Response.Write("<script>alert('"+ hashPw + "');</script>");
            //return;
            try
            {
                //SqlConnection con = new SqlConnection(strcon);
                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}
                //SqlCommand cmd = new SqlCommand("SELECT * from users where email='" + TextBox1.Text.Trim() + "'", con);

                //SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows)
                //{
                //    string pass = TextBox2.Text.Trim();
                //    while (dr.Read())
                //    {
                //        Response.Write("<script>alert('" + dr.GetValue(2).ToString() + "');</script>");
                //        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(pass, dr.GetValue(4).ToString());
                //        if (isPasswordValid == true)
                //        {
                //            Response.Write("<script>alert('" + dr.GetValue(2).ToString() + "');</script>");
                //        }
                //        else
                //        {
                //            Response.Write("<script>alert('Invalid credentials');</script>");
                //        }
                //    }
                //}
                //else
                //{
                //    Response.Write("<script>alert('Invalid credentials');</script>");
                //}
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT u.id, u.name, u.email, r.name as role, u.password from users u join roles r on u.role_id=r.id where u.email=@Email", con))
                    {
                        cmd.Parameters.AddWithValue("@Email", TextBox1.Text.Trim());

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                string username=TextBox1.Text.Trim();
                                string pass = TextBox2.Text.Trim();
                                while (dr.Read())
                                {
                                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(pass, dr["password"].ToString());
                                    if (isPasswordValid == true)
                                    {
                                        string basicAuth = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + pass));

                                        // Set basicAuth ke session
                                        Response.Write("<script>alert('Login Success');</script>");
                                        Session["email"] = dr["email"].ToString();
                                        Session["name"] = dr["name"].ToString();
                                        Session["role"] = dr["role"].ToString();
                                        Session["BasicAuth"] = "Basic " + basicAuth;
                                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                        1,                                      // Versi tiket
                                        username,                               // Nama pengguna
                                        DateTime.Now,                           // Waktu pembuatan tiket
                                        DateTime.Now.AddMinutes(30),            // Waktu kedaluwarsa tiket
                                        false,                                  // Persistent?
                                        "user data",                            // Data pengguna
                                        FormsAuthentication.FormsCookiePath);   // Path cookie

                                        // Enkripsi tiket
                                        string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                                        // Buat cookie otentikasi
                                        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                                        Response.Cookies.Add(authCookie);

                                        // Redirect ke halaman asli atau default
                                        //string returnUrl = Request.QueryString["ReturnUrl"];
                                        //if (!String.IsNullOrEmpty(returnUrl))
                                        //{
                                        //    Response.Redirect(returnUrl);
                                        //}
                                        //else
                                        //{
                                        //    Response.Redirect("~/Default.aspx");
                                        //}
                                    }
                                    else
                                    {
                                        Response.Write("<script>alert('Invalid credentials');</script>");
                                    }

                                }
                                Response.Redirect("home.aspx");
                            }
                            else
                            {
                                Response.Write("<script>alert('Invalid credentials');</script>");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error"+ex+"');</script>");
            }
        }
    }
}