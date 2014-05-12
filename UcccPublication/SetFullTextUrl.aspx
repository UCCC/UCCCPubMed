<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="SetFullTextUrl.aspx.cs" Inherits="SetFullTextUrl" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server" style="text-align:center">
                <font color="maroon" size="2" style="font-weight:bold">
                    Full Text URL<br />
                </font>
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />
                <table style="width: 60%" align="center">
                    <tr>
                        <td align="left">
                            Start Date:<asp:TextBox id="txtStartDate" Runat="server" Width="71px" />
                        </td>
                        <td align="left">
                    <asp:DropDownList ID="ddlProgram" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="SelectProgram" Width="160px">
                    </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            End Date: <asp:TextBox ID="txtEndDate" runat="server" Width="71px"></asp:TextBox>
                        </td>
                        <td align="left">
                    <asp:DropDownList ID="ddlMember" runat="server" Width="160px">
                    </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            &nbsp;</td>
                        <td align="left">
                    <asp:Button ID="btnGetPublications" runat="server" 
                        onclick="btnGetPublications_Click" Text="List Publications" Width="160px" />
                        </td>
                    </tr>
                </table>
                    <br />
                <div class="dashedline">
            <asp:GridView ID="gvPublication" 
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="publication_id"
                OnRowCancelingEdit="gvPublication_RowCancelingEdit" 
                OnRowDataBound="gvPublication_RowDataBound" 
                OnRowCommand="gvPublication_RowCommand" 
                OnRowEditing="gvPublication_RowEditing" 
                OnRowUpdating="gvPublication_RowUpdating" 
                ShowFooter="False" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" Font-Size="X-Small" ForeColor="Black" Width="688px"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="publication_id"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label>
                    </EditItemTemplate> 
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
                <asp:HyperLinkField    
                    HeaderText="PMID"  
                    DataNavigateUrlFields="pmid"  
                    DataNavigateUrlFormatString="http://www.ncbi.nlm.nih.gov/pubmed/{0}"
                    DataTextField="pmid"  
                />  
                <asp:TemplateField HeaderText="Full Text URL" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="full_text_url"> 
                    <ItemTemplate> 
                        <asp:HyperLink ID="hlFullTextUrl" NavigateUrl='<%# Bind("full_text_url") %>' runat="server" Text='<%# Bind("full_text_url") %>'></asp:HyperLink> 
                    </ItemTemplate> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtFullTextUrl" Width="260px" runat="server" Text='<%# Bind("full_text_url") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewFullTextUrl" Width="260px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField Visible="true" ControlStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="50px" ItemStyle-Width="50px" HeaderText="Edit" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <EditItemTemplate> 
                        <asp:LinkButton ID="lbkUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Save"></asp:LinkButton> 
                        <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
                    </ItemTemplate> 
                    <ControlStyle Width="50px"></ControlStyle>
                    <FooterStyle Width="50px"></FooterStyle>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField> 

            </Columns> 
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView> 

                </div>
            </div>
    </div>
</asp:Content>

