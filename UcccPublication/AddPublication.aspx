<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="AddPublication.aspx.cs" Inherits="AddPublication" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
        <font color="maroon" size="2" style="font-weight:bold">
                    Add Publication<br />
                </font>
            <div class="fullblock" id="inventoryDiv" runat="server">
                <div class="inventorycrumbs">
                    <table style="width: 100%">
                        <tr>
                            <td align="right">
                                Journal Title*</td>
                            <td align="left">
                                <asp:TextBox ID="txtJournalTitle" runat="server" TextMode="MultiLine" 
                                    Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                ISO Abbreviation</td>
                            <td align="left">
                                <asp:TextBox ID="txtISOAbbreviation" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Volume</td>
                            <td align="left">
                                <asp:TextBox ID="txtVolumn" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Issue</td>
                            <td align="left">
                                <asp:TextBox ID="txtIssue" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Pages</td>
                            <td align="left">
                                <asp:TextBox ID="txtMedlinePgn" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Year*</td>
                            <td align="left">
                                <asp:TextBox ID="txtYear" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Season</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlSeason" runat="server" Width="300px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Month</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlMonth" runat="server" Width="300px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Day</td>
                            <td align="left">
                                <asp:TextBox ID="txtDay" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Article Title*</td>
                            <td align="left">
                                <asp:TextBox ID="txtArticleTitle" runat="server" TextMode="MultiLine" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Abstract</td>
                            <td align="left">
                                <asp:TextBox ID="txtAbstract" runat="server" TextMode="MultiLine" Width="300px" 
                                    Height="73px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                PMID</td>
                            <td align="left">
                                <asp:TextBox ID="txtPmid" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                PMCID</td>
                            <td align="left">
                                <asp:TextBox ID="txtPmcid" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
                                <asp:HiddenField ID="hdnPubId" runat="server" />
                                <asp:Button ID="btnAuthor" runat="server" onclick="btnAuthor_Click" 
                                    Text="Authors" Visible="False" />
                                <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click" 
                                    Text="Clear" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                </div>
                <div class="dashedline">

              <asp:GridView ID="gvAuthor" ShowHeaderWhenEmpty="True"
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="publication_author_id"
                OnRowCancelingEdit="gvAuthor_RowCancelingEdit" 
                OnRowDataBound="gvAuthor_RowDataBound" 
                OnRowEditing="gvAuthor_RowEditing" 
                OnRowUpdating="gvAuthor_RowUpdating" 
                OnRowUpdated="gvAuthor_RowUpdated"
                OnRowCommand="gvAuthor_RowCommand" 
                OnRowDeleting="gvAuthor_RowDeleting" 
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" Font-Size="X-Small" ForeColor="Black"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="Publication_author_id" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="publication_author_id"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_author_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="author_id" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="author_id"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAuthorId" runat="server" Text='<%# Bind("author_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="If it is CC member" HeaderStyle-HorizontalAlign="Left" SortExpression="client_id"> 
                    <EditItemTemplate> 
                        <asp:DropDownList ID="ddlMember" Width="160px" runat="server" DataTextField="client" 
                            DataValueField="client_id"> 
                        </asp:DropDownList> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:DropDownList ID="ddlNewMember" AutoPostBack="False" OnSelectedIndexChanged="ddlNewMember_SelectedIndexChanged" Width="160px" runat="server"> </asp:DropDownList> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblMember" Width="160px" runat="server" Text='<%# Eval("client") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="80px" HeaderText="Last Name" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="LastName"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtLastName" Width="80px" runat="server" Text='<%# Bind("LastName") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewLastName" Width="80px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblLastName" runat="server" Text='<%# Bind("LastName") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="80px" HeaderText="First Name" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="ForeName"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtForeName" Width="80px" runat="server" Text='<%# Bind("ForeName") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewForeName" Width="80px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblForeName" runat="server" Text='<%# Bind("ForeName") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="40px" HeaderText="Initials" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="Initials"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtInitial" Width="40px" runat="server" Text='<%# Bind("Initials") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewInitials" Width="40px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblInitial" runat="server" Text='<%# Bind("Initials") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField Visible="true" ControlStyle-Width="120px" FooterStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-Width="80px" HeaderText="Edit" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <FooterTemplate> 
                        <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="False" CommandName="Insert" Text="Add"></asp:LinkButton> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:LinkButton OnClientClick="return confirm('Are you sure you want to delete this author from publication?');" ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton> 
                    </ItemTemplate> 
                    <ControlStyle Width="80px"></ControlStyle>
                    <FooterStyle Width="80px"></FooterStyle>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Width="80px"></ItemStyle>
                </asp:TemplateField> 
            </Columns> 
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView> 
                    <br />
                    <asp:Button ID="btnProcess" runat="server" onclick="btnProcess_Click" 
                        Text="Process the New Publication" />
                </div>
            </div>
        </div>
        <div class="clear2column">
    </div>
</asp:Content>

