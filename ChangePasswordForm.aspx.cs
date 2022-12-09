using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.HttpRequest;
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


namespace Employee_Management_System
{
    public partial class ChangePasswordForm : System.Web.UI.Page
    {
        SqlDataAdapter sda;
        static string cs = "Data Source=10.10.11.146;Initial Catalog=Employee_Managemnt_System;User ID=sa;Password=sqlserver@1";
        SqlConnection con = new SqlConnection(cs);
        protected void Page_Load(object sender, EventArgs e)
        {
           
            txt_email.Text = Request.QueryString["email"];    
        }

        protected void btnsubmit_Click(object sender, System.EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(cs))
            {

                SqlCommand cmd = new SqlCommand("usp_updateemplyee_password", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", txt_email.Text);
                cmd.Parameters.AddWithValue("@firstname", txt_fname.Text);
                cmd.Parameters.AddWithValue("@lastname", txt_lname.Text);
                cmd.Parameters.AddWithValue("@password", txt_newpassword.Text);
                con.Open();
                int res=cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    Response.Write("  <script>alert('Password Updated successfully.. !!')</script>  ");
                    Response.Redirect("LoginForm.aspx");
                }
                else
                {
                    Response.Write("  <script>alert('Please Enter Valid Information .. !!')</script>  ");
                }
            }
                
        }
    }
}