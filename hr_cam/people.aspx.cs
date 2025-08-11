using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class people : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        string UrlImage = ConfigurationManager.AppSettings["urlImagePerson"];
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // Set the cache expiration to a past time
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));

            // Set cache to NoStore to ensure the browser doesn't store the response
            Response.Cache.SetNoStore();
            //Response.Write("API URL: " + apiUrl + "<br>");

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
                        FillTableFromDatabase();
                        FillSpan();
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

                using (MySqlCommand cmd = new MySqlCommand("SELECT * from persons", con))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string identification_number = dr["identification_number"].ToString();
                            string name = dr["name"].ToString();
                            string gender = dr["gender"].ToString();
                            string image_file = dr["image_file"].ToString();
                            string personId = dr["id"].ToString();
                            string type=dr["type"].ToString();
                            string person_type = string.IsNullOrEmpty(type)? type : char.ToUpper(type[0]) + type.Substring(1);
                            string is_deleted=dr["is_deleted"].ToString();

                            TableRow row = new TableRow();

                            //TableCell identificationCell = new TableCell();
                            //identificationCell.Text = identification_number;
                            //row.Cells.Add(identificationCell);

                            //TableCell nameCell = new TableCell();
                            //nameCell.Text = name;
                            //row.Cells.Add(nameCell);


                            //TableCell genderCell = new TableCell();
                            //genderCell.Text = gender;
                            //row.Cells.Add(genderCell);

                            TableCell identificationCell = new TableCell { Text = identification_number };
                            row.Cells.Add(identificationCell);

                            TableCell nameCell = new TableCell() { Text = name };
                            row.Cells.Add(nameCell);

                            TableCell genderCell = new TableCell() { Text = gender };
                            row.Cells.Add(genderCell);
                            
                            TableCell typeCell = new TableCell() { Text = person_type };
                            row.Cells.Add(typeCell);

                            TableCell imageCell = new TableCell();
                            imageCell.Attributes.Add("align", "center");
                            imageCell.Attributes.Add("style", "width: 1%;");
                            // Buat string yang berisi HTML modal
                            string key = personId; // Ganti dengan nilai yang sesuai
                            string imagePath = UrlImage + image_file;
                            if (File.Exists(imagePath))
                            {
                                byte[] imageBytes = File.ReadAllBytes(imagePath);
                                string base64String = Convert.ToBase64String(imageBytes);
                                                //<img src='data:image/png;base64, {base64String}' alt='icon title' />
                                                //<img src='person_image.ashx?fileName={image_file}' alt='Person Image' />
                                string modalHtml = $@"
                                <a href='#' class='btn btn-success' data-bs-toggle='modal' data-bs-target='#modal-face-snapshot-{key}'>View Image</a>
                                <div class='modal modal-blur fade' id='modal-face-snapshot-{key}' tabindex='-1' role='dialog' aria-hidden='true'>
                                    <div class='modal-dialog modal-dialog-centered modal-dialog-scrollable' role='document'>
                                        <div class='modal-content'>
                                            <div class='modal-header'>
                                                <h5 class='modal-title'>Image</h5>
                                                <button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='Close'></button>
                                            </div>
                                            <div class='modal-body'>
                                                <img src='person_image/{image_file}' alt='icon title' />
                                            </div>
                                            <div class='modal-footer'>
                                                <button type='button' class='btn me-auto' data-bs-dismiss='modal'>Close</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>";

                                // Buat LiteralControl untuk menambahkan HTML ke TableCell
                                LiteralControl modalLiteral = new LiteralControl(modalHtml);

                                // Tambahkan LiteralControl ke TableCell
                                imageCell.Controls.Add(modalLiteral);
                            }
                            else
                            {
                                //imageCell.Controls.Add(new LiteralControl("<a href='#' onclick='selectImage({{personId}})' class='btn btn-info'>Add Image</a>"));
                                imageCell.Controls.Add(new LiteralControl("<a href=\"#\" class=\"btn btn-primary\" onclick=\"selectImage(" + personId + ")\">Add Image</a>"));
                            }

                            // Tambahkan TableCell ke TableRow
                            row.Cells.Add(imageCell);



                            // Tambahkan tombol edit dengan ikon Font Awesome
                            //string editUrl = "edit_user_admin.aspx?id=" + userId;
                            //TableCell editCell = new TableCell();
                            //editCell.Style["width"] = "100px";
                            //LiteralControl editButton = new LiteralControl();
                            //editButton.Text = "<a href='"+editUrl+"' class='btn btn-warning'>";
                            //editButton.Text += "<span class='btn-icon'><span class='material-icons'>edit</span></span>";
                            //editButton.Text += "</a>";
                            //editCell.Controls.Add(editButton);
                            //row.Cells.Add(editCell);

                            //string deleteUrl = "delete_user_admin.aspx?id=" + userId;
                            //TableCell deleteCell = new TableCell();
                            //deleteCell.Style["width"] = "100px";
                            //LiteralControl deleteButton = new LiteralControl();
                            //deleteButton.Text = "<a href=\"" + deleteUrl + "\" class=\"btn btn-danger\" onclick=\"return confirm('Are you sure?');\">";
                            //deleteButton.Text += "<span class='btn-icon'><span class='material-icons'>delete</span></span>";
                            //deleteButton.Text += "</a>";
                            //deleteCell.Controls.Add(deleteButton);
                            //row.Cells.Add(deleteCell);

                            string deleteUrl = "delete_employee.aspx?id=" + personId;

                            TableCell actionCell = new TableCell();
                            actionCell.Style["width"] = "200px"; // Adjust width as needed
                            LiteralControl actionButtons = new LiteralControl();

                            // Create the Delete button HTML
                            //string deleteButtonHtml = "<a href=\"" + deleteUrl + "\" class=\"btn btn-danger\" onclick=\"return confirm('Are you sure?');\">";
                            if (is_deleted == "True" || is_deleted=="1") {
                                //string deleteButtonHtml2 = "<span class=\"badge text-bg-danger\">Deleted</span>";
                                //string deleteButtonHtml = "<a href=\"\" class=\"btn btn-danger\">";
                                //string deleteButtonHtml = "<span class='btn btn-danger btn-icon' style='padding:1px;height: 39px;'>Deleted</span>";
                                string deleteButtonHtml = "<span class=\"badge rounded-pill text-bg-danger\">Deleted</span>";
                                //deleteButtonHtml += "</a>";

                                string syncButtonHtml = "<a href=\"#\" class=\"btn btn-warning\" onclick=\"syncSingle(" + personId + ")\">";
                                syncButtonHtml += "<span class='btn-icon'><span class='material-icons'>sync</span></span>";
                                syncButtonHtml += "</a>";

                                actionButtons.Text = syncButtonHtml + " " + deleteButtonHtml;
                            }
                            else
                            {
                                string deleteButtonHtml = "<a href=\"\" class=\"btn btn-danger\" onclick=\"confirmDelete(event," + personId + ")\">";
                                deleteButtonHtml += "<span class='btn-icon'><span class='material-icons'>delete</span></span>";
                                deleteButtonHtml += "</a>";

                                string syncButtonHtml = "<a href=\"#\" class=\"btn btn-warning\" onclick=\"syncSingle(" + personId + ")\">";
                                syncButtonHtml += "<span class='btn-icon'><span class='material-icons'>sync</span></span>";
                                syncButtonHtml += "</a>";

                                actionButtons.Text = syncButtonHtml + " " + deleteButtonHtml;
                            }

                            
                            // Combine Edit and Delete buttons

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


        private void FillSpan()
        {
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(id) as jumlah FROM persons WHERE (is_synced = 0 OR is_updated = 1) AND is_approved = 1 AND image_file IS NOT NULL", con))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                TextBox1.Text= dr["jumlah"].ToString();
                                var badgeSpan = FindControl("badgeSpan") as HtmlGenericControl;
                                if (badgeSpan != null)
                                {
                                    badgeSpan.InnerText = dr["jumlah"].ToString();
                                }
                                else
                                {
                                    //Response.Write("Ga masuk");
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                }
            }
            
        }
        [WebMethod]
        public static async Task<string> UploadFile()
        {
            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files["file"];  // Ensure the key is 'file'
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = HttpContext.Current.Server.MapPath("~/Uploads/" + fileName);

                    // Save the file locally
                    file.SaveAs(filePath);

                    // Upload the file to the API
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:3000/employee/import");
                        using (var content = new MultipartFormDataContent())
                        {
                            content.Add(new StreamContent(File.OpenRead(filePath)), "file", fileName);  // Ensure the key is 'file'

                            HttpResponseMessage response = await client.PostAsync("upload", content);  // Ensure POST method
                            string responseString = await response.Content.ReadAsStringAsync();  // Read the response content

                            if (response.IsSuccessStatusCode)
                            {
                                return "File uploaded successfully! API Response: " + responseString;
                            }
                            else
                            {
                                return "Error uploading file to the API. API Response: " + responseString;
                            }
                        }
                    }
                }
                return "No file uploaded.";
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return "An error occurred: " + ex.Message;
            }
        }

    }
}