<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <table align="center">
                    <tr>
                        <td>
                            <br /><br /><br /><br />
                        </td>
                        <td>
                            <br /><br /><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" AssociatedControlID="UserName" ID="UserNameLabel">User name:</asp:Label></td>
                        <td>
                            <asp:TextBox runat="server" ID="UserName"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" ValidationGroup="Login1"
                                ErrorMessage="User Name is required." ToolTip="User Name is required." ID="UserNameRequired">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" AssociatedControlID="Password" ID="PasswordLabel">Password:</asp:Label></td>
                        <td>
                            <asp:TextBox runat="server" TextMode="Password" ID="Password"></asp:TextBox><br /><br />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" ValidationGroup="Login1"
                                ErrorMessage="Password is required." ToolTip="Password is required." ID="PasswordRequired">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <OrganMap:RolloverButton ID="LoginButton" runat="server" OnCommand="CommandBtn_Click" Text="Login" CommandName="Login" />
                        </td>
                    </tr>
                </table>
    
    </div>

</asp:Content>

