using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class person_history : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        private int PageSize => 10;

        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1; }
            set { ViewState["CurrentPage"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrentPage = 1;
                FillEventHistory(CurrentPage);
            }
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            CurrentPage = int.Parse(btn.CommandArgument);
            FillEventHistory(CurrentPage);
        }

        private void FillEventHistory(int pageNumber)
        {
            TableBody.Controls.Clear();
            int offset = (pageNumber - 1) * PageSize;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                con.Open();

                string query = @"
                    SELECT p.identification_number, p.name, ph.entry_code, ph.extra_status_info, ph.created_at
                    FROM persons p
                    JOIN person_history ph ON ph.person_id = p.id
                    ORDER BY ph.created_at DESC
                    LIMIT @PageSize OFFSET @Offset";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);
                    cmd.Parameters.AddWithValue("@Offset", offset);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        int rowNumber = offset + 1;
                        while (dr.Read())
                        {
                            string badge = dr["identification_number"].ToString();
                            string name = dr["name"].ToString();
                            string entry_code = dr["entry_code"].ToString();
                            string extra_status_info = dr["extra_status_info"].ToString();
                            string created = Convert.ToDateTime(dr["created_at"]).ToString("yyyy-MM-dd HH:mm");

                            string entry = "", extra_info = "";

                            if (entry_code == "0")
                            {
                                entry = "Comply";
                                if (extra_status_info == "0") extra_info = "Status berubah dari Comply";
                                else if (extra_status_info == "1") extra_info = "Status sebelumnya: Not Comply or Badge Expired";
                                else if (extra_status_info == "2") extra_info = "Status sebelumnya: FTW Rejected Medical / Expired";
                                else if (extra_status_info == "3") extra_info = "Status sebelumnya: Daily Checkup Failed";
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
                                extra_info = extra_status_info == "0" ? "Gagal Daily Check Up" : "Absen saat Daily Check Up";
                            }

                            TableRow row = new TableRow();
                            row.Cells.Add(new TableCell() { Text = rowNumber.ToString() }); rowNumber++;
                            row.Cells.Add(new TableCell() { Text = badge });
                            row.Cells.Add(new TableCell() { Text = name });
                            row.Cells.Add(new TableCell() { Text = entry });
                            row.Cells.Add(new TableCell() { Text = extra_info });
                            row.Cells.Add(new TableCell() { Text = created });

                            TableBody.Controls.Add(row);
                        }
                    }
                }
            }

            int totalRecords = GetTotalCount();
            BindPager(totalRecords, pageNumber);
        }

        private int GetTotalCount()
        {
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                con.Open();
                string countQuery = "SELECT COUNT(*) FROM person_history;";
                using (MySqlCommand cmd = new MySqlCommand(countQuery, con))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private void BindPager(int totalRecords, int currentPage)
        {
            int totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);
            List<ListItem> pages = new List<ListItem>();

            if (totalPages <= 1) return;

            // Prev
            if (currentPage > 1)
                pages.Add(new ListItem("«", (currentPage - 1).ToString()));

            // Page numbers
            int start = currentPage - 2 > 0 ? currentPage - 2 : 1;
            int end = start + 4 <= totalPages ? start + 4 : totalPages;

            for (int i = start; i <= end; i++)
            {
                ListItem li = new ListItem(i.ToString(), i.ToString());
                li.Selected = (i == currentPage);
                pages.Add(li);
            }

            // Next
            if (currentPage < totalPages)
                pages.Add(new ListItem("»", (currentPage + 1).ToString()));

            rptPager.DataSource = pages.Select(p => new
            {
                Text = p.Text,
                Value = p.Value,
                CssClass = p.Selected ? "page-item active" : "page-item"
            });
            rptPager.DataBind();
        }
    }
}
