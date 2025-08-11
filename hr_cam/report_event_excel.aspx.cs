using iText.Html2pdf;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using OfficeOpenXml;
using ClosedXML.Excel;

namespace hr_cam
{
    public partial class Report_event_excel : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        //string UrlImage = ConfigurationManager.AppSettings["urlImage"];
        //string UrlImagePerson = ConfigurationManager.AppSettings["urlImagePerson"];
        string UrlImage = "~/image_file/";
        string UrlImagePerson = "~/person_image/";
        string UrlLogo = ConfigurationManager.AppSettings["urlLogo"];
        protected void Page_Load(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
                FillEventHistory();
            if (!IsPostBack)
            {
                //Response.Write("camera site:"+Session["site_event"].ToString());
                //Response.Write("camera:"+Session["camera"].ToString());
                //Response.Write("from_event:"+Session["from_event"].ToString());
                //Response.Write("to_event:"+Session["to_event"].ToString());
                //if (Request.QueryString["export"] == "excel")
                //{
                //ExportToExcel();
                //ExportTableToExcel();
                //}
            }
        }

        private void FillEventHistory()
        {
            //string camera_site = Session["site_event"].ToString();
            string from = Session["from_event"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from);
            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            //Response.Write("<script>alert('Isinya:" + Session["to_people"] +"')</script>");
            if (Session["to_event"] != null && !string.IsNullOrEmpty(Session["to_event"].ToString()))
            {
                //Response.Write("<script>alert('Ga kosong')</script>");
                to = Session["to_event"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                //Response.Write("<script>alert('kosong')</script>");

            }

            //DateTime fromDate;
            //DateTime toDate;
            int x = 0;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                //if (camera_site == "none")
                //{

                //    Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                //}
                //else
                //{
                string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL)";
                if (from_date != null)
                {
                    query += @" AND ce.occurred_at >=@from";
                }
                if (to_date != null)
                {
                    query += @" AND ce.occurred_at <=@to";
                }
                query += @" ORDER BY ce.occurred_at ASC";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    if (from_date != null)
                    {
                        cmd.Parameters.AddWithValue("@from", from);
                    }
                    if (to_date != null)
                    {
                        cmd.Parameters.AddWithValue("@to", to);
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
                                string occurred_at = dr["occurred_at"].ToString();
                                string person = dr["person"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string person_type = dr["person_type"].ToString();
                                string person_image = dr["person_image"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                TableRow row = new TableRow();
                                //TableCell no = new TableCell();
                                //no.Text = x.ToString(); 
                                TableCell no = new TableCell() { Text = x.ToString() };
                                row.Cells.Add(no);
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
                                        // string imagePath = @UrlImage + image_file;
                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
                                        // string base64String = Convert.ToBase64String(imageBytes);
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = UrlImage + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                        // string imagePath2 = @UrlImage + plate_number_file;
                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = UrlImage + plate_number_file,
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
                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        // string imagePath = @UrlImage + image_file;
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
                                        if (!string.IsNullOrEmpty(person_image))
                                        {
                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                            {
                                                ImageUrl = UrlImagePerson + person_image,
                                                AlternateText = "icon title"
                                            };
                                            img2.Style.Add("width", "150px");
                                            imageCell2.Controls.Add(img2);
                                            row.Cells.Add(imageCell);
                                            row.Cells.Add(imageCell2);
                                        }
                                        else
                                        {
                                            imageCell.ColumnSpan = 2;
                                            row.Cells.Add(imageCell);
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
                                //TableCell occurred = new TableCell();
                                //occurred.Text = occurred_at;
                                //TableCell loc = new TableCell();
                                //loc.Text = location;
                                //TableCell sites = new TableCell();
                                //sites.Text = site;
                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sites = new TableCell() { Text = site };
                                TableCell status = new TableCell() { Text = event_type };
                                //row.Cells.Add(no);
                                //row.Cells.Add(imageCell);
                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sites);
                                row.Cells.Add(pers);
                                row.Cells.Add(status);



                                TableBody.Controls.Add(row);
                                if (to_date == null)
                                {
                                    DateTime occurredAt = (DateTime)dr["occurred_at"];
                                    string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    Label2.Text = formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                }
                                else if (from_date == null)
                                {
                                    if (x.ToString() == "1")
                                    {
                                        DateTime occurredAt = (DateTime)dr["occurred_at"];
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
                    if (from_date != null && to_date != null)
                    {
                        string per1 = from.ToString();
                        string per2 = to.ToString();
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
                //}

            }
        }

        //private void FillEventHistory()
        //{
        //    //string camera_site = Session["site_event"].ToString();
        //    string from = Session["from_event"].ToString();
        //    string to = null;
        //    if (Session["to_event"] != null)
        //    {
        //        to = Session["to_event"].ToString();
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
        //        //if (camera_site == "none")
        //        //{

        //        //    Response.Write("<script>alert('Camera Site cannot be empty')</script>");
        //        //}
        //        //else
        //        //{
        //            if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
        //            {
        //                string per1 = from.ToString();
        //                string per2 = to.ToString();
        //                Label1.Text = per1.Substring(8, 2) + "-" + per1.Substring(5, 2) + "-" + per1.Substring(0, 4);
        //                Label2.Text = per2.Substring(8, 2) + "-" + per2.Substring(5, 2) + "-" + per2.Substring(0, 4);
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from AND DATE(ce.occurred_at) <=@to ORDER BY ce.occurred_at ASC", con))
        //                {
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
        //                                TableRow row = new TableRow();
        //                                //TableCell no = new TableCell();
        //                                //no.Text = x.ToString(); 
        //                                TableCell no = new TableCell() { Text = x.ToString() };
        //                            row.Cells.Add(no);
        //                            TableCell imageCell = new TableCell();
        //                                TableCell imageCell2 = new TableCell();
        //                                TableCell pers = new TableCell();
        //                                int cek = event_type.IndexOf("Plate");

        //                                if (cek != -1)
        //                                {
        //                                    if (!string.IsNullOrEmpty(plate_number_file))
        //                                    {
        //                                    //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                    //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                    //img.AlternateText = "icon title";
        //                                    //img.Style.Add("width", "150px");
        //                                    //imageCell.Controls.Add(img);
        //                                    // string imagePath = @UrlImage + image_file;
        //                                    // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                    // string base64String = Convert.ToBase64String(imageBytes);
        //                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = UrlImage + image_file,
        //                                        AlternateText = "icon title"
        //                                    };
        //                                    img.Style.Add("width", "150px");
        //                                    imageCell.Controls.Add(img);
        //                                    // string imagePath2 = @UrlImage + plate_number_file;
        //                                    // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                    // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = UrlImage + plate_number_file,
        //                                        AlternateText = "icon title"
        //                                    };
        //                                    img2.Style.Add("width", "150px");
        //                                    imageCell2.Controls.Add(img2);
        //                                    row.Cells.Add(imageCell);
        //                                    row.Cells.Add(imageCell2);
        //                                }
        //                                    pers.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    if (!string.IsNullOrEmpty(image_file))
        //                                    {
        //                                        // string imagePath = @UrlImage + image_file;
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
        //                                    imageCell.ColumnSpan = 2;
        //                                    row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };
        //                                //row.Cells.Add(no);
        //                                //row.Cells.Add(imageCell);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
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
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from ORDER BY ce.occurred_at ASC", con))
        //                {
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
        //                                TableRow row = new TableRow();

        //                                //TableCell no = new TableCell();
        //                                //no.Text = x.ToString();
        //                                TableCell no = new TableCell() { Text = x.ToString() };
        //                            row.Cells.Add(no);
        //                            TableCell imageCell = new TableCell();
        //                                TableCell imageCell2 = new TableCell();
        //                                TableCell pers = new TableCell();
        //                                int cek = event_type.IndexOf("Plate");

        //                                if (cek != -1)
        //                                {
        //                                    if (!string.IsNullOrEmpty(plate_number_file))
        //                                    {
        //                                    //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                    //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                    //img.AlternateText = "icon title";
        //                                    //img.Style.Add("width", "150px");
        //                                    //imageCell.Controls.Add(img);
        //                                    // string imagePath = @UrlImage + image_file;
        //                                    // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                    // string base64String = Convert.ToBase64String(imageBytes);
        //                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = UrlImage + image_file,
        //                                        AlternateText = "icon title"
        //                                    };
        //                                    img.Style.Add("width", "150px");
        //                                    imageCell.Controls.Add(img);
        //                                    // string imagePath2 = @UrlImage + plate_number_file;
        //                                    // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                    // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = UrlImage + plate_number_file,
        //                                        AlternateText = "icon title"
        //                                    };
        //                                    img2.Style.Add("width", "150px");
        //                                    imageCell2.Controls.Add(img2);
        //                                    row.Cells.Add(imageCell);
        //                                    row.Cells.Add(imageCell2);
        //                                }
        //                                    pers.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    if (!string.IsNullOrEmpty(image_file))
        //                                    {
        //                                        // string imagePath = @UrlImage + image_file;
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
        //                                    imageCell.ColumnSpan = 2;
        //                                    row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }

        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };
        //                                //row.Cells.Add(no);
        //                                //row.Cells.Add(imageCell);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
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
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) <=@to ORDER BY ce.occurred_at ASC", con))
        //                {
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
        //                                TableRow row = new TableRow();

        //                                //TableCell no = new TableCell();
        //                                //no.Text = x.ToString();
        //                                TableCell no = new TableCell() { Text = x.ToString() };
        //                            row.Cells.Add(no);
        //                            TableCell imageCell = new TableCell();
        //                                TableCell imageCell2 = new TableCell();
        //                                TableCell pers = new TableCell();
        //                                int cek = event_type.IndexOf("Plate");

        //                                if (cek != -1)
        //                                {
        //                                    if (!string.IsNullOrEmpty(plate_number_file))
        //                                    {
        //                                    //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                    //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                    //img.AlternateText = "icon title";
        //                                    //img.Style.Add("width", "150px");
        //                                    //imageCell.Controls.Add(img);
        //                                    // string imagePath = @UrlImage + image_file;
        //                                    // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                    // string base64String = Convert.ToBase64String(imageBytes);
        //                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = UrlImage + image_file,
        //                                        AlternateText = "icon title"
        //                                    };
        //                                    img.Style.Add("width", "150px");
        //                                    imageCell.Controls.Add(img);
        //                                    // string imagePath2 = @UrlImage + plate_number_file;
        //                                    // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                    // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = UrlImage + plate_number_file,
        //                                        AlternateText = "icon title"
        //                                    };
        //                                    img2.Style.Add("width", "150px");
        //                                    imageCell2.Controls.Add(img2);
        //                                    row.Cells.Add(imageCell);
        //                                    row.Cells.Add(imageCell2);
        //                                }
        //                                    pers.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    if (!string.IsNullOrEmpty(image_file))
        //                                    {
        //                                        // string imagePath = @UrlImage + image_file;
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
        //                                    imageCell.ColumnSpan = 2;
        //                                    row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };

        //                                //row.Cells.Add(imageCell);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
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
        //        //}

        //    }
        //}

        private void ExportTableToExcel()
        {
            var tableHtml = TableBody.InnerHtml; // Mengambil HTML dari tbody
            var rows = tableHtml.Split(new[] { "</tr>" }, StringSplitOptions.RemoveEmptyEntries);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Sheet1");

                // Menambahkan header kolom
                ws.Cell(1, 1).Value = "No";
                ws.Cell(1, 2).Value = "Image 1";
                ws.Cell(1, 3).Value = "Image 2";
                ws.Cell(1, 4).Value = "Occurred At";
                ws.Cell(1, 5).Value = "Location";
                ws.Cell(1, 6).Value = "Site";
                ws.Cell(1, 7).Value = "Person";
                ws.Cell(1, 8).Value = "Status";

                // Menambahkan data baris
                for (int i = 0; i < rows.Length; i++)
                {
                    var columns = rows[i].Split(new[] { "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < columns.Length; j++)
                    {
                        var cellData = columns[j].Replace("<td>", "").Trim();
                        ws.Cell(i + 2, j + 1).Value = cellData;
                    }
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=Export.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
            }
        }


        //protected void ExportToExcel()
        //{
        //    // Create an Excel package
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        // Add a worksheet
        //        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

        //        // Render HTML to string
        //        StringWriter sw = new StringWriter();
        //        HtmlTextWriter hw = new HtmlTextWriter(sw);
        //        form1.RenderControl(hw);
        //        string htmlContent = sw.ToString();

        //        // Here you would need to parse HTML manually if you want to handle complex HTML
        //        // For simplicity, let's assume the HTML is simple and directly write it to the first cell
        //        worksheet.Cells[1, 1].Value = htmlContent;

        //        // Set response to download file
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", "attachment;filename=EventReport.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.BinaryWrite(package.GetAsByteArray());
        //        Response.End();
        //    }
        //}

        // Event handler untuk tombol Export to Excel


        // Override method to allow controls to render properly
        public override void VerifyRenderingInServerForm(Control control)
        {
            // This is required to allow rendering of controls
        }


        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Buat Workbook Excel
            using (XLWorkbook workbook = new XLWorkbook())
            {
                try
                {
                    var worksheet = workbook.Worksheets.Add("Report Event History");

                    // Header Utama
                    worksheet.Cell(1, 1).Value = "Report Event History";
                    worksheet.Range(1, 1, 1, 7).Merge().Style.Font.SetBold().Font.SetFontSize(14);

                    // Header Periode
                    string per1 = "2024-09-20";
                    string per2 = "2024-09-20";
                    worksheet.Cell(2, 1).Value = $"Periode {per1} - {per2}";
                    worksheet.Range(2, 1, 2, 7).Merge().Style.Font.SetItalic();

                    // Header Kolom
                    worksheet.Cell(3, 1).Value = "Image";
                    worksheet.Cell(3, 2).Value = "";
                    worksheet.Cell(3, 3).Value = "Occurred At";
                    worksheet.Cell(3, 4).Value = "Location";
                    worksheet.Cell(3, 5).Value = "Site";
                    worksheet.Cell(3, 6).Value = "Person";
                    worksheet.Cell(3, 7).Value = "Status";
                    worksheet.Row(3).Style.Font.SetBold();

                    // Ambil data dari database
                    string from = Session["from_event"].ToString();
                    DateTime parsedDateTime = DateTime.Parse(from);
                    string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    string to = null;
                    string to_date = null;
                    if (Session["to_event"] != null && !string.IsNullOrEmpty(Session["to_event"].ToString()))
                    {
                        //Response.Write("<script>alert('Ga kosong')</script>");
                        to = Session["to_event"].ToString();
                        DateTime parsedDateTime2 = DateTime.Parse(to);
                        to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        //Response.Write("<script>alert('kosong')</script>");

                    }

                    int currentRow = 4;

                    using (MySqlConnection con = new MySqlConnection(strcon))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        string query = @"SELECT ce.image_file, ce.plate_number_file, ce.occurred_at, 
                                    c.location, cs.name AS site, p.name as person, 
                                    p.type as person_type, p.image_file as person_image, ce.type as event_type
                                FROM camera_events as ce
                                JOIN cameras as c ON ce.camera_id = c.name
                                JOIN camera_sites AS cs ON c.camera_site_id = cs.id
                                LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number
                                WHERE (ce.image_file != '' OR ce.image_file IS NOT NULL)";

                        if (!string.IsNullOrEmpty(from_date))
                            query += " AND ce.occurred_at >= @from";
                        if (!string.IsNullOrEmpty(to_date))
                            query += " AND ce.occurred_at <= @to";
                        //query += " ORDER BY ce.occurred_at ASC limit @limit offset @offset";
                        query += " ORDER BY ce.occurred_at ASC";

                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            //cmd.Parameters.AddWithValue("@limit", 1000);
                            //cmd.Parameters.AddWithValue("@offset", 0);
                            if (!string.IsNullOrEmpty(from_date))
                                cmd.Parameters.AddWithValue("@from", from_date);
                            if (!string.IsNullOrEmpty(to_date))
                                cmd.Parameters.AddWithValue("@to", to_date);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    // Ambil data dari query
                                    string imageFile = dr["image_file"].ToString();
                                    string plateNumberFile = dr["plate_number_file"].ToString();
                                    string occurredAt = dr["occurred_at"].ToString();
                                    string location = dr["location"].ToString();
                                    string site = dr["site"].ToString();
                                    string person = dr["person"].ToString();
                                    string personType = dr["person_type"].ToString();
                                    string eventType = dr["event_type"].ToString();

                                    // Masukkan data ke worksheet
                                    //worksheet.Cell(currentRow, 1).Value = "foto";
                                    worksheet.Cell(currentRow, 2).Value = "foto";
                                    worksheet.Cell(currentRow, 3).Value = occurredAt;
                                    worksheet.Cell(currentRow, 4).Value = location;
                                    worksheet.Cell(currentRow, 5).Value = site;
                                    worksheet.Cell(currentRow, 6).Value = person;
                                    worksheet.Cell(currentRow, 7).Value = eventType;
                                    worksheet.Row(currentRow).Height = 112.5;
                                    worksheet.Column(1).Width = 20;
                                    worksheet.Column(2).Width = 20;
                                    // Tambahkan gambar ke worksheet
                                    if (!string.IsNullOrEmpty(imageFile))
                                    {
                                        string imagePath = Server.MapPath("~/image_file/" + imageFile); // Pastikan gambar ada di folder yang bisa diakses
                                        if (File.Exists(imagePath))
                                        {
                                            var image = worksheet.AddPicture(imagePath)
                                                .MoveTo(worksheet.Cell(currentRow, 1))
                                                .WithSize(150, 150); // Set ukuran gambar
                                        }
                                    }

                                    //if (!string.IsNullOrEmpty(plateNumberFile))
                                    //{
                                    //    string plateImagePath = Server.MapPath("~/image_file/" + plateNumberFile);
                                    //    if (File.Exists(plateImagePath))
                                    //    {
                                    //        var plateImage = worksheet.AddPicture(plateImagePath)
                                    //            .MoveTo(worksheet.Cell(currentRow, 2))
                                    //            .WithSize(150, 150); // Set ukuran gambar
                                    //    }
                                    //}

                                    currentRow++;
                                }
                            }
                        }
                    }

                    // Simpan workbook ke output stream
                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment;filename=Report_Event_History.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.BinaryWrite(stream.ToArray());
                        Response.Flush();
                        Response.End();
                    }
                }
                catch (System.OutOfMemoryException)
                {
                    // Tangani kesalahan OutOfMemory
                    Response.Write("Kapasitas memori tidak cukup untuk memproses file yang besar ini.");
                }
                finally
                {
                    workbook.Dispose();  // Lepaskan workbook dari memori
                }
            }
        }

        protected void btnExportExcel2_Click(object sender, EventArgs e)
        {
            // Mengambil data
            DataTable dataTable = GetData(); // Asumkan GetData() mengembalikan DataTable

            int batchSize = 1000; // Batasi jumlah baris per file
            int totalRows = dataTable.Rows.Count;
            int batchCount = (int)Math.Ceiling((double)totalRows / batchSize);

            for (int batchIndex = 0; batchIndex < batchCount; batchIndex++)
            {
                // Set response header sebelum mengirim data
                string fileName = $"Export_Data_Batch_{batchIndex + 1}.xlsx";
                

                using (XLWorkbook workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Data");

                    // Tambahkan header kolom
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dataTable.Columns[i].ColumnName;
                    }

                    // Tambahkan data batch
                    for (int rowIndex = 0; rowIndex < batchSize; rowIndex++)
                    {
                        int actualRow = batchIndex * batchSize + rowIndex;
                        if (actualRow >= totalRows) break;

                        DataRow row = dataTable.Rows[actualRow];
                        for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
                        {
                            worksheet.Cell(rowIndex + 2, colIndex + 1).Value = row[colIndex].ToString();
                        }
                    }

                    // Simpan workbook ke memory stream
                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);

                        // Tulis ke response output
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                        Response.BinaryWrite(stream.ToArray());
                        Response.Flush();
                    }
                }
            }

            Response.End(); // Akhiri respons
        }

        private DataTable GetData()
        {
            // Di sini, kembalikan DataTable dari database atau sumber lain
            DataTable table = new DataTable();
            table.Columns.Add("Nama");
            table.Columns.Add("Umur");

            // Tambahkan contoh data
            for (int i = 1; i <= 3500; i++) // Contoh data 3500 baris
            {
                table.Rows.Add($"Nama {i}", i % 50 + 20); // Misal, umur antara 20 sampai 70
            }
            return table;
        }



    }
}