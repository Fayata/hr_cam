using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Web.UI;
using static Mysqlx.Expect.Open.Types.Condition.Types;

namespace hr_cam
{
    public partial class cobe_people : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        string UrlImage = ConfigurationManager.AppSettings["urlImagePerson"];
        //private int pageSize = 10; // Number of records per page
        //private int currentPage = 1; // Current page index, set initially to 1
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlPageSize.SelectedValue = PageSize.ToString();
                FillTableFromDatabase();
                UpdatePaginationControls();
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

                if (ImageFilter == "Exists")
                {
                    filterQuery = "WHERE image_file IS NOT NULL AND image_file <> ''";
                }
                else if (ImageFilter == "NotExists")
                {
                    filterQuery = "WHERE image_file IS NULL OR image_file = ''";
                }

                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    string searchTerm = txtSearch.Text.Trim();
                    searchQuery = $@"
                {(string.IsNullOrEmpty(filterQuery) ? "WHERE" : "AND")}
                (identification_number LIKE '%{searchTerm}%' OR 
                 name LIKE '%{searchTerm}%' OR 
                 gender LIKE '%{searchTerm}%' OR 
                 type LIKE '%{searchTerm}%')";
                }

                string query = $"SELECT * FROM persons {filterQuery} {searchQuery} LIMIT {PageSize} OFFSET {offset}";


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

                            TableCell imageCell = new TableCell();
                            string personId = dr["id"].ToString();
                            string image_file = dr["image_file"].ToString();
                            string key = personId; // Ganti dengan nilai yang sesuai
                            string imagePath = UrlImage + image_file;
                            if (File.Exists(imagePath))
                            {
                                byte[] imageBytes = File.ReadAllBytes(imagePath);
                                string base64String = Convert.ToBase64String(imageBytes);
                                //<img src='data:image/png;base64, {base64String}' alt='icon title' />
                                //<img src='person_image.ashx?fileName={image_file}' alt='Person Image' />
                                string modalHtml = $@"
                                <a href='#' class='btn btn-success' data-bs-toggle='modal' data-bs-target='#modal-face-snapshot-
                                {key}'>View Image</a>
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
                                $"<a href=\"#\" class=\"btn btn-danger\" onclick=\"confirmDelete(event,{personId})\"><span class='btn-icon'><span class='material-icons'>delete</span></span></a>";

                            string syncButtonHtml = $"<a href=\"#\" class=\"btn btn-warning\" onclick=\"syncSingle({personId})\"><span class='btn-icon'><span class='material-icons'>sync</span></span></a>";

                            actionCell.Controls.Add(new LiteralControl(syncButtonHtml + " " + deleteButtonHtml));
                            row.Cells.Add(actionCell);

                            TableBody.Rows.Add(row);
                        }
                    }
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
                if (ImageFilter == "Exists")
                {
                    filterQuery = "WHERE image_file IS NOT NULL AND image_file <> ''";
                }
                else if (ImageFilter == "NotExists")
                {
                    filterQuery = "WHERE image_file IS NULL OR image_file = ''";
                }
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    string searchTerm = txtSearch.Text.Trim();
                    searchQuery = $@"
                {(string.IsNullOrEmpty(filterQuery) ? "WHERE" : "AND")}
                (identification_number LIKE '%{searchTerm}%' OR 
                 name LIKE '%{searchTerm}%' OR 
                 gender LIKE '%{searchTerm}%' OR 
                 type LIKE '%{searchTerm}%')";
                }
                // Use the same filter query for counting records
                using (MySqlCommand cmd = new MySqlCommand($"SELECT COUNT(*) FROM persons {filterQuery} {searchQuery}", con))
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

    }
}
