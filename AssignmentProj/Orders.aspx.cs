using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace AssignmentProj
{
    public partial class Orders : System.Web.UI.Page
    {
        // Creating instance of SqlConnection
        //string connectionString = @"Data Source=LPCL-PG033Z4W;Initial Catalog=Assignment;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlConnection conn = new SqlConnection("Data Source=LPCL-PG033YL7;Initial Catalog=Assignment;Integrated Security=True");
        DataSet ds = new DataSet();
        //SqlCommand sqlcmd;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                PopulateGridview();
                // BindList();
                // GridInfo();
            }
        }
        void PopulateGridview()
        {
            DataTable dt = new DataTable();
            // using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM dbo.OrderDetail", conn);
                sqlDa.Fill(dt);
            }
            if (dt.Rows.Count > 0)
            {
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                GridView2.DataSource = dt;
                GridView2.DataBind();
                GridView2.Rows[0].Cells.Clear();
                GridView2.Rows[0].Cells.Add(new TableCell());
                GridView2.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                GridView2.Rows[0].Cells[0].Text = "No Data Found ..!";
                GridView2.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }


        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    //using (SqlConnection sqlCon = new SqlConnection(conn))
                    {
                        conn.Open();
                        string query = "INSERT INTO dbo.OrderDetail (ProductName,Rate,Quantity,TotalAmount) VALUES (@ProductName,@Rate,@Quantity,@TotalAmount)";
                        SqlCommand sqlCmd = new SqlCommand(query, conn);
                        sqlCmd.Parameters.AddWithValue("@ProductName", (GridView2.FooterRow.FindControl("txtProductNameFooter") as DropDownList).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Rate", (GridView2.FooterRow.FindControl("txtRateFooter") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Quantity", (GridView2.FooterRow.FindControl("txtQuantityFooter") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@TotalAmount", (GridView2.FooterRow.FindControl("txtTotalAmountFooter") as TextBox).Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        conn.Close();
                        PopulateGridview();
                        lblSuccessMessage.Text = "New Record Added";
                        lblErrorMessage.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            PopulateGridview();
        }

        protected void Gridview2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            PopulateGridview();
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                // using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE dbo.OrderDetail SET ProductName=@ProductName,Rate=@Rate,Quantity=@Quantity,TotalAmount=@TotalAmount WHERE ProductName = @Name";
                    SqlCommand sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.AddWithValue("@ProductName", (GridView2.Rows[e.RowIndex].FindControl("txtProductName") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Rate", (GridView2.Rows[e.RowIndex].FindControl("txtRate") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Quantity", (GridView2.Rows[e.RowIndex].FindControl("txtQuantity") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@TotalAmount", (GridView2.Rows[e.RowIndex].FindControl("txtTotalQuantity") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Name", Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    conn.Close();
                    GridView2.EditIndex = -1;
                    PopulateGridview();
                    lblSuccessMessage.Text = "Selected Record Updated";
                    lblErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM dbo.OrderDetail WHERE ProductName = @Name";
                    SqlCommand sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.AddWithValue("@Name", GridView2.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    conn.Close();
                    PopulateGridview();
                    lblSuccessMessage.Text = "Selected Record Deleted";
                    lblErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }
        }
        /*if (this.GridView2.Rows.Count > 0 && Session["myDatatable"] != null)
        {
            DataTable dt = (DataTable)Session["myDatatable"];
            dt.Clear();
            foreach (GridViewRow row in this.GridView2.Rows)
            {
                string ProductName = (row.FindControl("ProductName") as DropDownList).SelectedItem.Text;
                string ProductID = (row.FindControl("ProductId") as TextBox).Text;
                string Rate = (row.FindControl("Rate") as TextBox).Text;
                string Quantity = (row.FindControl("quantity") as TextBox).Text;
                string TotalAmount = (row.FindControl("TotalAmount") as TextBox).Text;
                dt.Rows.Add(ProductName,ProductID,Rate,Quantity,TotalAmount);
                (row.FindControl("ProductName") as DropDownList).Items.FindByText(ProductName).Selected = true;
            }
            Session["myDatatable"] = dt;
        }

    }


    public void AddNewData()
    {
        DataTable dt = new DataTable();
        if (Session["myDatatable"] != null)
        {
            dt = (DataTable)Session["myDatatable"];
        }
        else
        {
            dt.Columns.Add("ProductName");
            dt.Columns.Add("ProductId");
            dt.Columns.Add("Rate");
            dt.Columns.Add("quantity");
            dt.Columns.Add("TotalAmount");
        }

        DataRow drow = dt.NewRow();
        drow["ProductName"] = "";
        drow["ProductId"] = "";
        drow["Rate"] = "";
        drow["quantity"] = "";
        drow["TotalAmount"] = "";
        dt.Rows.Add(drow);
        Session["myDatatable"] = dt;
    }

    public void BindGridview()
    {
        if (Session["myDatatable"] != null)
        {
            DataTable dt = (DataTable)Session["myDatatable"];

            if ((dt != null) && (dt.Rows.Count > 0))
            {
                GridView2.Visible = true;
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
            else
            {
                GridView2.Visible = false;
            }
        }
    }*/
        /*void DropDownList()
        {
            ListItem SelectItem = new ListItem("Select Customer", "-1");
            SelectItem.Selected = true;
            DropDownList1.Items.Insert(0, SelectItem);
        }*/






       // protected void Button1_Click(object sender, EventArgs e)
        //{
            //if (DropDownList1.SelectedValue == "-1")
            //{
             //   Response.Write("Please Select an Customer..");
            //}
            //else
            //{
                //string query = "select * from dbo.Customeer where CustomerName = @name";
                //SqlDataAdapter sd = new SqlDataAdapter(query, conn);
                //sd.SelectCommand.Parameters.AddWithValue("@name", DropDownList1.SelectedItem.Text);
                //DataTable dt = new DataTable();
                //sd.Fill(dt);
                //GridView1.DataSource = dt;
                //GridView1.DataBind();
                //DropDownList1.ItemType.Insert(0, "Select Customer");
                //Label1.Text = " One Row Found";
                //Label1.Visible = true;

           // }











            /* protected void Button1_Click(object sender, EventArgs e)
             {

             }

             void LoadRecord()
             {
                 SqlCommand comm = new SqlCommand("select * from dbo.Customeer ", conn);
                 SqlDataAdapter da = new SqlDataAdapter(comm);
                 DataTable dt = new DataTable();
                 da.Fill(dt);
                 GridView1.DataSource = dt;
                 GridView1.DataBind();
             }

             protected void Button3_Click(object sender, EventArgs e)
             {
                 // Creating instance of SqlCommand
                 SqlCommand comm = new SqlCommand("select * from dbo.Customeer Where CustomerID = '" + int.Parse(DropDownList1.Text) + "'", conn);
                 SqlDataAdapter d = new SqlDataAdapter(comm);
                 DataTable dt = new DataTable();
                 d.Fill(dt);
                 GridView1.DataSource = dt;
                 GridView1.DataBind();
             }

             protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
             {

             }

             /*protected void Button4_Click(object sender, EventArgs e)
             {
                 SqlCommand comm = new SqlCommand("select * from dbo.Product Where ProductID = '" + (DropDownList2.Text) + "'", conn);
                 SqlDataAdapter d = new SqlDataAdapter(comm);
                 DataTable dt = new DataTable();
                 d.Fill(dt);
                 GridView1.DataSource = dt;
                 GridView1.DataBind();

             }

             protected void DropDownList1_SelectedIndexChanged1(object sender, EventArgs e)
             {
                 // SqlCommand comm = new SqlCommand("select * from Customeer where CustomerName='" + this.CustomerName.SelectedValue + "'", conn);
                 //  SqlDataAdapter dataAadpter = new SqlDataAdapter(comm);
                 //  DataSet ds = new DataSet();
                 // dataAadpter.Fill(ds);
                 //ListView1.DataSource = ds;
                 /*if (ds != null)
                 {
                     LBEmailId.DataSource = ds.Tables[0];
                     LBEmailId.DataTextField = "ContactPerson_EmailID";
                     //LBEmailId.DataValueField = "Id";
                     LBEmailId.DataBind();
                 }*/
       // }

        protected void Button2_Click(object sender, EventArgs e)
        {
            GridView2.Visible = true;
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label1.Text = DropDownList1.SelectedItem.Text;
        }

        protected void txtProductNameFooter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList countryList = (DropDownList)sender;

            var ddlCountry = countryList.SelectedItem;
            TextBox txtCountry = (TextBox)(GridView2.HeaderRow.FindControl("txtRateFooter"));
            txtCountry.Text = ddlCountry.Value;
        }

        /* protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
         {
             conn.Open();
             sqlcmd.CommandText = "Select * from[Rate] Where ProductName=@ProductName and ProductId =@ProductId";
             //sqlcmd.Parameters.AddWithValue("@ProductName", D.SelectedValue);

         }*/

        /*private void BindList()
        {
            conn.Open();
            sqlcmd.CommandText = "select * from dbo.Product";
            sqlcmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(ds);
            conn.Close();
            DropDownList4.DataSource = ds;
            DropDownList4.DataTextField = "ProductName";
            DropDownList4.DataBind();

            DropDownList4.Items.Insert(0, "Select ProductName");

        }*/
        /* private void GridInfo()
         {

             DataTable dt = new DataTable();
             DataRow dr;
             dt.Columns.Add(new System.Data.DataColumn("ProductName", typeof(String)));
             dt.Columns.Add(new System.Data.DataColumn("ProductID", typeof(String)));
             dt.Columns.Add(new System.Data.DataColumn("Rate", typeof(String)));
             dt.Columns.Add(new System.Data.DataColumn("Quantity", typeof(String)));
             dt.Columns.Add(new System.Data.DataColumn("TotalAmount", typeof(String)));

             foreach (GridViewRow row in GridView2.Rows)
             {
                 TextBox ProductName = (TextBox)row.FindControl("txtProductName");
                 TextBox ProductID = (TextBox)row.FindControl("txtProductID");
                 TextBox Rate = (TextBox)row.FindControl("txtRate");
                 TextBox Quantity = (TextBox)row.FindControl("txtQuantity");
                 TextBox TotalAmount = (TextBox)row.FindControl("txtTotalAmount");
                 dr = dt.NewRow();
                 dr[0] = ProductName.Text;
                 dr[1] = ProductID.Text;
                 dr[2] = Rate.Text;
                 dr[3] = Quantity.Text;
                 dr[4] = TotalAmount.Text;
                 dt.Rows.Add(dr);
             }

             Session["QtyTable"] = dt;
             //Response.Redirect("Admin/Default.aspx");
         }*/


    }
}

