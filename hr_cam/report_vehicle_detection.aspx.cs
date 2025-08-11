using iText.Html2pdf;
//using iText.Layout.Element;
using iText.Kernel.Pdf;
using iText.Layout;
using MySql.Data.MySqlClient;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class Report_vehicle_detection : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string UrlImage = ConfigurationManager.AppSettings["urlImage"];
        string batas = ConfigurationManager.AppSettings["batas"];
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    //Response.Write("<script>alert('Coba masuk')</script>");
            //    if (Session["site_vehicle_detection"] == null)
            //    {
            //        string camera_site = "none";
            //        Session["site_vehicle_detection"] = camera_site;
            //        //Response.Write("<script>alert('" + Session["from"].ToString() + "')</script>");

            //    }
            //    DropDownList1.Items.Add(new ListItem("", "none"));
            //    //DropDownList2.Items.Add(new ListItem("", "none"));
            //    using (MySqlConnection con = new MySqlConnection(strcon))
            //    {
            //        if (con.State == ConnectionState.Closed)
            //        {
            //            con.Open();
            //        }

            //        using (MySqlCommand cmd = new MySqlCommand("SELECT * from camera_sites", con))
            //        {
            //            using (MySqlDataReader dr = cmd.ExecuteReader())
            //            {
            //                if (dr.HasRows)
            //                {
            //                    while (dr.Read())
            //                    {
            //                        DropDownList1.Items.Add(new ListItem(dr.GetValue(1).ToString(), dr.GetValue(0).ToString()));
            //                        //DropDownList2.Items.Add(new ListItem(dr.GetValue(1).ToString(), dr.GetValue(0).ToString()));
            //                    }
            //                }
            //                else
            //                {
            //                }
            //            }
            //        }

            //    }
            //    if (Session["from_vehicle_detection"] != null)
            //    {
            //    }
            //    else
            //    {
            //        string today = DateTime.Now.ToString("yyyy-MM-dd");
            //        Session["from_vehicle_detection"] = today;

            //    }
            //    if (Session["to_vehicle_detection"] != null)
            //    {
            //        TextBox2.Text = Session["to_vehicle_detection"].ToString();
            //    }
            //    TextBox1.Text = Session["from_vehicle_detection"].ToString();
            //    DropDownList1.SelectedValue = Session["site_vehicle_detection"].ToString();
            //    FillEventHistory();
            //}
            if (!IsPostBack)
            {
                //Response.Write("<script>alert('Coba masuk')</script>");
                SelectMultiple.Items.Add(new ListItem("", "none"));

                //Response.Write("<script>alert('" + Session["site_vehicle_detection"].ToString() + "')</script>");
                if (Session["camera_vehicle_detection"] == null)
                {
                    string camera = "0";
                    Session["camera_vehicle_detection"] = camera;
                    //Response.Write("<script>alert('" + Session["from"].ToString() + "')</script>");

                }
                int jumlahCamera = Convert.ToInt32(Session["camera_vehicle_detection"].ToString());
                if (jumlahCamera > 0)
                {
                    if (Session["camera_vehicle_detection0"].ToString() == "all")
                    {
                        ListItem item2 = new ListItem("All Camera", "all");
                        item2.Selected = true;
                        SelectMultiple.Items.Add(item2);
                    }
                    else
                    {
                        ListItem item2 = new ListItem("All Camera", "all");
                        SelectMultiple.Items.Add(item2);

                    }
                }
                else
                {
                    SelectMultiple.Items.Add(new ListItem("All Camera", "all"));
                }
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.id, c.name, c.location, cs.name as site from cameras c join camera_sites cs on c.camera_site_id=cs.id where c.name like 'LPR%'", con))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                var selectedIds = new List<string>();
                                for (int i = 0; i < jumlahCamera; i++)
                                {
                                    var sessionValue = Session[$"camera_vehicle_detection{i}"];
                                    if (sessionValue == null) break;
                                    selectedIds.Add(sessionValue.ToString());
                                }
                                while (dr.Read())
                                {
                                    //SelectMultiple.Items.Add(new ListItem(dr["site"].ToString() + " - " + dr["location"].ToString(), dr["id"].ToString()));
                                    string id = dr["id"].ToString();
                                    string siteLocation = dr["site"].ToString() + " - " + dr["location"].ToString();

                                    // Tambahkan item ke dalam SelectMultiple
                                    ListItem item = new ListItem(siteLocation, id);

                                    // Tandai sebagai terpilih jika ID ada dalam array selectedIds
                                    if (selectedIds.Contains(id))
                                    {
                                        item.Selected = true;
                                    }

                                    SelectMultiple.Items.Add(item);
                                }
                            }
                            else
                            {
                            }
                        }
                    }

                }
                DropDownList2.Items.Add(new ListItem("All", "all"));
                DropDownList2.Items.Add(new ListItem("Valid DVP", "0"));
                DropDownList2.Items.Add(new ListItem("Expiring DVP", "1"));
                // DropDownList2.Items.Add(new ListItem("", "2"));
                DropDownList2.Items.Add(new ListItem("Unrecognized", "no"));
                if (Session["from_vehicle_detection"] != null)
                {
                }
                else
                {
                    string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                    Session["from_vehicle_detection"] = today;

                }
                if (Session["to_vehicle_detection"] != null)
                {
                    TextBox2.Text = Session["to_vehicle_detection"].ToString();
                }
                if (Session["status_vehicle_detection"] != null)
                {
                    DropDownList2.SelectedValue = Session["status_vehicle_detection"].ToString();
                }
                if (Session["plate_vehicle_detection"] != null)
                {
                    TextBox3.Text = Session["plate_vehicle_detection"].ToString();
                }
                TextBox1.Text = Session["from_vehicle_detection"].ToString();
                //DropDownList1.SelectedValue = Session["site_vehicle_detection"].ToString();
                FillEventHistory();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HtmlSelect selectMultiple = SelectMultiple;
            var i = 0;

            // Pastikan kontrol tidak null sebelum mengakses properti
            if (selectMultiple != null)
            {
                List<string> selectedValues = new List<string>();

                // Loop melalui semua item dalam selectMultiple
                foreach (ListItem item in selectMultiple.Items)
                {
                    if (item.Selected) // Memeriksa apakah item dipilih
                    {
                        selectedValues.Add(item.Value); // Menambahkan nilai item yang dipilih ke list
                        Session["camera_vehicle_detection" + i] = item.Value;
                        i++;
                        if (i == 1 && item.Value == "all")
                        {
                            break;
                        }
                        else
                        {
                        }
                    }
                }

                // Simpan data dalam session
                Session["SelectedValues"] = selectedValues;
                Session["camera_vehicle_detection"] = i;
                //Response.Write("jumlah i:" + Session["camera_vehicle_detection"]);
                for (int x = 0; x < i; x++)
                {
                    //Response.Write("camera nya:" + Session["camera_vehicle_detection" + x].ToString());

                }
                // Opsional: Tampilkan pesan atau alihkan halaman
                //Response.Write("Data berhasil disimpan ke dalam session.");
            }
            else
            {
                // Menangani kasus ketika kontrol tidak ditemukan
                //Response.Write("SelectMultiple kontrol tidak ditemukan.");
            }
            //Response.Write(Session["SelectedValues"]);
            // Opsional: Tampilkan pesan atau alihkan halaman
            //Response.Write("Data berhasil disimpan ke dalam session.");
            string dari = TextBox1.Text;
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
                //Response.Write("masuk semua apa ");
                //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                dari = today;

            }
            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            Session["from_vehicle_detection"] = dari;
            Session["to_vehicle_detection"] = ke;
            Response.Redirect("report_vehicle_detection.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            HtmlSelect selectMultiple = SelectMultiple;
            var i = 0;

            // Pastikan kontrol tidak null sebelum mengakses properti
            if (selectMultiple != null)
            {
                List<string> selectedValues = new List<string>();

                // Loop melalui semua item dalam selectMultiple
                foreach (ListItem item in selectMultiple.Items)
                {
                    if (item.Selected) // Memeriksa apakah item dipilih
                    {
                        selectedValues.Add(item.Value); // Menambahkan nilai item yang dipilih ke list
                        Session["camera_vehicle_detection" + i] = item.Value;
                        i++;
                        if (i == 1 && item.Value == "all")
                        {
                            break;
                        }
                        else
                        {
                        }
                    }
                }

                // Simpan data dalam session
                Session["SelectedValues"] = selectedValues;
                Session["camera_vehicle_detection"] = i;
                //Response.Write("jumlah i:" + Session["camera_vehicle_detection"]);
                for (int x = 0; x < i; x++)
                {
                    //Response.Write("camera nya:" + Session["camera_vehicle_detection" + x].ToString());

                }
                // Opsional: Tampilkan pesan atau alihkan halaman
                //Response.Write("Data berhasil disimpan ke dalam session.");
            }
            else
            {
                // Menangani kasus ketika kontrol tidak ditemukan
                //Response.Write("SelectMultiple kontrol tidak ditemukan.");
            }
            //Response.Write(Session["SelectedValues"]);
            // Opsional: Tampilkan pesan atau alihkan halaman
            //Response.Write("Data berhasil disimpan ke dalam session.");
            string dari = TextBox1.Text;
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
                //Response.Write("masuk semua apa ");
                //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                dari = today;

            }
            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            Session["from_vehicle_detection"] = dari;
            Session["to_vehicle_detection"] = ke;
            if (Session["camera_vehicle_detection"].ToString() == "none")
            {
                Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                //Response.Redirect("report_vehicle_detection.aspx");
            }
            else
            {
                //Response.Redirect("report_vehicle_detection_pdf.aspx");
                // Mendapatkan markup HTML dari halaman ExportToPdf.aspx
                StringWriter sw = new StringWriter();
                Server.Execute("report_vehicle_detection_pdf.aspx", sw);
                string htmlContent = sw.ToString();

                // Membuat objek PdfWriter untuk menulis ke PDF
                string outputFilePath = Server.MapPath("~/Output.pdf");
                PdfWriter writer = new PdfWriter(outputFilePath);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, iText.Kernel.Geom.PageSize.A4.Rotate());

                // Menambahkan markup HTML ke PDF
                HtmlConverter.ConvertToPdf(htmlContent, pdf, new ConverterProperties());

                // Menutup dokumen PDF
                document.Close();

                // Mengarahkan pengguna untuk mengunduh file PDF yang dihasilkan
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=Report License Plate Recognition.pdf");
                Response.TransmitFile(outputFilePath);
                Response.End();
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            HtmlSelect selectMultiple = SelectMultiple;
            var i = 0;

            if (selectMultiple != null)
            {
                List<string> selectedValues = new List<string>();
                foreach (ListItem item in selectMultiple.Items)
                {
                    if (item.Selected) // Memeriksa apakah item dipilih
                    {
                        selectedValues.Add(item.Value); // Menambahkan nilai item yang dipilih ke list
                        Session["camera_vehicle_detection" + i] = item.Value;
                        i++;
                        if (i == 1 && item.Value == "all")
                        {
                            break;
                        }
                        else
                        {
                        }
                    }
                }
                Session["SelectedValues"] = selectedValues;
                Session["camera_vehicle_detection"] = i;
            }
            else
            {
            }
            string status = DropDownList2.SelectedValue;
            //Response.Write(status);
            string plate = TextBox3.Text;
            //Response.Write(plate);
            string dari = TextBox1.Text;
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                dari = today;

            }
            string ke = TextBox2.Text;
            Session["status_vehicle_detection"] = status;
            Session["plate_vehicle_detection"] = plate;
            Session["from_vehicle_detection"] = dari;
            Session["to_vehicle_detection"] = ke;
            Response.Redirect("report_vehicle_detection.aspx");
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
                string status_vehicle = DropDownList2.SelectedValue;
                string plate_vehicle = TextBox3.Text;
                //if (!string.IsNullOrEmpty(Session["status_vehicle_detection"].ToString()))
                //{
                //    string status = Session["status_vehicle_detection"].ToString();
                //}
                //else
                //{
                //    string status = "all";
                //}
                //if (!string.IsNullOrEmpty(Session["person"].ToString()))
                //{
                //    string status = Session["status_vehicle_detection"].ToString();
                //}
                //else
                //{
                //    string status = "all";
                //}

                //    DateTime fromDate;
                //DateTime toDate;
                int x = 0;

                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (camera == "none")
                    {

                        //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                    }
                    else
                    {
                        //Response.Write(fromDate);
                        //Response.Write("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id  WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) >="+from+" AND DATE(ce.occurred_at) <="+to+" AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC");
                        string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code,v.plate_number as plate, ce.plate_number FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) ";
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_vehicle_detection0"].ToString() == "all")
                            {
                                query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                            }
                            else
                            {
                                query += @"AND (";
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
                        if (plate_vehicle != "")
                        {
                            query += @" AND ce.plate_number like '%" + plate_vehicle + "%'";
                        }
                        if (status_vehicle != "all")
                        {
                            if (status_vehicle == "no")
                            {
                                //Response.Write("masuk"+status_vehicle);
                                query += @" AND ce.plate_number not in (SELECT plate_number from vehicles where id in (select vehicle_id from vehicle_history))";
                            }
                            else
                            {
                                query += @" AND vh.entry_code='" + status_vehicle + "'";
                            }
                        }
                        query += @" AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC";
                        //Response.Write(query);
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
                                        //int cek2 = event_type.IndexOf("MisMatch");
                                        //if (cek2 != -1)
                                        //{
                                        //    statusnya = "Unrecognized";
                                        //}
                                        //else
                                        //{
                                        //    statusnya = "Registered";
                                        //}
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
                                                    ImageUrl = "image_file/" + image_file,
                                                    AlternateText = "icon title"
                                                };
                                                img.Style.Add("width", "150px");
                                                imageCell.Controls.Add(img);
                                            }
                                            pers.Text = person;
                                        }
                                        imageCell.HorizontalAlign = HorizontalAlign.Center;
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
                                        //row.Cells.Add(no);
                                        row.Cells.Add(imageCell);
                                        row.Cells.Add(imageCell2);
                                        row.Cells.Add(plate);
                                        row.Cells.Add(occurred);
                                        row.Cells.Add(loc);
                                        row.Cells.Add(sites);
                                        //row.Cells.Add(pers);
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

        //            //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
        //        }
        //        else
        //        {
        //            if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
        //            {
        //                //Response.Write(fromDate);
        //                //Response.Write("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id  WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) >="+from+" AND DATE(ce.occurred_at) <="+to+" AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC");
        //                string per1 = from.ToString();
        //                string per2 = to.ToString();
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
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + plate_number_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img2.Style.Add("width", "150px");
        //                                        imageCell2.Controls.Add(img2);
        //                                        row.Cells.Add(imageCell);
        //                                        row.Cells.Add(imageCell2);
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
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
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
        //                                //TableCell status = new TableCell();
        //                                //status.Text = statusnya;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = statusnya };
        //                                //row.Cells.Add(no);
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
        //                                //Console.Write(occurred_at);
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
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + plate_number_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img2.Style.Add("width", "150px");
        //                                        imageCell2.Controls.Add(img2);
        //                                        row.Cells.Add(imageCell);
        //                                        row.Cells.Add(imageCell2);
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
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
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
        //                                //TableCell status = new TableCell();
        //                                //status.Text = statusnya;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = statusnya };
        //                                //row.Cells.Add(no);
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
        //            else if (DateTime.TryParse(to, out toDate))
        //            {
        //                string per2 = to.ToString();
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
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
        //                                        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + plate_number_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img2.Style.Add("width", "150px");
        //                                        imageCell2.Controls.Add(img2);
        //                                        row.Cells.Add(imageCell);
        //                                        row.Cells.Add(imageCell2);
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
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
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
        //                                //TableCell status = new TableCell();
        //                                //status.Text = statusnya;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = statusnya };
        //                                //row.Cells.Add(no);
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
        //        }

        //    }
        //}

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            HtmlSelect selectMultiple = SelectMultiple;
            var i = 0;

            // Pastikan kontrol tidak null sebelum mengakses properti
            if (selectMultiple != null)
            {
                List<string> selectedValues = new List<string>();

                // Loop melalui semua item dalam selectMultiple
                foreach (ListItem item in selectMultiple.Items)
                {
                    if (item.Selected) // Memeriksa apakah item dipilih
                    {
                        selectedValues.Add(item.Value); // Menambahkan nilai item yang dipilih ke list
                        Session["camera_vehicle_detection" + i] = item.Value;
                        i++;
                        if (i == 1 && item.Value == "all")
                        {
                            break;
                        }
                        else
                        {
                        }
                    }
                }

                // Simpan data dalam session
                Session["SelectedValues"] = selectedValues;
                Session["camera_vehicle_detection"] = i;
                //Response.Write("jumlah i:" + Session["camera_vehicle_detection"]);
                for (int x = 0; x < i; x++)
                {
                    //Response.Write("camera nya:" + Session["camera_vehicle_detection" + x].ToString());

                }
                // Opsional: Tampilkan pesan atau alihkan halaman
                //Response.Write("Data berhasil disimpan ke dalam session.");
            }
            else
            {
                // Menangani kasus ketika kontrol tidak ditemukan
                //Response.Write("SelectMultiple kontrol tidak ditemukan.");
            }
            //Response.Write(Session["SelectedValues"]);
            // Opsional: Tampilkan pesan atau alihkan halaman
            //Response.Write("Data berhasil disimpan ke dalam session.");
            string dari = TextBox1.Text;
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
                //Response.Write("masuk semua apa ");
                //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                dari = today;

            }
            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            Session["from_vehicle_detection"] = dari;
            Session["to_vehicle_detection"] = ke;
            if (Session["camera_vehicle_detection"].ToString() == "none")
            {
                Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                //Response.Redirect("report_vehicle_detection.aspx");
            }
            else
            {
                //ExportToExcelWithImages();
                ExportCSV();
            }
        }

        protected void btnExportExcel2_Click(object sender, EventArgs e)
        {
            HtmlSelect selectMultiple = SelectMultiple;
            var i = 0;

            // Pastikan kontrol tidak null sebelum mengakses properti
            if (selectMultiple != null)
            {
                List<string> selectedValues = new List<string>();

                // Loop melalui semua item dalam selectMultiple
                foreach (ListItem item in selectMultiple.Items)
                {
                    if (item.Selected) // Memeriksa apakah item dipilih
                    {
                        selectedValues.Add(item.Value); // Menambahkan nilai item yang dipilih ke list
                        Session["camera_vehicle_detection" + i] = item.Value;
                        i++;
                        if (i == 1 && item.Value == "all")
                        {
                            break;
                        }
                        else
                        {
                        }
                    }
                }

                // Simpan data dalam session
                Session["SelectedValues"] = selectedValues;
                Session["camera_vehicle_detection"] = i;
                //Response.Write("jumlah i:" + Session["camera_vehicle_detection"]);
                for (int x = 0; x < i; x++)
                {
                    //Response.Write("camera nya:" + Session["camera_vehicle_detection" + x].ToString());

                }
                // Opsional: Tampilkan pesan atau alihkan halaman
                //Response.Write("Data berhasil disimpan ke dalam session.");
            }
            else
            {
                // Menangani kasus ketika kontrol tidak ditemukan
                //Response.Write("SelectMultiple kontrol tidak ditemukan.");
            }
            //Response.Write(Session["SelectedValues"]);
            // Opsional: Tampilkan pesan atau alihkan halaman
            //Response.Write("Data berhasil disimpan ke dalam session.");
            string dari = TextBox1.Text;
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
                //Response.Write("masuk semua apa ");
                //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                dari = today;

            }
            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            Session["from_vehicle_detection"] = dari;
            Session["to_vehicle_detection"] = ke;
            //if (Session["camera_vehicle_detection"] == "none")
            //{
            //    Response.Write("<script>alert('Camera Site cannot be empty')</script>");
            //    //Response.Redirect("report_vehicle_detection.aspx");
            //}
            //else
            //{

            //ExportToExcelWithImages2();
            ExportCSV2();
            //Response.Write(hari_ini);
            //}
        }

        protected void ExportCSV()
        {
            string fileName = "Report_License_Plate_Recognition.csv";

            string camera = Session["camera_vehicle_detection"].ToString();
            if (camera != "0")
            {
                int jumlahCamera = Convert.ToInt32(camera);
                string from = Session["from_vehicle_detection"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
                string per2 = "";
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_vehicle_detection"] +"')</script>");
                if (Session["to_vehicle_detection"].ToString() != "")
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_vehicle_detection"].ToString();
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
                        string query = @"SELECT MAX(ce.occurred_at) as to_date, ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code,v.plate_number FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) ";
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_vehicle_detection0"].ToString() == "all")
                            {
                                query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                            }
                            else
                            {
                                query += @"AND (";
                                for (int y = 0; y < jumlahCamera; y++)
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@camera{y}";
                                    }
                                    else
                                    {
                                        query += " OR ";
                                        query += $"c.id=@camera{y}";

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
                        query += @" AND ce.type like '%Plate%' ORDER BY ce.occurred_at DESC LIMIT 1";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_vehicle_detection0"].ToString() == "all")
                                {

                                }
                                else
                                {
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_vehicle_detection" + y].ToString());
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
                                        DateTime occurredAt = (DateTime)dr["to_date"];
                                        string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                        per2 += formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                    }
                                }
                            }
                        }
                    }
                }

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Charset = "";
                Response.ContentType = "text/csv";
                string fullUrl = Request.Url.AbsoluteUri;
                string absolutePath = Request.Url.AbsolutePath;
                string baseUrl = fullUrl.Replace(absolutePath, "");
                // Siapkan header untuk CSV
                string csvContent = $"\"Report License Plate Recognition\"\n";
                csvContent += $"\"Periode {per1} to {per2}\"\n";
                csvContent += "\"Image\",\"License Plate Number\",\"Occurred At\",\"Location\",\"Site\",\"Status\"\n";

                int x = 0;
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (camera == "none")
                    {

                        //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                    }
                    else
                    {
                        //Response.Write(fromDate);
                        //Response.Write("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id  WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) >="+from+" AND DATE(ce.occurred_at) <="+to+" AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC");
                        string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code,v.plate_number as plate, ce.plate_number FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) ";
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_vehicle_detection0"].ToString() == "all")
                            {
                                query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                            }
                            else
                            {
                                query += @"AND (";
                                for (int y = 0; y < jumlahCamera; y++)
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@camera{y}";
                                    }
                                    else
                                    {
                                        query += " OR ";
                                        query += $"c.id=@camera{y}";

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
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_vehicle_detection" + y].ToString());
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
                                        string link1 = baseUrl + "/image_file/" + image_file;

                                        csvContent += $"\"{link1}\",\"{plate_number}\",\"{occurred_at}\",\"{location}\",\"{site}\",\"{statusnya}\"\n";
                                    }
                                    dr.Close();
                                }
                                else
                                {
                                }
                            }

                        }

                        Response.Output.Write(csvContent);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
        protected void ExportToExcelWithImages()
        {
            string fileName = "Report License Plate Recognition.xls";

            string camera = Session["camera_vehicle_detection"].ToString();
            if (camera != "0")
            {
                int jumlahCamera = Convert.ToInt32(camera);
                string from = Session["from_vehicle_detection"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
                string per2 = "";
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_vehicle_detection"] +"')</script>");
                if (Session["to_vehicle_detection"].ToString() != "")
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_vehicle_detection"].ToString();
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
                        string query = @"SELECT MAX(ce.occurred_at) as to_date, ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code,v.plate_number FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) ";
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_vehicle_detection0"].ToString() == "all")
                            {
                                query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                            }
                            else
                            {
                                query += @"AND (";
                                for (int y = 0; y < jumlahCamera; y++)
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@camera{y}";
                                    }
                                    else
                                    {
                                        query += " OR ";
                                        query += $"c.id=@camera{y}";

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
                        query += @" AND ce.type like '%Plate%' ORDER BY ce.occurred_at DESC LIMIT 1";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_vehicle_detection0"].ToString() == "all")
                                {

                                }
                                else
                                {
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_vehicle_detection" + y].ToString());
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
                                        DateTime occurredAt = (DateTime)dr["to_date"];
                                        string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                        per2 += formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                    }
                                }
                            }
                        }
                    }
                }

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                // Siapkan HTML Table yang valid
                string tableHtml = $@"<table border='1'>
                <tr>
                    <td colspan='7' align='center'><b>Report License Plate Recognition</b></td>
                </tr>
                <tr>
                    <td colspan='7' align='center'><b>Periode {per1} to {per2}</b></td>
                </tr>
                <tr>
                    <td><b>No</b></td>
                    <td><b>Image</b></td>
                    <td><b>License Plate Number</b></td>
                    <td><b>Occurred At</b></td>
                    <td><b>Location</b></td>
                    <td><b>Site</b></td>
                    <td><b>Status</b></td>
                </tr>";
                int x = 0;
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (camera == "none")
                    {

                        //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                    }
                    else
                    {
                        //Response.Write(fromDate);
                        //Response.Write("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id  WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) >="+from+" AND DATE(ce.occurred_at) <="+to+" AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC");
                        string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code,v.plate_number as plate, ce.plate_number FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) ";
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_vehicle_detection0"].ToString() == "all")
                            {
                                query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                            }
                            else
                            {
                                query += @"AND (";
                                for (int y = 0; y < jumlahCamera; y++)
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@camera{y}";
                                    }
                                    else
                                    {
                                        query += " OR ";
                                        query += $"c.id=@camera{y}";

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
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_vehicle_detection" + y].ToString());
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
                                        tableHtml += $@"<tr>
                                                <td>{x}</td>";
                                        string path = UrlImage + image_file;
                                        tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path}' alt='Foto' width='150' height='150'/></center></td>";

                                        tableHtml += $@"<td>{plate_number}</td>
                                                <td>{occurred_at}</td>
                                                <td>{location}</td>
                                                <td>{site}</td>
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

                        tableHtml += @"</table>";

                        // Menuliskan HTML ke response
                        Response.Output.Write(tableHtml);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }

        protected void ExportCSV2()
        {
            string fileName = "Report_License_Plate_Unrecognized_In_A_Week.csv";

            string camera = Session["camera_vehicle_detection"].ToString();
            //if (camera != "0")
            //{
            int jumlahCamera = Convert.ToInt32(camera);
            //DateTime today = DateTime.Today;
            //DateTime oneWeekAgo = today.AddDays(-7);
            //DateTime today = DateTime.Today;
            DateTime today;
            int daysToSunday;
            DateTime lastSunday;
            // Cek apakah input valid
            //if (DateTime.TryParse(TextBox1.Text, out today))
            //{
            //    // Hitung hari Minggu sebelumnya
            //     daysToSunday= (int)today.DayOfWeek; // 0 = Minggu
            //     lastSunday= today.AddDays(-daysToSunday);

            //}
            //else
            //{
            today = DateTime.Today;
            daysToSunday = (int)today.DayOfWeek; // 0 = Minggu
            lastSunday = today.AddDays(-daysToSunday);
            //}
            //int daysToSunday = (int)today.DayOfWeek; // Hari Minggu adalah 0, Senin adalah 1, dst.
            //DateTime lastSunday = today.AddDays(-daysToSunday);
            //Response.Write(lastSunday);
            //string from_date = oneWeekAgo.ToString("yyyy-MM-dd 00:00:00");
            string from_date = lastSunday.ToString("yyyy-MM-dd 00:00:00");
            string to_date = today.ToString("yyyy-MM-dd 23:59:59");
            string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
            string per2 = to_date.Substring(8, 2) + "-" + to_date.Substring(5, 2) + "-" + to_date.Substring(0, 4);
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "text/csv";
            string fullUrl = Request.Url.AbsoluteUri;
            string absolutePath = Request.Url.AbsolutePath;
            string baseUrl = fullUrl.Replace(absolutePath, "");
            // Siapkan header untuk CSV
            string csvContent = $"\"Report License Plate Unrecognized In A Week\"\n";
            csvContent += $"\"Periode {per1} to {per2}\"\n";
            csvContent += "\"Image\",\"License Plate Number\",\"Entry Times (Days)\"\n";
            int x = 0;
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                if (camera == "none")
                {

                    //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                }
                else
                {
                    string query = @"SELECT count(DISTINCT DATE(ce.occurred_at)) as jumlah, ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code,v.plate_number as plate, ce.plate_number FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) AND ce.plate_number not in (SELECT plate_number from vehicles) ";
                    query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                    //if (jumlahCamera > 0)
                    //{
                    //    if (Session["camera_vehicle_detection0"].ToString() == "all")
                    //    {
                    //        query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                    //    }
                    //    else
                    //    {
                    //        query += @"AND (";
                    //        for (int y = 0; y < jumlahCamera; y++)
                    //        {
                    //            if (y == 0)
                    //            {
                    //                query += $"c.id=@camera{y}";
                    //            }
                    //            else
                    //            {
                    //                query += " OR ";
                    //                query += $"c.id=@camera{y}";

                    //            }
                    //        }
                    //        query += @")";
                    //    }
                    //}
                    if (from_date != null)
                    {
                    }
                    if (to_date != null)
                    {
                    }
                    query += @" AND ce.occurred_at >= @from ";
                    query += @" AND ce.occurred_at <= @to ";
                    query += @" AND ce.type like '%Plate%' GROUP BY ce.plate_number HAVING COUNT(DISTINCT DATE(ce.occurred_at)) >= @batas ORDER BY ce.occurred_at ASC";
                    //Response.Write(query);
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        //if (jumlahCamera > 0)
                        //{
                        //    if (Session["camera_vehicle_detection0"].ToString() == "all")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        for (int y = 0; y < jumlahCamera; y++)
                        //        {
                        //            cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_vehicle_detection" + y].ToString());
                        //        }
                        //    }
                        //}
                        if (from_date != null)
                        {
                        }
                        if (to_date != null)
                        {
                        }
                        cmd.Parameters.AddWithValue("@from", from_date);
                        cmd.Parameters.AddWithValue("@to", to_date);
                        cmd.Parameters.AddWithValue("@batas", batas);
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
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
                                    string code = dr["entry_code"].ToString();
                                    string plate_number = dr["plate_number"].ToString();
                                    string summary = dr["jumlah"].ToString();
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
                                    string link1 = baseUrl + "/image_file/" + image_file;

                                    csvContent += $"\"{link1}\",\"{plate_number}\",\"{summary}\"\n";
                                }
                                //<td>{location}</td>
                                //<td>{site}</td>
                                //<td>{statusnya}</td>
                                dr.Close();
                            }
                            else
                            {
                            }
                        }

                    }

                    Response.Output.Write(csvContent);
                    Response.Flush();
                    Response.End();
                }
            }
            //}
        }
        protected void ExportToExcelWithImages2()
        {
            string fileName = "Report License Plate Unrecognized In A Week.xls";

            string camera = Session["camera_vehicle_detection"].ToString();
            //if (camera != "0")
            //{
            int jumlahCamera = Convert.ToInt32(camera);
            //DateTime today = DateTime.Today;
            //DateTime oneWeekAgo = today.AddDays(-7);
            //DateTime today = DateTime.Today;
            DateTime today;
            int daysToSunday;
            DateTime lastSunday;
            // Cek apakah input valid
            //if (DateTime.TryParse(TextBox1.Text, out today))
            //{
            //    // Hitung hari Minggu sebelumnya
            //     daysToSunday= (int)today.DayOfWeek; // 0 = Minggu
            //     lastSunday= today.AddDays(-daysToSunday);

            //}
            //else
            //{
            today = DateTime.Today;
            daysToSunday = (int)today.DayOfWeek; // 0 = Minggu
            lastSunday = today.AddDays(-daysToSunday);
            //}
            //int daysToSunday = (int)today.DayOfWeek; // Hari Minggu adalah 0, Senin adalah 1, dst.
            //DateTime lastSunday = today.AddDays(-daysToSunday);
            //Response.Write(lastSunday);
            //string from_date = oneWeekAgo.ToString("yyyy-MM-dd 00:00:00");
            string from_date = lastSunday.ToString("yyyy-MM-dd 00:00:00");
            string to_date = today.ToString("yyyy-MM-dd 23:59:59");
            string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
            string per2 = to_date.Substring(8, 2) + "-" + to_date.Substring(5, 2) + "-" + to_date.Substring(0, 4);
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            // Siapkan HTML Table yang valid
            string tableHtml = $@"<table border='1'>
                <tr>
                    <td colspan='4' align='center'><b>Report License Plate Unrecognized In A Week</b></td>
                </tr>
                <tr>
                    <td colspan='4' align='center'><b>Periode {per1} to {per2}</b></td>
                </tr>
                <tr>
                    <td><b>No</b></td>
                    <td><b>Image</b></td>
                    <td><b>License Plate Number</b></td>
                    <td><b>Entry Times (Days)</b></td>
                </tr>";
            int x = 0;
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                if (camera == "none")
                {

                    //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                }
                else
                {
                    string query = @"SELECT count(DISTINCT DATE(ce.occurred_at)) as jumlah, ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code,v.plate_number as plate, ce.plate_number FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) AND ce.plate_number not in (SELECT plate_number from vehicles) ";
                    query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                    //if (jumlahCamera > 0)
                    //{
                    //    if (Session["camera_vehicle_detection0"].ToString() == "all")
                    //    {
                    //        query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                    //    }
                    //    else
                    //    {
                    //        query += @"AND (";
                    //        for (int y = 0; y < jumlahCamera; y++)
                    //        {
                    //            if (y == 0)
                    //            {
                    //                query += $"c.id=@camera{y}";
                    //            }
                    //            else
                    //            {
                    //                query += " OR ";
                    //                query += $"c.id=@camera{y}";

                    //            }
                    //        }
                    //        query += @")";
                    //    }
                    //}
                    if (from_date != null)
                    {
                    }
                    if (to_date != null)
                    {
                    }
                    query += @" AND ce.occurred_at >= @from ";
                    query += @" AND ce.occurred_at <= @to ";
                    query += @" AND ce.type like '%Plate%' GROUP BY ce.plate_number HAVING COUNT(DISTINCT DATE(ce.occurred_at)) >= @batas ORDER BY ce.occurred_at ASC";
                    //Response.Write(query);
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        //if (jumlahCamera > 0)
                        //{
                        //    if (Session["camera_vehicle_detection0"].ToString() == "all")
                        //    {

                        //    }
                        //    else
                        //    {
                        //        for (int y = 0; y < jumlahCamera; y++)
                        //        {
                        //            cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_vehicle_detection" + y].ToString());
                        //        }
                        //    }
                        //}
                        if (from_date != null)
                        {
                        }
                        if (to_date != null)
                        {
                        }
                        cmd.Parameters.AddWithValue("@from", from_date);
                        cmd.Parameters.AddWithValue("@to", to_date);
                        cmd.Parameters.AddWithValue("@batas", batas);
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
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
                                    string code = dr["entry_code"].ToString();
                                    string plate_number = dr["plate_number"].ToString();
                                    string summary = dr["jumlah"].ToString();
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
                                    tableHtml += $@"<tr>
                                                <td>{x}</td>";
                                    string path = UrlImage + image_file;
                                    tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path}' alt='Foto' width='150' height='150'/></center></td>";

                                    tableHtml += $@"<td>{plate_number}</td>
                                                <td>{summary}</td>
                                            </tr>";
                                }
                                //<td>{location}</td>
                                //<td>{site}</td>
                                //<td>{statusnya}</td>
                                dr.Close();
                            }
                            else
                            {
                            }
                        }

                    }

                    tableHtml += @"</table>";

                    // Menuliskan HTML ke response
                    Response.Output.Write(tableHtml);
                    Response.Flush();
                    Response.End();
                }
            }
            //}
        }
        protected void ExportToExcellWithImages()
        {
            // Buat workbook baru
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Report LPR");

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
            ICell cellImageHeader2 = rowHeader.CreateCell(2);
            cellImageHeader2.SetCellValue("License Plate Number");
            cellImageHeader2.CellStyle = headerStyle;
            rowHeader.CreateCell(3).SetCellValue("Occurred At");
            rowHeader.GetCell(3).CellStyle = headerStyle;
            rowHeader.CreateCell(4).SetCellValue("Location");
            rowHeader.GetCell(4).CellStyle = headerStyle;
            rowHeader.CreateCell(5).SetCellValue("Site");
            rowHeader.GetCell(5).CellStyle = headerStyle;
            rowHeader.CreateCell(6).SetCellValue("Status");
            rowHeader.GetCell(6).CellStyle = headerStyle;

            // Atur lebar kolom agar sesuai dengan ukuran gambar (150px)
            sheet.SetColumnWidth(1, 150 * 256 / 8); // Kolom Image 1
            sheet.SetColumnWidth(2, 150 * 256 / 8); // Kolom Image 2

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

                //DateTime fromDate;
                //DateTime toDate;
                int x = 0;
                int i = 3;
                string per1 = "";
                string per2 = "";
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (camera == "none")
                    {

                        //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                    }
                    else
                    {
                        //Response.Write(fromDate);
                        //Response.Write("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id  WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) >="+from+" AND DATE(ce.occurred_at) <="+to+" AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC");
                        string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code,v.plate_number FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) ";
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_vehicle_detection0"].ToString() == "all")
                            {
                                query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                            }
                            else
                            {
                                query += @"AND (";
                                for (int y = 0; y < jumlahCamera; y++)
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@camera{y}";
                                    }
                                    else
                                    {
                                        query += " OR ";
                                        query += $"c.id=@camera{y}";

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
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_vehicle_detection" + y].ToString());
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
                                        string statusnya = dr["entry_code"].ToString();
                                        string plate_number = dr["plate_number"].ToString();
                                        if (statusnya == "" || statusnya == null)
                                        {
                                            statusnya = "Unrecognized";
                                        }
                                        IRow row = sheet.CreateRow(i);
                                        row.CreateCell(0).SetCellValue(x);
                                        row.CreateCell(2).SetCellValue(plate_number);
                                        row.CreateCell(3).SetCellValue(occurred_at);
                                        row.CreateCell(4).SetCellValue(location);
                                        row.CreateCell(5).SetCellValue(site);
                                        row.CreateCell(6).SetCellValue(statusnya);
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
                                        }
                                        else
                                        {

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
                                        //string imagePath4 = Server.MapPath(path2); // Path gambar kedua
                                        //if (File.Exists(imagePath4))
                                        //{
                                        //    byte[] imageData4 = File.ReadAllBytes(imagePath4);
                                        //    int pictureIndex4 = workbook.AddPicture(imageData4, PictureType.PNG);
                                        //    IDrawing drawing4 = sheet.CreateDrawingPatriarch();
                                        //    IClientAnchor anchor4 = drawing4.CreateAnchor(0, 0, 0, 0, 2, i, 3, i + 1);
                                        //    IPicture picture4 = drawing4.CreatePicture(anchor4, pictureIndex4);
                                        //    picture4.Resize(1.0, 1.0); // Fit gambar sesuai ukuran cell tanpa resize lebih lanjut
                                        //}
                                        //else
                                        //{
                                        //    // Jika gambar kedua tidak ada, biarkan cell kosong
                                        //    row.CreateCell(2).SetCellValue(""); // Mengisi cell dengan kosong
                                        //}
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

                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6)); // Merge baris 1 kolom 0-7
                        IRow headerRow1 = sheet.CreateRow(0);
                        ICell headerCell1 = headerRow1.CreateCell(0);
                        headerCell1.SetCellValue("License Plate Recognition");
                        headerCell1.CellStyle = headerStyle;

                        // Merge cells untuk periode
                        sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 6)); // Merge baris 2 kolom 0-7
                        IRow headerRow2 = sheet.CreateRow(1);
                        ICell headerCell2 = headerRow2.CreateCell(0);
                        headerCell2.SetCellValue($"Periode {per1} to {per2}");
                        headerCell2.CellStyle = headerStyle;

                        using (MemoryStream stream = new MemoryStream())
                        {
                            workbook.Write(stream);
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment;filename=ReportLicensePlateRecognition.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.BinaryWrite(stream.ToArray());
                            Response.End();
                        }
                    }
                }
            }
        }

        protected void ExportToExcellWithImages2()
        {
            // Buat workbook baru
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Report LPR");

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
            ICell cellImageHeader2 = rowHeader.CreateCell(2);
            cellImageHeader2.SetCellValue("License Plate Number");
            cellImageHeader2.CellStyle = headerStyle;
            rowHeader.CreateCell(3).SetCellValue("Occurred At");
            rowHeader.GetCell(3).CellStyle = headerStyle;
            rowHeader.CreateCell(4).SetCellValue("Location");
            rowHeader.GetCell(4).CellStyle = headerStyle;
            rowHeader.CreateCell(5).SetCellValue("Site");
            rowHeader.GetCell(5).CellStyle = headerStyle;
            rowHeader.CreateCell(6).SetCellValue("Status");
            rowHeader.GetCell(6).CellStyle = headerStyle;

            // Atur lebar kolom agar sesuai dengan ukuran gambar (150px)
            sheet.SetColumnWidth(1, 150 * 256 / 8); // Kolom Image 1
            sheet.SetColumnWidth(2, 150 * 256 / 8); // Kolom Image 2

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

                //DateTime fromDate;
                //DateTime toDate;
                int x = 0;
                int i = 3;
                string per1 = "";
                string per2 = "";
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (camera == "none")
                    {

                        //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                    }
                    else
                    {
                        //Response.Write(fromDate);
                        //Response.Write("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id  WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND DATE(ce.occurred_at) >="+from+" AND DATE(ce.occurred_at) <="+to+" AND ce.type like '%Plate%' ORDER BY ce.occurred_at ASC");

                        DateTime today = DateTime.Today;
                        DateTime oneWeekAgo = today.AddDays(-7);
                        string seminggu = oneWeekAgo.ToString("yyyy-MM-dd 00:00:00");
                        string hari_ini = today.ToString("yyyy-MM-dd 23:59:59");
                        string query = @"SELECT count(*) as jumlah, ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file, vh.entry_code,v.plate_number as plate, ce.plate_number FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN vehicles v on ce.plate_number=v.plate_number LEFT JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) WHERE (ce.image_file != '' OR ce.image_file != NULL) AND ce.plate_number not in (SELECT plate_number from vehicles) ";
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_vehicle_detection0"].ToString() == "all")
                            {
                                query += @" AND c.id in (SELECT id from cameras where name like 'LPR%')";
                            }
                            else
                            {
                                query += @"AND (";
                                for (int y = 0; y < jumlahCamera; y++)
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@camera{y}";
                                    }
                                    else
                                    {
                                        query += " OR ";
                                        query += $"c.id=@camera{y}";

                                    }
                                }
                                query += @")";
                            }
                        }
                        if (from_date != null)
                        {
                        }
                        if (to_date != null)
                        {
                        }
                        query += @" AND ce.occurred_at >= @from ";
                        query += @" AND ce.occurred_at <= @to ";
                        query += @" AND ce.type like '%Plate%' GROUP BY ce.plate_number HAVING count(*) > @batas ORDER BY ce.occurred_at ASC";
                        //Response.Write(query);
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_vehicle_detection0"].ToString() == "all")
                                {

                                }
                                else
                                {
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_vehicle_detection" + y].ToString());
                                    }
                                }
                            }
                            if (from_date != null)
                            {
                            }
                            if (to_date != null)
                            {
                            }
                            cmd.Parameters.AddWithValue("@from", seminggu);
                            cmd.Parameters.AddWithValue("@to", hari_ini);
                            cmd.Parameters.AddWithValue("@batas", batas);
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
                                        string statusnya = dr["entry_code"].ToString();
                                        string plate_number = dr["plate_number"].ToString();
                                        if (statusnya == "" || statusnya == null)
                                        {
                                            statusnya = "Unrecognized";
                                        }
                                        IRow row = sheet.CreateRow(i);
                                        row.CreateCell(0).SetCellValue(x);
                                        row.CreateCell(2).SetCellValue(plate_number);
                                        row.CreateCell(3).SetCellValue(occurred_at);
                                        row.CreateCell(4).SetCellValue(location);
                                        row.CreateCell(5).SetCellValue(site);
                                        row.CreateCell(6).SetCellValue(statusnya);
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
                                        }
                                        else
                                        {

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
                                        //string imagePath4 = Server.MapPath(path2); // Path gambar kedua
                                        //if (File.Exists(imagePath4))
                                        //{
                                        //    byte[] imageData4 = File.ReadAllBytes(imagePath4);
                                        //    int pictureIndex4 = workbook.AddPicture(imageData4, PictureType.PNG);
                                        //    IDrawing drawing4 = sheet.CreateDrawingPatriarch();
                                        //    IClientAnchor anchor4 = drawing4.CreateAnchor(0, 0, 0, 0, 2, i, 3, i + 1);
                                        //    IPicture picture4 = drawing4.CreatePicture(anchor4, pictureIndex4);
                                        //    picture4.Resize(1.0, 1.0); // Fit gambar sesuai ukuran cell tanpa resize lebih lanjut
                                        //}
                                        //else
                                        //{
                                        //    // Jika gambar kedua tidak ada, biarkan cell kosong
                                        //    row.CreateCell(2).SetCellValue(""); // Mengisi cell dengan kosong
                                        //}

                                    }
                                    dr.Close();
                                }
                                else
                                {
                                }
                            }
                            per1 = seminggu.Substring(8, 2) + "-" + seminggu.Substring(5, 2) + "-" + seminggu.Substring(0, 4);
                            per2 = hari_ini.Substring(8, 2) + "-" + hari_ini.Substring(5, 2) + "-" + hari_ini.Substring(0, 4);
                        }

                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6)); // Merge baris 1 kolom 0-7
                        IRow headerRow1 = sheet.CreateRow(0);
                        ICell headerCell1 = headerRow1.CreateCell(0);
                        headerCell1.SetCellValue("License Plate Unrecognized");
                        headerCell1.CellStyle = headerStyle;

                        // Merge cells untuk periode
                        sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 6)); // Merge baris 2 kolom 0-7
                        IRow headerRow2 = sheet.CreateRow(1);
                        ICell headerCell2 = headerRow2.CreateCell(0);
                        headerCell2.SetCellValue($"Periode {per1} to {per2}");
                        headerCell2.CellStyle = headerStyle;

                        using (MemoryStream stream = new MemoryStream())
                        {
                            workbook.Write(stream);
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment;filename=ReportLicensePlateUnrecognized.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.BinaryWrite(stream.ToArray());
                            Response.End();
                        }
                    }
                }
            }
        }

    }
}