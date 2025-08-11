using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class Report_vehicle_detection_pdf : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string UrlImage = ConfigurationManager.AppSettings["urlImage"];
        string UrlLogo = ConfigurationManager.AppSettings["urlLogo"];
        protected void Page_Load(object sender, EventArgs e)
        {
            string imagePath = UrlLogo + "pertamina_logo2.png";
            string imagePath2 = UrlLogo + "logo.png";

            // Membaca gambar dari file lokal menjadi array byte
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            byte[] imageBytes2 = File.ReadAllBytes(imagePath2);

            // Mengonversi array byte gambar ke format base64
            string base64String = Convert.ToBase64String(imageBytes);
            string base64String2 = Convert.ToBase64String(imageBytes2);

            // Membuat string untuk src image dengan format base64
            string srcBase64 = $"data:image/jpeg;base64,{base64String}";
            string src2Base64 = $"data:image/jpeg;base64,{base64String2}";

            // Menetapkan nilai src dari elemen img di code.aspx
            logonya.Attributes["src"] = srcBase64;
            logo2.Attributes["src"] = src2Base64;
            if (!IsPostBack)
            {
                //Response.Write("camera site:"+Session["site_vehicle_detection"].ToString());
                //Response.Write("camera:"+Session["camera"].ToString());
                //Response.Write("from_vehicle_detection:"+Session["from_vehicle_detection"].ToString());
                //Response.Write("to_vehicle_detection:"+Session["to_vehicle_detection"].ToString());
                FillEventHistory();
            }
        }

        private void FillEventHistory()
        {
            string camera = Session["camera_vehicle_detection"].ToString();
            if (camera != "0")
            {
                int jumlahCamera = Convert.ToInt32(camera);
                string from = Session["from_vehicle_detection"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_vehicle_detection"] +"')</script>");
                if (Session["to_vehicle_detection"].ToString() != "")
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_vehicle_detection"].ToString();
                    DateTime parsedDateTime2 = DateTime.Parse(to);
                    to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    //Response.Write("<script>alert('kosong')</script>");

                }

                //    DateTime fromDate;
                //DateTime toDate;
                int x = 0;

                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (camera == "0")
                    {

                        Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                    }
                    else
                    {

                        string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code,v.plate_number as plate, ce.plate_number FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) ";
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_vehicle_detection0"].ToString() == "all")
                            {
                                query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                            }
                            else
                            {
                                query += @" AND (";
                                for (int i = 0; i < jumlahCamera; i++)
                                {
                                    if (i == 0)
                                    {
                                        query += $"c.id=@camera{i}";
                                    }
                                    else
                                    {
                                        query += " OR ";
                                        query += $"c.id=@camera{i}";

                                    }
                                }
                                query += @")";
                            }
                        }
                        if (from_date != null)
                        {
                            query += @" AND ce.occurred_at >= @from ";
                        }
                        if (to_date != null)
                        {
                            query += @" AND ce.occurred_at <= @to ";
                        }
                        query += @" AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_vehicle_detection0"].ToString() == "all")
                                {

                                }
                                else
                                {
                                    for (int i = 0; i < jumlahCamera; i++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{i}", Session["camera_vehicle_detection" + i].ToString());
                                    }
                                }
                            }
                            if (from_date != null)
                            {
                                cmd.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date != null)
                            {
                                cmd.Parameters.AddWithValue("@to", to_date);
                            }
                            cmd.Prepare();
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        x++;
                                        Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                        string image_file = dr["image_file"].ToString();
                                        string plate_number_file = dr["plate_number_file"].ToString();
                                        DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                        string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                        string person = dr["person"].ToString();
                                        string location = dr["location"].ToString();
                                        string event_type = dr["event_type"].ToString();
                                        string site = dr["site"].ToString();
                                        string person_type = dr["person_type"].ToString();
                                        string code = dr["entry_code"].ToString();
                                        string plate_number = dr["plate_number"].ToString();
                                        string statusnya = "";
                                        if (code == "" || code == null)
                                        {
                                            statusnya += "Unrecognized";
                                        }
                                        else
                                        {
                                            if (code == "0")
                                            {
                                                statusnya += "Comply";
                                            }
                                            else if (code == "1")
                                            {
                                                statusnya += "Not Approved";
                                            }
                                            else if (code == "2")
                                            {
                                                statusnya += "";
                                            }
                                        }
                                        TableRow row = new TableRow();
                                        //TableCell no = new TableCell();
                                        //no.Text = x.ToString();
                                        TableCell no = new TableCell() { Text = x.ToString() };
                                        TableCell imageCell = new TableCell();
                                        TableCell imageCell2 = new TableCell();
                                        TableCell pers = new TableCell();
                                        int cek = event_type.IndexOf("Plate");

                                        if (cek != -1)
                                        {
                                            if (!string.IsNullOrEmpty(plate_number_file))
                                            {
                                                //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                                                //img.ImageUrl = "data:image/png;base64," + base64String;
                                                //img.AlternateText = "icon title";
                                                //img.Style.Add("width", "150px");
                                                //imageCell.Controls.Add(img);
                                                // string imagePath = UrlImage + image_file;
                                                // byte[] imageBytes = File.ReadAllBytes(imagePath);
                                                // string base64String = Convert.ToBase64String(imageBytes);
                                                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                                {
                                                    ImageUrl = UrlImage + image_file,
                                                    AlternateText = "icon title"
                                                };
                                                img.Style.Add("width", "150px");
                                                imageCell.Controls.Add(img);
                                                // string imagePath2 = UrlImage + plate_number_file;
                                                // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
                                                // string base64String2 = Convert.ToBase64String(imageBytes2);
                                                System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                                {
                                                    ImageUrl = UrlImage + plate_number_file,
                                                    AlternateText = "icon title"
                                                };
                                                img2.Style.Add("width", "150px");
                                                imageCell2.Controls.Add(img2);
                                                //row.Cells.Add(imageCell);
                                                //row.Cells.Add(imageCell2);
                                            }
                                            pers.Text = "";
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(image_file))
                                            {
                                                // string imagePath = UrlImage + image_file;
                                                // byte[] imageBytes = File.ReadAllBytes(imagePath);
                                                // string base64String = Convert.ToBase64String(imageBytes);
                                                //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                                                //img.ImageUrl = "data:image/png;base64," + base64String;
                                                //img.AlternateText = "icon title";
                                                //img.Style.Add("width", "150px");
                                                //imageCell.Controls.Add(img);
                                                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                                {
                                                    ImageUrl = UrlImage + image_file,
                                                    AlternateText = "icon title"
                                                };
                                                img.Style.Add("width", "150px");
                                                imageCell.Controls.Add(img);
                                            }
                                            pers.Text = person;
                                        }
                                        //TableCell occurred = new TableCell();
                                        //occurred.Text = occurred_at;
                                        //TableCell loc = new TableCell();
                                        //loc.Text = location;
                                        //TableCell sites = new TableCell();
                                        //sites.Text = site;
                                        //TableCell status = new TableCell();
                                        //status.Text = statusnya;
                                        TableCell occurred = new TableCell() { Text = occurred_at };
                                        TableCell loc = new TableCell() { Text = location };
                                        TableCell sites = new TableCell() { Text = site };
                                        TableCell status = new TableCell() { Text = statusnya };
                                        TableCell plate = new TableCell() { Text = plate_number };
                                        row.Cells.Add(no);
                                        row.Cells.Add(imageCell);
                                        row.Cells.Add(imageCell2);
                                        row.Cells.Add(plate);
                                        row.Cells.Add(occurred);
                                        row.Cells.Add(loc);
                                        row.Cells.Add(sites);
                                        //row.Cells.Add(pers);
                                        row.Cells.Add(status);


                                        TableBody.Controls.Add(row);
                                        if (to_date == null)
                                        {
                                            //DateTime occurredAt = (DateTime)dr["occurred_at"];
                                            string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                            Label2.Text = formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                        }
                                        else if (from_date == null)
                                        {
                                            if (x.ToString() == "1")
                                            {
                                                //DateTime occurredAt = (DateTime)dr["occurred_at"];
                                                string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                                Label1.Text = formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                            }
                                        }
                                    }
                                    dr.Close();
                                }
                                else
                                {
                                }
                            }
                        }
                        if (from_date != null && to_date != null)
                        {
                            string per1 = from_date.ToString();
                            string per2 = to_date.ToString();
                            Label1.Text = per1.Substring(8, 2) + "-" + per1.Substring(5, 2) + "-" + per1.Substring(0, 4);
                            Label2.Text = per2.Substring(8, 2) + "-" + per2.Substring(5, 2) + "-" + per2.Substring(0, 4);
                        }
                        else if (from_date != null)
                        {
                            string per1 = from_date.ToString();
                            Label1.Text = per1.Substring(8, 2) + "-" + per1.Substring(5, 2) + "-" + per1.Substring(0, 4);
                        }
                        else if (to_date != null)
                        {
                            string per2 = to_date.ToString();
                            Label2.Text = per2.Substring(8, 2) + "-" + per2.Substring(5, 2) + "-" + per2.Substring(0, 4);
                        }


                    }
                }
            }
        }
        //private void FillEventHistory()
        //{
        //    string camera_site = Session["site_vehicle_detection"].ToString();
        //    string from = Session["from_vehicle_detection"].ToString();
        //    string to = null;
        //    if (Session["to_vehicle_detection"] != null)
        //    {
        //        to = Session["to_vehicle_detection"].ToString();
        //    }

        //    DateTime fromDate;
        //    DateTime toDate;
        //    int x = 0;

        //    using (MySqlConnection con = new MySqlConnection(strcon))
        //    {
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        if (camera_site == "none")
        //        {

        //            Response.Write("<script>alert('Camera Site cannot be empty')</script>");
        //        }
        //        else
        //        {
        //            if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
        //            {
        //                string per1 = from.ToString();
        //                string per2 = to.ToString();
        //                Label1.Text = per1.Substring(8, 2) + "-" + per1.Substring(5, 2) + "-" + per1.Substring(0, 4);
        //                Label2.Text = per2.Substring(8, 2) + "-" + per2.Substring(5, 2) + "-" + per2.Substring(0, 4);
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id  WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from AND DATE(ce.occurred_at) <=@to AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC", con))
        //                {
        //                    cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                    cmd.Parameters.AddWithValue("@from", from);
        //                    cmd.Parameters.AddWithValue("@to", to);
        //                    cmd.Prepare();
        //                    using (MySqlDataReader dr = cmd.ExecuteReader())
        //                    {
        //                        if (dr.HasRows)
        //                        {
        //                            while (dr.Read())
        //                            {
        //                                x++;
        //                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
        //                                string image_file = dr["image_file"].ToString();
        //                                string plate_number_file = dr["plate_number_file"].ToString();
        //                                string occurred_at = dr["occurred_at"].ToString();
        //                                string person = dr["person"].ToString();
        //                                string location = dr["location"].ToString();
        //                                string event_type = dr["event_type"].ToString();
        //                                string site = dr["site"].ToString();
        //                                string person_type = dr["person_type"].ToString();
        //                                string statusnya;
        //                                int cek2 = event_type.IndexOf("MisMatch");
        //                                if (cek2 != -1)
        //                                {
        //                                    statusnya = "Unrecognized";
        //                                }
        //                                else
        //                                {
        //                                    statusnya = "Registered";
        //                                }
        //                                TableRow row = new TableRow();
        //                                //TableCell no = new TableCell();
        //                                //no.Text = x.ToString();
        //                                TableCell no = new TableCell() { Text = x.ToString() };
        //                                TableCell imageCell = new TableCell();
        //                                TableCell imageCell2 = new TableCell();
        //                                TableCell pers = new TableCell();
        //                                int cek = event_type.IndexOf("Plate");

        //                                if (cek != -1)
        //                                {
        //                                    if (!string.IsNullOrEmpty(plate_number_file))
        //                                    {
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = UrlImage + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = UrlImage + plate_number_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img2.Style.Add("width", "150px");
        //                                        imageCell2.Controls.Add(img2);
        //                                        //row.Cells.Add(imageCell);
        //                                        //row.Cells.Add(imageCell2);
        //                                    }
        //                                    pers.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    if (!string.IsNullOrEmpty(image_file))
        //                                    {
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = UrlImage + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                //TableCell status = new TableCell();
        //                                //status.Text = statusnya;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = statusnya };
        //                                row.Cells.Add(no);
        //                                row.Cells.Add(imageCell);
        //                                row.Cells.Add(imageCell2);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                //row.Cells.Add(pers);
        //                                row.Cells.Add(status);


        //                                TableBody.Controls.Add(row);
        //                            }
        //                            dr.Close();
        //                        }
        //                        else
        //                        {
        //                        }
        //                    }
        //                }
        //            }
        //            else if (DateTime.TryParse(from.ToString(), out fromDate))
        //            {
        //                string per1 = from.ToString();
        //                Label1.Text = per1.Substring(8, 2) + "-" + per1.Substring(5, 2) + "-" + per1.Substring(0, 4);
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id  WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC", con))
        //                {
        //                    cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                    cmd.Parameters.AddWithValue("@from", from);
        //                    cmd.Prepare();
        //                    using (MySqlDataReader dr = cmd.ExecuteReader())
        //                    {
        //                        if (dr.HasRows)
        //                        {
        //                            while (dr.Read())
        //                            {
        //                                x++;
        //                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
        //                                string image_file = dr["image_file"].ToString();
        //                                string plate_number_file = dr["plate_number_file"].ToString();
        //                                string occurred_at = dr["occurred_at"].ToString();
        //                                string person = dr["person"].ToString();
        //                                string location = dr["location"].ToString();
        //                                string event_type = dr["event_type"].ToString();
        //                                string site = dr["site"].ToString();
        //                                string person_type = dr["person_type"].ToString();
        //                                string statusnya;
        //                                int cek2 = event_type.IndexOf("MisMatch");
        //                                if (cek2 != -1)
        //                                {
        //                                    statusnya = "Unrecognized";
        //                                }
        //                                else
        //                                {
        //                                    statusnya = "Registered";
        //                                }
        //                                TableRow row = new TableRow();

        //                                //TableCell no = new TableCell();
        //                                //no.Text = x.ToString();
        //                                TableCell no = new TableCell() { Text = x.ToString() };
        //                                TableCell imageCell = new TableCell();
        //                                TableCell imageCell2 = new TableCell();
        //                                TableCell pers = new TableCell();
        //                                int cek = event_type.IndexOf("Plate");

        //                                if (cek != -1)
        //                                {
        //                                    if (!string.IsNullOrEmpty(plate_number_file))
        //                                    {
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = UrlImage + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = UrlImage + plate_number_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img2.Style.Add("width", "150px");
        //                                        imageCell2.Controls.Add(img2);
        //                                        //row.Cells.Add(imageCell);
        //                                        //row.Cells.Add(imageCell2);
        //                                    }
        //                                    pers.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    if (!string.IsNullOrEmpty(image_file))
        //                                    {
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = UrlImage + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    }
        //                                    pers.Text = person;
        //                                }

        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                //TableCell status = new TableCell();
        //                                //status.Text = statusnya;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = statusnya };
        //                                row.Cells.Add(no);
        //                                row.Cells.Add(imageCell);
        //                                row.Cells.Add(imageCell2);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                //row.Cells.Add(pers);
        //                                row.Cells.Add(status);

        //                                TableBody.Controls.Add(row);
        //                                DateTime occurredAt = (DateTime)dr["occurred_at"];
        //                                string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
        //                                Label2.Text = formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
        //                            }
        //                            dr.Close();
        //                        }
        //                        else
        //                        {
        //                        }
        //                    }
        //                }
        //            }
        //            else if (DateTime.TryParse(to, out toDate))
        //            {
        //                string per2 = to.ToString();
        //                Label2.Text = per2.Substring(8, 2) + "-" + per2.Substring(5, 2) + "-" + per2.Substring(0, 4);
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id  WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) <=@to AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC", con))
        //                {
        //                    cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                    cmd.Parameters.AddWithValue("@to", to);
        //                    cmd.Prepare();
        //                    using (MySqlDataReader dr = cmd.ExecuteReader())
        //                    {
        //                        if (dr.HasRows)
        //                        {
        //                            while (dr.Read())
        //                            {
        //                                x++;
        //                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
        //                                string image_file = dr["image_file"].ToString();
        //                                string plate_number_file = dr["plate_number_file"].ToString();
        //                                string occurred_at = dr["occurred_at"].ToString();
        //                                string person = dr["person"].ToString();
        //                                string location = dr["location"].ToString();
        //                                string event_type = dr["event_type"].ToString();
        //                                string site = dr["site"].ToString();
        //                                string person_type = dr["person_type"].ToString();
        //                                string statusnya;
        //                                int cek2 = event_type.IndexOf("MisMatch");
        //                                if (cek2 != -1)
        //                                {
        //                                    statusnya = "Unrecognized";
        //                                }
        //                                else
        //                                {
        //                                    statusnya = "Registered";
        //                                }
        //                                TableRow row = new TableRow();

        //                                //TableCell no = new TableCell();
        //                                //no.Text = x.ToString();
        //                                TableCell no = new TableCell() { Text = x.ToString() };
        //                                TableCell imageCell = new TableCell();
        //                                TableCell imageCell2 = new TableCell();
        //                                TableCell pers = new TableCell();
        //                                int cek = event_type.IndexOf("Plate");

        //                                if (cek != -1)
        //                                {
        //                                    if (!string.IsNullOrEmpty(plate_number_file))
        //                                    {
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = UrlImage + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = UrlImage + plate_number_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img2.Style.Add("width", "150px");
        //                                        imageCell2.Controls.Add(img2);
        //                                        //row.Cells.Add(imageCell);
        //                                        //row.Cells.Add(imageCell2);
        //                                    }
        //                                    pers.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    if (!string.IsNullOrEmpty(image_file))
        //                                    {
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = UrlImage + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                //TableCell status = new TableCell();
        //                                //status.Text = statusnya;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = statusnya };
        //                                row.Cells.Add(no);
        //                                row.Cells.Add(imageCell);
        //                                row.Cells.Add(imageCell2);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                //row.Cells.Add(pers);
        //                                row.Cells.Add(status);

        //                                TableBody.Controls.Add(row);
        //                                if (x.ToString() == "1")
        //                                {
        //                                    DateTime occurredAt = (DateTime)dr["occurred_at"];
        //                                    string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
        //                                    Label1.Text = formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
        //                                }
        //                            }
        //                            dr.Close();
        //                        }
        //                        else
        //                        {
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //}
    }
}