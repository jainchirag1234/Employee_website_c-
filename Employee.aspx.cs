using System;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee : System.Web.UI.Page
{
    string conStr = "Data Source=LAPTOP-KH2JGH1C\\chirag;Initial Catalog=Dept100;User Id=sa;Password=sa123; Integrated Security=true";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDepartments();
            LoadDesignations();
        }
    }

    private void LoadDepartments()
    {
        using (SqlConnection con = new SqlConnection(conStr))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Department", con);
            ddlDepartment.DataSource = cmd.ExecuteReader();
            ddlDepartment.DataTextField = "dept_name";
            ddlDepartment.DataValueField = "dept_id";
            ddlDepartment.DataBind();
        }
    }

    private void LoadDesignations()
    {
        using (SqlConnection con = new SqlConnection(conStr))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Designation", con);
            ddlDesignation.DataSource = cmd.ExecuteReader();
            ddlDesignation.DataTextField = "desg_name";
            ddlDesignation.DataValueField = "desg_id";
            ddlDesignation.DataBind();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dob;
            if (!DateTime.TryParse(txtDOB.Text, out dob))
            {
                lblMessage.Text = "❗ Enter a valid Date of Birth.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            double salary;
            if (!double.TryParse(txtSalary.Text, out salary))
            {
                lblMessage.Text = "❗ Enter a valid Salary.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (fileCV.HasFile)
            {
                string extension = Path.GetExtension(fileCV.FileName).ToLower();
                if (extension == ".pdf")
                {
                    // Generate unique filename
                    string filename = Path.GetFileNameWithoutExtension(fileCV.FileName)
                                      + "_" + Guid.NewGuid() + ".pdf";

                    // Save file in a web-accessible folder
                    string folderPath = Server.MapPath("~/CVs/");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string filePath = Path.Combine(folderPath, filename);
                    fileCV.SaveAs(filePath);

                    // Save relative path to database
                    string cvRelativePath = "~/CVs/" + filename;
                      
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        string query = @"INSERT INTO Employee 
                             (name, dob, designation, dept, salary, cv_path)
                             VALUES (@name, @dob, @designation, @dept, @salary, @cv)";

                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@dob", dob);
                        cmd.Parameters.AddWithValue("@designation", ddlDesignation.SelectedValue);
                        cmd.Parameters.AddWithValue("@dept", ddlDepartment.SelectedValue);
                        cmd.Parameters.AddWithValue("@salary", salary);
                        cmd.Parameters.AddWithValue("@cv", cvRelativePath); // store relative path

                        cmd.ExecuteNonQuery();
                        lblMessage.Text = "✅ Employee Registered Successfully!";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                }
                else
                {
                    lblMessage.Text = "❌ Only PDF files are allowed!";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }


        }
        catch (Exception ex)
        {
            lblMessage.Text = "❌ Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int empno;
            if (!int.TryParse(txtno.Text, out empno))
            {
                lblMessage.Text = "❗ Enter a valid Employee Number.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            double salary;
            if (!double.TryParse(txtSalary.Text, out salary))
            {
                lblMessage.Text = "❗ Enter a valid Salary.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = "UPDATE Employee SET dept = @dept, designation = @designation, salary = @salary WHERE empno = @empno";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@dept", ddlDepartment.SelectedValue);
                cmd.Parameters.AddWithValue("@designation", ddlDesignation.SelectedValue);
                cmd.Parameters.AddWithValue("@salary", salary);
                cmd.Parameters.AddWithValue("@empno", empno);

                int rows = cmd.ExecuteNonQuery();

                lblMessage.Text = (rows > 0) ? "✅ Employee updated successfully." : "❌ Update failed.";
                lblMessage.ForeColor = (rows > 0) ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "❌ Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Employee WHERE DATEDIFF(YEAR, dob, GETDATE()) > 58", con);
                int rows = cmd.ExecuteNonQuery();
                lblMessage.Text = rows + " employee(s) deleted who are older than 58.";
                lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "❌ Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string query = @"SELECT e.empno, e.name, d.dept_name, g.desg_name, e.dob, e.salary, e.cv_path 
                         FROM Employee e
                         INNER JOIN Department d ON e.dept = d.dept_id
                         INNER JOIN Designation g ON e.designation = g.desg_id
                         WHERE 1=1";

        using (SqlConnection con = new SqlConnection(conStr))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            decimal salary;
            if (decimal.TryParse(txtSalarySearch.Text, out salary))
            {
                query += " AND e.salary > @salary";
                cmd.Parameters.AddWithValue("@salary", salary);
            }

            if (!string.IsNullOrEmpty(txtDesignationSearch.Text))
            {
                query += " AND g.desg_name LIKE @desg";
                cmd.Parameters.AddWithValue("@desg", "%" + txtDesignationSearch.Text + "%");
            }

            cmd.CommandText = query;

            try
            {
                con.Open();
                GridView1.DataSource = cmd.ExecuteReader();
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Error: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        {
            string query = @"SELECT e.empno, 
                                e.name, 
                                d.dept_name, 
                                g.desg_name, 
                                e.dob, 
                                e.salary, 
                                e.cv_path
                         FROM Employee e
                         INNER JOIN Department d ON e.dept = d.dept_id
                         INNER JOIN Designation g ON e.designation = g.desg_id";

            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}


