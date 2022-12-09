using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace Employee_Management_System
{
    public partial class RegistrationForm : System.Web.UI.Page
    {
        string cs = "Data Source=10.10.11.146;Initial Catalog=Employee_Managemnt_System;User ID=sa;Password=sqlserver@1";
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (validateRegistrationForm())
                {


                    insertData();
                    lbl_register.Text = "User Successfully Register..!";
                    lbl_register.Font.Bold = true;
                    lbl_register.ForeColor = System.Drawing.Color.Green;
                    emptyFields();
                    Response.Redirect("LoginForm.aspx");
                }
                else
                {
                    lbl_register.Text = "";
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }


       

        //insert data into database
        private void insertData()
        {
           


            //mariatal status radio button
            string mariatalstatus = string.Empty;
            if (rb_single.Checked)
            {
                 mariatalstatus = "Single";
            }
            else if (rb_married.Checked)
            {
                 mariatalstatus = "Married";
            }



            //photograph upload button
            //int length = FileUpload1.PostedFile.ContentLength;
            //byte[] pic = new byte[length];

            HttpPostedFile postedFile = FileUpload1.PostedFile;
            string filename = Path.GetFileName(postedFile.FileName);
            string fileExtension = Path.GetExtension(filename);
            int fileSize = postedFile.ContentLength;

            Stream stream = postedFile.InputStream;
            BinaryReader binaryReader = new BinaryReader(stream);
            Byte[] bytes = binaryReader.ReadBytes((int)stream.Length);




            //data insertion           

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("usp_insertemployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@firstname", txt_fname.Text);
                cmd.Parameters.AddWithValue("@lastname", txt_lname.Text);
                cmd.Parameters.AddWithValue("@mobileno", txt_mobile.Text);
                cmd.Parameters.AddWithValue("@address", txt_address.Text);
                cmd.Parameters.AddWithValue("@email", txt_email.Text);
                cmd.Parameters.AddWithValue("@gender", dd_gender.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@dob", DateTime.ParseExact(txt_dtp.Text, "dd/MM/yyyy", null));//Convert.ToDateTime(txt_dtp.Text));
                cmd.Parameters.AddWithValue("@mariatalstatus", mariatalstatus);
                cmd.Parameters.AddWithValue("@password", txt_password.Text);
                cmd.Parameters.AddWithValue("@photograph", FileUpload1.PostedFile.FileName);
                cmd.Parameters.AddWithValue("@qualification", txt_qualification.Text);
                cmd.Parameters.AddWithValue("@skills", txt_skills.Text);
                cmd.Parameters.AddWithValue("@experience", txt_experience.Text);

           
                cmd.Parameters.AddWithValue("@myphoto", bytes);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Write("Data Inserted Successfully..!");
            }
        }

        

       
        private bool validateRegistrationForm()
        {


            //Birth date validation
            //bool ret = true;
            //if (string.IsNullOrEmpty(txt_dtp.Text))
            //{
            //    ret = false;
            //    lbl_birth.Text = "Please select Date..!";
            //}
            //else if (txt_dtp))
            //{
            //    ret = false;
            //    lbl_birth.Text = "Please Select Valid Date..!";
            //}

            //else
            //{
            //    lbl_birth.Text = "";
            //}
            //first name
            bool ret = true;
            if (string.IsNullOrEmpty(txt_fname.Text))
            {
                ret = false;
                lbl_fname.Text = "First Name Required..!";
            }
            else if  (!Regex.IsMatch(txt_fname.Text, @"^[\p{L}\p{M}' \.\-]+$"))
            {
                ret = false;
                lbl_fname.Text = "Alphabtes Are Allowed";
            }

            else
            {
                lbl_fname.Text = "";
            }
            //last name
            if (string.IsNullOrEmpty(txt_lname.Text))
            {
                ret = false;
                lbl_lname.Text = "Last Name Required..!";
            }
            else if  (!Regex.IsMatch(txt_lname.Text, @"^[\p{L}\p{M}' \.\-]+$"))
            {
                ret = false;
                lbl_lname.Text = "Alphabtes Are Allowed";
            }
            else
            {
                lbl_lname.Text = "";
            }
            //Email
            if (string.IsNullOrEmpty(txt_email.Text))
            {
                ret = false;
                lbl_email.Text = "Email Required..!";
            }
            else if (!Regex.IsMatch(txt_email.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                ret = false;
                lbl_email.Text = "Invalid Email Address";
            }
               
            else
            {
                lbl_email.Text = "";
            }

            //////////////////////////////////////////////////////////mobile no.

            if (string.IsNullOrEmpty(txt_mobile.Text))
            {
                ret = false;
                lbl_mobile.Text = "Mobile No. Required..!";
            }
            else if (!Regex.IsMatch(txt_mobile.Text, @"\+?[0-9]{10}"))
            {
                ret = false;
                lbl_mobile.Text = "Only Digits Allowed";
            }    
            else
            {
                lbl_mobile.Text = "";
            }
            /////////////////address
            if (string.IsNullOrEmpty(txt_address.Text))
            {
                ret = false;
                lbl_address.Text = "Address Required..!";
            }
            else if (!Regex.IsMatch(txt_address.Text, @"^[\p{L}\p{M}' \.\-]+$"))
            {
                ret = false;
                lbl_address.Text = "Alphabtes Are Aloowed";
            }

            else
            {
                lbl_address.Text = "";
            }////////////////////gender
            if (string.IsNullOrEmpty(dd_gender.Text))
            {
                ret = false;
                lbl_gender.Text = "Gender Not Selected..!";
            }
            else
            {
                lbl_gender.Text = "";
            }
            //////////////////////Dob
            if (string.IsNullOrEmpty(txt_dtp.Text))
            {
                ret = false;
                lbl_dob.Text = "DOB Required..!";
            }
            else
            {
                lbl_dob.Text = "";
            }
            /////////////////////////password

            if (string.IsNullOrEmpty(txt_password.Text))
            {
                ret = false;
                lbl_password.Text = "Password Required..!";
            }
            else
            {
                lbl_password.Text = "";
            }
            /////////////mariatal status
            if (rb_single.Checked)
            {

                lbl_mariatalstatus.Text = "";
            }
            else if (rb_married.Checked)
            {
                lbl_mariatalstatus.Text = "";
            }
            else
            {
                ret = false;
                lbl_mariatalstatus.Text = "Select Mariatal status..!";
            }
            /////////////qualification
            if (string.IsNullOrEmpty(txt_qualification.Text))
            {
                ret = false;
                lbl_qualification.Text = "Field Required..!";
            }
            else if (!Regex.IsMatch(txt_qualification.Text, @"^[\p{L}\p{M}' \.\-]+$"))
            {
                ret = false;
                lbl_qualification.Text = "Alphabets Are Allowed";
            }
            else
            {
                lbl_qualification.Text = "";
            }
            ///////////////skills
            if (string.IsNullOrEmpty(txt_skills.Text))
            {
                ret = false;
                lbl_skills.Text = "Field Required..!";
            }
            else if (!Regex.IsMatch(txt_skills.Text, @"^[\p{L}\p{M}' \.\-]+$"))
            {
                ret = false;
                lbl_skills.Text = "Alphabets Are Allowed";
            }
            else
            {
                lbl_skills.Text = "";
            }
            ///////expperience
            if (string.IsNullOrEmpty(txt_experience.Text))
            {
                ret = false;
                lbl_experience.Text = "Field Required..!";
            }
            else if (!Regex.IsMatch(txt_experience.Text, @"\+?[0-9]"))
            {
                ret = false;
                lbl_experience.Text = "Only Number Allowed";
            }    
            else
            {
                lbl_experience.Text = "";
            }
            //////////upload photograph
            if (!FileUpload1.HasFile)
            {
                ret = false;
                lbl_Upload.Text = "Please Select The File";
                lbl_Upload.ForeColor = System.Drawing.Color.Red;
                //FileUpload1.SaveAs(Server.MapPath("~/Uploads/" + FileUpload1.FileName));
                //lbl_Upload.Text = "File Uploaded";
                //lbl_Upload.ForeColor = System.Drawing.Color.Green;

            }
            else
            {
                lbl_Upload.Text = "";
            }
            return ret;
        }

        private void emptyFields()
        {
            txt_address.Text = "";
            txt_dtp.Text = "";
            txt_email.Text = "";
            txt_experience.Text = "";
            txt_fname.Text = "";
            txt_lname.Text = "";
            txt_mobile.Text = "";
            txt_password.Text = "";
            txt_qualification.Text = "";
            txt_skills.Text = "";
            dd_gender.SelectedIndex = 0;
            rb_married.Checked = false;
            rb_single.Checked = false;
        }


    }
}