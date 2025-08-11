using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class statistic_detail : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            string camera = Request.QueryString["camera"];
            string type = Request.QueryString["type"];
            string detection = "";
            if (type == "FaceRecognitionMatch")
            {
                detection = "Unique Face Recognized";
            }
            else if (type == "FaceRecognitionNotMatch")
            {
                detection = "Face Unrecognized";
            }
            else if (type == "PlateMatch")
            {
                detection = "Unique License Plate Recognized";
            }
            else if (type == "PlateMisMatch")
            {
                detection = "Unique License Plate Unrecognized";
            }
            else
            {
                detection = "All Detection";
            }
            // Lakukan sesuatu dengan parameter
            if (!IsPostBack)
            {
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where name=@name", con))
                    {
                        cmd.Parameters.AddWithValue("@name", camera);
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    Judul.Text = dr["location"].ToString() + " - " + detection;
                                }
                                dr.Close();
                            }
                            else
                            {
                            }
                        }
                    }
                }
                if (type == "PlateMisMatch")
                {
                    FillTable2(camera);
                    FillTable3(camera);
                }
                else
                {
                    FillTable(camera, type);
                }
            }
        }

        protected void FillTable(string camera, string type)
        {
            string detect = "";
            if (type == "PlateMisMatch")
            {
                detect += "Plate MisMatch";
            }
            else
            {
                detect += Regex.Replace(type, "([a-z])([A-Z])", "$1 $2");
            }
            DateTime parsedDateTime;
            string from_date;
            if (Session["from_event_dashboard"] != null && !string.IsNullOrEmpty(Session["from_event_dashboard"].ToString()))
            {
                string from = Session["from_event_dashboard"].ToString();
                parsedDateTime = DateTime.Parse(from);
                from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd 00:0000");
                from_date = today;
            }
            //string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            if (Session["to_event_dashboard"] != null && !string.IsNullOrEmpty(Session["to_event_dashboard"].ToString()))
            {
                to = Session["to_event_dashboard"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            { }
            int x = 0;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type,p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code, v.plate_number as plate, ce.plate_number, vh.entry_code as code_vehicle, v.type as vehicle_type FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL) AND ce.camera_id=@camera_id";
                if (detect != "none")
                {
                    query += @" AND ce.type=@type";
                }
                if (from_date != null)
                {
                    query += @" AND ce.occurred_at >=@from";
                }
                if (to_date != null)
                {
                    query += @" AND ce.occurred_at <=@to";
                }
                if (detect != "none")
                {
                    if (detect != "Face Recognition Not Match")
                    {
                        if (detect == "Face Recognition Match")
                        {
                            query += @" GROUP BY ce.person_identification_number";
                        }
                        else
                        {
                            query += @" GROUP BY ce.plate_number";
                        }
                    }
                }
                query += @" ORDER BY ce.occurred_at ASC";
                //Response.Write(query);
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@camera_id", camera);
                    if (detect != "none")
                    {
                        cmd.Parameters.AddWithValue("@type", detect);
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
                                //Response.Write("<script>alert('" + query + "')</script>");
                                //Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string person = dr["person"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string person_type = dr["person_type"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                string statusnya = "";
                                string person_image = dr["person_image"].ToString();
                                string plate_number = dr["plate_number"].ToString();
                                string vehicle_type = dr["vehicle_type"].ToString();
                                TableRow row = new TableRow();
                                TableCell no = new TableCell() { Text = x.ToString() };
                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();
                                int cek = event_type.IndexOf("Plate");

                                if (cek != -1)
                                {
                                    string code_vehicle = dr["code_vehicle"].ToString();
                                    if (code_vehicle == null || code_vehicle == "")
                                    {
                                        statusnya += "Unrecognized";
                                    }
                                    else
                                    {
                                        //statusnya += dr["code_vehicle"].ToString();
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
                                    }
                                    if (!string.IsNullOrEmpty(plate_number_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + plate_number_file,
                                            AlternateText = "icon title"
                                        };
                                        img2.Style.Add("width", "150px");
                                        imageCell2.Controls.Add(img2);
                                        row.Cells.Add(imageCell);
                                        row.Cells.Add(imageCell2);
                                    }
                                    pers.Text = "";
                                }
                                else
                                {
                                    statusnya += event_type;
                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                        row.Cells.Add(imageCell);
                                        if (!string.IsNullOrEmpty(person_image))
                                        {
                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                            {
                                                ImageUrl = "person_image/" + person_image,
                                                AlternateText = "icon title"
                                            };
                                            img2.Style.Add("width", "150px");
                                            imageCell2.Controls.Add(img2);
                                            row.Cells.Add(imageCell2);
                                        }
                                        else
                                        {
                                            imageCell2.Text = "";
                                            row.Cells.Add(imageCell2);
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(person_image))
                                    {
                                        string warna = "";
                                        string entry = "";
                                        if (person_type == "blacklist")
                                        {
                                            warna += "red";
                                        }
                                        else
                                        {
                                            if (entry_code == "0")
                                            {
                                                warna += "green";
                                                entry += "Comply";
                                            }
                                            else if (entry_code == "1")
                                            {
                                                warna += "orange";
                                                entry += "Not Comply or Badge Expired";
                                            }
                                            else if (entry_code == "2")
                                            {
                                                warna += "red";
                                                entry += "FTW Rejected Medical / Expired";
                                            }
                                            else if (entry_code == "3")
                                            {
                                                warna += "#9f1239";
                                                entry += "Daily Checkup Failed";
                                            }
                                        }
                                        pers.Text = $"<span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry}</span>";
                                    }
                                    else
                                    {
                                        pers.Text = person;
                                    }
                                }
                                imageCell.HorizontalAlign = HorizontalAlign.Center;
                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sites = new TableCell() { Text = site };
                                TableCell status = new TableCell() { Text = statusnya };
                                TableCell plate = new TableCell() { Text = plate_number };
                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sites);
                                row.Cells.Add(pers);
                                row.Cells.Add(plate);
                                row.Cells.Add(status);



                                TableBody.Controls.Add(row);
                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }
            }
        }

        protected void FillTable2(string camera)
        {
            DateTime parsedDateTime;
            string from_date;
            if (Session["from_event_dashboard"] != null && !string.IsNullOrEmpty(Session["from_event_dashboard"].ToString()))
            {
                string from = Session["from_event_dashboard"].ToString();
                parsedDateTime = DateTime.Parse(from);
                from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd 00:0000");
                from_date = today;
            }
            //string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            if (Session["to_event_dashboard"] != null && !string.IsNullOrEmpty(Session["to_event_dashboard"].ToString()))
            {
                to = Session["to_event_dashboard"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            { }
            int x = 0;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type,p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code, v.plate_number as plate, ce.plate_number, vh.entry_code as code_vehicle, v.type as vehicle_type FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL) AND ce.camera_id=@camera_id AND (ce.plate_number not in (SELECT plate_number from vehicles)) AND (ce.plate_number!='' or ce.plate_number!=null)";
                if (from_date != null)
                {
                    query += @" AND ce.occurred_at >=@from";
                }
                if (to_date != null)
                {
                    query += @" AND ce.occurred_at <=@to";
                }
                query += @" GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";
                //Response.Write(query);
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@camera_id", camera);
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
                                //Response.Write("<script>alert('" + query + "')</script>");
                                //Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string person = dr["person"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string person_type = dr["person_type"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                string statusnya = "";
                                string person_image = dr["person_image"].ToString();
                                string plate_number = dr["plate_number"].ToString();
                                string vehicle_type = dr["vehicle_type"].ToString();

                                TableRow row = new TableRow();
                                TableCell no = new TableCell() { Text = x.ToString() };
                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();
                                int cek = event_type.IndexOf("Plate");

                                if (cek != -1)
                                {
                                    string code_vehicle = dr["code_vehicle"].ToString();
                                    if (code_vehicle == null || code_vehicle == "")
                                    {
                                        statusnya += "Unrecognized";
                                    }
                                    else
                                    {
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
                                    }
                                    if (!string.IsNullOrEmpty(plate_number_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + plate_number_file,
                                            AlternateText = "icon title"
                                        };
                                        img2.Style.Add("width", "150px");
                                        imageCell2.Controls.Add(img2);
                                        row.Cells.Add(imageCell);
                                        row.Cells.Add(imageCell2);
                                    }
                                    pers.Text = "";
                                }
                                else
                                {
                                    statusnya += event_type;
                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                        row.Cells.Add(imageCell);
                                        if (!string.IsNullOrEmpty(person_image))
                                        {
                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                            {
                                                ImageUrl = "person_image/" + person_image,
                                                AlternateText = "icon title"
                                            };
                                            img2.Style.Add("width", "150px");
                                            imageCell2.Controls.Add(img2);
                                            row.Cells.Add(imageCell2);
                                        }
                                        else
                                        {
                                            imageCell2.Text = "";
                                            row.Cells.Add(imageCell2);
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(person_image))
                                    {
                                        string warna = "";
                                        if (person_type == "blacklist")
                                        {
                                            warna += "red";
                                        }
                                        else
                                        {
                                            if (entry_code == "0")
                                            {
                                                warna += "green";
                                            }
                                            else if (entry_code == "1")
                                            {
                                                warna += "orange";
                                            }
                                            else if (entry_code == "2")
                                            {
                                                warna += "red";
                                            }
                                        }
                                        pers.Text = $"<span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry_code}</span>";
                                    }
                                    else
                                    {
                                        pers.Text = person;
                                    }
                                }
                                imageCell.HorizontalAlign = HorizontalAlign.Center;
                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sites = new TableCell() { Text = site };
                                TableCell status = new TableCell() { Text = statusnya };
                                TableCell plate = new TableCell() { Text = plate_number };
                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sites);
                                row.Cells.Add(pers);
                                row.Cells.Add(plate);
                                row.Cells.Add(status);



                                TableBody.Controls.Add(row);
                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }
            }
        }

        protected void FillTable3(string camera)
        {
            DateTime parsedDateTime;
            string from_date;
            if (Session["from_event_dashboard"] != null && !string.IsNullOrEmpty(Session["from_event_dashboard"].ToString()))
            {
                string from = Session["from_event_dashboard"].ToString();
                parsedDateTime = DateTime.Parse(from);
                from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd 00:0000");
                from_date = today;
            }
            //string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            if (Session["to_event_dashboard"] != null && !string.IsNullOrEmpty(Session["to_event_dashboard"].ToString()))
            {
                to = Session["to_event_dashboard"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            { }
            int x = 0;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type,p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code, v.plate_number as plate, ce.plate_number, vh.entry_code as code_vehicle, v.type as vehicle_type FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL) AND ce.camera_id=@camera_id AND (ce.plate_number='' or ce.plate_number=NULL or ce.plate_number IS NULL)";
                if (from_date != null)
                {
                    query += @" AND ce.occurred_at >=@from";
                }
                if (to_date != null)
                {
                    query += @" AND ce.occurred_at <=@to";
                }
                query += @" GROUP BY ce.plate_number ORDER BY ce.occurred_at ASC";
                //Response.Write(query);
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@camera_id", camera);
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
                                //Response.Write("<script>alert('" + query + "')</script>");
                                //Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string person = dr["person"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string person_type = dr["person_type"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                string statusnya = "";
                                string person_image = dr["person_image"].ToString();
                                string plate_number = dr["plate_number"].ToString();
                                string vehicle_type = dr["vehicle_type"].ToString();
                                TableRow row = new TableRow();
                                TableCell no = new TableCell() { Text = x.ToString() };
                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();
                                int cek = event_type.IndexOf("Plate");

                                if (cek != -1)
                                {
                                    string code_vehicle = dr["code_vehicle"].ToString();
                                    if (code_vehicle == null || code_vehicle == "")
                                    {
                                        statusnya += "Unrecognized";
                                    }
                                    else
                                    {
                                        //statusnya += dr["code_vehicle"].ToString();
                                        if (code_vehicle == "0")
                                        {
                                            statusnya += "Valid DVP";
                                        }
                                        else if (code_vehicle == "1")
                                        {
                                            statusnya += "Expiring DVP";
                                        }
                                        else if (code_vehicle == "2")
                                        {
                                            statusnya += "";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(plate_number_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + plate_number_file,
                                            AlternateText = "icon title"
                                        };
                                        img2.Style.Add("width", "150px");
                                        imageCell2.Controls.Add(img2);
                                        row.Cells.Add(imageCell);
                                        row.Cells.Add(imageCell2);
                                    }
                                    pers.Text = "";
                                }
                                else
                                {
                                    statusnya += event_type;
                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                        row.Cells.Add(imageCell);
                                        if (!string.IsNullOrEmpty(person_image))
                                        {
                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                            {
                                                ImageUrl = "person_image/" + person_image,
                                                AlternateText = "icon title"
                                            };
                                            img2.Style.Add("width", "150px");
                                            imageCell2.Controls.Add(img2);
                                            row.Cells.Add(imageCell2);
                                        }
                                        else
                                        {
                                            imageCell2.Text = "";
                                            row.Cells.Add(imageCell2);
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(person_image))
                                    {
                                        string warna = "";
                                        if (person_type == "blacklist")
                                        {
                                            warna += "red";
                                        }
                                        else
                                        {
                                            if (entry_code == "0")
                                            {
                                                warna += "green";
                                            }
                                            else if (entry_code == "1")
                                            {
                                                warna += "yellow";
                                            }
                                            else if (entry_code == "2")
                                            {
                                                warna += "orange";
                                            }
                                        }
                                        pers.Text = $"<span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry_code}</span>";
                                    }
                                    else
                                    {
                                        pers.Text = person;
                                    }
                                }
                                imageCell.HorizontalAlign = HorizontalAlign.Center;
                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sites = new TableCell() { Text = site };
                                TableCell status = new TableCell() { Text = statusnya };
                                TableCell plate = new TableCell() { Text = plate_number };
                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sites);
                                row.Cells.Add(pers);
                                row.Cells.Add(plate);
                                row.Cells.Add(status);



                                TableBody.Controls.Add(row);
                            }
                            dr.Close();
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