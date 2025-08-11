using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace hr_cam
{
    public partial class delete_employee : System.Web.UI.Page
    {
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
                    if (IsPostBack)
                    {
                        string id = Request.QueryString["id"];
                        if (id != "")
                        {
                            DeleteResourceAsync(id).Wait();
                        }
                        else { }
                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
        }

        private async Task DeleteResourceAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/employee/delete/"); // Ganti dengan URL dasar API Anda

                HttpResponseMessage response = await client.DeleteAsync($"resource/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // Berhasil menghapus
                    Response.Write("<script>alert('Resource deleted successfully');</script>");
                }
                else
                {
                    // Gagal menghapus
                    Response.Write("<script>alert('Failed to delete resource');</script>");
                }
            }
        }
    }
}