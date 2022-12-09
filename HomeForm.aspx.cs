using System.IO;  
using System.Data;  
using System.Data.SqlClient;  
using System.Configuration;  
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Web;  
using System.Web.UI;  
using System.Web.UI.WebControls;  
using System.IO;  
using System.Data;  
using System.Data.SqlClient;  
using System.Configuration;
using System.Text.RegularExpressions;


namespace Employee_Management_System
{
    public partial class HomeForm : System.Web.UI.Page
    {
      
        SqlDataAdapter sda;
        static string cs = "Data Source=10.10.11.146;Initial Catalog=Employee_Managemnt_System;User ID=sa;Password=sqlserver@1";
        SqlConnection con = new SqlConnection(cs);
        protected void Page_Load(object sender, EventArgs e)
        {
            




            
            if (!IsPostBack)
            {
              //  homeformPage();
                DataTable dt = this.BindMenuData(0);
                DynamicMenuControlPopulation(dt, 0, null);
                userDataBind();

                //user profile



            }
           // emptyFields();
        }


        //page indexing
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            userDataBind();
            
        }

        private void userDataBind()
        {
            ////
         
            try
            {
                SqlCommand cmd = new SqlCommand("usp_displayAllEmployeeRecords", con);
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


       

          



        protected DataTable BindMenuData(int parentmenuId)
        {
            string user = Request.QueryString["username"];
            string pass = Request.QueryString["password"];
            //declaration of variable used  
            DataSet ds = new DataSet();
            DataTable dt;
            DataRow dr;
            DataColumn menu;
            DataColumn pMenu;
            DataColumn title;
            DataColumn description;
            DataColumn URL;

            //create an object of datatable  
            dt = new DataTable();

            //creating column of datatable with datatype  
            menu = new DataColumn("MenuId", Type.GetType("System.Int32"));
            pMenu = new DataColumn("ParentId", Type.GetType("System.Int32"));
            title = new DataColumn("Title", Type.GetType("System.String"));
            description = new DataColumn("Description", Type.GetType("System.String"));
            URL = new DataColumn("URL", Type.GetType("System.String"));

            //bind data table columns in datatable  
            dt.Columns.Add(menu);//1st column  
            dt.Columns.Add(pMenu);//2nd column  
            dt.Columns.Add(title);//3rd column  
            dt.Columns.Add(description);//4th column  
            dt.Columns.Add(URL);//5th column  

            //creating data row and assiging the value to columns of datatable  
            //1st row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 1;
            dr["ParentId"] = 0;
            dr["Title"] = "Home";
            dr["Description"] = "";
            dr["URL"] = "~/HomeForm.aspx";
            dt.Rows.Add(dr);

            ////2nd row of data table  
            //dr = dt.NewRow();
            //dr["MenuId"] = 2;
            //dr["ParentId"] = 1;
            //dr["Title"] = "Profile";
            //dr["Description"] = "Profile";
            //dr["URL"] = "~/UserProfileForm.aspx";
            //dt.Rows.Add(dr);

            //3rd row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 3;
            dr["ParentId"] = 0;
            dr["Title"] = "About Us";
            dr["Description"] = "About us page";
            dr["URL"] = "~/AboutUsForm.aspx";
            dt.Rows.Add(dr);

            //4th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 4;
            dr["ParentId"] = 0;
            dr["Title"] = "FeedBack";
            dr["Description"] = "Contact Us page";
            dr["URL"] = "~/FeedBackForm.aspx";
            dt.Rows.Add(dr);

            //5th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 5;
            dr["ParentId"] = 2;
            dr["Title"] = "User Profile";
            dr["Description"] = "User Details";
            dr["URL"] = "~/UserProfileForm.aspx";
            dt.Rows.Add(dr);

            //6th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 6;
            dr["ParentId"] = 2;
            dr["Title"] = "Change Password";
            dr["Description"] = "change password Menu";
            dr["URL"] = ("");
            dt.Rows.Add(dr);

            //7th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 7;
            dr["ParentId"] = 2;
            dr["Title"] = "Emergency Contact";
            dr["Description"] = "Emergency Contact page";
            dr["URL"] = "~/EmergencyContactForm.aspx";
            dt.Rows.Add(dr);

            //8th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 8;
            dr["ParentId"] = 1;
            dr["Title"] = "Add User";
            dr["Description"] = "New User Add";
            dr["URL"] = "~/AddUser.aspx";
            dt.Rows.Add(dr);

            ////9th row of data table  
            //dr = dt.NewRow();
            //dr["MenuId"] = 9;
            //dr["ParentId"] = 7;
            //dr["Title"] = "International";
            //dr["Description"] = "International outsourcing page";
            //dr["URL"] = "~/International.aspx";
            //dt.Rows.Add(dr);

            ds.Tables.Add(dt);
            var dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "ParentId='" + parentmenuId + "'";
            DataSet ds1 = new DataSet();
            var newdt = dv.ToTable();
            return newdt;
        
        }

        /// <summary>  
        /// This is a recursive function to fetchout the data to create a menu from data table  
        /// </summary>  
        /// <param name="dt">datatable</param>  
        /// <param name="parentMenuId">parent menu Id of integer type</param>  
        /// <param name="parentMenuItem"> Menu Item control</param>  
        protected void DynamicMenuControlPopulation(DataTable dt, int parentMenuId, MenuItem parentMenuItem)
        {
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
            foreach (DataRow row in dt.Rows)
            {
                MenuItem menuItem = new MenuItem
                {
                    Value = row["MenuId"].ToString(),
                    Text = row["Title"].ToString(),
                    NavigateUrl = row["URL"].ToString(),
                    Selected = row["URL"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)
                };
                if (parentMenuId == 0)
                {
                    Menu1.Items.Add(menuItem);
                    DataTable dtChild = this.BindMenuData(int.Parse(menuItem.Value));
                    DynamicMenuControlPopulation(dtChild, int.Parse(menuItem.Value), menuItem);
                }
                else
                {

                    parentMenuItem.ChildItems.Add(menuItem);
                    DataTable dtChild = this.BindMenuData(int.Parse(menuItem.Value));
                    DynamicMenuControlPopulation(dtChild, int.Parse(menuItem.Value), menuItem);

                }
            }
        }

       


       //update records through data transfer by query string
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.SelectedRow;
                int empid = Convert.ToInt32((sender as LinkButton).CommandArgument);
                //Server.Transfer("~/AddUser.aspx");
                Response.Redirect("AddUser.aspx?empid="+empid);

            }
            catch (Exception ex)
            {
                Response.Write("error");
            }
        }

