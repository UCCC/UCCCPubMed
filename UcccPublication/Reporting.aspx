<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Reporting.aspx.cs" Inherits="Reporting" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
        <font color="maroon" size="2" style="font-weight:bold">
                    Publication Report<br /><br />
                </font>
        <div id="fullblock">
            <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
            <br />
            <table style="width: 100%" border="1">
                <tr valign="top">
                    <td align="left" style="height: 66px">
                        <asp:CheckBox ID="chxInst" runat="server" Text="With Institution" />
                        <br />
                        <asp:CheckBox ID="chxAbstract" runat="server" Text="With Abstract" />
                        <br />
                        <asp:CheckBox ID="chxPmid" runat="server" 
                            Text="With PMID" />
                        <br />
                    </td>
                    <td align="left" style="height: 66px">
                        Start Date: <asp:TextBox id="txtStartDate" Runat="server" Width="100px" />
                        <br />
                        End Date:&nbsp;&nbsp; <asp:TextBox ID="txtEndDate" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td align="left">
                        <font color="maroon" size="2" style="font-weight:bold">
                        Publication Report for Program<br /><br />
                    </font>
                                <asp:DropDownList ID="ddlProgram" runat="server" Width="300px" 
                                    AutoPostBack="True" onselectedindexchanged="SelectProgram">
                                </asp:DropDownList><br /><br />
                                <asp:Button ID="btnReportProgramNon0FG" runat="server" 
                                    Text="Focus Groups Excluding 0" Width="300px" 
                                    onclick="btnReportProgramNon0FG_Click" />
                                <asp:Button ID="btnReportProgram0FG" runat="server" 
                                    onclick="btnReportProgram0FG_Click" 
                                     Text="Focus Group 0 Only" Width="300px" /><br /><br />
                    </td>
                    <td align="left">
                        <font color="maroon" size="2" style="font-weight:bold">
                        Publication Report for Member</font><br /><br />
                        <asp:DropDownList ID="ddlMember" runat="server" Width="300px" Visible="False">
                                    </asp:DropDownList>
                        <br />
                        <table style="width: 85%">
                            <tr>
                                <td style="width: 115px">
                                    <asp:ListBox ID="lbSource" runat="server" Height="160px" 
                                        SelectionMode="Multiple" Width="140px"></asp:ListBox>
                                </td>
                                <td style="width: 21px">
                                    <asp:Button ID="btnGo" runat="server" onclick="btnGo_Click" Text="&gt;" />
                                    <br />
                                    <asp:Button ID="btnBack" runat="server" onclick="btnBack_Click" Text="&lt;" />
                                </td>
                                <td>
                                    <asp:ListBox ID="lbDistination" runat="server" Height="160px" 
                                        SelectionMode="Multiple" Width="140px"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top" id="tdAll" runat="server">
                        <font color="maroon" size="2" style="font-weight:bold">
                        Report All<br /><br />
                    </font>
                        Report all publications for all programs 
                        and all members.<br />
                        <asp:Button ID="btnGetAllPublication" runat="server" 
                         Text="Report All" 
                        onclick="btnGetAllPublication_Click" Width="300px" Visible="False" />
                    </td>
                    <td align="left">
                        <br /><br />
                        <asp:Button ID="btnReportMemberNon0FG" runat="server" 
                            Text="Focus Group Excluding 0" onclick="btnReportMemberNon0FG_Click" 
                            Width="300px" />
                        <br />
                        <asp:Button ID="btnReportMember0FG" runat="server" 
                            onclick="btnReportMember0FG_Click" Text="Focus Group 0 Only" Width="300px" />
                                    <asp:Button ID="btnReportMemberAll" runat="server" 
                                         Text="All" Visible="false" 
                                        onclick="btnReportMemberAll_Click" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <font color="maroon" size="2" style="font-weight:bold">
                        Shared Resources 
                        Report<br />
                        <br />
                        <asp:DropDownList ID="ddlResource" runat="server" Width="300px">
                        </asp:DropDownList>
                        <br />
                        <br />
                    </font>
                                <asp:Button ID="btnResource" runat="server" 
                                    Text="Shared Resources Report" Width="300px" 
                                    onclick="btnResource_Click" />
                                <br /><br />
                    </td>
                    <td align="left" runat="server" visible="false">
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
        <div class="clear2column">
        </div>
    </div>
</asp:Content>

