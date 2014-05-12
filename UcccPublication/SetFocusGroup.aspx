<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="SetFocusGroup.aspx.cs" Inherits="SetFocusGroup" %>
<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server" style="text-align:center">
                <font color="maroon" size="2" style="font-weight:bold">
                    Set Focus Groups<br />
                </font>
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />
                <table style="width: 60%" align="center">
                    <tr>
                        <td align="left">
                    <asp:RadioButton ID="rbtnAll" runat="server" GroupName="allOrFg" Text="All" 
                        Checked="True" />
                            </td>
                        <td align="left">
                            Start Date:<asp:TextBox id="txtStartDate" Runat="server" Width="71px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                    <asp:RadioButton ID="rbtnNoFg" runat="server" GroupName="allOrFg" 
                        Text="Focus Group Not Set" />
                            </td>
                        <td align="left">
                            End Date: <asp:TextBox ID="txtEndDate" runat="server" Width="71px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                                <asp:RadioButton ID="rbtnFGSet" runat="server" GroupName="allOrFg" 
                                    Text="Focus Group Already Set" />
                            </td>
                        <td align="left">
                    <asp:DropDownList ID="ddlProgram" runat="server" Width="160px">
                    </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                                <asp:RadioButton ID="rbtnFG0" runat="server" GroupName="allOrFg" 
                                    Text="Does not fit a current focus group" />
                            </td>
                        <td align="left">
                                <asp:Button ID="btnGetPublicationlist" runat="server" 
                        onclick="btnGetPublicationlist_Click" Text="List Publications" Width="160px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                                <asp:RadioButton ID="rbtnMultiMember" runat="server" GroupName="allOrFg" 
                                    Text="Multi-program member" />
                            </td>
                        <td align="left">
                                <asp:Button ID="btnSave" runat="server" Text="Save Focus Group" 
                    onclick="btnSave_Click" Visible="False" Width="160px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                                <asp:RadioButton ID="rbtnNotProgramRelated" runat="server" GroupName="allOrFg" 
                                    Text="Not Cancer-related" />
                            </td>
                        <td align="left">
                                &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left">
                                <asp:RadioButton ID="rbtnReview" runat="server" GroupName="allOrFg" 
                                    Text="Review/Editorial" />
                            </td>
                        <td align="left">
                                &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left">
                                <asp:RadioButton ID="rbtnRemoved" runat="server" GroupName="allOrFg" 
                                    Text="Not Cancer Related" Visible="False" />
                            </td>
                        <td align="left">
                                &nbsp;</td>
                    </tr>
                </table>
                <div class="dashedline">
            <asp:GridView ID="gvPublication" 
                runat="server" 
                AutoGenerateColumns="False" 
                DataKeyNames="publication_program_id"
                OnRowCancelingEdit="gvPublication_RowCancelingEdit" 
                OnRowDataBound="gvPublication_RowDataBound" 
                OnRowCommand="gvPublication_RowCommand" 
                OnRowEditing="gvPublication_RowEditing" 
                OnRowUpdating="gvPublication_RowUpdating" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" 
                CellPadding="4"  
                GridLines="Vertical" ForeColor="Black" Width="688px"
                > 
                <RowStyle BackColor="#F7F7DE" HorizontalAlign="left" />
            <Columns> 
                <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="publication_program_id"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_program_id") %>'></asp:Label>
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_program_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Review/Editorial" Visible="false" HeaderStyle-HorizontalAlign="Left" SortExpression="review_editorial"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblReviewEditorial" runat="server" Text='<%# Bind("review_editorial") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:HyperLinkField ItemStyle-VerticalAlign="Top"    
                    HeaderText="PMID"  
                    DataNavigateUrlFields="pmid"  
                    DataNavigateUrlFormatString="http://www.ncbi.nlm.nih.gov/pubmed/{0}"
                    DataTextField="pmid"  
                />  
                <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="Publication" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="publication"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblPublication" runat="server" Text='<%# Bind("publication") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="Authors" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="authorlist_no_inst"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist_no_inst") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="Focus Group" HeaderStyle-HorizontalAlign="Left"> 
                    <ItemStyle Wrap="false" Width="200px" />
                    <ItemTemplate>
                        <asp:RadioButtonList ID="rblFocusGroup" runat="server" RepeatDirection="Vertical">
                        </asp:RadioButtonList>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:HyperLinkField ItemStyle-VerticalAlign="Top"    
                    HeaderText="Full_Text"  
                    DataNavigateUrlFields="full_text_url"  
                    DataNavigateUrlFormatString="{0}"
                    Text="Full Text"
                />  

            </Columns> 
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView> 

                    <br />
                    <asp:Button ID="btnSelectAllToRestore" runat="server" onclick="btnSelectAllToRestore_Click" 
                        Text="Select All To Restore" Width="249px" Visible="False" />
                        <asp:Button ID="btnRestoreSelected" runat="server" 
                            onclick="btnRestoreSelected_Click" Text="Restore Selected Pubs" 
                            Width="249px" Visible="False" />
                <br />

            <asp:GridView ID="gvRemoved" 
                runat="server" 
                AutoGenerateColumns="False" 
                DataKeyNames="publication_id"
                OnRowCancelingEdit="gvRemoved_RowCancelingEdit" 
                OnRowDataBound="gvRemoved_RowDataBound" 
                OnRowCommand="gvRemoved_RowCommand" 
                OnRowEditing="gvRemoved_RowEditing" 
                OnRowUpdating="gvRemoved_RowUpdating" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" 
                CellPadding="4"  
                GridLines="Vertical" ForeColor="Black" Width="688px"
                > 
                <RowStyle BackColor="#F7F7DE" HorizontalAlign="left" />
            <Columns> 
                <asp:TemplateField HeaderText="pubId" Visible="false"  HeaderStyle-HorizontalAlign="Left"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblPubId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label>
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblPubId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:HyperLinkField ItemStyle-VerticalAlign="Top"    
                    HeaderText="PMID"  
                    DataNavigateUrlFields="pmid"  
                    DataNavigateUrlFormatString="http://www.ncbi.nlm.nih.gov/pubmed/{0}"
                    DataTextField="pmid"  
                />  
                <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="Publication" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="publication"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblPublication" runat="server" Text='<%# Bind("publication") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="Authors" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="authorlist_no_inst"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist_no_inst") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Select To Restore" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chxSelectToRestore" />
                    </ItemTemplate>
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

