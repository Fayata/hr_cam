using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;

namespace hr_cam
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string scheduleUrl = ConfigurationManager.AppSettings["ScheduleUrl"];
            //Response.Write("<script>alert('" + scheduleUrl + "')</script>");
            linkJadwal.HRef = scheduleUrl;
            //if (!IsPostBack)
            //{
            //    string currentUrl = Request.Url.GetLeftPart(UriPartial.Path);
            //    linkJadwal.HRef = currentUrl;
            //}
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
            // currentPage berisi nama file halaman yang sedang dibuka saat ini

            // Periksa apakah halaman yang sedang dibuka adalah halaman yang sesuai dengan NavLink
            // Misalnya, untuk NavLink "Home", jika halaman yang dibuka adalah "Home.aspx"
            if (currentPage.Equals("home.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                //homeNavLink.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "Home"
            }
            else if (currentPage.Equals("statistics.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                //home2NavLink.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "Home"
            }
            else if (currentPage.Equals("user_admin.aspx", StringComparison.InvariantCultureIgnoreCase) || currentPage.Equals("add_user_admin.aspx", StringComparison.InvariantCultureIgnoreCase) || currentPage.Equals("edit_user_admin.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                //user_adminNavLink.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("camera.aspx", StringComparison.InvariantCultureIgnoreCase) || currentPage.Equals("add_camera.aspx", StringComparison.InvariantCultureIgnoreCase) || currentPage.Equals("edit_camera.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                //dataEntryNavLink.Attributes.Add("class", "nav-link active  text-primary dropdown-toggle"); // Tambahkan kelas active ke NavLink "About"
                A11.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("entry_people.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                //dataEntryNavLink.Attributes.Add("class", "nav-link active  text-primary dropdown-toggle"); // Tambahkan kelas active ke NavLink "About"
                A1.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("vehicle.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                //dataEntryNavLink.Attributes.Add("class", "nav-link active  text-primary dropdown-toggle"); // Tambahkan kelas active ke NavLink "About"
                A2.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("dpo.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                //dataEntryNavLink.Attributes.Add("class", "nav-link active  text-primary dropdown-toggle"); // Tambahkan kelas active ke NavLink "About"
                //dpo.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("nvr.aspx", StringComparison.InvariantCultureIgnoreCase) || currentPage.Equals("add_nvr.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                //dataEntryNavLink.Attributes.Add("class", "nav-link active  text-primary dropdown-toggle"); // Tambahkan kelas active ke NavLink "About"
                A8.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("vms.aspx", StringComparison.InvariantCultureIgnoreCase) || currentPage.Equals("add_vms.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                //dataEntryNavLink.Attributes.Add("class", "nav-link active  text-primary dropdown-toggle"); // Tambahkan kelas active ke NavLink "About"
                A9.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            //reportNavLink.Attributes.Add("class", "nav-link active  text-primary dropdown-toggle"); // Tambahkan kelas active ke NavLink "About"
            else if (currentPage.Equals("list_report.aspx", StringComparison.InvariantCultureIgnoreCase) || currentPage.Equals("create_report.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A17.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_event.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A10.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_dpo.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A4.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_people_detection.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A5.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_not_fit.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A3.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_vehicle_detection.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A6.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_people.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A7.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_traffic_detection.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A12.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_daily_created_people.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A13.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_daily_updated_people.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A14.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_daily_created_vehicle.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A15.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }
            else if (currentPage.Equals("report_daily_updated_vehicle.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                A16.Attributes.Add("class", "nav-link active  text-primary"); // Tambahkan kelas active ke NavLink "About"
            }

            try
            {
                if (Session["email"] == null)
                {
                    LinkButton1.Visible = false; // welcome link button
                    LinkButton2.Visible = true; // sign in link button

                    LinkButton3.Visible = false; // logout link button
                    Response.Redirect("login.aspx");

                }
                else
                {
                    LinkButton1.Visible = true; // welcome link button
                    LinkButton1.Text = "Welcome back, " + Session["name"].ToString();
                    LinkButton2.Visible = false; // sign in link button

                    LinkButton3.Visible = true; // logout link button
                    if (Session["role"].ToString() == "Admin")
                    {
                        PlaceHolderRole1.Visible = true;
                    }
                    else
                    {
                        PlaceHolderRole1.Visible = false;
                    }
                }
                //else if (Session["role"].Equals("user"))
                //{
                //    LinkButton1.Visible = true; // welcome link button
                //    LinkButton1.Text = "Welcome " + Session["username"].ToString();
                //    LinkButton2.Visible = false; // sign in link button

                //    LinkButton3.Visible = true; // logout link button
                //}
                //else if (Session["role"].Equals("admin"))
                //{
                //    LinkButton1.Visible = true; // welcome link button
                //    LinkButton1.Text = "Hello";
                //    LinkButton2.Visible = false; // sign up link button

                //    LinkButton3.Visible = true; // logout link button
                //}
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error" + ex + "');</script>");
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("home.aspx");
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            //Session["username"] = "";
            //Session["name"] = "";
            //Session["role"] = "";
            //Session["status"] = "";
            FormsAuthentication.SignOut();
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(authCookie);
            Session.Abandon();

            LinkButton1.Visible = false; // user login link button
            LinkButton2.Visible = true; // sign up link button

            LinkButton3.Visible = false; // logout link button
            Response.Redirect("login.aspx");

        }
    }
}