        protected void GridView1_SelectedIndexChanged1(object sender, EventArgs e)
        {
            
            GridViewRow row = GridView1.SelectedRow;
            int empid = Convert.ToInt32((sender as LinkButton).CommandArgument);
            //System.Windows.Forms.MessageBox.Show("Do You Want to Delete Record");
            
           
            SqlCommand Comm = new SqlCommand("usp_deleteemployee " + empid + "", con);
            con.Open();
            Comm.ExecuteNonQuery();
            userDataBind();
           // emptyFields();
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            Response.Redirect("LoginForm.aspx");
        }

        //private void emptyFields()
        //{
        //    lbl_update.Text = "";
        //    txt_address.Text = "";
        // //   txt_dtp.Text = "";
        //    txt_email.Text = "";
        //    txt_experience.Text = "";
        //    txt_fname.Text = "";
        //    txt_lname.Text = "";
        //    txt_mobile.Text = "";
        //    txt_password.Text = "";
        //    txt_qualification.Text = "";
        //    txt_skills.Text = "";
        //    //dd_gender.SelectedIndex = 0;
        //    //rb_married.Checked = false;
        //    //rb_single.Checked = false;
        //}

        //private bool validateRegistrationForm()
        //{
        //    //first name
        //    bool ret = true;
        //    if (string.IsNullOrEmpty(txt_fname.Text))
        //    {
        //        ret = false;
        //        lbl_fname.Text = "First Name Required..!";
        //    }
        //    else if (!Regex.IsMatch(txt_fname.Text, @"^[\p{L}\p{M}' \.\-]+$"))
        //    {
        //        ret = false;
        //        lbl_fname.Text = "Alphabtes Are Aloowed";
        //    }

        //    else
        //    {
        //        lbl_fname.Text = "";
        //    }
        //    //last name
        //    if (string.IsNullOrEmpty(txt_lname.Text))
        //    {
        //        ret = false;
        //        lbl_lname.Text = "Last Name Required..!";
        //    }
        //    else if (!Regex.IsMatch(txt_lname.Text, @"^[\p{L}\p{M}' \.\-]+$"))
        //    {
        //        ret = false;
        //        lbl_lname.Text = "Alphabtes Are Aloowed";
        //    }
        //    else
        //    {
        //        lbl_lname.Text = "";
        //    }
        //    //Email
        //    if (string.IsNullOrEmpty(txt_email.Text))
        //    {
        //        ret = false;
        //        lbl_email.Text = "Email Required..!";
        //    }
        //    else if (!Regex.IsMatch(txt_email.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
        //    {
        //        ret = false;
        //        lbl_email.Text = "Invalid Email Address";
        //    }

        //    else
        //    {
        //        lbl_email.Text = "";
        //    }

