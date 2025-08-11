using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace hr_cam
{
    public partial class entry_people : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        string UrlImage = ConfigurationManager.AppSettings["urlImagePerson"];
        string UrlImage2 = ConfigurationManager.AppSettings["urlImagePerson2"];
        string apiurlreport = ConfigurationManager.AppSettings["ApiUrlReport"];
        string api_user = ConfigurationManager.AppSettings["ApiUser"];
        string api_password = ConfigurationManager.AppSettings["ApiPassword"];
        private int PageSize
        {
            get
            {
                if (ViewState["PageSize"] == null)
                {
                    ViewState["PageSize"] = 10; // Default page size
                }
                return (int)ViewState["PageSize"];
            }
            set
            {
                ViewState["PageSize"] = value;
            }

        }
        protected int currentPage
        {
            get
            {
                // If ViewState is null, default to page 1
                if (ViewState["CurrentPage"] == null)
                {
                    ViewState["CurrentPage"] = 1;
                }
                return (int)ViewState["CurrentPage"];
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }
        protected string ImageFilter
        {
            get
            {
                return ddlImageFilter.SelectedValue;
            }
        }
        protected string StatusFilter
        {
            get
            {
                return ddlStatusFilter.SelectedValue;
            }
        }
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
                        string action = Request.QueryString["action"];
                        string personId = Request.QueryString["id"];

                        if (action == "update" && !string.IsNullOrEmpty(personId))
                        {
                            // Jalankan fungsi update status berdasarkan personId
                            UpdateStatus(personId);
                        }
                        ddlPageSize.SelectedValue = PageSize.ToString();
                        FillTableFromDatabase();
                        UpdatePaginationControls();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Reload the data based on the search term
            currentPage = 1; // Reset to first page whenever search is initiated
            FillTableFromDatabase();
            UpdatePaginationControls();
        }

        private void FillTableFromDatabase()
        {
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                con.Open();

                int offset = (currentPage - 1) * PageSize;
                string filterQuery = "";
                string searchQuery = "";
                string status = "";

                //if (ImageFilter == "Exists")
                //{
                //    filterQuery = "WHERE image_file IS NOT NULL AND image_file <> ''";
                //}
                //else if (ImageFilter == "NotExists")
                //{
                //    filterQuery = "WHERE image_file IS NULL OR image_file = ''";
                //}

                //if (!string.IsNullOrEmpty(txtSearch.Text))
                //{
                //    string searchTerm = txtSearch.Text.Trim();
                //    searchQuery = $@"
                //{(string.IsNullOrEmpty(filterQuery) ? "WHERE" : "AND")}
                //(identification_number LIKE '%{searchTerm}%' OR 
                // name LIKE '%{searchTerm}%' OR 
                // gender LIKE '%{searchTerm}%' OR 
                // type LIKE '%{searchTerm}%')";
                //}
                if (ImageFilter == "Exists")
                {
                    filterQuery = "AND image_file IS NOT NULL AND image_file != ''";
                }
                else if (ImageFilter == "NotExists")
                {
                    filterQuery = "AND image_file IS NULL OR image_file = ''";
                }
                if (StatusFilter == "All")
                {
                }
                else
                {
                    status = $@"AND ph.entry_code={StatusFilter}";
                }

                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    string searchTerm = txtSearch.Text.Trim();
                    searchQuery = $@" AND
                (identification_number LIKE '%{searchTerm}%' OR 
                 name LIKE '%{searchTerm}%' OR 
                 gender LIKE '%{searchTerm}%' OR 
                 type LIKE '%{searchTerm}%')";
                }

                //string query = $"SELECT * FROM persons {filterQuery} {searchQuery} LIMIT {PageSize} OFFSET {offset}";
                string query = $"SELECT p.id ,p.identification_number ,p.name ,p.gender ,p.type ,p.image_file ,p.is_deleted, ph.entry_code, ph.created_at as last_updated from persons p left join person_history ph on p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id =p.id) WHERE 1 {filterQuery} {searchQuery} {status} group by p.identification_number LIMIT {PageSize} OFFSET {offset}";
                //Response.Write(query);

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        TableBody.Rows.Clear(); // Clear existing rows
                        TableHeaderRow headerRow = new TableHeaderRow();
                        headerRow.Cells.Add(new TableHeaderCell { Text = "Identification Number" });
                        headerRow.Cells.Add(new TableHeaderCell { Text = "Name" });
                        headerRow.Cells.Add(new TableHeaderCell { Text = "Gender" });
                        headerRow.Cells.Add(new TableHeaderCell { Text = "Type" });
                        headerRow.Cells.Add(new TableHeaderCell { Text = "Entry Code" });
                        headerRow.Cells.Add(new TableHeaderCell { Text = "Last Updated" });
                        headerRow.Cells.Add(new TableHeaderCell { Text = "Image" });
                        headerRow.Cells.Add(new TableHeaderCell { Text = "Actions" });
                        TableBody.Rows.Add(headerRow);
                        while (dr.Read())
                        {
                            TableRow row = new TableRow();

                            TableCell identificationCell = new TableCell { Text = dr["identification_number"].ToString() };
                            row.Cells.Add(identificationCell);

                            TableCell nameCell = new TableCell { Text = dr["name"].ToString() };
                            row.Cells.Add(nameCell);

                            TableCell genderCell = new TableCell { Text = dr["gender"].ToString() };
                            row.Cells.Add(genderCell);

                            TableCell typeCell = new TableCell { Text = dr["type"].ToString() };
                            row.Cells.Add(typeCell);
                            string code = dr["entry_code"].ToString();
                            string statusnya = "";
                            if (code == "0")
                            {
                                statusnya += "Comply";
                            }
                            else if (code == "1")
                            {
                                statusnya += "Not Comply or Badge Expired";
                            }
                            else if (code == "2")
                            {
                                statusnya += "FTW Rejected Medical / Expired";
                            }
                            else if (code == "3")
                            {
                                statusnya += "Daily Checkup Failed";
                            }
                            TableCell entryCell = new TableCell { Text = statusnya };
                            row.Cells.Add(entryCell);

                            TableCell lastupdatedCell = new TableCell { Text = dr["last_updated"].ToString() };
                            row.Cells.Add(lastupdatedCell);

                            TableCell imageCell = new TableCell();
                            string personId = dr["id"].ToString();
                            string image_file = dr["image_file"].ToString();
                            string key = personId; // Ganti dengan nilai yang sesuai
                            string imagePath = UrlImage2 + image_file;
                            if (File.Exists(imagePath) || image_file != "")
                            {
                                byte[] imageBytes = File.ReadAllBytes(imagePath);
                                string base64String = Convert.ToBase64String(imageBytes);
                                //<img src='data:image/png;base64, {base64String}' alt='icon title' />
                                //<img src='person_image.ashx?fileName={image_file}' alt='Person Image' />
                                //string modalHtml = $@"
                                //<a href='#' class='btn btn-success' data-bs-toggle='modal' data-bs-target='#modal-face-snapshot-
                                //{key}'>View Image</a>
                                //<div class='modal modal-blur fade' id='modal-face-snapshot-{key}' tabindex='-1' role='dialog' aria-hidden='true'>
                                //    <div class='modal-dialog modal-dialog-centered modal-dialog-scrollable' role='document'>
                                //        <div class='modal-content'>
                                //            <div class='modal-header'>
                                //                <h5 class='modal-title'>Image</h5>
                                //                <button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='Close'></button>
                                //            </div>
                                //            <div class='modal-body'>
                                //                <img src='person_image/{image_file}' alt='icon title' />
                                //            </div>
                                //            <div class='modal-footer'>
                                //                <button type='button' class='btn me-auto' data-bs-dismiss='modal'>Close</button>
                                //            </div>
                                //        </div>
                                //    </div>
                                //</div>";

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
                                imageCell.Controls.Add(new LiteralControl("<a href=\"#\" class=\"btn btn-primary\" onclick=\"selectImage(" + dr["id"].ToString() + ")\">Add Image</a>"));
                            }
                            row.Cells.Add(imageCell);



                            TableCell actionCell = new TableCell();
                            //string personId = dr["id"].ToString();
                            string isDeleted = dr["is_deleted"].ToString();

                            string deleteButtonHtml = isDeleted == "True" ?
                                "<span class=\"badge rounded-pill text-bg-danger\">Deleted</span>" :
                                $"<a href=\"#\" class=\"btn btn-danger\" onclick=\"confirmDelete(event,{personId})\"><span class='btn-icon'><i class='fa-solid fa-trash-can'></i></span></a>";

                            string syncButtonHtml = $"<a href=\"#\" class=\"btn btn-warning\" onclick=\"syncSingle({personId})\"><span class='btn-icon'><i class='fa-solid fa-arrows-rotate'></i></span></a>";
                            //LinkButton updateButtonHtml = new LinkButton();
                            //updateButtonHtml.Text = "<span class='btn-icon'>Update Status</span>";
                            //updateButtonHtml.CssClass = "btn btn-primary";
                            //updateButtonHtml.CommandArgument = dr["identification_number"].ToString(); // ini ID-nya
                            //updateButtonHtml.CommandName = "UpdateStatus";
                            //updateButtonHtml.OnClientClick = "return false;"; // optional: cegah reload jika butuh

                            //updateButtonHtml.Click += new EventHandler(UpdateStatus_Click);

                            //string updateButtonHtml = $"<a href=\"#\" class=\"btn btn-primary\"><span class='btn-icon'>Update Status</i></span></a>";
                            string updateButtonHtml = $"<a href=\"entry_people.aspx?action=update&id={dr["identification_number"].ToString()}\" class=\"btn btn-primary\"><span class='btn-icon'>Update Status</span></a>";
                            if (code == "3")
                            {
                                actionCell.Controls.Add(new LiteralControl(updateButtonHtml + " " + syncButtonHtml + " " + deleteButtonHtml));
                            }
                            else
                            {
                                actionCell.Controls.Add(new LiteralControl(syncButtonHtml + " " + deleteButtonHtml));

                            }
                            row.Cells.Add(actionCell);

                            TableBody.Rows.Add(row);
                        }
                    }
                }
            }
        }

        //private void UpdateStatusLama(string id)
        //{
        //    string person_id = id.Substring(4);
        //    var jsonObject = new
        //    {
        //        id = person_id,
        //        status = true,
        //    };


        //    var json = new JavaScriptSerializer().Serialize(jsonObject);

        //    //Response.Write(json);

        //    using (var client = new HttpClient())
        //    {
        //        var byteArray = Encoding.ASCII.GetBytes(api_user + ":" + api_password);
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        //        var content = new StringContent(json, encoding: Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PostAsync(apiurlreport + "/v2/persons/daily-check-up", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string responseData = await response.Content.ReadAsStringAsync();
        //            Session["success"] = "On process to get report";
        //            Response.Redirect("list_report.aspx");
        //            //lblOutput.Text = $"Response: {responseData}";
        //        }
        //        else
        //        {
        //            string responseBody = await response.Content.ReadAsStringAsync();

        //            var jsonData = (JObject)JsonConvert.DeserializeObject(responseBody);
        //            Session["fail"] = "Error: " + jsonData["message"];
        //            Response.Redirect("list_report.aspx");
        //            //lblOutput.Text = "Error: " + response.Content + ". json: " + json;
        //        }
        //    }
        //    // result = "123"
        //    //Response.Write("<script>alert('id nya: " + person_id + "')</script>");
        //    // Logic update ke DB atau lainnya
        //    // Contoh:
        //    // MyDb.UpdateStatusById(id);

        //    // Misal redirect balik setelah selesai
        //    //Response.Redirect("YourPage.aspx?msg=updated");
        //}

        private void UpdateStatus(string id)
        {
            string person_id = id.Substring(4);
            //var jsonObject = new
            //{
            //    id = person_id,
            //    status = true,
            //};

            //var json = new JavaScriptSerializer().Serialize(jsonObject);
            var jsonObject = new
            {
                id = person_id,
                status = true,
            };

            // Bungkus dalam list/array
            var jsonList = new[] { jsonObject };

            // Serialize sebagai array
            var json = new JavaScriptSerializer().Serialize(jsonList);

            using (var client = new HttpClient())
            {
                var byteArray = Encoding.ASCII.GetBytes(api_user + ":" + api_password);
                //var byteArray = Encoding.ASCII.GetBytes("walfa:kosong");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var content = new StringContent(json, encoding: Encoding.UTF8, "application/json");

                //HttpResponseMessage response = client.PostAsync("http://10.231.37.134:3001/v2/persons/daily-check-up", content).GetAwaiter().GetResult();
                HttpResponseMessage response = client.PostAsync(apiurlreport + "/v2/persons/daily-check-up", content).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string responseData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Session["success"] = "Entry Code successfully changed";
                    Response.Redirect("entry_people.aspx");
                }
                else
                {
                    string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //var jsonData = (JObject)JsonConvert.DeserializeObject(responseBody);
                    Session["fail"] = "Error: " + responseBody;
                    Response.Redirect("entry_people.aspx");
                }
            }
        }

        private void UpdatePaginationControls()
        {
            lblPageNumber.Text = $"Page {currentPage} of {GetTotalPages()}";
            lnkPrevious.Enabled = currentPage > 1;
            lnkNext.Enabled = currentPage < GetTotalPages();
        }

        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                FillTableFromDatabase();
                UpdatePaginationControls();
            }
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (currentPage < GetTotalPages())
            {
                currentPage++;
                FillTableFromDatabase();
                UpdatePaginationControls();
            }
        }

        private int GetTotalPages()
        {
            int totalRecords = 0;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                con.Open();
                string filterQuery = "";
                string searchQuery = "";
                string status = "";
                if (ImageFilter == "Exists")
                {
                    filterQuery = "AND image_file IS NOT NULL AND image_file != ''";
                }
                else if (ImageFilter == "NotExists")
                {
                    filterQuery = "AND image_file IS NULL OR image_file = ''";
                }
                if (StatusFilter == "All")
                {
                }
                else
                {
                    status = $@"AND ph.entry_code={StatusFilter}";
                }

                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    string searchTerm = txtSearch.Text.Trim();
                    searchQuery = $@" AND
                (identification_number LIKE '%{searchTerm}%' OR 
                 name LIKE '%{searchTerm}%' OR 
                 gender LIKE '%{searchTerm}%' OR 
                 type LIKE '%{searchTerm}%')";
                }

                //string query = $"SELECT * FROM persons {filterQuery} {searchQuery} LIMIT {PageSize} OFFSET {offset}";
                string query = $"SELECT count(*), p.id ,p.identification_number ,p.name ,p.gender ,p.type ,p.image_file ,p.is_deleted, ph.entry_code, ph.created_at as last_updated from persons p left join person_history ph on p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id =p.id) WHERE 1 {filterQuery} {searchQuery} {status}";
                // Use the same filter query for counting records
                //using (MySqlCommand cmd = new MySqlCommand($"SELECT COUNT(*) FROM persons {filterQuery} {searchQuery}", con))
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    totalRecords = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            // Calculate total pages based on filtered record count
            return (int)Math.Ceiling((double)totalRecords / PageSize);
        }


        protected void ddlImageFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1; // Reset to first page whenever filter changes
            FillTableFromDatabase();
            UpdatePaginationControls();
        }
        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1; // Reset to first page whenever filter changes
            FillTableFromDatabase();
            UpdatePaginationControls();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set PageSize to the selected value in the dropdown
            PageSize = int.Parse(ddlPageSize.SelectedValue);

            // Reset current page to 1 after changing page size
            currentPage = 1;

            // Reload the data with the new page size
            FillTableFromDatabase();

            // Update pagination controls to reflect the new page size and current page
            UpdatePaginationControls();
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
                                TextBox1.Text = dr["jumlah"].ToString();
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