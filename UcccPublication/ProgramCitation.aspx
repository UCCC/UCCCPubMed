<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="ProgramCitation.aspx.cs" Inherits="ProgramCitation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                    Program Citation Statistics<br />
            </font>
            <div  class="inventorycrumbs">
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />
                Start Date:
                <asp:TextBox runat="server" ID="txtStartDate"></asp:TextBox>
                End Date:
                <asp:TextBox runat="server" ID="txtEndDate"></asp:TextBox>
                <asp:DropDownList ID="ddlProgram" runat="server" Width="200px">
                </asp:DropDownList>
                <div><br /> <br />
                    <asp:Button id="btnPublicationStat" runat="server" Text="Citation Counts" onclick="btnPublicationStat_Click" 
                        ></asp:Button><br /><br />
                </div> 
                <div>
                    <asp:GridView ID="gvPublication" runat="server" AutoGenerateColumns="false"
                        DataKeyNames="publication_id"
                        OnRowCommand="gvPublication_RowCommand"
                        OnRowEditing="gvPublication_RowEditing" 
                        OnRowCancelingEdit="gvPublication_RowCancelingEdit" 
                        OnRowDataBound="gvPublication_RowDataBound" 
                        OnRowUpdating="gvPublication_RowUpdating" 
                        >          
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
                            <asp:TemplateField HeaderText="Title" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="title"> 
                                <ItemTemplate> 
                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("title") %>'></asp:Label> 
                                </ItemTemplate> 
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Authors" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="authorlist"> 
                                <ItemTemplate> 
                                    <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label> 
                                </ItemTemplate> 
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Cited by" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="citation"> 
                                <EditItemTemplate> 
                                    <asp:TextBox ID="txtCitedBy" Width="30px" runat="server" Text='<%# Bind("citation") %>'></asp:TextBox> 
                                </EditItemTemplate> 
                                <ItemTemplate> 
                                    <asp:Label ID="lblCitedBy" runat="server" Text='<%# Bind("citation") %>'></asp:Label> 
                                </ItemTemplate> 
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField> 
                            <asp:TemplateField Visible="true" HeaderText="View" ShowHeader="False" HeaderStyle-HorizontalAlign="Center"> 
                                <ItemTemplate> 
                                    <asp:LinkButton ID="lnkGoogle" runat="server" CausesValidation="False" CommandName="Google" Text="Google Scholar"></asp:LinkButton> 
                                </ItemTemplate> 
                            </asp:TemplateField> 
                            <asp:HyperLinkField    
                                HeaderText="PMID"  
                                DataNavigateUrlFields="pmid"  
                                DataNavigateUrlFormatString="http://www.ncbi.nlm.nih.gov/pubmed/{0}"
                                DataTextField="pmid"  
                            />  
                            <asp:TemplateField Visible="true" HeaderText="Edit Citation" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
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
                    </asp:GridView>
                    <br />
                    <asp:HiddenField ID="hdnPublicationId" runat="server" />
                    <div id="onePubDiv" runat="server" visible="false">
                        <table style="width: 100%">
                        <tr>
                            <td colspan="2">
                                <br />
                                <asp:Button ID="btnSave" runat="server" Text="Save" />
                            </td>
                        </tr>
                        </table>
                    </div>
                </div>
                </div>
            </div>
        <div class="clear2column">
        </div>
    </div>
</asp:Content>

