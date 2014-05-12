<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="SetSharedResource.aspx.cs" Inherits="SetSharedResource" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                    Set Shared Resources<br />
                </font>
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />
                <table align="center" style="width: 63%">
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
                            PMID:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtPmid" runat="server" Width="71px"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Button ID="btnGetAuthorlist" runat="server" 
                        onclick="btnGetAuthorlist_Click" Text="List Publications" Width="160px" /></td>
                    </tr>
                    </table>
                    <br />
                    
                    <asp:HiddenField ID="hdnPublicationId" runat="server" />
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
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" ForeColor="Black" Width="688px"> 
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
                    <EditItemTemplate> 
                        <asp:Label ID="lblPublication" Width="200px" runat="server" Text='<%# Bind("publication") %>'></asp:Label> 
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblPublication" runat="server" Text='<%# Bind("publication") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Authors" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="authorlist"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblAuthorlist" Width="180px" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label> 
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField Visible="true" HeaderText="" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <ItemTemplate> 
                        <asp:LinkButton ID="lnkSource" runat="server" CausesValidation="False" CommandName="resource" Text="Shared Resources"></asp:LinkButton> 
                    </ItemTemplate> 
                </asp:TemplateField> 
                <asp:HyperLinkField    
                    HeaderText="PMID"  
                    DataNavigateUrlFields="pmid"  
                    DataNavigateUrlFormatString="http://www.ncbi.nlm.nih.gov/pubmed/{0}"
                    DataTextField="pmid"  
                />  
                <asp:TemplateField Visible="false" ControlStyle-Width="50px" FooterStyle-Width="50px" HeaderStyle-Width="50px" ItemStyle-Width="50px" HeaderText="Edit URL" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
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
                        <div id="onePubDiv" runat="server" visible="false">
                    <table style="width: 100%" border="1">
                            <tr>
                                <td align="right" class="clsFormLabel" style="width: 61px">
                                    PMID</td>
                                <td align="left">
                                    <asp:Label ID="lblPmid" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top" class="clsFormLabel" style="width: 61px">
                                    Publication</td>
                                <td align="left">
                                    <asp:Label ID="lblPublication" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top" class="clsFormLabel" style="width: 61px">
                                    Author List</td>
                                <td align="left">
                                    <asp:Label ID="lblAuthorlist" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                        <tr>
                            <td colspan="2" align="left">
                          <asp:CheckBoxList id="chxlResource" 
                               CellPadding="5"
                               CellSpacing="5"
                               RepeatColumns="2"
                               RepeatDirection="Vertical"
                               RepeatLayout="Table"
                               Width="700px"
                               TextAlign="Right"
                               runat="server">
 
                          </asp:CheckBoxList>
                                <br />
                                <asp:Button ID="btnSaveResource" runat="server" onclick="btnSaveResource_Click" 
                                    Text="Save" />
                                <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                                    Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                    </div>
</div>
</asp:Content>

