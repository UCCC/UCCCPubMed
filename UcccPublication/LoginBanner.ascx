<%@ Control Language="C#" ClassName="LoginBanner" %>
<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>

<script runat="server">
    protected void Logoutbtn_Click(object sender, System.EventArgs e)
    {
        FormsAuthentication.SignOut();
    }
</script>

<div id="loginbanner">
    <asp:LoginView ID="LoginView1" runat="server">
        <LoggedInTemplate>
            <div style="float: left">
                <h2>
                    Welcome
                    <asp:LoginName ID="LoginName1" runat="server" />
                    . <strong>You are logged in.</strong>
                </h2>
            </div>
            <div class="actionbuttons">
                <OrganMap:RolloverButton ID="Logoutbtn" runat="server" Text="Logout" OnClick="Logoutbtn_Click" />
            </div>
        </LoggedInTemplate>
        <AnonymousTemplate>
            <div style="float: left">
                <h2>
                    Please login
                </h2>
            </div>
            <div class="actionbuttons">
                <OrganMap:RolloverLink ID="Logintbtn" runat="server" Text="Login" NavigateURL="default.aspx" />
                &nbsp;
                <OrganMap:RolloverLink ID="RegisterBtn" runat="server" Text="Register" NavigateURL="member_register.aspx" />
            </div>
        </AnonymousTemplate>
    </asp:LoginView>
</div>
