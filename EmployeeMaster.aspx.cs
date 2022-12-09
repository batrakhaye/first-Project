using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.SqlDataSource;
//using System.Web.UI.WebControls.GridView;
using System.Data.SqlClient;
//using System.Data.SqlClient.SqlConnection;
//using System.Web.UI.Page;
using System.Data;

namespace Employee_Management_System
{
    public partial class EmployeeMaster : System.Web.UI.Page
    {

        
        //Persist Security Info=True
        int res=0;
        static string con =  "Data Source=10.10.11.146;Initial Catalog=Employee_Managemnt_System;User ID=sa;Password=sqlserver@1";
        SqlConnection db = new SqlConnection(con);
        protected void Page_Load(object sender, EventArgs e)
        {
                HideButton(); 
          
            if (db.State == ConnectionState.Open)
            {
                db.Close();
            }
            db.Open();
            if (!IsPostBack)
            {
                DisplayRecord();
            }
        
        }
        void HideButton()
        {
            lbl_delete.Visible = false;
            lbl_update.Visible = false;
            lbl_delete.Visible = false;
            warning.Visible = false;
            btn_update.Visible = false;
            btn_delete.Visible = false;
            btn_create.Visible = false;
        }

        protected void btn_create_Click(object sender, EventArgs e)
        {
            try
            {            
                    string insert = "insert into EmployeeMasterDB1(EmployeeID,EmployeeName,EmployeeDesignation,EmployeeSalary) values ('" + txt_EmployeeID.Text + "','" + txt_name.Text + "','" + txt_designation.Text + "','" + txt_salary.Text + "')";
                    SqlCommand cmd = new SqlCommand(insert, db);
                     res = cmd.ExecuteNonQuery();
               
                    if (res != 0)
                    {
                        lbl_insert.Visible = true;
                        //Response.Write("  <script>alert('Data Inserted successfully.. !!')</script>  ");
                    }
                    DisplayRecord();
                    EmptyField();
            }
            catch (Exception ex)
            {
                //EmpMasterValidation();
        
                        
                    warning.Visible = true;
                //Response.Write("<script>alert('Employee Id Already Exist ....!')</script>");
            }
            
        }
        
      

        void EmptyField()
        {
            txt_EmployeeID.Text = "";
            txt_name.Text = "";
            txt_designation.Text = "";
            txt_salary.Text = "";

        }
        void SearchRecord()
        {
            try
            {
                //SqlCommand cmd = new SqlCommand("select  '" + txt_name.Text + "'=EmployeeName,'" + txt_designation.Text + "'=EmployeeDesignamtion,'" + txt_salary.Text + "'=EmployeeSalary from EmployeeMasterDB  where EmployeeID='" + txt_EmployeeID.Text + "');", db);
                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //sda.Fill(dt);
                SqlCommand Comm1 = new SqlCommand("select * from EmployeeMasterDB1  where EmployeeID='" + txt_EmployeeID.Text + "' ", db);
                SqlDataReader DR1 = Comm1.ExecuteReader();
                if (DR1.Read())
                {
                    txt_EmployeeID.Text = DR1.GetValue(0).ToString();
                    txt_name.Text = DR1.GetValue(1).ToString();
                    txt_designation.Text = DR1.GetValue(2).ToString();
                    txt_salary.Text = DR1.GetValue(3).ToString();
                }
                else 
                {
                    Response.Write("<script>alert('No Record Available... !!')</script> ");      
                }
            }
            catch
            {
                Response.Write("<script>alert('Exception Occured... !!')</script> ");            
            }
        }

        void SearchByAny()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from EmployeeMasterDB1 where   EmployeeDesignation like '" + txt_searchall.Text + "%' or EmployeeName like '" + txt_searchall.Text + "%' or EmployeeSalary like '" + txt_searchall.Text + "%'  order by EmployeeID asc", db);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        void DisplayRecord()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from EmployeeMasterDB1 order by EmployeeID asc", db);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected void btn_display_Click(object sender, EventArgs e)
        {
            try
            {
                // DisplayRecord();
                SearchRecord();

            }
            catch (Exception ex)
            { 
                Response.Write("<script>alert('Exception Occured... !!')</script> ");   
            }
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            try
            {


                string insert = ("update EmployeeMasterDB1 set EmployeeName='" + txt_name.Text + "',EmployeeDesignation='" + txt_designation.Text + "',EmployeeSalary='" + txt_salary.Text + "' where EmployeeID='" + txt_EmployeeID.Text + "'");
                SqlCommand cmd = new SqlCommand(insert, db);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    lbl_update.Visible = true;
                    //Response.Write("  <script>alert('Data Updated Successfully... !!')</script>  ");
                }


            }
            catch (Exception ex)
            {
               // EmpMasterValidation();
                Response.Write("<script>alert('Excption Occured... !!')</script>");
            }
            EmptyField();

            DisplayRecord();
        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {


                string insert = ("delete EmployeeMasterDB1  where EmployeeID='" + txt_EmployeeID.Text + "'");
                SqlCommand cmd = new SqlCommand(insert, db);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    lbl_delete.Visible = true;
                    //Response.Write("  <script>alert('Data Deleted Successfully... !!')</script>  ");
                }


                EmptyField();
                DisplayRecord();
                
            }
            catch (Exception ex)
            {
            //    EmpMasterValidation();
                Response.Write("<script>alert('Excption Occured... !!')</script>");
            
               
            }
        }

        protected void btn_LogOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("LoginForm.aspx");
          
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // DisplayRecord();
            HideButton();
            btn_update.Visible = true;
            btn_delete.Visible = true;
            btn_create.Visible = false;
            GridViewRow row = GridView1.SelectedRow;

            int EmployeeID = Convert.ToInt32((sender as LinkButton).CommandArgument);
            try
            {
                //SqlCommand cmd = new SqlCommand("select  '" + txt_name.Text + "'=EmployeeName,'" + txt_designation.Text + "'=EmployeeDesignamtion,'" + txt_salary.Text + "'=EmployeeSalary from EmployeeMasterDB  where EmployeeID='" + txt_EmployeeID.Text + "');", db);
                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //sda.Fill(dt);
                SqlCommand Comm1 = new SqlCommand("select * from EmployeeMasterDB1  where EmployeeID='" + EmployeeID + "' ", db);
                SqlDataReader DR1 = Comm1.ExecuteReader();
                if (DR1.Read())
                {
                    txt_EmployeeID.Text = DR1.GetValue(0).ToString();
                    txt_name.Text = DR1.GetValue(1).ToString();
                    txt_designation.Text = DR1.GetValue(2).ToString();
                    txt_salary.Text = DR1.GetValue(3).ToString();
                }
                else
                {
                    Response.Write("<script>alert('No Record Available... !!')</script> ");
                }
            }
            catch
            {
                Response.Write("<script>alert('Exception Occured... !!')</script> ");
            }


           

        }

        protected void btn_addnew_Click(object sender, EventArgs e)
        {
                HideButton();
                lbl_insert.Visible = false;
                btn_create.Visible = true;
              
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            btn_update.Visible = false;
            btn_delete.Visible = false;
            btn_create.Visible = false;
            lbl_insert.Visible = false;
            DisplayRecord();
            EmptyField();
           
           
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            DisplayRecord();
            SearchByAny();
        }

        protected void btn_searchall_Click(object sender, EventArgs e)
        {
            try
            {
            SearchByAny();

            }
            catch(Exception ex)
                {
                    Response.Write("<script>alert('Exception Occured... !!')</script> ");
                }
            }
    

        
    }
}