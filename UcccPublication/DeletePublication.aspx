<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="DeletePublication.aspx.cs" Inherits="DeletePublication" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function PubDeleteConfirmation() {
            return confirm("Are you sure you want to delete all selected pubs?");
        }    
    </script>
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                    Delete Publications<br />
                </font>
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />
                    Start Date:
                    <asp:TextBox id="txtStartDate" Runat="server" Width="71px" />
                    &nbsp;&nbsp;&nbsp;
                    End Date:<asp:TextBox ID="txtEndDate" runat="server" Width="71px"></asp:TextBox>
                    <br /><br />
                <div>
                    <asp:Button id="btnGetPublication" runat="server" Text="List Publications with No Member Listed" 
                        onclick="btnGetPublication_Click" Width="351px"></asp:Button><br />
                    <asp:Button ID="btnNoProgram" runat="server" onclick="btnNoProgram_Click" 
                        Text="List Publications with No Program Assigned" Width="351px" />
                    <br />
                    <asp:TextBox ID="txtPmid" runat="server"></asp:TextBox>
                    <asp:Button ID="btnDeletePmid" runat="server" onclick="btnDeletePmid_Click" 
                        Text="Delete Pub of PMID" />
                    <br /><br />
                    <asp:Button ID="btnSelectAll" runat="server" onclick="btnSelectAll_Click" Visible="false" 
                        Text="Select All" />
                    <asp:Button ID="btnClearAll" runat="server" onclick="btnClearAll_Click" 
                        Text="Clear All" Visible="False" />
                    <asp:Button ID="btnDelete" runat="server" onclick="btnDeleteSelected_Click" Text="Delete All Selected" 
                        OnClientClick="if (!PubDeleteConfirmation()) return false;" Visible="False" />
                    <br /><br />
                </div> 
                <asp:GridView ID="gvPublication" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="4" CssClass="clsFormLabelLeft" DataKeyNames="publication_id" 
                    ForeColor="Black" GridLines="Vertical" 
                    OnRowDataBound="gvPublication_RowDataBound" 
                    OnRowDeleting="gvPublication_RowDeleting" ShowFooter="True" Width="688px">
                    <RowStyle BackColor="#F7F7DE" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="ID" 
                            SortExpression="publication_id" Visible="false">
                            <EditItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:CheckBox ID="chxSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Title" 
                            SortExpression="article" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblArticle" runat="server" Text='<%# Bind("article") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Journal" 
                            SortExpression="journal" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblJournal" runat="server" Text='<%# Bind("journal") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Authors" 
                            SortExpression="authorlist" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:HyperLinkField DataNavigateUrlFields="pmid" 
                            DataNavigateUrlFormatString="http://www.ncbi.nlm.nih.gov/pubmed/{0}" 
                            DataTextField="pmid" HeaderText="PMID" />
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                            HeaderText="Delete this pub" ShowHeader="False" Visible="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" 
                                    CommandName="Delete" 
                                    OnClientClick="return confirm('Are you sure you want to delete pub from database?');" 
                                    Text="Delete">
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle Width="50px" />
                            <FooterStyle Width="50px" />
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <br /><br />
                    <br />
                    <asp:HiddenField ID="hdnPublicationId" runat="server" />
                    <div id="onePubDiv" runat="server" visible="false">
                    </div>
            </div>
    </div>
</asp:Content>

