 <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Employee.aspx.cs" Inherits="Employee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    Employee Management System
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
  
    <div>
    <asp:TextBox ID="txtno" runat="server" Placeholder="Enter No" ReadOnly="True"></asp:TextBox>
        <br />
        <br />
    <asp:TextBox ID="txtName" runat="server" Placeholder="Enter Name"></asp:TextBox>
        <br />
        <br />
<asp:TextBox ID="txtDOB" runat="server" TextMode="Date"></asp:TextBox>
        <br />
        <br />

<asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
        <br />
        <br />
<asp:DropDownList ID="ddlDesignation" runat="server"></asp:DropDownList>
        <br />
        <br />

<asp:TextBox ID="txtSalary" runat="server" Placeholder="Enter Salary"></asp:TextBox>
        <br />
        <br />
<asp:FileUpload ID="fileCV" runat="server" />
        <br />
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" style="height: 29px" Text="Load" />
        <br /> <br /><asp:Button ID="Button1" runat="server" Text="Submit" OnClick="btnSubmit_Click" />

<%--<asp:Button ID="btnSubmit" runat="server" Text="Register Employee" OnClick="btnSubmit_Click1"  />
    --%>    <br />
        <br />
        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
        <br />
        <br />
        <asp:Button ID="Delete" runat="server" Text="Delete" OnClick="Delete_Click" />
        <br />
        <br />
         
        <br />
        <br />
<asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
        <asp:TextBox ID="txtSalarySearch" runat="server" />
<asp:TextBox ID="txtDesignationSearch" runat="server" />
<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />

<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="empno" HeaderText="Employee No" SortExpression="empno" />
        <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" />
        <asp:BoundField DataField="dept_name" HeaderText="Department" SortExpression="dept_name" />
        <asp:BoundField DataField="desg_name" HeaderText="Designation" SortExpression="desg_name" />
        <asp:BoundField DataField="dob" HeaderText="Date of Birth" SortExpression="dob" DataFormatString="{0:dd-MM-yyyy}" />
        <asp:BoundField DataField="salary" HeaderText="Salary" SortExpression="salary" DataFormatString="{0:C}" />

        <asp:TemplateField HeaderText="CV">
    <ItemTemplate>
       <asp:HyperLink ID="lnkCV" runat="server"
    NavigateUrl='<%# string.IsNullOrEmpty(Eval("cv_path").ToString()) ? "#" : ResolveUrl(Eval("cv_path").ToString()) %>'
    Text='<%# string.IsNullOrEmpty(Eval("cv_path").ToString()) ? "No CV" : "View CV" %>'
    Target="_blank" />


    </ItemTemplate>
</asp:TemplateField>

    </Columns>
</asp:GridView>

</asp:Content>






