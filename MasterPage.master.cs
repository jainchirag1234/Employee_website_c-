using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

public partial class MasterPage : System.Web.UI.MasterPage
{
    //string conStr = "Data Source=LAPTOP-KH2JGH1C\\CHIRAG;Initial Catalog=Course;Integrated Security=True";

    protected void NavigationMenu_MenuItemClick(object sender, System.Web.UI.WebControls.MenuEventArgs e)
    {
        // Redirect to the selected item's URL

        string selectedUrl = e.Item.NavigateUrl;
        Response.Redirect(selectedUrl);

    }

}

