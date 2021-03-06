﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>
<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
        <div id="columnright">
            <asp:Label runat="server" ForeColor="Red" ID="ErrorMessage"></asp:Label>                
            <div class="rightblock" id="inventoryDiv" runat="server">
                <div>
    <span style="font-size: 14pt"><font color="maroon" size="4" style="font-weight:bold">
        Change Password
        <br />
    </font></span>

                </div>
                <div class="inventorycrumbs">

            <table style="width: 218">
                <tr>
                    <td class="clsFormLabel" style="width: 120px">
                        User Name</td>
                    <td style="width: 120px">
                        <asp:TextBox ID="txtUserName" runat="server" Width="120px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="clsFormLabel" style="width: 120px">
                        Current Password</td>
                    <td style="width: 120px">
                        <asp:TextBox ID="txtOldPwd" TextMode="Password" runat="server" Width="120px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="clsFormLabel" style="width: 120px">
                        New Password</td>
                    <td style="width: 120px">
                        <asp:TextBox ID="txtNewPwd" TextMode="Password" runat="server" Width="120px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="clsFormLabel" style="width: 120px">
                        Confirm New Password</td>
                    <td style="width: 120px">
                        <asp:TextBox ID="txtConfirmPwd" TextMode="Password" runat="server" Width="120px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2" class="clsFormLabel" style="width: 120px">
                        <p>
                            <OrganMap:RolloverButton ID="btnChange" runat="server" OnCommand="btnChange_Click" Text="Change" />
                        </p>
                    </td>
                </tr>
            </table>

                </div>
                <!-- begin inventory item -->
            </div>
        </div>
    </div>
</asp:Content>

