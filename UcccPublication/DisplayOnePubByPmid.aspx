<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="DisplayOnePubByPmid.aspx.cs" Inherits="DisplayOnePubByPmid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <link type="text/css" rel="Stylesheet" href="pubsite.css" />
    <asp:HiddenField ID="hdnPubCnt" runat="server" />
    <asp:HiddenField ID="hdnMemberCnt" runat="server" />
    <div id="body">
        <div class="fullblock" id="Div1" runat="server">
            <font color="maroon" size="2" style="font-weight:bold">
                    Publication with PMID: 
                <asp:Label ID="lblPmid" runat="server" Text=""></asp:Label><br /><br />
                </font>
        <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
            <div style="text-align:center">
                <br />
                <table style="width: 100%" id="tblPub" runat="server">
                    <tr>
                        <td align="right" valign="top" style="width:200px">
                            Publication:</td>
                        <td align="left">
                            <asp:Label ID="lblPublication" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Programmatic Symbol:</td>
                        <td align="left">
                            <asp:Label ID="lblProgrammatic" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Cited by:</td>
                        <td align="left">
                            <asp:Label ID="lblCitation" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Initial:</td>
                        <td align="left">
                            <asp:Label ID="lblInitConfirmed" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Address:</td>
                        <td align="left">
                            <asp:Label ID="lblAddressConfirmed" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        <div class="clear2column">
        </div>
        </div>
    </div>
</asp:Content>

