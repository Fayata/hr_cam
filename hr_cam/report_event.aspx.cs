using iText.Html2pdf;
//using iText.Layout.Element;
using iText.Kernel.Pdf;
using iText.Layout;
using MySql.Data.MySqlClient;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class Report_event : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string UrlImage = ConfigurationManager.AppSettings["urlImage"];
        string UrlImagePerson = ConfigurationManager.AppSettings["urlImagePerson"];
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(IsPostBack);
            //Response.Write("Hallo");
            //Console.WriteLine("Hallo");
            //Console.WriteLine(IsPostBack);
            if (!IsPostBack)
            {

                if (Session["site_event"] == null)
                {
                    string camera_site = "none";
                    Session["site_event"] = camera_site;
                    //Response.Write("<script>alert('" + Session["from"].ToString() + "')</script>");

                }

                //DropDownList1.Items.Add(new ListItem("", "none"));
                //DropDownList2.Items.Add(new ListItem("", "none"));
                //using (MySqlConnection con = new MySqlConnection(strcon))
                //{
                //    if (con.State == ConnectionState.Closed)
                //    {
                //        con.Open();
                //    }

                //    using (MySqlCommand cmd = new MySqlCommand("SELECT * from camera_sites", con))
                //    {
                //        using (MySqlDataReader dr = cmd.ExecuteReader())
                //        {
                //            if (dr.HasRows)
                //            {
                //                while (dr.Read())
                //                {
                //                    DropDownList1.Items.Add(new ListItem(dr.GetValue(1).ToString(), dr.GetValue(0).ToString()));
                //                    //DropDownList2.Items.Add(new ListItem(dr.GetValue(1).ToString(), dr.GetValue(0).ToString()));
                //                }
                //            }
                //            else
                //            {
                //            }
                //        }
                //    }

                //}
                if (Session["from_event"] != null)
                {
                }
                else
                {
                    string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                    Session["from_event"] = today;

                }
                if (Session["to_event"] != null)
                {
                    TextBox2.Text = Session["to_event"].ToString();
                }
                TextBox1.Text = Session["from_event"].ToString();
                //DropDownList1.SelectedValue = Session["site_event"].ToString();
                FillEventHistory();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string camera_site = DropDownList1.SelectedValue;
            //Response.Write("<script>alert('site " + camera_site + "')</script>");
            string dari = TextBox1.Text;
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
                //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                dari = today;

            }
            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            //Session["site_event"] = camera_site;
            Session["from_event"] = dari;
            Session["to_event"] = ke;
            Response.Redirect("report_event.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //string camera_site = DropDownList1.SelectedValue;
            //Response.Write("<script>alert('site " + camera_site + "')</script>");
            string dari = TextBox1.Text;
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
                //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                dari = today;

            }
            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            //Session["site_event"] = camera_site;
            Session["from_event"] = dari;
            Session["to_event"] = ke;
            //if (camera_site == "none")
            //{
            //    Response.Write("<script>alert('Camera Site cannot be empty')</script>");
            //    //Response.Redirect("report_event.aspx");
            //}
            //else
            //{
            //Response.Redirect("report_event_pdf.aspx");
            // Mendapatkan markup HTML dari halaman ExportToPdf.aspx
            StringWriter sw = new StringWriter();
            Server.Execute("report_event_pdf.aspx", sw);
            string htmlContent = sw.ToString();

            // Menyiapkan path output untuk file PDF
            string outputFilePath = Server.MapPath("~/Output.pdf");

            // Membuat objek PdfWriter untuk menulis ke PDF
            PdfWriter writer = new PdfWriter(outputFilePath);

            // Membuat objek PdfDocument dengan orientasi landscape (A4)
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, iText.Kernel.Geom.PageSize.A4.Rotate());

            // Menambahkan markup HTML ke PDF
            HtmlConverter.ConvertToPdf(htmlContent, pdf, new ConverterProperties());

            // Menutup dokumen PDF
            document.Close();

            // Mengarahkan pengguna untuk mengunduh file PDF yang dihasilkan
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Report Event History.pdf");
            Response.TransmitFile(outputFilePath);
            Response.End();
            //}
        }

        protected void btnExportExcel2_Click(object sender, EventArgs e)
        {
            //string camera_site = DropDownList1.SelectedValue;
            //Response.Write("<script>alert('site " + camera_site + "')</script>");
            string dari = TextBox1.Text;
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
                //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                dari = today;

            }
            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            //Session["site_event"] = camera_site;
            Session["from_event"] = dari;
            Session["to_event"] = ke;
            //if (camera_site == "none")
            //{
            //    Response.Write("<script>alert('Camera Site cannot be empty')</script>");
            //    //Response.Redirect("report_event.aspx");
            //}
            //else
            //{
            Response.Redirect("report_event_excel.aspx");

        }

        private void FillEventHistory()
        {
            string camera_site = Session["site_event"].ToString();
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

                //    //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                //}
                //else
                //{
                string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type,p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code, v.plate_number as plate, ce.plate_number, vh.entry_code as code_vehicle FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL)";
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
                                Response.Write("<script>alert('" + query + "')</script>");
                                Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string image_file = dr["image_file"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                //Response.Write(occurred_at + "atas");
                                string person = dr["person"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string person_type = dr["person_type"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                string plate_number = dr["plate_number"].ToString();
                                string statusnya = "";
                                //Response.Write(entry_code);
                                //if (!string.IsNullOrEmpty(person_type))
                                //{
                                //    string person_typenya = char.ToUpper(person_type[0]) + person_type.Substring(1);
                                //}

                                string person_image = dr["person_image"].ToString();
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
                                    //Response.Write(event_type);
                                    if (!string.IsNullOrEmpty(plate_number_file))
                                    {
                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                                        //img.ImageUrl = "data:image/png;base64," + base64Striing;
                                        //img.AlternateText = "icon title";
                                        //img.Style.Add("width", "150px");
                                        //imageCell.Controls.Add(img);
                                        // string imagePath = UrlImage + image_file;
                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
                                        // string base64String = Convert.ToBase64String(imageBytes);
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                        // string imagePath2 = UrlImage + plate_number_file;
                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
                                        // string imagePath = UrlImage + image_file;
                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
                                        // string base64String = Convert.ToBase64String(imageBytes);
                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                                        //img.ImageUrl = "data:image/png;base64," + base64Stiring;
                                        //img.AlternateText = "icon title";
                                        //img.Style.Add("width", "150px");
                                        //imageCell.Controls.Add(img);
                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                        {
                                            ImageUrl = "image_file/" + image_file,
                                            AlternateText = "icon title"
                                        };
                                        img.Style.Add("width", "150px");
                                        imageCell.Controls.Add(img);
                                        row.Cells.Add(imageCell);
                                        //imageCell.ColumnSpan = 2;
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
                                    if (!string.IsNullOrEmpty(person_type))
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
                                //TableCell occurred = new TableCell();
                                //occurred.Text = occurred_at;
                                //TableCell loc = new TableCell();
                                //loc.Text = location;
                                //TableCell sites = new TableCell();
                                //sites.Text = site;
                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sites = new TableCell() { Text = site };
                                TableCell status = new TableCell() { Text = statusnya };
                                TableCell plate = new TableCell() { Text = plate_number };
                                //row.Cells.Add(no);
                                //row.Cells.Add(imageCell);
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

                //}

            }
        }

        //private void FillEventHistory()
        //{
        //    string camera_site = Session["site_event"].ToString();
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

        //        //    //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
        //        //}
        //        //else
        //        //{
        //            if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
        //            {
        //                string per1 = from.ToString();
        //                string per2 = to.ToString();
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file != '' or ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from AND DATE(ce.occurred_at) <=@to ORDER BY ce.occurred_at ASC", con))
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
        //                            //Response.Write(occurred_at + "atas");
        //                                string person = dr["person"].ToString();
        //                                string location = dr["location"].ToString();
        //                                string event_type = dr["event_type"].ToString();
        //                                string site = dr["site"].ToString();
        //                                string person_type = dr["person_type"].ToString();
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
        //                                    //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                    //img.ImageUrl = "data:image/png;base64," + base64Striing;
        //                                    //img.AlternateText = "icon title";
        //                                    //img.Style.Add("width", "150px");
        //                                    //imageCell.Controls.Add(img);
        //                                    // string imagePath = UrlImage + image_file;
        //                                    // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                    // string base64String = Convert.ToBase64String(imageBytes);
        //                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = "image_file/" + image_file,
        //                                        AlternateText = "icon title"
        //                                    };
        //                                    img.Style.Add("width", "150px");
        //                                    imageCell.Controls.Add(img);
        //                                    // string imagePath2 = UrlImage + plate_number_file;
        //                                    // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                    // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = "image_file/" + plate_number_file,
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
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64Stiring;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    //imageCell.ColumnSpan = 2;
        //                                    imageCell2.Text = "";
        //                                    row.Cells.Add(imageCell);
        //                                    row.Cells.Add(imageCell2);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                imageCell.HorizontalAlign = HorizontalAlign.Center;
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
        //                            //row.Cells.Add(no);
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

        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file != '' or ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from ORDER BY ce.occurred_at ASC", con))
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
        //                            //Response.Write(occurred_at + "tengah");
        //                            string person = dr["person"].ToString();
        //                                string location = dr["location"].ToString();
        //                                string event_type = dr["event_type"].ToString();
        //                                string site = dr["site"].ToString();
        //                                string person_type = dr["person_type"].ToString();
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
        //                                    //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                    //img.ImageUrl = "data:image/png;base64," + base64Striing;
        //                                    //img.AlternateText = "icon title";
        //                                    //img.Style.Add("width", "150px");
        //                                    //imageCell.Controls.Add(img);
        //                                    // string imagePath = UrlImage + image_file;
        //                                    // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                    // string base64String = Convert.ToBase64String(imageBytes);
        //                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = "image_file/" + image_file,
        //                                        AlternateText = "icon title"
        //                                    };
        //                                    img.Style.Add("width", "150px");
        //                                    imageCell.Controls.Add(img);
        //                                    // string imagePath2 = UrlImage + plate_number_file;
        //                                    // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                    // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = "image_file/" + plate_number_file,
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
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64Striing;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    //imageCell.ColumnSpan = 2;
        //                                    imageCell2.Text = "";
        //                                    row.Cells.Add(imageCell);
        //                                    row.Cells.Add(imageCell2);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                imageCell.HorizontalAlign = HorizontalAlign.Center;
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
        //                            //row.Cells.Add(no);
        //                                //row.Cells.Add(imageCell);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);

        //                                TableBody.Controls.Add(row);
        //                        }
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
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file != '' or ce.image_file != NULL) AND DATE(ce.occurred_at) <=@to ORDER BY ce.occurred_at ASC", con))
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
        //                            //Response.Write(occurred_at + "bawah");
        //                            string person = dr["person"].ToString();
        //                                string location = dr["location"].ToString();
        //                                string event_type = dr["event_type"].ToString();
        //                                string site = dr["site"].ToString();
        //                                string person_type = dr["person_type"].ToString();
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
        //                                    //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                    //img.ImageUrl = "data:image/png;base64," + base64Striing;
        //                                    //img.AlternateText = "icon title";
        //                                    //img.Style.Add("width", "150px");
        //                                    //imageCell.Controls.Add(img);
        //                                    // string imagePath = UrlImage + image_file;
        //                                    // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                    // string base64String = Convert.ToBase64String(imageBytes);
        //                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = "image_file/" + image_file,
        //                                        AlternateText = "icon title"
        //                                    };
        //                                    img.Style.Add("width", "150px");
        //                                    imageCell.Controls.Add(img);
        //                                    // string imagePath2 = UrlImage + plate_number_file;
        //                                    // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                    // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                    System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                    {
        //                                        ImageUrl = "image_file/" + plate_number_file,
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
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64Striing;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    //imageCell.ColumnSpan = 2;
        //                                    imageCell2.Text = "";
        //                                    row.Cells.Add(imageCell);
        //                                    row.Cells.Add(imageCell2);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                imageCell.HorizontalAlign = HorizontalAlign.Center;
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
        //                                row.Cells.Add(imageCell);
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
        //        //}

        //    }
        //}



        //protected void ExportToExcelWithImages()
        //{
        //    // Buat workbook baru
        //    IWorkbook workbook = new XSSFWorkbook();
        //    ISheet sheet = workbook.CreateSheet("Report Event History");

        //    // Buat style untuk header
        //    ICellStyle headerStyle = workbook.CreateCellStyle();
        //    IFont font = workbook.CreateFont();
        //    font.IsBold = true; // Atur font menjadi bold
        //    headerStyle.SetFont(font);

        //    // Tambahkan header dan merge kolom Image 1 dan Image 2
        //    IRow headerRow = sheet.CreateRow(0);
        //    ICell cell0 = headerRow.CreateCell(0);
        //    cell0.SetCellValue("No");
        //    cell0.CellStyle = headerStyle;

        //    ICell cell1 = headerRow.CreateCell(1);
        //    cell1.SetCellValue("Image");
        //    cell1.CellStyle = headerStyle;
        //    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 2)); // Merge kolom Image 1 dan Image 2

        //    ICell cell3 = headerRow.CreateCell(3);
        //    cell3.SetCellValue("Occurred At");
        //    cell3.CellStyle = headerStyle;

        //    ICell cell4 = headerRow.CreateCell(4);
        //    cell4.SetCellValue("Location");
        //    cell4.CellStyle = headerStyle;

        //    ICell cell5 = headerRow.CreateCell(5);
        //    cell5.SetCellValue("Site");
        //    cell5.CellStyle = headerStyle;

        //    ICell cell6 = headerRow.CreateCell(6);
        //    cell6.SetCellValue("Person");
        //    cell6.CellStyle = headerStyle;

        //    ICell cell7 = headerRow.CreateCell(7);
        //    cell7.SetCellValue("Status");
        //    cell7.CellStyle = headerStyle;

        //    // Atur lebar kolom agar sesuai dengan ukuran gambar (150px)
        //    sheet.SetColumnWidth(1, 150 * 256 / 8); // Kolom Image 1
        //    sheet.SetColumnWidth(2, 150 * 256 / 8); // Kolom Image 2

        //    // Tambahkan data baris
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        IRow row = sheet.CreateRow(i);
        //        row.CreateCell(0).SetCellValue(i);
        //        row.CreateCell(3).SetCellValue(DateTime.Now.ToString("dd MMM yyyy"));
        //        row.CreateCell(4).SetCellValue("Location " + i);
        //        row.CreateCell(5).SetCellValue("Site " + i);
        //        row.CreateCell(6).SetCellValue("Person " + i);
        //        row.CreateCell(7).SetCellValue("Status " + i);

        //        // Set tinggi baris agar sesuai dengan ukuran gambar (dalam poin)
        //        row.HeightInPoints = (float)(150 * 0.75); // 150px sesuai dengan 150 * 0.75 untuk ukuran Excel

        //        // Masukkan gambar pertama ke dalam cell kedua
        //        string imagePath1 = Server.MapPath("~/bootstrap/logo.png");
        //        byte[] imageData1 = File.ReadAllBytes(imagePath1);
        //        int pictureIndex1 = workbook.AddPicture(imageData1, PictureType.PNG);
        //        IDrawing drawing1 = sheet.CreateDrawingPatriarch();
        //        IClientAnchor anchor1 = drawing1.CreateAnchor(0, 0, 0, 0, 1, i, 2, i + 1);
        //        IPicture picture1 = drawing1.CreatePicture(anchor1, pictureIndex1);
        //        picture1.Resize(1.0, 1.0); // Fit gambar sesuai ukuran cell tanpa resize lebih lanjut

        //        // Masukkan gambar kedua ke dalam cell ketiga, jika ada
        //        string imagePath2 = Server.MapPath("~/bootstrap/logo2.png"); // Path gambar kedua
        //        if (File.Exists(imagePath2))
        //        {
        //            byte[] imageData2 = File.ReadAllBytes(imagePath2);
        //            int pictureIndex2 = workbook.AddPicture(imageData2, PictureType.PNG);
        //            IDrawing drawing2 = sheet.CreateDrawingPatriarch();
        //            IClientAnchor anchor2 = drawing2.CreateAnchor(0, 0, 0, 0, 2, i, 3, i + 1);
        //            IPicture picture2 = drawing2.CreatePicture(anchor2, pictureIndex2);
        //            picture2.Resize(1.0, 1.0); // Fit gambar sesuai ukuran cell tanpa resize lebih lanjut
        //        }
        //        else
        //        {
        //            // Jika gambar kedua tidak ada, biarkan cell kosong
        //            row.CreateCell(2).SetCellValue(""); // Mengisi cell dengan kosong
        //        }
        //    }

        //    // Tulis workbook ke output stream
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        workbook.Write(stream);
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", "attachment;filename=ReportEventHistory.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.BinaryWrite(stream.ToArray());
        //        Response.End();
        //    }
        //}


        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string dari = TextBox1.Text;
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
                //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                dari = today;

            }
            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            //Session["site_event"] = camera_site;
            Session["from_event"] = dari;
            Session["to_event"] = ke;
            // Menentukan nama file untuk file Excel
            //export();
            exportToCSV();
            //ExportToExcelWithImages();
        }

        protected void exportt()
        {
            string fileName = "Report Event History.xls";
            string from = Session["from_event"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from);
            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
            string per2 = "";
            if (Session["to_event"] != null && !string.IsNullOrEmpty(Session["to_event"].ToString()))
            {
                to = Session["to_event"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                per2 += to_date.Substring(8, 2) + "-" + to_date.Substring(5, 2) + "-" + to_date.Substring(0, 4);
            }
            else
            {
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    string query = @"SELECT MAX(ce.occurred_at) as to_date, ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL)";
                    if (from_date != null)
                    {
                        query += @" AND ce.occurred_at >=@from";
                    }
                    if (to_date != null)
                    {
                        query += @" AND ce.occurred_at <=@to";
                    }
                    query += @" limit 1";
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
                                    DateTime occurredAt = (DateTime)dr["to_date"];
                                    string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    per2 += formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                }
                            }
                        }
                    }
                }
            }
            // Menentukan tipe konten untuk Excel
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report Event History");

                // Header
                worksheet.Cells[1, 1].Value = "No";
                worksheet.Cells[1, 2].Value = "Image";
                worksheet.Cells[1, 3].Value = "Occurred At";
                worksheet.Cells[1, 4].Value = "Location";
                worksheet.Cells[1, 5].Value = "Site";
                worksheet.Cells[1, 6].Value = "Person";
                worksheet.Cells[1, 7].Value = "Status";

                int rowIndex = 2;

                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    con.Open();
                    string query = @"SELECT MAX(ce.occurred_at) as to_date, ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL)";
                    if (from_date != null)
                    {
                        query += @" AND ce.occurred_at >=@from";
                    }
                    if (to_date != null)
                    {
                        query += @" AND ce.occurred_at <=@to";
                    }
                    query += @" limit 1";
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
                            while (dr.Read())
                            {
                                worksheet.Cells[rowIndex, 1].Value = rowIndex - 1; // No
                                worksheet.Cells[rowIndex, 3].Value = Convert.ToDateTime(dr["occurred_at"]).ToString("yyyy-MM-dd HH:mm:ss"); // Occurred At
                                worksheet.Cells[rowIndex, 4].Value = dr["location"].ToString(); // Location
                                worksheet.Cells[rowIndex, 5].Value = dr["site"].ToString(); // Site
                                worksheet.Cells[rowIndex, 6].Value = dr["person"].ToString(); // Person

                                // Menambahkan gambar
                                string imagePath = UrlImage + dr["image_file"].ToString();
                                if (File.Exists(imagePath))
                                {
                                    var excelImage = worksheet.Drawings.AddPicture($"Image{rowIndex}", new FileInfo(imagePath));
                                    excelImage.SetPosition(rowIndex - 1, 0, 1, 0); // Set posisi gambar (rowIndex - 1 untuk menyesuaikan dengan Excel)
                                    excelImage.SetSize(150, 150); // Ukuran gambar
                                }

                                rowIndex++;
                            }
                        }
                    }
                }

                // Simpan file ke response
                var stream = new MemoryStream();
                package.SaveAs(stream);
                Response.BinaryWrite(stream.ToArray());
                Response.Flush();
                Response.End();
            }
        }
        protected void export()
        {
            string fileName = "Report Event History.xls";
            string from = Session["from_event"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from);
            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
            string per2 = "";
            if (Session["to_event"] != null && !string.IsNullOrEmpty(Session["to_event"].ToString()))
            {
                to = Session["to_event"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                per2 += to_date.Substring(8, 2) + "-" + to_date.Substring(5, 2) + "-" + to_date.Substring(0, 4);
            }
            else
            {
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    string query = @"SELECT MAX(ce.occurred_at) as to_date, ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL)";
                    if (from_date != null)
                    {
                        query += @" AND ce.occurred_at >=@from";
                    }
                    if (to_date != null)
                    {
                        query += @" AND ce.occurred_at <=@to";
                    }
                    query += @" limit 1";
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
                                    DateTime occurredAt = (DateTime)dr["to_date"];
                                    string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    per2 += formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                }
                            }
                        }
                    }
                }
            }
            // Menentukan tipe konten untuk Excel
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            // Siapkan HTML Table yang valid
            string tableHtml = $@"<table border='1'>
            <tr>
                <td colspan='9' align='center'><b>Report Event History</b></td>
            </tr>
            <tr>
                <td colspan='9' align='center'><b>Periode {per1} to {per2}</b></td>
            </tr>
            <tr>
                <td><b>No</b></td>
                <td><b>Image</b></td>
                <td></td>
                <td><b>Occurred At</b></td>
                <td><b>Location</b></td>
                <td><b>Site</b></td>
                <td><b>Person</b></td>
                <td><b>Plate Number</b></td>
                <td><b>Status</b></td>
            </tr>";


            //DateTime fromDate;
            //DateTime toDate;
            int x = 0;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code, v.plate_number as plate, ce.plate_number, vh.entry_code as code_vehicle FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL)";
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
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string person = dr["person"].ToString();
                                string location = dr["location"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string person_type = dr["person_type"].ToString();
                                string person_image = dr["person_image"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                string plate_number = dr["plate_number"].ToString();
                                //Response.Write(entry_code);
                                //string pers = "";
                                string statusnya = "";
                                int cek = event_type.IndexOf("Plate");
                                tableHtml += $@"<tr>
                                                <td>{x}</td>";
                                //string image_file = dr["image_file"].ToString();
                                //string imagePath = UrlImage + image_file;
                                //byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                                //string base64Image = Convert.ToBase64String(imageBytes);
                                //string imgTag = $"<img src='data:image/png;base64,{base64Image}' alt='Foto' width='150' height='150'/>";
                                //tableHtml += $@"<td style='width:155; height:155;'><center>{imgTag}</center></td>";

                                string path = UrlImage + image_file;
                                //tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path}' alt='Foto' width='150' height='150'/></center></td>";
                                tableHtml += $@"<td style='width:155; height:155;'><center>link foto</center></td>";

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
                                        string path2 = UrlImage + plate_number_file;
                                        //tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path2}' alt='Foto' width='150' height='150px'/></center></td>";
                                        tableHtml += $@"<td style='width:155; height:155;'><center>link foto</center></td>";
                                        //string imagePath2 = UrlImage + plate_number_file;
                                        //byte[] imageBytes2 = System.IO.File.ReadAllBytes(imagePath2);
                                        //string base64Image2 = Convert.ToBase64String(imageBytes2);
                                        //string imgTag2 = $"<img src='data:image/png;base64,{base64Image2}' alt='Foto' width='150' height='150'/>";
                                        //tableHtml += $@"<td style='width:155; height:155;'><center>{imgTag2}</center></td>";
                                    }
                                }
                                else
                                {
                                    statusnya += event_type;
                                    if (!string.IsNullOrEmpty(person) && !string.IsNullOrEmpty(person_image))
                                    {
                                        string path2 = UrlImagePerson + person_image;
                                        //tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path2}' alt='Foto' width='150' height='150'/></center></td>";
                                        tableHtml += $@"<td style='width:155; height:155;'><center>link foto</center></td>";
                                        //string imagePath2 = UrlImagePerson + person_image;
                                        //byte[] imageBytes2 = System.IO.File.ReadAllBytes(imagePath2);
                                        //string base64Image2 = Convert.ToBase64String(imageBytes2);
                                        //string imgTag2 = $"<img src='data:image/png;base64,{base64Image2}' alt='Foto' width='150' height='150'/>";
                                        //tableHtml += $@"<td style='width:155; height:155;'><center>{imgTag2}</center></td>";
                                    }
                                    else
                                    {
                                        tableHtml += $@"<td style='width:155;height:155;'><center></center></td>";
                                    }
                                }
                                tableHtml += $@"<td>{occurred_at}</td>
                                                <td>{location}</td>
                                                <td>{site}</td>";
                                if (cek != -1)
                                {
                                    tableHtml += $@"<td></td>";
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(person))
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
                                        tableHtml += $@"<td><span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry}</span></td>";
                                    }
                                    else
                                    {
                                        tableHtml += $@"<td>{person}</td>";
                                    }
                                }
                                tableHtml += $@"<td>{plate_number}</td>
                                                <td>{statusnya}</td>
                                            </tr>";

                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }
                }
                //}

            }
            tableHtml += @"</table>";

            // Menuliskan HTML ke response
            Response.Output.Write(tableHtml);
            Response.Flush();
            Response.End();
        }

        protected void exportToCSV()
        {
            string fileName = "Report_Event_History.csv";
            string from = Session["from_event"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from);
            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
            string per2 = "";


            if (Session["to_event"] != null && !string.IsNullOrEmpty(Session["to_event"].ToString()))
            {
                to = Session["to_event"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                per2 += to_date.Substring(8, 2) + "-" + to_date.Substring(5, 2) + "-" + to_date.Substring(0, 4);
            }
            else
            {
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    string query = @"SELECT MAX(ce.occurred_at) as to_date FROM camera_events as ce WHERE (ce.image_file != '' or ce.image_file != NULL)";
                    if (from_date != null)
                    {
                        query += @" AND ce.occurred_at >= @from";
                    }
                    if (to_date != null)
                    {
                        query += @" AND ce.occurred_at <= @to";
                    }
                    query += @" LIMIT 1";
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
                                    DateTime occurredAt = (DateTime)dr["to_date"];
                                    string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    per2 += formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                }
                            }
                        }
                    }
                }
            }

            // Menentukan tipe konten untuk CSV
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "text/csv";
            string fullUrl = Request.Url.AbsoluteUri;
            string absolutePath = Request.Url.AbsolutePath;
            string baseUrl = fullUrl.Replace(absolutePath, "");
            // Siapkan header untuk CSV
            string csvContent = $"\"Report Event History\"\n";
            csvContent += $"\"Periode {per1} to {per2}\"\n";
            csvContent += "\"Image\",,\"Occurred At\",\"Location\",\"Site\",\"Person\",\"Plate Number\",\"Status\"\n";

            int x = 0;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                string query = @"SELECT ce.camera_id, ce.type AS event_type, ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code, v.plate_number as plate, ce.plate_number, vh.entry_code as code_vehicle FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id = cs.id LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) LEFT JOIN vehicles v ON ce.plate_number = v.plate_number LEFT JOIN vehicle_history vh ON v.id = vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL)";
                if (from_date != null)
                {
                    query += @" AND ce.occurred_at >= @from";
                }
                if (to_date != null)
                {
                    query += @" AND ce.occurred_at <= @to";
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
                                string image_file = dr["image_file"].ToString();
                                string occurred_at = Convert.ToDateTime(dr["occurred_at"]).ToString("yyyy-MM-dd HH:mm:ss");
                                string person = dr["person"].ToString();
                                string location = dr["location"].ToString();
                                string plate_number_file = dr["plate_number_file"].ToString();
                                string event_type = dr["event_type"].ToString();
                                string site = dr["site"].ToString();
                                string plate_number = dr["plate_number"].ToString();
                                string status = "";
                                string person_type = dr["person_type"].ToString();
                                string person_image = dr["person_image"].ToString();
                                string entry_code = dr["entry_code"].ToString();
                                int cek = event_type.IndexOf("Plate");
                                // Tentukan status dari event_type
                                if (event_type.Contains("Plate"))
                                {
                                    string code_vehicle = dr["code_vehicle"].ToString();
                                    status = string.IsNullOrEmpty(code_vehicle) ? "Unrecognized" :
                                             code_vehicle == "0" ? "Comply" :
                                             code_vehicle == "1" ? "Not Approved" :
                                             code_vehicle == "2" ? "" : "";
                                }
                                else
                                {
                                    status = event_type;
                                }
                                string person_name = "";
                                string link1 = baseUrl + "/image_file/" + image_file;
                                string link2 = baseUrl;
                                if (cek != -1)
                                {
                                    link2 += "/image_file/" + plate_number_file;
                                    // Tidak ada data, tambahkan kolom kosong ke CSV
                                    person_name += "";
                                }
                                else
                                {
                                    link2 += "/person_image/" + person_image;
                                    if (!string.IsNullOrEmpty(person))
                                    {
                                        string entry = "";
                                        if (person_type == "blacklist")
                                        {
                                            entry = "Blacklist";
                                        }
                                        else
                                        {
                                            if (entry_code == "0")
                                            {
                                                entry = "Comply";
                                            }
                                            else if (entry_code == "1")
                                            {
                                                entry = "Not Comply or Badge Expired";
                                            }
                                            else if (entry_code == "2")
                                            {
                                                entry = "FTW Rejected Medical / Expired";
                                            }
                                            else if (entry_code == "3")
                                            {
                                                entry = "Daily Checkup Failed";
                                            }
                                        }

                                        // Format string untuk CSV dengan informasi person dan entry
                                        person_name += $"{char.ToUpper(person_type[0]) + person_type.Substring(1)} - {person} - {entry}";
                                    }
                                    else
                                    {
                                        // Jika tidak ada data untuk person, tambahkan nilai kosong
                                        person_name += $"{person}";
                                    }
                                }


                                // Menambahkan setiap baris data ke CSV
                                csvContent += $"\"{link1}\",\"{link2}\",\"{occurred_at}\",\"{location}\",\"{site}\",\"{person_name}\",\"{plate_number}\",\"{status}\"\n";
                            }
                        }
                    }
                }
            }

            // Tulis konten CSV ke response
            Response.Output.Write(csvContent);
            Response.Flush();
            Response.End();
        }



        protected void btnExportExcell_Click(object sender, EventArgs e)
        {
            Response.Redirect("report_event_excel.aspx");
            //string dari = TextBox1.Text;
            //DateTime fromDate;
            //if (DateTime.TryParse(dari.ToString(), out fromDate))
            //{
            //    //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            //}
            //else
            //{
            //    string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
            //    dari = today;

            //}
            ////Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            //string ke = TextBox2.Text;
            ////Session["site_event"] = camera_site;
            //Session["from_event"] = dari;
            //Session["to_event"] = ke;
            //ExportToExcelWithImages();
        }

        protected void ExportToExcelWithImages()
        {
            // Buat workbook baru
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Report Event History");

            // Buat style untuk header
            ICellStyle headerStyle = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.IsBold = true; // Atur font menjadi bold
            headerStyle.SetFont(font);
            headerStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; // Spesifikasikan namespace NPOI



            // Tambahkan header dan merge kolom Image 1 dan Image 2
            IRow rowHeader = sheet.CreateRow(3);
            rowHeader.CreateCell(0).SetCellValue("No");
            rowHeader.GetCell(0).CellStyle = headerStyle;

            ICell cellImageHeader = rowHeader.CreateCell(1);
            cellImageHeader.SetCellValue("Image");
            cellImageHeader.CellStyle = headerStyle;
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 2)); // Merge kolom Image 1 dan Image 2

            rowHeader.CreateCell(3).SetCellValue("Occurred At");
            rowHeader.GetCell(3).CellStyle = headerStyle;
            rowHeader.CreateCell(4).SetCellValue("Location");
            rowHeader.GetCell(4).CellStyle = headerStyle;
            rowHeader.CreateCell(5).SetCellValue("Site");
            rowHeader.GetCell(5).CellStyle = headerStyle;
            rowHeader.CreateCell(6).SetCellValue("Person");
            rowHeader.GetCell(6).CellStyle = headerStyle;
            rowHeader.CreateCell(7).SetCellValue("Status");
            rowHeader.GetCell(7).CellStyle = headerStyle;

            // Atur lebar kolom agar sesuai dengan ukuran gambar (150px)
            sheet.SetColumnWidth(1, 150 * 256 / 8); // Kolom Image 1
            sheet.SetColumnWidth(2, 150 * 256 / 8); // Kolom Image 2

            string from = Session["from_event"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from);
            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            if (Session["to_event"] != null && !string.IsNullOrEmpty(Session["to_event"].ToString()))
            {
                to = Session["to_event"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {

            }

            //DateTime fromDate;
            //DateTime toDate;

            int i = 3;
            int x = 0;
            string per1 = "";
            string per2 = "";

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type,p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' or ce.image_file != NULL)";
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
                                i++;
                                x++;
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
                                string person_image = dr["person_image"].ToString();
                                IRow row = sheet.CreateRow(i);
                                row.CreateCell(0).SetCellValue(x);
                                row.CreateCell(3).SetCellValue(occurred_at);
                                row.CreateCell(4).SetCellValue(location);
                                row.CreateCell(5).SetCellValue(site);
                                row.CreateCell(7).SetCellValue(event_type);
                                int cek = event_type.IndexOf("Plate");
                                string path = "";
                                string path2 = "";
                                if (cek != -1)
                                {
                                    if (!string.IsNullOrEmpty(plate_number_file))
                                    {
                                        path += "~/image_file/" + image_file;
                                        path2 += "~/image_file/" + plate_number_file;
                                    }
                                    row.CreateCell(6).SetCellValue("");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        path += "~/image_file/" + image_file;
                                        path2 += "~/person_image/" + person_image;
                                    }
                                    if (!string.IsNullOrEmpty(person_image))
                                    {
                                        ICellStyle cellStyle = workbook.CreateCellStyle();
                                        IFont personFont = workbook.CreateFont();
                                        string warna = "";

                                        if (person_type == "blacklist")
                                        {
                                            warna = "red";
                                        }
                                        else
                                        {
                                            if (entry_code == "0")
                                            {
                                                warna = "green";
                                            }
                                            else if (entry_code == "1")
                                            {
                                                warna = "orange";
                                            }
                                            else if (entry_code == "2")
                                            {
                                                warna = "red";
                                            }
                                        }

                                        // Set warna font
                                        personFont.Color = IndexedColors.Red.Index; // Contoh menggunakan warna merah
                                        if (warna == "green") personFont.Color = IndexedColors.Green.Index;
                                        if (warna == "orange") personFont.Color = IndexedColors.Orange.Index;
                                        if (warna == "red") personFont.Color = IndexedColors.Red.Index;

                                        cellStyle.SetFont(personFont);

                                        // Atur teks dalam sel
                                        ICell cellPerson = row.CreateCell(6);
                                        cellPerson.SetCellValue($"[{char.ToUpper(person_type[0]) + person_type.Substring(1)}] - {person} - {entry_code}");
                                        cellPerson.CellStyle = cellStyle;
                                    }
                                    else
                                    {
                                        row.CreateCell(6).SetCellValue(person);
                                    }

                                }
                                row.HeightInPoints = (float)(150 * 0.75); // 150px sesuai dengan 150 * 0.75 untuk ukuran Excel

                                // Masukkan gambar pertama ke dalam cell kedua
                                string imagePath3 = Server.MapPath(path);
                                byte[] imageData3 = File.ReadAllBytes(imagePath3);
                                int pictureIndex3 = workbook.AddPicture(imageData3, PictureType.PNG);
                                IDrawing drawing3 = sheet.CreateDrawingPatriarch();
                                IClientAnchor anchor3 = drawing3.CreateAnchor(0, 0, 0, 0, 1, i, 2, i + 1);
                                IPicture picture3 = drawing3.CreatePicture(anchor3, pictureIndex3);
                                picture3.Resize(1.0, 1.0); // Fit gambar sesuai ukuran cell tanpa resize lebih lanjut

                                // Masukkan gambar kedua ke dalam cell ketiga, jika ada
                                string imagePath4 = Server.MapPath(path2); // Path gambar kedua
                                if (File.Exists(imagePath4))
                                {
                                    byte[] imageData4 = File.ReadAllBytes(imagePath4);
                                    int pictureIndex4 = workbook.AddPicture(imageData4, PictureType.PNG);
                                    IDrawing drawing4 = sheet.CreateDrawingPatriarch();
                                    IClientAnchor anchor4 = drawing4.CreateAnchor(0, 0, 0, 0, 2, i, 3, i + 1);
                                    IPicture picture4 = drawing4.CreatePicture(anchor4, pictureIndex4);
                                    picture4.Resize(1.0, 1.0); // Fit gambar sesuai ukuran cell tanpa resize lebih lanjut
                                }
                                else
                                {
                                    // Jika gambar kedua tidak ada, biarkan cell kosong
                                    row.CreateCell(2).SetCellValue(""); // Mengisi cell dengan kosong
                                }
                                if (to_date == null)
                                {
                                    //DateTime occurredAt = (DateTime)dr["occurred_at"];
                                    string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    per2 = formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                }
                                else if (from_date == null)
                                {
                                    if (x.ToString() == "1")
                                    {
                                        //DateTime occurredAt = (DateTime)dr["occurred_at"];
                                        string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                        per1 = formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
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
                        per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
                        per2 = to_date.Substring(8, 2) + "-" + to_date.Substring(5, 2) + "-" + to_date.Substring(0, 4);
                    }
                    else if (from_date != null)
                    {
                        per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
                    }
                    else if (to_date != null)
                    {
                        per2 = to_date.Substring(8, 2) + "-" + to_date.Substring(5, 2) + "-" + to_date.Substring(0, 4);
                    }
                }
            }
            //for (int j = 1; j < 5000; j++)
            //{
            //    i++;
            //    x++;
            //    string image_file = "logo.png";
            //    string plate_number_file = "plate_number_file" + j;
            //    string occurred_at = "occurred_at"+j;
            //    string person = "person" + j;
            //    string location = "location" + j;
            //    string event_type = "event_type" + j;
            //    string site = "site" + j;
            //    string person_type = "person_type" + j;
            //    string entry_code = "0";
            //    string person_image = "logo.png";
            //    IRow row = sheet.CreateRow(i);
            //    row.CreateCell(0).SetCellValue(x);
            //    row.CreateCell(3).SetCellValue(occurred_at);
            //    row.CreateCell(4).SetCellValue(location);
            //    row.CreateCell(5).SetCellValue(site);
            //    row.CreateCell(7).SetCellValue(event_type);
            //    int cek = event_type.IndexOf("Plate");
            //    string path = "";
            //    string path2 = "";
            //    if (cek != -1)
            //    {
            //        if (!string.IsNullOrEmpty(plate_number_file))
            //        {
            //            path += "~/image_file/" + image_file;
            //            path2 += "~/image_file/" + plate_number_file;
            //        }
            //        row.CreateCell(6).SetCellValue("");
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrEmpty(image_file))
            //        {
            //            path += "~/image_file/" + image_file;
            //            path2 += "~/person_image/" + person_image;
            //        }
            //        if (!string.IsNullOrEmpty(person_image))
            //        {
            //            ICellStyle cellStyle = workbook.CreateCellStyle();
            //            IFont personFont = workbook.CreateFont();
            //            string warna = "";

            //            if (person_type == "blacklist")
            //            {
            //                warna = "red";
            //            }
            //            else
            //            {
            //                if (entry_code == "0")
            //                {
            //                    warna = "green";
            //                }
            //                else if (entry_code == "1")
            //                {
            //                    warna = "yellow";
            //                }
            //                else if (entry_code == "2")
            //                {
            //                    warna = "orange";
            //                }
            //            }

            //            // Set warna font
            //            personFont.Color = IndexedColors.Red.Index; // Contoh menggunakan warna merah
            //            if (warna == "green") personFont.Color = IndexedColors.Green.Index;
            //            if (warna == "yellow") personFont.Color = IndexedColors.Yellow.Index;
            //            if (warna == "orange") personFont.Color = IndexedColors.Orange.Index;

            //            cellStyle.SetFont(personFont);

            //            // Atur teks dalam sel
            //            ICell cellPerson = row.CreateCell(6);
            //            cellPerson.SetCellValue($"[{char.ToUpper(person_type[0]) + person_type.Substring(1)}] - {person} - {entry_code}");
            //            cellPerson.CellStyle = cellStyle;
            //        }
            //        else
            //        {
            //            row.CreateCell(6).SetCellValue(person);
            //        }

            //    }
            //    row.HeightInPoints = (float)(150 * 0.75); // 150px sesuai dengan 150 * 0.75 untuk ukuran Excel

            //    // Masukkan gambar pertama ke dalam cell kedua
            //    string imagePath3 = Server.MapPath(path);
            //    byte[] imageData3 = File.ReadAllBytes(imagePath3);
            //    int pictureIndex3 = workbook.AddPicture(imageData3, PictureType.PNG);
            //    IDrawing drawing3 = sheet.CreateDrawingPatriarch();
            //    IClientAnchor anchor3 = drawing3.CreateAnchor(0, 0, 0, 0, 1, i, 2, i + 1);
            //    IPicture picture3 = drawing3.CreatePicture(anchor3, pictureIndex3);
            //    picture3.Resize(1.0, 1.0); // Fit gambar sesuai ukuran cell tanpa resize lebih lanjut

            //    // Masukkan gambar kedua ke dalam cell ketiga, jika ada
            //    string imagePath4 = Server.MapPath(path2); // Path gambar kedua
            //    if (File.Exists(imagePath4))
            //    {
            //        byte[] imageData4 = File.ReadAllBytes(imagePath4);
            //        int pictureIndex4 = workbook.AddPicture(imageData4, PictureType.PNG);
            //        IDrawing drawing4 = sheet.CreateDrawingPatriarch();
            //        IClientAnchor anchor4 = drawing4.CreateAnchor(0, 0, 0, 0, 2, i, 3, i + 1);
            //        IPicture picture4 = drawing4.CreatePicture(anchor4, pictureIndex4);
            //        picture4.Resize(1.0, 1.0); // Fit gambar sesuai ukuran cell tanpa resize lebih lanjut
            //    }
            //    else
            //    {
            //        // Jika gambar kedua tidak ada, biarkan cell kosong
            //        row.CreateCell(2).SetCellValue(""); // Mengisi cell dengan kosong
            //    }
            //}
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 7)); // Merge baris 1 kolom 0-7
            IRow headerRow1 = sheet.CreateRow(0);
            ICell headerCell1 = headerRow1.CreateCell(0);
            headerCell1.SetCellValue("Report Event History");
            headerCell1.CellStyle = headerStyle;

            // Merge cells untuk periode
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 7)); // Merge baris 2 kolom 0-7
            IRow headerRow2 = sheet.CreateRow(1);
            ICell headerCell2 = headerRow2.CreateCell(0);
            headerCell2.SetCellValue($"Periode {per1} to {per2}");
            headerCell2.CellStyle = headerStyle;

            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ReportEventHistory.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
        }

        protected void ExportToExcelWithImages2()
        {
            // Buat workbook baru


            string from = Session["from_event"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from);
            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            if (Session["to_event"] != null && !string.IsNullOrEmpty(Session["to_event"].ToString()))
            {
                to = Session["to_event"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {

            }

            //DateTime fromDate;
            //DateTime toDate;

            //int i = 3;
            //int x = 0;
            //string per1 = "";
            //string per2 = "";

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                //int batchSize = 1000; // Jumlah data per file Excel
                //int totalRecords = GetTotalRecordCount(from_date, to_date, strcon); // Dapatkan total jumlah data
                //int totalPages = (int)Math.Ceiling((double)totalRecords / batchSize); // Hitung total halaman

                //for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
                //{
                int totalRecords = GetTotalRecordCount(from_date, to_date, strcon); // Fungsi yang menghitung total record dari database
                int batchSize = 1000; // Batas per file
                int fileIndex = 1; // Digunakan untuk penamaan file

                for (int offset = 0; offset < totalRecords; offset += batchSize)
                {
                    //int offset = pageIndex * batchSize;
                    ExportToExcel(from_date, to_date, batchSize, offset, fileIndex);
                    fileIndex++;


                }
            }
        }

        public void ExportToExcel(string from_date, string to_date, int batchSize, int offset, int fileIndex)
        {
            Response.Write("Masuk KO");
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = @"SELECT ce.camera_id, ce.type AS event_type, ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code 
                         FROM camera_events as ce 
                         JOIN cameras as c ON ce.camera_id = c.name 
                         LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number 
                         JOIN camera_sites AS cs ON c.camera_site_id = cs.id 
                         LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)
                         WHERE (ce.image_file != '' or ce.image_file != NULL)";

                // Tambahkan kondisi tanggal jika ada
                if (from_date != null)
                {
                    query += " AND ce.occurred_at >= @from";
                }
                if (to_date != null)
                {
                    query += " AND ce.occurred_at <= @to";
                }

                // Tambahkan limit dan offset untuk paging
                query += @" ORDER BY ce.occurred_at ASC LIMIT @limit OFFSET @offset";
                //Response.Write("limit "+batchSize);
                //Response.Write("offset "+ pageIndex * batchSize);
                //Response.Write(query);

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    if (from_date != null)
                    {
                        cmd.Parameters.AddWithValue("@from", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd.Parameters.AddWithValue("@to", to_date);
                    }

                    // Parameter untuk paging
                    cmd.Parameters.AddWithValue("@limit", batchSize);
                    cmd.Parameters.AddWithValue("@offset", offset);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            // Buat file Excel baru untuk batch ini
                            IWorkbook workbook = new XSSFWorkbook();
                            ISheet sheet = workbook.CreateSheet("Report Event History");

                            // Buat style untuk header
                            ICellStyle headerStyle = workbook.CreateCellStyle();
                            IFont font = workbook.CreateFont();
                            font.IsBold = true; // Atur font menjadi bold
                            headerStyle.SetFont(font);
                            headerStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; // Spesifikasikan namespace NPOI



                            // Tambahkan header dan merge kolom Image 1 dan Image 2
                            IRow rowHeader = sheet.CreateRow(3);
                            rowHeader.CreateCell(0).SetCellValue("No");
                            rowHeader.GetCell(0).CellStyle = headerStyle;

                            ICell cellImageHeader = rowHeader.CreateCell(1);
                            cellImageHeader.SetCellValue("Image");
                            cellImageHeader.CellStyle = headerStyle;
                            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 2)); // Merge kolom Image 1 dan Image 2

                            rowHeader.CreateCell(3).SetCellValue("Occurred At");
                            rowHeader.GetCell(3).CellStyle = headerStyle;
                            rowHeader.CreateCell(4).SetCellValue("Location");
                            rowHeader.GetCell(4).CellStyle = headerStyle;
                            rowHeader.CreateCell(5).SetCellValue("Site");
                            rowHeader.GetCell(5).CellStyle = headerStyle;
                            rowHeader.CreateCell(6).SetCellValue("Person");
                            rowHeader.GetCell(6).CellStyle = headerStyle;
                            rowHeader.CreateCell(7).SetCellValue("Status");
                            rowHeader.GetCell(7).CellStyle = headerStyle;

                            // Atur lebar kolom agar sesuai dengan ukuran gambar (150px)
                            sheet.SetColumnWidth(1, 150 * 256 / 8); // Kolom Image 1
                            sheet.SetColumnWidth(2, 150 * 256 / 8); // Kolom Image 2

                            // Tambahkan header dan isi data dari MySQL
                            //int rowIndex = 0;
                            int i = 3;
                            int x = 0;
                            string per1 = "";
                            string per2 = "";
                            while (dr.Read())
                            {
                                i++;
                                x++;
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
                                string person_image = dr["person_image"].ToString();
                                IRow row = sheet.CreateRow(i);
                                row.CreateCell(0).SetCellValue(x);
                                row.CreateCell(3).SetCellValue(occurred_at);
                                row.CreateCell(4).SetCellValue(location);
                                row.CreateCell(5).SetCellValue(site);
                                row.CreateCell(7).SetCellValue(event_type);
                                int cek = event_type.IndexOf("Plate");
                                string path = "";
                                string path2 = "";
                                if (cek != -1)
                                {
                                    if (!string.IsNullOrEmpty(plate_number_file))
                                    {
                                        path += "~/image_file/" + image_file;
                                        path2 += "~/image_file/" + plate_number_file;
                                    }
                                    row.CreateCell(6).SetCellValue("");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(image_file))
                                    {
                                        path += "~/image_file/" + image_file;
                                        path2 += "~/person_image/" + person_image;
                                    }
                                    if (!string.IsNullOrEmpty(person_image))
                                    {
                                        ICellStyle cellStyle = workbook.CreateCellStyle();
                                        IFont personFont = workbook.CreateFont();
                                        string warna = "";

                                        if (person_type == "blacklist")
                                        {
                                            warna = "red";
                                        }
                                        else
                                        {
                                            if (entry_code == "0")
                                            {
                                                warna = "green";
                                            }
                                            else if (entry_code == "1")
                                            {
                                                warna = "orange";
                                            }
                                            else if (entry_code == "2")
                                            {
                                                warna = "red";
                                            }
                                        }

                                        // Set warna font
                                        personFont.Color = IndexedColors.Red.Index; // Contoh menggunakan warna merah
                                        if (warna == "green") personFont.Color = IndexedColors.Green.Index;
                                        if (warna == "orange") personFont.Color = IndexedColors.Orange.Index;
                                        if (warna == "red") personFont.Color = IndexedColors.Red.Index;

                                        cellStyle.SetFont(personFont);

                                        // Atur teks dalam sel
                                        ICell cellPerson = row.CreateCell(6);
                                        cellPerson.SetCellValue($"[{char.ToUpper(person_type[0]) + person_type.Substring(1)}] - {person} - {entry_code}");
                                        cellPerson.CellStyle = cellStyle;
                                    }
                                    else
                                    {
                                        row.CreateCell(6).SetCellValue(person);
                                    }

                                }
                                row.HeightInPoints = (float)(150 * 0.75); // 150px sesuai dengan 150 * 0.75 untuk ukuran Excel

                                // Masukkan gambar pertama ke dalam cell kedua
                                string imagePath3 = Server.MapPath(path);
                                byte[] imageData3 = File.ReadAllBytes(imagePath3);
                                int pictureIndex3 = workbook.AddPicture(imageData3, PictureType.PNG);
                                IDrawing drawing3 = sheet.CreateDrawingPatriarch();
                                IClientAnchor anchor3 = drawing3.CreateAnchor(0, 0, 0, 0, 1, i, 2, i + 1);
                                IPicture picture3 = drawing3.CreatePicture(anchor3, pictureIndex3);
                                picture3.Resize(1.0, 1.0); // Fit gambar sesuai ukuran cell tanpa resize lebih lanjut

                                // Masukkan gambar kedua ke dalam cell ketiga, jika ada
                                string imagePath4 = Server.MapPath(path2); // Path gambar kedua
                                if (File.Exists(imagePath4))
                                {
                                    byte[] imageData4 = File.ReadAllBytes(imagePath4);
                                    int pictureIndex4 = workbook.AddPicture(imageData4, PictureType.PNG);
                                    IDrawing drawing4 = sheet.CreateDrawingPatriarch();
                                    IClientAnchor anchor4 = drawing4.CreateAnchor(0, 0, 0, 0, 2, i, 3, i + 1);
                                    IPicture picture4 = drawing4.CreatePicture(anchor4, pictureIndex4);
                                    picture4.Resize(1.0, 1.0); // Fit gambar sesuai ukuran cell tanpa resize lebih lanjut
                                }
                                else
                                {
                                    // Jika gambar kedua tidak ada, biarkan cell kosong
                                    row.CreateCell(2).SetCellValue(""); // Mengisi cell dengan kosong
                                }
                                if (to_date == null)
                                {
                                    //DateTime occurredAt = (DateTime)dr["occurred_at"];
                                    string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                    per2 = formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                }
                                else if (from_date == null)
                                {
                                    if (x.ToString() == "1")
                                    {
                                        //DateTime occurredAt = (DateTime)dr["occurred_at"];
                                        string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                        per1 = formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                    }
                                }
                            }
                            if (from_date != null && to_date != null)
                            {
                                per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
                                per2 = to_date.Substring(8, 2) + "-" + to_date.Substring(5, 2) + "-" + to_date.Substring(0, 4);
                            }
                            else if (from_date != null)
                            {
                                per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
                            }
                            else if (to_date != null)
                            {
                                per2 = to_date.Substring(8, 2) + "-" + to_date.Substring(5, 2) + "-" + to_date.Substring(0, 4);
                            }
                            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 7)); // Merge baris 1 kolom 0-7
                            IRow headerRow1 = sheet.CreateRow(0);
                            ICell headerCell1 = headerRow1.CreateCell(0);
                            headerCell1.SetCellValue("Report Event History");
                            headerCell1.CellStyle = headerStyle;

                            // Merge cells untuk periode
                            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 7)); // Merge baris 2 kolom 0-7
                            IRow headerRow2 = sheet.CreateRow(1);
                            ICell headerCell2 = headerRow2.CreateCell(0);
                            headerCell2.SetCellValue($"Periode {per1} to {per2}");
                            headerCell2.CellStyle = headerStyle;


                            // Simpan file Excel per batch
                            //using (MemoryStream stream = new MemoryStream())
                            //{
                            //    //workbook.Write(stream);
                            //    string fileName = $"Report_Event_Part_{pageIndex + 1}.xlsx";
                            //    //byte[] byteArray = stream.ToArray();

                            //    //// Unduh file
                            //    //Response.Clear();
                            //    //Response.Buffer = true;
                            //    //Response.AddHeader("content-disposition", $"attachment;filename={fileName}");
                            //    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            //    //Response.BinaryWrite(byteArray);
                            //    //Response.Flush();
                            //    workbook.Write(stream);
                            //    Response.Clear();
                            //    Response.Buffer = true;
                            //    Response.AddHeader("content-disposition", $"attachment;filename={fileName}");
                            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            //    Response.BinaryWrite(stream.ToArray());
                            //    Response.Flush();
                            //}
                            using (MemoryStream stream = new MemoryStream())
                            {

                                // Generate Excel content based on dataBatch
                                //CreateExcelContent(sheet, dataBatch);

                                // Write workbook to the memory stream
                                workbook.Write(stream);

                                // Kirimkan file ke browser
                                Response.Clear();
                                Response.Buffer = true;
                                Response.AddHeader("content-disposition", $"attachment;filename=ReportEventHistory_Part{fileIndex}.xlsx");
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.BinaryWrite(stream.ToArray());

                                // End the response for the current file
                                Response.Flush();

                            }

                        }
                    }
                }
            }
        }
        public int GetTotalRecordCount(string from_date, string to_date, string connectionString)
        {
            int totalCount = 0;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // Query untuk menghitung total record
                string countQuery = @"SELECT COUNT(*) FROM camera_events as ce 
                              JOIN cameras as c ON ce.camera_id = c.name 
                              LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number 
                              JOIN camera_sites AS cs ON c.camera_site_id = cs.id 
                              LEFT JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)
                              WHERE (ce.image_file != '' or ce.image_file != NULL)";

                // Tambahkan kondisi tanggal jika ada
                if (!string.IsNullOrEmpty(from_date))
                {
                    countQuery += " AND ce.occurred_at >= @from";
                }
                if (!string.IsNullOrEmpty(to_date))
                {
                    countQuery += " AND ce.occurred_at <= @to";
                }

                using (MySqlCommand cmd = new MySqlCommand(countQuery, con))
                {
                    if (!string.IsNullOrEmpty(from_date))
                    {
                        cmd.Parameters.AddWithValue("@from", from_date);
                    }
                    if (!string.IsNullOrEmpty(to_date))
                    {
                        cmd.Parameters.AddWithValue("@to", to_date);
                    }

                    // Eksekusi query untuk mendapatkan total record count
                    totalCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return totalCount;
        }

    }
}