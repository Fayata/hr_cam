using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class list_report : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id_download"] != null)
            {
                string id_download = Request.QueryString["id_download"];
                downloadFile(id_download);
                //Response.Write(filename);
                // Gunakan data sesuai kebutuhan
            }
            if (Request.QueryString["id_delete"] != null)
            {
                string id_delete = Request.QueryString["id_delete"];
                deleteFile(id_delete);
                //Response.Write(filename);
                // Gunakan data sesuai kebutuhan
            }
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            //Response.Write(url)
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
                FillTable();
            }

        }

        private void FillTable()
        {
            int x = 0;
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = @"SELECT * from reports ORDER BY id DESC";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    // Tambahkan parameter lain yang diperlukan
                    //cmd.Parameters.AddWithValue("@type", "blacklist");
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                x++;
                                //Response.Write("<script>console.log('isinya " + dr.GetValue(0).ToString() + "')</script>");
                                string id = dr["id"].ToString();
                                string filename = dr["filename"].ToString();
                                string created_at = dr["created_at"].ToString();
                                string queries = dr["queries"].ToString();
                                string status = dr["status"].ToString();
                                //string status;
                                //if (is_completed == "True")
                                //{
                                //    status = "Completed";
                                //}
                                //else
                                //{
                                //    status = "Not Complete";
                                //}
                                string limitedqueries = "";
                                if (queries != "" && queries != null)
                                {
                                    //limitedqueries += GetFirst50Words(queries);
                                    //string limitedqueries2 = GetFirst100Params(queries);
                                    limitedqueries += (queries.Length > 100 ? queries.Substring(0, 100) : queries) + " ...";
                                    //Response.Write(outputString);
                                    //Response.Write("<script>alert('" + limitedqueries2 + "')</script>");

                                }
                                else
                                {

                                }
                                string name = "";
                                if (filename.Length > 20)
                                {
                                    name += filename.Substring(0, 20) + "...";
                                }
                                else
                                {
                                    name += filename;
                                }
                                //string limitedqueries = GetFirst50Words(queries);
                                //Response.Write("<script>alert('" + limitedqueries + "')</script>");
                                TableRow row = new TableRow();
                                TableCell filenameCell = new TableCell() { Text = name };
                                filenameCell.Attributes.Add("title", filename);
                                TableCell createdAtCell = new TableCell() { Text = created_at };
                                TableCell queriesCell = new TableCell() { Text = limitedqueries };
                                queriesCell.Style["white-space"] = "normal"; // Allow line breaks within the cell
                                queriesCell.Style["word-wrap"] = "break-word"; // Break words if necessary to fit within the cell
                                queriesCell.Style["word-break"] = "break-word"; // Break words if necessary to fit within the cell
                                queriesCell.Style["max-width"] = "300px";
                                queriesCell.Attributes.Add("title", queries);
                                TableCell statusCell = new TableCell() { Text = status };
                                row.Cells.Add(filenameCell);
                                row.Cells.Add(createdAtCell);
                                row.Cells.Add(queriesCell);
                                row.Cells.Add(statusCell);
                                if (status == "completed" || status == "failed")
                                {
                                    TableCell actionCell = new TableCell();
                                    //actionCell.Style["width"] = "200px";
                                    if (status == "completed")
                                    {
                                        string downloadButtonHtml = $"<a href=\"list_report.aspx?id_download={id}\" class=\"btn btn-success\">Download</a>";
                                        string deleteButtonHtml = $"<a href=\"list_report.aspx?id_delete={id}\" class=\"btn btn-danger\" onclick=\"return confirm('Are you sure?');\"><span class='btn-icon'><i class='fa-solid fa-trash-can'></i></span></a>";
                                        actionCell.Controls.Add(new LiteralControl(downloadButtonHtml + " " + deleteButtonHtml));
                                    }
                                    else
                                    {
                                        string deleteButtonHtml = $"<a href=\"list_report.aspx?id_delete={id}\" class=\"btn btn-danger\" onclick=\"return confirm('Are you sure?');\"><span class='btn-icon'><i class='fa-solid fa-trash-can'></i></span></a>";
                                        actionCell.Controls.Add(new LiteralControl(deleteButtonHtml));
                                    }
                                    row.Cells.Add(actionCell);
                                }
                                else
                                {
                                    TableCell actionnCell = new TableCell() { Text = "" };
                                    row.Cells.Add(actionnCell);
                                }

                                TableBody.Controls.Add(row);

                            }
                            dr.Close();
                        }
                    }
                }
            }
        }

        private void downloadFile(string id)
        {
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = @"SELECT * from reports where id=@id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    // Tambahkan parameter lain yang diperlukan
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                string filename = dr["filename"].ToString();
                                if (filename != string.Empty)
                                {
                                    WebClient req = new WebClient();
                                    HttpResponse response = HttpContext.Current.Response;
                                    string filePath = "~/report_file/" + filename;
                                    response.Clear();
                                    response.ClearContent();
                                    response.ClearHeaders();
                                    response.Buffer = true;
                                    response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                                    //byte[] data = req.DownloadData("C:\\Users\\ati\\source\\repos\\hr_cam\\hr_cam\\report_file\\" + filename);
                                    byte[] data = req.DownloadData(Server.MapPath(filePath));
                                    response.BinaryWrite(data);
                                    response.End();
                                }

                            }
                            dr.Close();
                        }
                    }
                }
            }

        }

        private string GetFirst50Words(string input)
        {
            string[] words = input.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length <= 50)
            {
                return input; // Jika kurang dari 100 kata, kembalikan string aslinya
            }
            else
            {
                return string.Join(" ", words.Take(50)) + "..."; // Ambil 100 kata pertama dan tambahkan "..."
            }
        }

        private string GetFirst100Params(string query)
        {
            // Pisahkan berdasarkan '&' untuk mendapatkan setiap pasangan kunci-nilai
            string[] parameters = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            if (parameters.Length <= 10)
            {
                return query; // Jika kurang dari atau sama dengan 100 parameter, kembalikan string aslinya
            }
            else
            {
                // Ambil 100 parameter pertama dan gabungkan kembali dengan '&'
                return string.Join("&", parameters.Take(10)) + "..."; // Tambahkan "..." di akhir jika lebih dari 100 parameter
            }
        }
        private void deleteFile(string id)
        {
            string filename = "";
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = @"SELECT * from reports where id=@id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    // Tambahkan parameter lain yang diperlukan
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                filename += dr["filename"].ToString();


                            }
                            dr.Close();
                        }
                    }
                }
            }
            //Response.Write(filename);
            if (filename != string.Empty)
            {
                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (MySqlCommand cmd = new MySqlCommand("DELETE from reports where id='" + id + "'", con))
                    {

                        cmd.ExecuteNonQuery();
                        con.Close();
                        string filePath = "~/report_file/" + filename;
                        string path = Server.MapPath(filePath);
                        Response.Write(path);

                        if (File.Exists(path))
                        {
                            // If file found, delete it
                            File.Delete(path);
                            //Response.Write("File Ada.");
                        }
                        else
                        {
                            //Response.Write("File not found");
                        }
                        Session["fail"] = "Report have been succesfully removed.";
                        Response.Redirect("list_report.aspx");
                    }
                }
            }
        }
    }
}
