<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="PmidCSVtoReport.aspx.cs" Inherits="PmidCSVtoReport" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
            <br />
            <table style="width: 100%" border="1">
                <tr valign="top">
                    <td align="left" style="height: 66px">
                        Start Date: <asp:TextBox id="txtStartDate" Runat="server" Width="100px" />
                        <br />
                        End Date:&nbsp;&nbsp; <asp:TextBox ID="txtEndDate" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                    <font color="maroon" size="2" style="font-weight:bold">
                    Report for PMIDs</font><br />
                        <br />
                        Open a csv file of PMIDs, the first row is the column name: PMID.<br />
                    <asp:FileUpload ID="fu" runat="server" />
                    <asp:Button ID="btnOpenCsv" runat="server" onclick="btnOpenCsv_Click" 
                           Text="Open PMID CSV File to Report" Width="300px" />

                    </td>
                </tr>
                </table>
    </div>
</asp:Content>

