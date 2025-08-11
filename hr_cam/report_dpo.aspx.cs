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
    public partial class Report_dpo : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string UrlImage = ConfigurationManager.AppSettings["urlImage"];
        string UrlImagePerson = ConfigurationManager.AppSettings["urlImagePerson"];
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    //Response.Write("<script>alert('Coba masuk')</script>");
            //        DropDownList2.Items.Add(new ListItem("", "none"));
            //    if (Session["site_dpo"] == null)
            //    {
            //        //Response.Write("<script>alert('" + Session["site_dpo"].ToString() + "')</script>");
            //        string camera_site = "none";
            //        Session["site_dpo"] = camera_site;
            //        //Response.Write("<script>alert('" + Session["from"].ToString() + "')</script>");

            //    }
            //    else
            //    {
            //        //Response.Write("<script>alert('" + Session["site_dpo"].ToString() + "')</script>");

            //        using (MySqlConnection con = new MySqlConnection(strcon))
            //        {
            //            if (con.State == ConnectionState.Closed)
            //            {
            //                con.Open();
            //            }

            //            using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where camera_site_id=@camera_site", con))
            //            {
            //                cmd.Parameters.AddWithValue("@camera_site", Session["site_dpo"].ToString());
            //                using (MySqlDataReader dr = cmd.ExecuteReader())
            //                {
            //                    if (dr.HasRows)
            //                    {
            //                        while (dr.Read())
            //                        {
            //                            DropDownList2.Items.Add(new ListItem(dr["name"].ToString() + " - "+ dr["location"].ToString(), dr["id"].ToString()));
            //                        }
            //                    }
            //                    else
            //                    {
            //                    }
            //                }
            //            }

            //        }
            //    }
            //    if (Session["camera_dpo"] == null)
            //    {
            //        string camera = "none";
            //        Session["camera_dpo"] = camera;
            //        //Response.Write("<script>alert('" + Session["from"].ToString() + "')</script>");

            //    }
            //    DropDownList1.Items.Add(new ListItem("", "none"));
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
            //    if (Session["from_dpo"] != null)
            //    {
            //    }
            //    else
            //    {
            //        string today = DateTime.Now.ToString("yyyy-MM-dd");
            //        Session["from_dpo"] = today;

            //    }
            //    if (Session["to_dpo"] != null)
            //    {
            //        TextBox2.Text = Session["to_dpo"].ToString();
            //    }
            //    TextBox1.Text = Session["from_dpo"].ToString();
            //    DropDownList1.SelectedValue = Session["site_dpo"].ToString();
            //    DropDownList2.SelectedValue = Session["camera_dpo"].ToString();
            //    FillEventHistory();
            //}

            if (!IsPostBack)
            {
                //Response.Write("<script>alert('Coba masuk')</script>");
                SelectMultiple.Items.Add(new ListItem("", "none"));
                //SelectMultiple.Items.Add(new ListItem("All Camera", "all"));




                //Response.Write("<script>alert('" + Session["site_dpo"].ToString() + "')</script>");
                if (Session["camera_dpo"] == null)
                {
                    string camera = "0";
                    Session["camera_dpo"] = camera;
                    //Response.Write("<script>alert('" + Session["from"].ToString() + "')</script>");

                }
                int jumlahCamera = Convert.ToInt32(Session["camera_dpo"].ToString());
                if (jumlahCamera > 0)
                {
                    if (Session["camera_dpo0"].ToString() == "all")
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

                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.id, c.name, c.location, cs.name as site from cameras c join camera_sites cs on c.camera_site_id=cs.id where c.name not like 'LPR%'", con))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                var selectedIds = new List<string>();
                                for (int i = 0; i < jumlahCamera; i++)
                                {
                                    var sessionValue = Session[$"camera_dpo{i}"];
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
                //DropDownList1.Items.Add(new ListItem("", "none"));
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
                if (Session["from_dpo"] != null)
                {
                }
                else
                {
                    string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                    Session["from_dpo"] = today;

                }
                if (Session["to_dpo"] != null)
                {
                    TextBox2.Text = Session["to_dpo"].ToString();
                }
                TextBox1.Text = Session["from_dpo"].ToString();
                //DropDownList1.SelectedValue = Session["site_dpo"].ToString();
                FillEventHistory();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string camera_site = DropDownList1.SelectedValue;
            //string camera = DropDownList2.SelectedValue;
            //Response.Write("<script>alert('camera " + camera + "')</script>");
            //Response.Write("<script>alert('site " + camera_site + "')</script>");
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
                        //Response.Write("<script>alert('" + item.Value + "')</script>");
                        Session["camera_dpo" + i] = item.Value;
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
                Session["camera_dpo"] = i;
                //Response.Write("jumlah i:" + Session["camera_dpo"]);
                for (int x = 0; x < i; x++)
                {
                    //Response.Write("camera nya:" + Session["camera_dpo" + x].ToString());

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
            //Session["site_dpo"] = camera_site;
            //Session["camera_dpo"] = camera;
            Session["from_dpo"] = dari;
            Session["to_dpo"] = ke;
            Response.Redirect("report_dpo.aspx");
        }

        //protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // Bersihkan item sebelumnya di DropDownList2
        //    DropDownList2.Items.Clear();
        //    string camera_site = DropDownList1.SelectedValue;
        //    if (camera_site == "none") { 
        //        DropDownList2.Items.Add(new ListItem("", "none"));
        //    } else {
        //        DropDownList2.Items.Add(new ListItem("", "none"));
        //        using (MySqlConnection con = new MySqlConnection(strcon))
        //        {
        //            if (con.State == ConnectionState.Closed)
        //            {
        //                con.Open();
        //            }

        //            using (MySqlCommand cmd = new MySqlCommand("SELECT * from cameras where camera_site_id=@camera_site", con))
        //            {
        //                cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                using (MySqlDataReader dr = cmd.ExecuteReader())
        //                {
        //                    if (dr.HasRows)
        //                    {
        //                        while (dr.Read())
        //                        {
        //                            DropDownList2.Items.Add(new ListItem(dr["name"].ToString()+" - " + dr["location"].ToString(), dr["id"].ToString()));
        //                        }
        //                    }
        //                    else
        //                    {
        //                    }
        //                }
        //            }

        //        }
        //    }
        //}

        protected void Button2_Click(object sender, EventArgs e)
        {
            //string camera_site = DropDownList1.SelectedValue;
            //string camera= DropDownList2.SelectedValue;
            //Response.Write("<script>alert('site " + camera_site + "')</script>");
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
                        Session["camera_dpo" + i] = item.Value;
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
                Session["camera_dpo"] = i;
                //Response.Write("jumlah i:" + Session["camera_dpo"]);
                //for (int x = 0; x < i; x++)
                //{
                //    Response.Write("camera nya:" + Session["camera" + x].ToString());

                //}
                // Opsional: Tampilkan pesan atau alihkan halaman
                //Response.Write("Data berhasil disimpan ke dalam session.");
            }
            else
            {
                // Menangani kasus ketika kontrol tidak ditemukan
                //Response.Write("SelectMultiple kontrol tidak ditemukan.");
            }
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
            //Session["site_dpo"] = camera_site;
            //Session["camera_dpo"] = camera_site;
            Session["from_dpo"] = dari;
            Session["to_dpo"] = ke;
            //Response.Write(Session["camera_dpo"]);
            if (Session["camera_dpo"].ToString() == "0")
            {
                Response.Write("<script>alert('Camera cannot be empty')</script>");
                //Response.Redirect("report_dpo.aspx");
            }
            else
            {
                //Response.Redirect("report_dpo_pdf.aspx");
                // Mendapatkan markup HTML dari halaman ExportToPdf.aspx
                // Menyimpan markup HTML ke dalam StringWriter
                StringWriter sw = new StringWriter();
                Server.Execute("report_dpo_pdf.aspx", sw);
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
                Response.AppendHeader("Content-Disposition", "attachment; filename=Report Face Recognition DPO.pdf");
                Response.TransmitFile(outputFilePath);
                Response.End();

            }
        }

        //private void FillEventHistory()
        //{
        //    string camera_site = Session["site_dpo"].ToString();
        //    string camera = Session["camera_dpo"].ToString();
        //    string from = Session["from_dpo"].ToString();
        //    string to = null;
        //    if (Session["to_dpo"] != null)
        //    {
        //        to = Session["to_dpo"].ToString();
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
        //                string per1 = from.ToString();
        //                string per2 = to.ToString();
        //                if (camera == "none")
        //                {
        //                    using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND p.type=@type AND DATE(ce.occurred_at) >=@from AND DATE(ce.occurred_at) <=@to AND ce.type like '%Face%' ORDER BY ce.occurred_at ASC", con))
        //                    {
        //                        cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                        cmd.Parameters.AddWithValue("@type", "blacklist");
        //                        cmd.Parameters.AddWithValue("@from", from);
        //                        cmd.Parameters.AddWithValue("@to", to);
        //                        cmd.Prepare();
        //                        using (MySqlDataReader dr = cmd.ExecuteReader())
        //                        {
        //                            if (dr.HasRows)
        //                            {
        //                                while (dr.Read())
        //                                {
        //                                    x++;
        //                                    Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
        //                                    string image_file = dr["image_file"].ToString();
        //                                    string plate_number_file = dr["plate_number_file"].ToString();
        //                                    string occurred_at = dr["occurred_at"].ToString();
        //                                    string person = dr["person"].ToString();
        //                                    string location = dr["location"].ToString();
        //                                    string event_type = dr["event_type"].ToString();
        //                                    string site = dr["site"].ToString();
        //                                    string person_type = dr["person_type"].ToString();
        //                                    TableRow row = new TableRow();
        //                                    //TableCell no = new TableCell();
        //                                    //no.Text = x.ToString();
        //                                    TableCell no = new TableCell() { Text = x.ToString() };
        //                                    TableCell imageCell = new TableCell();
        //                                    TableCell pers = new TableCell();
        //                                    int cek = event_type.IndexOf("Plate");

        //                                    if (cek != -1)
        //                                    {
        //                                        if (!string.IsNullOrEmpty(plate_number_file))
        //                                        {
        //                                            // string imagePath = UrlImage + plate_number_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                        }
        //                                        pers.Text = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        if (!string.IsNullOrEmpty(image_file))
        //                                        {
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                        }
        //                                        pers.Text = person;
        //                                    }
        //                                    imageCell.HorizontalAlign = HorizontalAlign.Center;
        //                                    //TableCell occurred = new TableCell();
        //                                    //occurred.Text = occurred_at;
        //                                    //TableCell loc = new TableCell();
        //                                    //loc.Text = location;
        //                                    //TableCell sites = new TableCell();
        //                                    //sites.Text = site;
        //                                    TableCell occurred = new TableCell() { Text = occurred_at };
        //                                    TableCell loc = new TableCell() { Text = location };
        //                                    TableCell sites = new TableCell() { Text = site };
        //                                    TableCell status = new TableCell() { Text = event_type };
        //                                    //row.Cells.Add(no);
        //                                    row.Cells.Add(imageCell);
        //                                    row.Cells.Add(occurred);
        //                                    row.Cells.Add(loc);
        //                                    row.Cells.Add(sites);
        //                                    row.Cells.Add(pers);
        //                                    row.Cells.Add(status);



        //                                    TableBody.Controls.Add(row);
        //                                }
        //                                dr.Close();
        //                            }
        //                            else
        //                            {
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Response.Write("<script>alert('apa camera"+camera+"')</script>");
        //                    using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE c.id=@camera AND cs.id=@camera_site  AND (ce.image_file != '' OR ce.image_file != NULL) AND p.type=@type AND DATE(ce.occurred_at) >=@from AND DATE(ce.occurred_at) <=@to AND ce.type like '%Face%' ORDER BY ce.occurred_at ASC", con))
        //                    {
        //                        cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                        cmd.Parameters.AddWithValue("@camera", camera);
        //                        cmd.Parameters.AddWithValue("@type", "blacklist");
        //                        cmd.Parameters.AddWithValue("@from", from);
        //                        cmd.Parameters.AddWithValue("@to", to);
        //                        cmd.Prepare();
        //                        using (MySqlDataReader dr = cmd.ExecuteReader())
        //                        {
        //                            if (dr.HasRows)
        //                            {
        //                                while (dr.Read())
        //                                {
        //                                    x++;
        //                                    Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
        //                                    string image_file = dr["image_file"].ToString();
        //                                    string plate_number_file = dr["plate_number_file"].ToString();
        //                                    string occurred_at = dr["occurred_at"].ToString();
        //                                    string person = dr["person"].ToString();
        //                                    string location = dr["location"].ToString();
        //                                    string event_type = dr["event_type"].ToString();
        //                                    string site = dr["site"].ToString();
        //                                    string person_type = dr["person_type"].ToString();
        //                                    TableRow row = new TableRow();
        //                                    //TableCell no = new TableCell();
        //                                    //no.Text = x.ToString();
        //                                    TableCell no = new TableCell() { Text = x.ToString() };
        //                                    TableCell imageCell = new TableCell();
        //                                    TableCell pers = new TableCell();
        //                                    int cek = event_type.IndexOf("Plate");

        //                                    if (cek != -1)
        //                                    {
        //                                        if (!string.IsNullOrEmpty(plate_number_file))
        //                                        {
        //                                            // string imagePath = UrlImage + plate_number_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                        }
        //                                        pers.Text = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        if (!string.IsNullOrEmpty(image_file))
        //                                        {
        //                                            // string imagePath = UrlImage + image_file;
        //                                            // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                            // string base64String = Convert.ToBase64String(imageBytes);
        //                                            //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                            //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                            //img.AlternateText = "icon title";
        //                                            //img.Style.Add("width", "150px");
        //                                            //imageCell.Controls.Add(img);
        //                                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                            {
        //                                                ImageUrl = "image_file/" + image_file,
        //                                                AlternateText = "icon title"
        //                                            };
        //                                            img.Style.Add("width", "150px");
        //                                            imageCell.Controls.Add(img);
        //                                        }
        //                                        pers.Text = person;
        //                                    }
        //                                    imageCell.HorizontalAlign = HorizontalAlign.Center;
        //                                    //TableCell occurred = new TableCell();
        //                                    //occurred.Text = occurred_at;
        //                                    //TableCell loc = new TableCell();
        //                                    //loc.Text = location;
        //                                    //TableCell sites = new TableCell();
        //                                    //sites.Text = site;
        //                                    TableCell occurred = new TableCell() { Text = occurred_at };
        //                                    TableCell loc = new TableCell() { Text = location };
        //                                    TableCell sites = new TableCell() { Text = site };
        //                                    TableCell status = new TableCell() { Text = event_type };
        //                                    //row.Cells.Add(no);
        //                                    row.Cells.Add(imageCell);
        //                                    row.Cells.Add(occurred);
        //                                    row.Cells.Add(loc);
        //                                    row.Cells.Add(sites);
        //                                    row.Cells.Add(pers);
        //                                    row.Cells.Add(status);



        //                                    TableBody.Controls.Add(row);
        //                                }
        //                                dr.Close();
        //                            }
        //                            else
        //                            {
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else if (DateTime.TryParse(from.ToString(), out fromDate))
        //            {
        //                string per1 = from.ToString();

        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND p.type=@type AND DATE(ce.occurred_at) >=@from AND ce.type like '%Face%' ORDER BY ce.occurred_at ASC", con))
        //                {
        //                    cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                    cmd.Parameters.AddWithValue("@type", "blacklist");
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
        //                                TableCell imageCell = new TableCell();
        //                                TableCell pers = new TableCell();
        //                                int cek = event_type.IndexOf("Plate");

        //                                if (cek != -1)
        //                                {
        //                                    if (!string.IsNullOrEmpty(plate_number_file))
        //                                    {
        //                                        // string imagePath = UrlImage + plate_number_file;
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
        //            else if (DateTime.TryParse(to, out toDate))
        //            {
        //                string per2 = to.ToString();
        //                using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.plate_number_file, ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE cs.id=@camera_site AND (ce.image_file != '' OR ce.image_file != NULL) AND p.type=@type AND DATE(ce.occurred_at) <=@to AND ce.type like '%Face%' ORDER BY ce.occurred_at ASC", con))
        //                {
        //                    cmd.Parameters.AddWithValue("@camera_site", camera_site);
        //                    cmd.Parameters.AddWithValue("@type", "blacklist");
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
        //                                TableCell imageCell = new TableCell();
        //                                TableCell pers = new TableCell();
        //                                int cek = event_type.IndexOf("Plate");

        //                                if (cek != -1)
        //                                {
        //                                    if (!string.IsNullOrEmpty(plate_number_file))
        //                                    {
        //                                        // string imagePath = UrlImage + plate_number_file;
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
        //        }

        //    }
        //}

        private void FillEventHistory()
        {
            //string camera_site = Session["site_dpo"].ToString();
            string camera = Session["camera_dpo"].ToString();
            if (camera != "0")
            {
                int jumlahCamera = Convert.ToInt32(camera);
                string from = Session["from_dpo"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_dpo"] +"')</script>");
                if (Session["to_dpo"].ToString() != "")
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_dpo"].ToString();
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
                    string query = @"SELECT ce.camera_id, ce.type AS event_type, ce.plate_number_file, ce.occurred_at, 
                                ce.person_identification_number, ce.image_file, c.location, p.name as person, 
                                p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code 
                             FROM camera_events as ce 
                             JOIN cameras as c ON ce.camera_id = c.name 
                             LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number 
                             JOIN camera_sites AS cs ON c.camera_site_id = cs.id 
                             LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)
                             WHERE ";

                    // Loop untuk menambahkan parameter kamera ke query
                    if (jumlahCamera > 0)
                    {
                        if (Session["camera_dpo0"].ToString() == "all")
                        {
                            query += @" c.id in (SELECT id from cameras where name not like 'LPR%')";
                        }
                        else
                        {
                            query += @"(";
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

                    query += @" AND (ce.image_file != '' OR ce.image_file IS NOT NULL) 
                       AND p.type=@type 
                       AND ce.type LIKE '%Face%'";
                    if (from_date != null)
                    {
                        query += @" AND ce.occurred_at >= @from ";
                    }
                    if (to_date != null)
                    {
                        query += @" AND ce.occurred_at <= @to ";
                    }
                    query += @"ORDER BY ce.occurred_at ASC";
                    //Response.Write(query);

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Menambahkan parameter untuk kamera
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_dpo0"].ToString() == "all")
                            {
                            }
                            else
                            {
                                for (int i = 0; i < jumlahCamera; i++)
                                {
                                    cmd.Parameters.AddWithValue($"@camera{i}", Session["camera_dpo" + i].ToString());
                                }
                            }
                        }

                        // Tambahkan parameter lain yang diperlukan
                        cmd.Parameters.AddWithValue("@type", "blacklist");
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
                                    string person_image = dr["person_image"].ToString();
                                    string entry_code = dr["entry_code"].ToString();
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
                                            // string imagePath = UrlImage + plate_number_file;
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
                                        if (!string.IsNullOrEmpty(person_image))
                                        {
                                            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image
                                            {
                                                ImageUrl = "person_image/" + person_image,
                                                AlternateText = "icon title"
                                            };
                                            img2.Style.Add("width", "150px");
                                            imageCell2.Controls.Add(img2);
                                        }
                                        else
                                        {
                                            imageCell2.Text = "";
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
                                    TableCell occurred = new TableCell() { Text = occurred_at };
                                    TableCell loc = new TableCell() { Text = location };
                                    TableCell sites = new TableCell() { Text = site };
                                    TableCell code = new TableCell() { Text = entry_code };
                                    TableCell status = new TableCell() { Text = event_type };
                                    //row.Cells.Add(no);
                                    row.Cells.Add(imageCell);
                                    row.Cells.Add(imageCell2);
                                    row.Cells.Add(occurred);
                                    row.Cells.Add(loc);
                                    row.Cells.Add(sites);
                                    row.Cells.Add(pers);
                                    //row.Cells.Add(code);
                                    row.Cells.Add(status);

                                    TableBody.Controls.Add(row);

                                }
                                dr.Close();
                            }
                        }
                    }
                }
            }
        }

        protected void btnExportExcell_Click(object sender, EventArgs e)
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
                        Session["camera_dpo" + i] = item.Value;
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
                Session["camera_dpo"] = i;
                //Response.Write("jumlah i:" + Session["camera_dpo"]);
                //for (int x = 0; x < i; x++)
                //{
                //    Response.Write("camera nya:" + Session["camera" + x].ToString());

                //}
                // Opsional: Tampilkan pesan atau alihkan halaman
                //Response.Write("Data berhasil disimpan ke dalam session.");
            }
            else
            {
                // Menangani kasus ketika kontrol tidak ditemukan
                //Response.Write("SelectMultiple kontrol tidak ditemukan.");
            }
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
            //Session["site_dpo"] = camera_site;
            //Session["camera_dpo"] = camera_site;
            Session["from_dpo"] = dari;
            Session["to_dpo"] = ke;
            //Response.Write(Session["camera_dpo"]);
            if (Session["camera_dpo"].ToString() == "0")
            {
                Response.Write("<script>alert('Camera cannot be empty')</script>");
                //Response.Redirect("report_dpo.aspx");
            }
            else
            {
                ExportToExcelWithImages();
            }
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
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
                        Session["camera_dpo" + i] = item.Value;
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
                Session["camera_dpo"] = i;
            }
            else
            {
            }
            string dari = TextBox1.Text;
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                dari = today;

            }
            string ke = TextBox2.Text;
            Session["from_dpo"] = dari;
            Session["to_dpo"] = ke;
            if (Session["camera_dpo"].ToString() == "0")
            {
                Response.Write("<script>alert('Camera cannot be empty')</script>");
            }
            else
            {
                //Export();
                ExportCSV();
            }
        }
        protected void ExportCSV()
        {
            string fileName = "Report_Face_Recognition_DPO.csv";
            string camera = Session["camera_dpo"].ToString();
            if (camera != "0")
            {
                int jumlahCamera = Convert.ToInt32(camera);
                string from = Session["from_dpo"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
                string per2 = "";
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_dpo"] +"')</script>");
                if (Session["to_dpo"].ToString() != "")
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_dpo"].ToString();
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
                        string query = @"SELECT MAX(ce.occurred_at) as to_date, ce.camera_id, ce.type AS event_type, ce.plate_number_file, ce.occurred_at, 
                                ce.person_identification_number, ce.image_file, c.location, p.name as person, 
                                p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code 
                             FROM camera_events as ce 
                             JOIN cameras as c ON ce.camera_id = c.name 
                             LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number 
                             JOIN camera_sites AS cs ON c.camera_site_id = cs.id 
                             LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)
                             WHERE ";

                        // Loop untuk menambahkan parameter kamera ke query
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_dpo0"].ToString() == "all")
                            {
                                query += @" c.id in (SELECT id from cameras where name not like 'LPR%')";
                            }
                            else
                            {
                                query += @"(";
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

                        query += @" AND (ce.image_file != '' OR ce.image_file IS NOT NULL) 
                       AND p.type=@type 
                       AND ce.type LIKE '%Face%'";
                        if (from_date != null)
                        {
                            query += @" AND ce.occurred_at >= @from ";
                        }
                        if (to_date != null)
                        {
                            query += @" AND ce.occurred_at <= @to ";
                        }
                        query += @"ORDER BY ce.occurred_at DESC LIMIT 1";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_dpo0"].ToString() == "all")
                                {
                                }
                                else
                                {
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_dpo" + y].ToString());
                                    }
                                }
                            }

                            // Tambahkan parameter lain yang diperlukan
                            cmd.Parameters.AddWithValue("@type", "blacklist");
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
                string csvContent = $"\"Report Face Recognition DPO\"\n";
                csvContent += $"\"Periode {per1} to {per2}\"\n";
                csvContent += "\"Image\",,\"Occurred At\",\"Location\",\"Site\",\"Person\",\"Status\"\n";

                int x = 0;
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (camera == "0")
                    {

                        Response.Write("<script>alert('Camera cannot be empty')</script>");
                    }
                    else
                    {

                        string query = @"SELECT ce.camera_id, ce.type AS event_type, ce.plate_number_file, ce.occurred_at, 
                                ce.person_identification_number, ce.image_file, c.location, p.name as person, 
                                p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code 
                             FROM camera_events as ce 
                             JOIN cameras as c ON ce.camera_id = c.name 
                             LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number 
                             JOIN camera_sites AS cs ON c.camera_site_id = cs.id 
                             LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)
                             WHERE ";

                        // Loop untuk menambahkan parameter kamera ke query
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_dpo0"].ToString() == "all")
                            {
                                query += @" c.id in (SELECT id from cameras where name not like 'LPR%')";
                            }
                            else
                            {
                                query += @"(";
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

                        query += @"AND (ce.image_file != '' OR ce.image_file IS NOT NULL) 
                       AND p.type=@type 
                       AND ce.type LIKE '%Face%'";
                        if (from_date != null)
                        {
                            query += @" AND ce.occurred_at >= @from ";
                        }
                        if (to_date != null)
                        {
                            query += @" AND ce.occurred_at <= @to ";
                        }
                        query += @"ORDER BY ce.occurred_at ASC";
                        //Response.Write(query);

                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            // Menambahkan parameter untuk kamera
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_dpo0"].ToString() == "all")
                                {
                                }
                                else
                                {
                                    for (int i = 0; i < jumlahCamera; i++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{i}", Session["camera_dpo" + i].ToString());
                                    }
                                }
                            }

                            // Tambahkan parameter lain yang diperlukan
                            cmd.Parameters.AddWithValue("@type", "blacklist");
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
                                        string person_image = dr["person_image"].ToString();
                                        string entry_code = dr["entry_code"].ToString();
                                        TableRow row = new TableRow();
                                        //TableCell no = new TableCell();
                                        //no.Text = x.ToString(); 
                                        TableCell no = new TableCell() { Text = x.ToString() };
                                        TableCell imageCell = new TableCell();
                                        TableCell imageCell2 = new TableCell();
                                        TableCell pers = new TableCell();
                                        int cek = event_type.IndexOf("Plate");

                                        string link1 = baseUrl + "/image_file/" + image_file;
                                        string link2 = baseUrl + "/person_image/" + person_image;

                                        csvContent += $"\"{link1}\",\"{link2}\",\"{occurred_at}\",\"{location}\",\"{site}\",\"{person}\",\"{event_type}\"\n";
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
                Response.Output.Write(csvContent);
                Response.Flush();
                Response.End();

            }
        }
        protected void Export()
        {
            string fileName = "Report Face Recognition DPO.xls";
            string camera = Session["camera_dpo"].ToString();
            if (camera != "0")
            {
                int jumlahCamera = Convert.ToInt32(camera);
                string from = Session["from_dpo"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
                string per2 = "";
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_dpo"] +"')</script>");
                if (Session["to_dpo"].ToString() != "")
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_dpo"].ToString();
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
                        string query = @"SELECT MAX(ce.occurred_at) as to_date, ce.camera_id, ce.type AS event_type, ce.plate_number_file, ce.occurred_at, 
                                ce.person_identification_number, ce.image_file, c.location, p.name as person, 
                                p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code 
                             FROM camera_events as ce 
                             JOIN cameras as c ON ce.camera_id = c.name 
                             LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number 
                             JOIN camera_sites AS cs ON c.camera_site_id = cs.id 
                             LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)
                             WHERE ";

                        // Loop untuk menambahkan parameter kamera ke query
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_dpo0"].ToString() == "all")
                            {
                                query += @" c.id in (SELECT id from cameras where name not like 'LPR%')";
                            }
                            else
                            {
                                query += @"(";
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

                        query += @" AND (ce.image_file != '' OR ce.image_file IS NOT NULL) 
                       AND p.type=@type 
                       AND ce.type LIKE '%Face%'";
                        if (from_date != null)
                        {
                            query += @" AND ce.occurred_at >= @from ";
                        }
                        if (to_date != null)
                        {
                            query += @" AND ce.occurred_at <= @to ";
                        }
                        query += @"ORDER BY ce.occurred_at DESC LIMIT 1";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_dpo0"].ToString() == "all")
                                {
                                }
                                else
                                {
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_dpo" + y].ToString());
                                    }
                                }
                            }

                            // Tambahkan parameter lain yang diperlukan
                            cmd.Parameters.AddWithValue("@type", "blacklist");
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
                <td colspan='8' align='center'><b>Report Face Recognition DPO</b></td>
            </tr>
            <tr>
                <td colspan='8' align='center'><b>Periode {per1} to {per2}</b></td>
            </tr>
            <tr>
                <td><b>No</b></td>
                <td><b>Image</b></td>
                <td></td>
                <td><b>Occurred At</b></td>
                <td><b>Location</b></td>
                <td><b>Site</b></td>
                <td><b>Person</b></td>
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
                    if (camera == "0")
                    {

                        Response.Write("<script>alert('Camera cannot be empty')</script>");
                    }
                    else
                    {

                        string query = @"SELECT ce.camera_id, ce.type AS event_type, ce.plate_number_file, ce.occurred_at, 
                                ce.person_identification_number, ce.image_file, c.location, p.name as person, 
                                p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code 
                             FROM camera_events as ce 
                             JOIN cameras as c ON ce.camera_id = c.name 
                             LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number 
                             JOIN camera_sites AS cs ON c.camera_site_id = cs.id 
                             LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)
                             WHERE ";

                        // Loop untuk menambahkan parameter kamera ke query
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_dpo0"].ToString() == "all")
                            {
                                query += @" c.id in (SELECT id from cameras where name not like 'LPR%')";
                            }
                            else
                            {
                                query += @"(";
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

                        query += @"AND (ce.image_file != '' OR ce.image_file IS NOT NULL) 
                       AND p.type=@type 
                       AND ce.type LIKE '%Face%'";
                        if (from_date != null)
                        {
                            query += @" AND ce.occurred_at >= @from ";
                        }
                        if (to_date != null)
                        {
                            query += @" AND ce.occurred_at <= @to ";
                        }
                        query += @"ORDER BY ce.occurred_at ASC";
                        //Response.Write(query);

                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            // Menambahkan parameter untuk kamera
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_dpo0"].ToString() == "all")
                                {
                                }
                                else
                                {
                                    for (int i = 0; i < jumlahCamera; i++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{i}", Session["camera_dpo" + i].ToString());
                                    }
                                }
                            }

                            // Tambahkan parameter lain yang diperlukan
                            cmd.Parameters.AddWithValue("@type", "blacklist");
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
                                        string person_image = dr["person_image"].ToString();
                                        string entry_code = dr["entry_code"].ToString();
                                        TableRow row = new TableRow();
                                        //TableCell no = new TableCell();
                                        //no.Text = x.ToString(); 
                                        TableCell no = new TableCell() { Text = x.ToString() };
                                        TableCell imageCell = new TableCell();
                                        TableCell imageCell2 = new TableCell();
                                        TableCell pers = new TableCell();
                                        int cek = event_type.IndexOf("Plate");

                                        tableHtml += $@"<tr>
                                                <td>{x}</td>";
                                        string path = UrlImage + image_file;
                                        tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path}' alt='Foto' width='150' height='150'/></center></td>";

                                        if (cek != -1)
                                        {
                                            if (!string.IsNullOrEmpty(plate_number_file))
                                            {
                                                string path2 = UrlImage + plate_number_file;
                                                tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path2}' alt='Foto' width='150' height='150px'/></center></td>";
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(person) && !string.IsNullOrEmpty(person_image))
                                            {
                                                string path2 = UrlImagePerson + person_image;
                                                tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path2}' alt='Foto' width='150' height='150'/></center></td>";
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
                                                //string warna = "";
                                                //if (person_type == "blacklist")
                                                //{
                                                //    warna += "red";
                                                //}
                                                //else
                                                //{
                                                //    if (entry_code == "0")
                                                //    {
                                                //        warna += "green";
                                                //    }
                                                //    else if (entry_code == "1")
                                                //    {
                                                //        warna += "yellow";
                                                //    }
                                                //    else if (entry_code == "2")
                                                //    {
                                                //        warna += "orange";
                                                //    }
                                                //}
                                                //tableHtml += $@"<td><span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry_code}</span></td>";
                                                tableHtml += $@"<td>{person}</td>";
                                            }
                                            else
                                            {
                                                tableHtml += $@"<td>{person}</td>";
                                            }
                                        }
                                        tableHtml += $@"<td>{event_type}</td>
                                            </tr>";

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
                tableHtml += @"</table>";

                // Menuliskan HTML ke response
                Response.Output.Write(tableHtml);
                Response.Flush();
                Response.End();

            }
        }
        protected void ExportToExcelWithImages()
        {
            string fileName = "Report Face Recognition DPO.xls";
            string camera = Session["camera_dpo"].ToString();
            if (camera != "0")
            {
                int jumlahCamera = Convert.ToInt32(camera);
                string from = Session["from_dpo"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
                string per2 = "";
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_dpo"] +"')</script>");
                if (Session["to_dpo"].ToString() != "")
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_dpo"].ToString();
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
                        string query = @"SELECT MAX(occurred_at) as to_date, ce.camera_id, ce.type AS event_type, ce.plate_number_file, ce.occurred_at, 
                                ce.person_identification_number, ce.image_file, c.location, p.name as person, 
                                p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code 
                             FROM camera_events as ce 
                             JOIN cameras as c ON ce.camera_id = c.name 
                             LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number 
                             JOIN camera_sites AS cs ON c.camera_site_id = cs.id 
                             LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)
                             WHERE ";

                        // Loop untuk menambahkan parameter kamera ke query
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_dpo0"].ToString() == "all")
                            {
                                query += @" c.id in (SELECT id from cameras where name not like 'LPR%')";
                            }
                            else
                            {
                                query += @"(";
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

                        query += @" AND (ce.image_file != '' OR ce.image_file IS NOT NULL) 
                       AND p.type=@type 
                       AND ce.type LIKE '%Face%'";
                        if (from_date != null)
                        {
                            query += @" AND ce.occurred_at >= @from ";
                        }
                        if (to_date != null)
                        {
                            query += @" AND ce.occurred_at <= @to ";
                        }
                        query += @"ORDER BY ce.occurred_at DESC LIMIT 1";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_dpo0"].ToString() == "all")
                                {
                                }
                                else
                                {
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_dpo" + y].ToString());
                                    }
                                }
                            }

                            // Tambahkan parameter lain yang diperlukan
                            cmd.Parameters.AddWithValue("@type", "blacklist");
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
                <td colspan='8' align='center'><b>Report Face Recognition DPO</b></td>
            </tr>
            <tr>
                <td colspan='8' align='center'><b>Periode {per1} to {per2}</b></td>
            </tr>
            <tr>
                <td><b>No</b></td>
                <td><b>Image</b></td>
                <td></td>
                <td><b>Occurred At</b></td>
                <td><b>Location</b></td>
                <td><b>Site</b></td>
                <td><b>Person</b></td>
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
                    if (camera == "0")
                    {

                        Response.Write("<script>alert('Camera cannot be empty')</script>");
                    }
                    else
                    {

                        string query = @"SELECT ce.camera_id, ce.type AS event_type, ce.plate_number_file, ce.occurred_at, 
                                ce.person_identification_number, ce.image_file, c.location, p.name as person, 
                                p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code 
                             FROM camera_events as ce 
                             JOIN cameras as c ON ce.camera_id = c.name 
                             LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number 
                             JOIN camera_sites AS cs ON c.camera_site_id = cs.id 
                             LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)
                             WHERE ";

                        // Loop untuk menambahkan parameter kamera ke query
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_dpo0"].ToString() == "all")
                            {
                                query += @" c.id in (SELECT id from cameras where name not like 'LPR%')";
                            }
                            else
                            {
                                query += @"(";
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

                        query += @"AND (ce.image_file != '' OR ce.image_file IS NOT NULL) 
                       AND p.type=@type 
                       AND ce.type LIKE '%Face%'";
                        if (from_date != null)
                        {
                            query += @" AND ce.occurred_at >= @from ";
                        }
                        if (to_date != null)
                        {
                            query += @" AND ce.occurred_at <= @to ";
                        }
                        query += @"ORDER BY ce.occurred_at ASC";
                        //Response.Write(query);

                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            // Menambahkan parameter untuk kamera
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_dpo0"].ToString() == "all")
                                {
                                }
                                else
                                {
                                    for (int i = 0; i < jumlahCamera; i++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{i}", Session["camera_dpo" + i].ToString());
                                    }
                                }
                            }

                            // Tambahkan parameter lain yang diperlukan
                            cmd.Parameters.AddWithValue("@type", "blacklist");
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
                                        string person_image = dr["person_image"].ToString();
                                        string entry_code = dr["entry_code"].ToString();
                                        TableRow row = new TableRow();
                                        //TableCell no = new TableCell();
                                        //no.Text = x.ToString(); 
                                        TableCell no = new TableCell() { Text = x.ToString() };
                                        TableCell imageCell = new TableCell();
                                        TableCell imageCell2 = new TableCell();
                                        TableCell pers = new TableCell();
                                        int cek = event_type.IndexOf("Plate");

                                        tableHtml += $@"<tr>
                                                <td>{x}</td>";
                                        string path = UrlImage + image_file;
                                        tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path}' alt='Foto' width='150' height='150'/></center></td>";

                                        if (cek != -1)
                                        {
                                            if (!string.IsNullOrEmpty(plate_number_file))
                                            {
                                                string path2 = UrlImage + plate_number_file;
                                                tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path2}' alt='Foto' width='150' height='150px'/></center></td>";
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(person) && !string.IsNullOrEmpty(person_image))
                                            {
                                                string path2 = UrlImagePerson + person_image;
                                                tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path2}' alt='Foto' width='150' height='150'/></center></td>";
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
                                                //string warna = "";
                                                //if (person_type == "blacklist")
                                                //{
                                                //    warna += "red";
                                                //}
                                                //else
                                                //{
                                                //    if (entry_code == "0")
                                                //    {
                                                //        warna += "green";
                                                //    }
                                                //    else if (entry_code == "1")
                                                //    {
                                                //        warna += "yellow";
                                                //    }
                                                //    else if (entry_code == "2")
                                                //    {
                                                //        warna += "orange";
                                                //    }
                                                //}
                                                //tableHtml += $@"<td><span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry_code}</span></td>";
                                                tableHtml += $@"<td>{person}</td>";
                                            }
                                            else
                                            {
                                                tableHtml += $@"<td>{person}</td>";
                                            }
                                        }
                                        tableHtml += $@"<td>{event_type}</td>
                                            </tr>";

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
        protected void ExportToExcelWithImages2()
        {
            // Buat workbook baru
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Report Face Recognition DPO");

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

            string camera = Session["camera_dpo"].ToString();
            if (camera != "0")
            {
                int jumlahCamera = Convert.ToInt32(camera);
                string from = Session["from_dpo"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_dpo"] +"')</script>");
                if (Session["to_dpo"].ToString() != "")
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_dpo"].ToString();
                    DateTime parsedDateTime2 = DateTime.Parse(to);
                    to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");

                }
                else
                {

                    //Response.Write("<script>alert('kosong')</script>");

                }

                //DateTime fromDate;
                //DateTime toDate;
                string per1 = "";
                string per2 = "";
                int i = 3;
                int x = 0;

                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    string query = @"SELECT ce.camera_id, ce.type AS event_type, ce.plate_number_file, ce.occurred_at, 
                                ce.person_identification_number, ce.image_file, c.location, p.name as person, 
                                p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code 
                             FROM camera_events as ce 
                             JOIN cameras as c ON ce.camera_id = c.name 
                             LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number 
                             JOIN camera_sites AS cs ON c.camera_site_id = cs.id 
                             LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)
                             WHERE ";

                    // Loop untuk menambahkan parameter kamera ke query
                    if (jumlahCamera > 0)
                    {
                        if (Session["camera_dpo0"].ToString() == "all")
                        {
                            query += @" c.id in (SELECT id from cameras where name not like 'LPR%')";
                        }
                        else
                        {
                            query += @"(";
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

                    query += @" AND (ce.image_file != '' OR ce.image_file IS NOT NULL) 
                       AND p.type=@type 
                       AND ce.type LIKE '%Face%'";
                    if (from_date != null)
                    {
                        query += @" AND ce.occurred_at >= @from ";
                    }
                    if (to_date != null)
                    {
                        query += @" AND ce.occurred_at <= @to ";
                    }
                    query += @"ORDER BY ce.occurred_at ASC";
                    //Response.Write(query);

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Menambahkan parameter untuk kamera
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_dpo0"].ToString() == "all")
                            {
                            }
                            else
                            {
                                for (int y = 0; y < jumlahCamera; y++)
                                {
                                    cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_dpo" + y].ToString());
                                }
                            }
                        }

                        // Tambahkan parameter lain yang diperlukan
                        cmd.Parameters.AddWithValue("@type", "blacklist");
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
                                        //if (!string.IsNullOrEmpty(person_image))
                                        //{
                                        //    ICellStyle cellStyle = workbook.CreateCellStyle();
                                        //    IFont personFont = workbook.CreateFont();
                                        //    string warna = "";

                                        //    if (person_type == "blacklist")
                                        //    {
                                        //        warna = "red";
                                        //    }
                                        //    else
                                        //    {
                                        //        if (entry_code == "0")
                                        //        {
                                        //            warna = "green";
                                        //        }
                                        //        else if (entry_code == "1")
                                        //        {
                                        //            warna = "yellow";
                                        //        }
                                        //        else if (entry_code == "2")
                                        //        {
                                        //            warna = "orange";
                                        //        }
                                        //    }

                                        //    // Set warna font
                                        //    personFont.Color = IndexedColors.Red.Index; // Contoh menggunakan warna merah
                                        //    if (warna == "green") personFont.Color = IndexedColors.Green.Index;
                                        //    if (warna == "yellow") personFont.Color = IndexedColors.Yellow.Index;
                                        //    if (warna == "orange") personFont.Color = IndexedColors.Orange.Index;

                                        //    cellStyle.SetFont(personFont);

                                        //    // Atur teks dalam sel
                                        //    ICell cellPerson = row.CreateCell(6);
                                        //    cellPerson.SetCellValue($"[{char.ToUpper(person_type[0]) + person_type.Substring(1)}] - {person} - {entry_code}");
                                        //    cellPerson.CellStyle = cellStyle;
                                        //}
                                        //else
                                        //{
                                        row.CreateCell(6).SetCellValue(person);
                                        //}

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
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 7)); // Merge baris 1 kolom 0-7
                IRow headerRow1 = sheet.CreateRow(0);
                ICell headerCell1 = headerRow1.CreateCell(0);
                headerCell1.SetCellValue("Face Recognition DPO");
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
                    Response.AddHeader("content-disposition", "attachment;filename=ReportFaceRecognitionDPO.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
            }
        }

    }
}