        //    //////////////////////////////////////////////////////////mobile no.

        //    if (string.IsNullOrEmpty(txt_mobile.Text))
        //    {
        //        ret = false;
        //        lbl_mobile.Text = "Mobile No. Required..!";
        //    }
        //    else if (!Regex.IsMatch(txt_mobile.Text, @"\+?[0-9]{10}"))
        //    {
        //        ret = false;
        //        lbl_mobile.Text = "Only Digits Allowed";
        //    }
        //    else
        //    {
        //        lbl_mobile.Text = "";
        //    }
        //    /////////////////address
        //    if (string.IsNullOrEmpty(txt_address.Text))
        //    {
        //        ret = false;
        //        lbl_address.Text = "Address Required..!";
        //    }
        //    else if (!Regex.IsMatch(txt_address.Text, @"^[\p{L}\p{M}' \.\-]+$"))
        //    {
        //        ret = false;
        //        lbl_address.Text = "Alphabtes Are Aloowed";
        //    }

        //    else
        //    {
        //        lbl_address.Text = "";
        //    }////////////////////gender
        //    //if (string.IsNullOrEmpty(dd_gender.Text))
        //    //{
        //    //    ret = false;
        //    //    lbl_gender.Text = "Gender Not Selected..!";
        //    //}
        //    //else
        //    //{
        //    //    lbl_gender.Text = "";
        //    //}
        //    ////////////////////////Dob
        //    //if (string.IsNullOrEmpty(txt_dtp.Text))
        //    //{
        //    //    ret = false;
        //    //    lbl_dob.Text = "DOB Required..!";
        //    //}
        //    //else
        //    //{
        //    //    lbl_dob.Text = "";
        //    //}
        //    /////////////////////////password

        //    if (string.IsNullOrEmpty(txt_password.Text))
        //    {
        //        ret = false;
        //        lbl_password.Text = "Password Required..!";
        //    }
        //    else
        //    {
        //        lbl_password.Text = "";
        //    }
        //    /////////////mariatal status
        //    //if (rb_single.Checked)
        //    //{

        //    //    lbl_mariatalstatus.Text = "";
        //    //}
        //    //else if (rb_married.Checked)
        //    //{
        //    //    lbl_mariatalstatus.Text = "";
        //    //}
        //    //else
        //    //{
        //    //    ret = false;
        //    //    lbl_mariatalstatus.Text = "Select Mariatal status..!";
        //    //}
        //    /////////////qualification
        //    if (string.IsNullOrEmpty(txt_qualification.Text))
        //    {
        //        ret = false;
        //        lbl_qualification.Text = "Field Required..!";
        //    }
        //    else if (!Regex.IsMatch(txt_qualification.Text, @"^[\p{L}\p{M}' \.\-]+$"))
        //    {
        //        ret = false;
        //        lbl_qualification.Text = "Alphabets Are Allowed";
        //    }
        //    else
        //    {
        //        lbl_qualification.Text = "";
        //    }
        //    ///////////////skills
        //    if (string.IsNullOrEmpty(txt_skills.Text))
        //    {
        //        ret = false;
        //        lbl_skills.Text = "Field Required..!";
        //    }
        //    else if (!Regex.IsMatch(txt_skills.Text, @"^[\p{L}\p{M}' \.\-\,]+$"))
        //    {
        //        ret = false;
        //        lbl_skills.Text = "Alphabets Are Allowed";
        //    }
        //    else
        //    {
        //        lbl_skills.Text = "";
        //    }
        //    ///////expperience
        //    if (string.IsNullOrEmpty(txt_experience.Text))
        //    {
        //        ret = false;
        //        lbl_experience.Text = "Field Required..!";
        //    }
        //    else if (!Regex.IsMatch(txt_experience.Text, @"\+?[0-9]"))
        //    {
        //        ret = false;
        //        lbl_experience.Text = "Only Number Allowed";
        //    }
        //    else
        //    {
        //        lbl_experience.Text = "";
        //    }
        //    //////////upload photograph
        //    //if (!FileUpload1.HasFile)
        //    //{
        //    //    ret = false;
        //    //    lbl_Upload.Text = "Please Select The File";
        //    //    lbl_Upload.ForeColor = System.Drawing.Color.Red;
        //    //    //FileUpload1.SaveAs(Server.MapPath("~/Uploads/" + FileUpload1.FileName));
        //    //    //lbl_Upload.Text = "File Uploaded";
        //    //    //lbl_Upload.ForeColor = System.Drawing.Color.Green;

        //    //}
        //    //else
        //    //{
        //    //    lbl_Upload.Text = "";
        //    //}
        //    return ret;
        //}
    }
}