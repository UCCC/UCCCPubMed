<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="QuickLook.aspx.cs" Inherits="QuickLook" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div style="text-align:center">
    <font color="maroon" size="2" style="font-weight:bold">
    <br />Search Database</font><br />
    <br />
    <table align="center" style="width: 39%">
        <tr>
            <td align="left" align="center" style="height: 26px; width: 163px;">
                PMID:<asp:TextBox ID="txtPmid" runat="server" Width="88px"></asp:TextBox>
            </td>
            <td align="left" align="center" style="height: 26px">
                PMCID:<asp:TextBox ID="txtPmcid" runat="server" Width="89px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" align="center" style="height: 26px; width: 163px;">
                Last Name of an Author:</td>
            <td align="left" align="center" style="height: 26px">
                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" align="center" style="height: 26px; width: 163px;">
                Whole or Partial Title:</td>
            <td align="left" align="center" style="height: 26px">
                <asp:TextBox ID="txtTitle" runat="server" TextMode="MultiLine" Width="253px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" 
            Text="Search" />
            </td>
        </tr>
    </table>
                      <asp:GridView ID="gvPublication" ShowHeaderWhenEmpty="True" HorizontalAlign="Center"
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
    <asp:HiddenField ID="hdnPubId" runat="server" />
    <asp:Label ID="ErrorMessage" runat="server"></asp:Label>
    <br />
    <asp:DetailsView ID="dvPublication" HorizontalAlign="Center" runat="server" 
        Height="50px" Width="500px" 
        AutoGenerateRows="False" CellPadding="4" 
        Font-Names="Verdana" 
        Font-Size="X-Small" ForeColor="Black" 
        GridLines="Vertical" BackColor="White" BorderColor="#DEDFDE" 
        BorderStyle="None" BorderWidth="1px">
        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <Fields>
            <asp:BoundField DataField="pmid" HeaderText="pmid" />
            <asp:BoundField DataField="pmcid" HeaderText="pmcid" />
            <asp:BoundField DataField="ISOAbbreviation" HeaderText="ISOAbbreviation" />
            <asp:BoundField DataField="Article_title" HeaderText="Article_title" />
            <asp:BoundField DataField="volume" HeaderText="volume" />
            <asp:BoundField DataField="issue" HeaderText="issue" />
            <asp:BoundField DataField="MedlinePgn" HeaderText="MedlinePgn" />
            <asp:BoundField DataField="pub_year" HeaderText="pub_year" />
            <asp:BoundField DataField="pub_season" HeaderText="pub_season" />
            <asp:BoundField DataField="pub_month" HeaderText="pub_month" />
            <asp:BoundField DataField="pub_day" HeaderText="pub_day" />
            <asp:TemplateField HeaderText="Authors" Visible="true" ItemStyle-HorizontalAlign="Left"> 
                <ItemTemplate> 
                    <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label> 
                </ItemTemplate> 
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:TemplateField> 
            <asp:BoundField DataField="final_confirm" HeaderText="final_confirm" />
            <asp:BoundField DataField="review_editorial" HeaderText="review_editorial" />
        </Fields>
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <HeaderTemplate>Publication Information</HeaderTemplate>
        <PagerStyle  BackColor="#F7F7DE" Font-Names="Verdana" Font-Size="Small" 
            ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#F7F7DE" />
    </asp:DetailsView>
    <table align="center" style="width: 35%">
        <tr>
            <td>
    <asp:GridView ID="gvProgram" HorizontalAlign="Center" 
        runat="server" 
        CssClass="clsFormLabelLeft"
        AutoGenerateColumns="False" 
        DataKeyNames="publication_program_id"
        BackColor="White" BorderColor="#DEDFDE" 
        BorderStyle="None" BorderWidth="1px" CellPadding="4" 
        GridLines="Vertical" ForeColor="Black"> 
    <RowStyle BackColor="#F7F7DE" />
    <HeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
    <Columns> 
        <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left"> 
            <ItemTemplate> 
                <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_program_id") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Programs" Visible="true" HeaderStyle-HorizontalAlign="Left"> 
            <ItemTemplate> 
                <asp:Label ID="lblProgram" runat="server" Text='<%# Bind("program") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Focus Group" Visible="true" HeaderStyle-HorizontalAlign="Left"> 
            <ItemTemplate> 
                <asp:Label ID="lblFocusGroup" runat="server" Text='<%# Bind("focus_group") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
    </Columns> 
        <FooterStyle BackColor="#CCCC99" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
    </asp:GridView> 
            </td>
            <td>
    <asp:GridView ID="gvResource" HorizontalAlign="Center" 
        runat="server" 
        CssClass="clsFormLabelLeft"
        AutoGenerateColumns="False" 
        BackColor="White" BorderColor="#DEDFDE" 
        BorderStyle="None" BorderWidth="1px" CellPadding="4" 
        GridLines="Vertical" ForeColor="Black"> 
    <RowStyle BackColor="#F7F7DE" />
    <HeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
    <Columns> 
        <asp:TemplateField HeaderText="Resources" Visible="true" HeaderStyle-HorizontalAlign="Left"> 
            <ItemTemplate> 
                <asp:Label ID="lblResource" runat="server" Text='<%# Bind("resource") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
    </Columns> 
        <FooterStyle BackColor="#CCCC99" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
    </asp:GridView> 
            </td>
        </tr>
    </table>
    <asp:Button ID="btnEdit" runat="server" onclick="btnEdit_Click" 
        Text=" Edit Info" Visible="False" />
    <asp:Button ID="btnImport" runat="server" onclick="btnImport_Click" 
        Text="Import" Visible="False" />
    <br /><br />
</div>
</asp:Content>

