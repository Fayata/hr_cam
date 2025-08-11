using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class vehicle : System.Web.UI.Page
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
                        DropDownList2.Items.Add(new ListItem("All", "all"));
                        DropDownList2.Items.Add(new ListItem("Comply", "0"));
                        DropDownList2.Items.Add(new ListItem("Not Approved", "1"));
                        //DropDownList2.Items.Add(new ListItem("", "2"));
                        DropDownList2.Items.Add(new ListItem("Unrecognized", "no"));
                        if (Session["status_vehicle"] != null)
                        {
                            DropDownList2.SelectedValue = Session["status_vehicle"].ToString();
                        }
                        if (Session["plate_vehicle"] != null)
                        {
                            TextBox3.Text = Session["plate_vehicle"].ToString();
                        }
                        FillTableFromDatabase();
                        FillSpan();
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
            string status_vehicle = DropDownList2.SelectedValue;
            string plate_vehicle = TextBox3.Text;
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                con.Open();
                string query = @"SELECT v.id, v.plate_number, v.status, v.type as v_type, v.is_deleted, v2.type, v2.owner, v2.brand, v2.model, vh.entry_code, vh.created_at as last_updated FROM vehicles v LEFT JOIN vehicle_details v2 ON v.id=v2.vehicle_id JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id)";
                if (plate_vehicle != "")
                {
                    query += @" AND v.plate_number like '%" + plate_vehicle + "%'";
                }
                if (status_vehicle != "all")
                {
                    if (status_vehicle == "no")
                    {
                        //Response.Write("masuk"+status_vehicle);
                        query += @" AND v.plate_number not in (SELECT plate_number from vehicles where id in (select vehicle_id from vehicle_history))";
                    }
                    else
                    {
                        query += @" AND vh.entry_code='" + status_vehicle + "'";
                    }
                }
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string police_no = dr["plate_number"].ToString();
                            string vehicle = dr["type"].ToString();
                            string owner_name = dr["owner"].ToString();
                            string brand = dr["brand"].ToString();
                            string model = dr["model"].ToString();
                            string vehicleId = dr["id"].ToString();
                            string type = dr["v_type"].ToString();
                            string code_vehicle = dr["entry_code"].ToString();
                            string is_deleted = dr["is_deleted"].ToString();
                            string last_updated = dr["last_updated"].ToString();
                            string statusnya = "";
                            if (code_vehicle == "0")
                            {
                                statusnya += "Comply";
                            }
                            else if (code_vehicle == "1")
                            {
                                statusnya += "Not Approved";
                            }
                            else if (code_vehicle == "2")
                            {
                                statusnya += "";
                            }
                            TableRow row = new TableRow();

                            TableCell typeCell = new TableCell() { Text = type };
                            row.Cells.Add(typeCell);

                            TableCell police_noCell = new TableCell() { Text = police_no };
                            row.Cells.Add(police_noCell);
                            //police_noCell.Text = police_no;

                            TableCell vehicleCell = new TableCell() { Text = vehicle };
                            //vehicleCell.Text = vehicle;
                            row.Cells.Add(vehicleCell);


                            TableCell owner_nameCell = new TableCell() { Text = owner_name };
                            //owner_nameCell.Text = owner_name;
                            row.Cells.Add(owner_nameCell);

                            TableCell brandCell = new TableCell() { Text = brand };
                            //brandCell.Text = brand;
                            row.Cells.Add(brandCell);

                            TableCell modelCell = new TableCell() { Text = model };
                            //modelCell.Text = model;
                            row.Cells.Add(modelCell);

                            TableCell statusCell = new TableCell() { Text = statusnya };
                            //modelCell.Text = model;
                            row.Cells.Add(statusCell);
                            TableCell lastupdatedCell = new TableCell() { Text = last_updated };
                            row.Cells.Add(lastupdatedCell);




                            // Tambahkan tombol edit dengan ikon Font Awesome
                            //string editUrl = "edit_user_admin.aspx?id=" + userId;
                            //TableCell editCell = new TableCell();
                            //editCell.Style["width"] = "100px";
                            //LiteralControl editButton = new LiteralControl();
                            //editButton.Text = "<a href='"+editUrl+"' class='btn btn-warning'>";
                            //editButton.Text += "<span class='btn-icon'><i class='fa-solid fa-pencil'></i></span>";
                            //editButton.Text += "</a>";
                            //editCell.Controls.Add(editButton);
                            //row.Cells.Add(editCell);

                            //string deleteUrl = "delete_user_admin.aspx?id=" + userId;
                            //TableCell deleteCell = new TableCell();
                            //deleteCell.Style["width"] = "100px";
                            //LiteralControl deleteButton = new LiteralControl();
                            //deleteButton.Text = "<a href=\"" + deleteUrl + "\" class=\"btn btn-danger\" onclick=\"return confirm('Are you sure?');\">";
                            //deleteButton.Text += "<span class='btn-icon'><i class='fa-solid fa-trash-can'></i></span>";
                            //deleteButton.Text += "</a>";
                            //deleteCell.Controls.Add(deleteButton);
                            //row.Cells.Add(deleteCell);

                            string deleteUrl = "delete_vehicle.aspx?id=" + vehicleId;

                            TableCell actionCell = new TableCell();
                            actionCell.Style["width"] = "200px"; // Adjust width as needed
                            LiteralControl actionButtons = new LiteralControl();

                            // Create the Delete button HTML
                            //string deleteButtonHtml = "<a href=\"" + deleteUrl + "\" class=\"btn btn-danger\" onclick=\"return confirm('Are you sure?');\">";
                            //deleteButtonHtml += "<span class='btn-icon'><i class='fa-solid fa-trash-can'></i></span>";
                            //deleteButtonHtml += "</a>";
                            if (is_deleted == "True" || is_deleted == "1")
                            {
                                string deleteButtonHtml = "<span class=\"badge rounded-pill text-bg-danger\">Deleted</span>";

                                string syncButtonHtml = "<a href=\"#\" class=\"btn btn-warning\" onclick=\"syncSingle(" + vehicleId + ")\">";
                                syncButtonHtml += "<span class='btn-icon'><i class='fa-solid fa-arrows-rotate'></i></span>";
                                syncButtonHtml += "</a>";
                                // Combine Edit and Delete buttons
                                actionButtons.Text = syncButtonHtml + " " + deleteButtonHtml;
                            }
                            else
                            {
                                string deleteButtonHtml = "<a href=\"\" class=\"btn btn-danger\" onclick=\"confirmDelete(event," + vehicleId + ")\">";
                                deleteButtonHtml += "<span class='btn-icon'><i class='fa-solid fa-trash-can'></i></span>";
                                deleteButtonHtml += "</a>";

                                string syncButtonHtml = "<a href=\"#\" class=\"btn btn-warning\" onclick=\"syncSingle(" + vehicleId + ")\">";
                                syncButtonHtml += "<span class='btn-icon'><i class='fa-solid fa-arrows-rotate'></i></span>";
                                syncButtonHtml += "</a>";
                                // Combine Edit and Delete buttons
                                actionButtons.Text = syncButtonHtml + " " + deleteButtonHtml;
                            }

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

        protected void Button3_Click(object sender, EventArgs e)
        {
            string status = DropDownList2.SelectedValue;
            string plate = TextBox3.Text;
            Session["status_vehicle"] = status;
            Session["plate_vehicle"] = plate;
            Response.Redirect("vehicle.aspx");
        }

        private void FillSpan()
        {
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(id) as jumlah FROM vehicles WHERE is_synced = 0 OR is_updated = 1", con))
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
    }
}