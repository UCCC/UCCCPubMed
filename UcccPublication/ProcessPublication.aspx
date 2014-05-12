<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ProcessPublication.aspx.cs" Inherits="ProcessPublication" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
     <link type="text/css" rel="Stylesheet" href="pubsite.css" />
    <asp:HiddenField ID="hdnPubCnt" runat="server" />
    <div id="body">
        <div class="fullblock" id="Div1" runat="server">
            <div style="text-align:left" class="fullblock" id="Div11" runat="server">
            <table style="width: 100%; margin-left: 0px;" align="center">
                <tr>
                    <td align="center" style="width: 628px">
                        <font color="maroon" size="2" style="font-weight:bold">
                    Process Publications<br />
                </font>
      
                    
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 628px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left" style="width: 628px">
                    <b>The system will:</b>     
                    <ul>
                    <li>Assign programs to publications. <br /></li>
                    <li>Format author list with program.<br /></li>
                    <li>Format author list with program and institution.<br /></li>
                    <li>Format article title.<br /></li>
                    <li>Get programmatic information. <br /></li>
                    <li>Get full text URL if it has PMCID. <br /></li>
                    <li>Make publication date and ePub date for report use.<br /></li>
                    <li>Help to confirm authorship by member name (initial).<br /></li>
                    </ul>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 628px">
                        <asp:Button ID="btnProcess" runat="server" onclick="btnProcess_Click" 
                            Text="Process Unprocessed Publications" Width="239px" />
                        <br />
                        <br />
                <font color="maroon" style="font-weight:bold">
                <asp:Button ID="btnReprocess" runat="server" onclick="btnReprocess_Click" 
                    Text="Re-process All Publications" Width="239px" />
                        <br />
                </font>Ihis may take several minutes.
                    </td>
                </tr>
                </table>
            </div>
            <div style="text-align:left" class="fullblock" id="inventoryDiv" runat="server">
            <font color="maroon" size="2" style="font-weight:bold">
                    Re-process Publications</font><br />
                <table style="width: 100%" border="1">
                    <tr>
                        <td colspan="2">
                            Start Date:<asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                            End Date:<asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                <font color="maroon" size="2" style="font-weight:bold">
                    Re-process Publications for a member</font><br />
                <br />
                <asp:DropDownList ID="ddlMember" runat="server" Width="239px">
                </asp:DropDownList><br />
                <br />
                <div>
                    <asp:Button id="btnGetPublication" runat="server" Text="List Publications of the Member" 
                        onclick="btnGetPublication_Click" Width="239px"></asp:Button>
                    <br />
                    <br />
                    <asp:Button ID="btnAllNoProgram" runat="server" onclick="btnAllNoProgram_Click" Width="239px" 
                        Text="List All Publications No Any Program" />
                    <br />
                </div> 
                            
                        </td>
                        <td valign="top">
                <font color="maroon" style="font-weight:bold">
                Re-process one Publication</font><br /><br />
                PMID:<asp:TextBox ID="txtPmid" runat="server" Width="60px"></asp:TextBox><br />
                    <asp:Button ID="btnProcessPubByPmid" runat="server" onclick="btnProcessPubByPmid_Click" Width="239px"  
                        Text="Re-Process" />                
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
            <asp:GridView ID="gvPublication" 
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="publication_id"
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" ForeColor="Black"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chxSelect" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="publication_id"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Publication" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="publication"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblPublication" runat="server" Text='<%# Bind("publication") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Journal" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="journal"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblJournal" runat="server" Text='<%# Bind("journal") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Authors" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="authorlist"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:HyperLinkField    
                    HeaderText="PMID"  
                    DataNavigateUrlFields="pmid"  
                    DataNavigateUrlFormatString="http://www.ncbi.nlm.nih.gov/pubmed/{0}"
                    DataTextField="pmid"  
                />  
            </Columns> 
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView> 
                        </td>
                    </tr>
                </table>
                <br /><br />
                    <asp:Button ID="btnSelectAll" runat="server" onclick="btnSelectAll_Click" 
                        Text="Select All" Visible="False" />
                    <asp:Button ID="btnProcessSelectedPubs" runat="server" 
                    onclick="btnProcessSelectedPubs_Click" Text="Re-Process Selected Publications" 
                    Visible="False" />
                    <asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" 
                    Text="Remove Selected Pubs" Visible="False" />
                    <br />
                    <asp:HiddenField ID="hdnPublicationId" runat="server" />
                </div>
                <div style="text-align:left" class="fullblock" id="Div3" runat="server" visible="false">
                <font color="maroon" size="2" style="font-weight:bold">
                    Special Publication Processings</font><br />
                    <br />
                    <div id="onePubDiv" runat="server">
                        <asp:Button ID="btnPutEpubDate" runat="server" onclick="btnPutEpubDate_Click" 
                            Text="Insert all ePub Date" />
                    </div>
                </div>

        </div>
    </div>
</asp:Content>

