using System;
using System.Web.UI.WebControls;

namespace OrganMapSite
{
    public class RolloverButton : Button
    {

        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute("onmouseover", "this.className='buttonsmall-ovr';");
            writer.AddAttribute("onmouseout", "this.className='buttonsmall';");
            writer.AddAttribute("class", "buttonsmall");
        }
    }

    public class RolloverLink : Button
    {

        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute("onmouseover", "this.className='buttonsmall-ovr'");
            writer.AddAttribute("onmouseout", "this.className='buttonsmall'");
            writer.AddAttribute("class", "buttonsmall");
            string navurl = NavigateURL;
            if ((base.OnClientClick == "" & navurl != ""))
            {
                writer.AddAttribute("onclick", "window.navigate('" + navurl + "');");
            }
        }

        protected override void OnClick(System.EventArgs e)
        {
            base.OnClick(e);
            string navurl = NavigateURL;
            if (navurl != "")
            {
                Page.Response.Redirect(NavigateURL);
            }
        }

        public string NavigateURL
        {
            get
            {

                object u = ViewState["NavigateURL"];
                return (u != null) ? System.Convert.ToString(u) : string.Empty;
            }
            set
            {
                ViewState["NavigateURL"] = value;
            }
        }
    }
}