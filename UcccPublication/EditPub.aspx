<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="EditPub.aspx.cs" Inherits="EditPub" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
        <font color="maroon" size="2" style="font-weight:bold">
                    Edit Publication Information<br />
        <br /></font>
                        Once publications are printed, we might want to update volumn/issue/page information.<br />


        <br />
                
            <table style="width: 97%">
                <tr>
                    <td align="right">
                        Whole or partial title:</td>
                    <td align="left">
                        <asp:TextBox ID="txtSearchTitle" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;</td>
                    <td align="left">
                        <asp:Button ID="btnSearch" runat="server" Text="Search in database" 
                            onclick="btnSearch_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        PMIDs:
                        </td>
                    <td align="left">
                        <asp:TextBox ID="txtPmidSearch" runat="server" Width="393px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;</td>
                    <td align="left">
                        <asp:Button ID="btnPmidSearch" runat="server" onclick="btnPmidSearch_Click" 
                            Text="Search in Database" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Whole or Patial Last Name:
                        </td>
                    <td align="left">
                        <asp:TextBox ID="txtLastName" runat="server" Width="393px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;</td>
                    <td align="left">
                        <asp:Button ID="btnSearchForLastName" runat="server" Text="Search in Database" 
                            onclick="btnSearchForLastName_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Select a member:</td>
                    <td align="left">
                        <asp:DropDownList ID="ddlMember" 
                            runat="server" Width="408px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;</td>
                    <td align="left">
                        <asp:Button ID="btnEPub" runat="server" onclick="btnEPub_Click" 
                            Text="List ePubs" Width="157px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                      <asp:GridView ID="gvPublication" ShowHeaderWhenEmpty="True"
                        runat="server" 
                        CssClass="clsFormLabelLeft"
                        AutoGenerateColumns="False" 
                        ShowFooter="True" 
                        OnRowCommand="gvPublication_RowCommand"
                        BackColor="White" BorderColor="#DEDFDE" 
                        BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                        GridLines="Vertical" Font-Size="X-Small" ForeColor="Black"> 
                        <RowStyle BackColor="#F7F7DE" />
                        <Columns> 
                            <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="publication_id"> 
                                <ItemTemplate> 
                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label> 
                                </ItemTemplate> 
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField> 
                            <asp:TemplateField Visible="true" HeaderText="" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                                <ItemTemplate> 
                                    <asp:LinkButton ID="lnkPmid" runat="server" CausesValidation="False" CommandName="Select" Text="Select"></asp:LinkButton> 
                                </ItemTemplate> 
                            </asp:TemplateField> 
                            <asp:HyperLinkField    
                                HeaderText="PMID"  
                                DataNavigateUrlFields="pmid"  
                                DataNavigateUrlFormatString="http://www.ncbi.nlm.nih.gov/pubmed/{0}"
                                DataTextField="pmid"  
                            />  
                            <asp:TemplateField HeaderText="Article Title" Visible="true"  HeaderStyle-HorizontalAlign="Left" SortExpression="article_title"> 
                                <ItemTemplate> 
                                    <asp:Label ID="lblArticleTitle" runat="server" Text='<%# Bind("article_title") %>'></asp:Label> 
                                </ItemTemplate> 
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField> 
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
            <br />
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
                                <asp:TextBox ID="txtVolume" runat="server" Width="300px"></asp:TextBox>
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
                                <asp:TextBox ID="txtArticleTitle" runat="server" TextMode="MultiLine" 
                                    Width="300px" Height="68px"></asp:TextBox>
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
                            <td align="right">
                                Collective Name</td>
                            <td align="left">
                                <asp:TextBox ID="txtCollectiveName" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
                                <asp:HiddenField ID="hdnPubId" runat="server" />
                                <asp:Button ID="btnAuthor" runat="server" onclick="btnAuthor_Click" 
                                    Text="Authors" />
                                <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click" 
                                    Text="Clear" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />

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
                        <asp:Label ID="lblId0" runat="server" 
                            Text='<%# Bind("publication_author_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="author_id" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="author_id"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAuthorId" runat="server" Text='<%# Bind("author_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="CC member" HeaderStyle-HorizontalAlign="Left" SortExpression="client_id"> 
                    <EditItemTemplate> 
                        <asp:DropDownList ID="ddlMember0" Width="160px" runat="server" DataTextField="client" 
                            DataValueField="client_id"> 
                        </asp:DropDownList> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:DropDownList ID="ddlNewMember" AutoPostBack="True" OnSelectedIndexChanged="ddlNewMember_SelectedIndexChanged" Width="160px" runat="server"> </asp:DropDownList> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblMember" Width="160px" runat="server" Text='<%# Eval("client") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="80px" HeaderText="Last Name" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="last_name"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtLastName0" Width="80px" runat="server" 
                            Text='<%# Bind("last_name") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewLastName" Width="80px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblLastName" runat="server" Text='<%# Bind("last_name") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="80px" HeaderText="First Name" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="first_name"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtFirstName" Width="80px" runat="server" Text='<%# Bind("first_name") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewFirstName" Width="80px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblFirstName" runat="server" Text='<%# Bind("first_name") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="40px" HeaderText="Initial" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="mi"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtInitial" Width="40px" runat="server" Text='<%# Bind("mi") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewInitial" Width="40px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblInitial" runat="server" Text='<%# Bind("mi") %>'></asp:Label> 
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
                    <asp:Button ID="btnProcessPub" runat="server" onclick="btnProcessPub_Click" 
                        Text="Re-process Publication" />


</div>
</asp:Content>

