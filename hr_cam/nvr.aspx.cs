using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class Nvr : System.Web.UI.Page
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
                    FillTableFromDatabase();
                    if (!IsPostBack)
                    {

                        if (Session["success"] != null)
                        {
                            // Menampilkan pesan sukses
                            successMessageDiv.Visible = true; // Menampilkan div pesan sukses
                            successMessage.Text = Session["success"].ToString(); // Menampilkan pesan sukses
                            Session.Remove("success"); // Hapus pesan dari session

                            // Mengubah properti display CSS untuk menampilkan div pesan sukses
                            successMessageDiv.Attributes.Add("style", "display:block;");
                        }
                        else if (Session["fail"] != null)
                        {
                            // Menampilkan pesan gagal
                            failMessageDiv.Visible = true; // Menampilkan div pesan gagal
                            failMessage.Text = Session["fail"].ToString(); // Menampilkan pesan gagal
                            Session.Remove("fail"); // Hapus pesan dari session

                            // Mengubah properti display CSS untuk menampilkan div pesan gagal
                            failMessageDiv.Attributes.Add("style", "display:block;");
                        }
                        else if (Session["update"] != null)
                        {
                            // Menampilkan pesan update
                            updateMessageDiv.Visible = true; // Menampilkan div pesan update
                            updateMessage.Text = Session["update"].ToString(); // Menampilkan pesan update
                            Session.Remove("update"); // Hapus pesan dari session

                            // Mengubah properti display CSS untuk menampilkan div pesan update
                            updateMessageDiv.Attributes.Add("style", "display:block;");

                            //Response.Write("ID: " + updateMessage);
                        }

                        //FillDataTable();
                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
        }

        private void FillTableFromDatabase()
        {
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT * from network_video_recorders", con))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string url = dr["url"].ToString();
                            string username = dr["username"].ToString();
                            string nvrId = dr["id"].ToString();

                            TableRow row = new TableRow();

                            //TableCell urlCell = new TableCell();
                            //urlCell.Text = url;
                            TableCell urlCell = new TableCell() { Text = url };
                            row.Cells.Add(urlCell);
                            
                            //TableCell usernameCell = new TableCell();
                            //usernameCell.Text = username;
                            TableCell usernameCell = new TableCell() { Text = username };
                            row.Cells.Add(usernameCell);

                            string deleteUrl = "delete_nvr.aspx?id=" + nvrId;

                            TableCell actionCell = new TableCell();
                            actionCell.Style["width"] = "200px"; // Adjust width as needed
                            LiteralControl actionButtons = new LiteralControl();

                            // Create the Delete button HTML
                            string deleteButtonHtml = "<a href=\"" + deleteUrl + "\" class=\"btn btn-danger\" onclick=\"return confirm('Are you sure you want to delete this item?');\">";
                            deleteButtonHtml += "<span class='btn-icon'><i class='fa-solid fa-trash-can'></i></span>";
                            deleteButtonHtml += "</a>";
                            //string deleteButtonHtml = "<a href=\"\" class=\"btn btn-danger\" onclick=\"confirmDelete(event," + nvrId + ")\">";
                            //deleteButtonHtml += "<span class='btn-icon'><i class='fa-solid fa-trash-can'></i></span>";
                            //deleteButtonHtml += "</a>";

                            // Combine Edit and Delete buttons
                            actionButtons.Text = deleteButtonHtml;

                            // Add the combined buttons to the cell
                            actionCell.Controls.Add(actionButtons);
                            row.Cells.Add(actionCell);


                            TableBody.Controls.Add(row);
                        }
                        dr.Close();
                    }
                }
            }
        }
    }
}