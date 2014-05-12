<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Inventory.aspx.cs" Inherits="Inventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function CompareConfirm(total, setFocus) {
            if (total == setFocus) {
                // your logic here
                return true;
            } else {
                // your logic here
                return confirm("Continue?");
            }
        }
    </script>

    <div id="body">
        <font color="maroon" size="2" style="font-weight:bold">
                    Publication Inventory<br />
                </font>
        <div id="fullblock">
            <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
            <br />
            <table style="width: 100%" border="1">
                <tr>
                    <td align="left">
                    Start Date:<asp:TextBox id="txtStartDate" Runat="server" Width="100px" />
                    &nbsp;&nbsp;&nbsp;
                    End Date:<asp:TextBox ID="txtEndDate" runat="server" Width="100px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;
                        &nbsp;
                        &nbsp;&nbsp;
                        </td>
                </tr>
                <tr>
                    <td align="left">
                    <table style="width: 93%" align="left">
                        <tr>
                            <td align="left" class="txtfield" style="width: 313px">
                                <asp:DropDownList ID="ddlProgram" runat="server" Width="300px" 
                                    AutoPostBack="True" onselectedindexchanged="SelectProgram">
                                </asp:DropDownList>
                                </td>
                            <td align="left" class="txtfield" style="width: 313px">
                                <asp:DropDownList ID="ddlResource" runat="server" Height="19px" Width="300px">
                                </asp:DropDownList>
                                </td>
                        </tr>
                        <tr>
                            <td align="left" class="txtfield" style="width: 313px" colspan="2">
                                    <asp:DropDownList ID="ddlMember" runat="server" Width="300px">
                                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="txtfield" style="width: 313px" colspan="2">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" class="txtfield" style="width: 313px">
                                <asp:Button ID="btnReviewEditorial" runat="server" Text="Review/Editorial" 
                                    Width="300px" onclick="btnReviewEditorial_Click" />
                            </td>
                            <td align="left" class="txtfield" style="width: 313px">
                                <asp:Button ID="btnProgPub4Resource" runat="server" 
                                    onclick="btnProgPub4Resource_Click" Text="Program Pubs for the Shared Resource" 
                                    Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="txtfield" style="width: 313px" colspan="2">
                                 <asp:Button ID="btnProgramRemoved" runat="server" 
                                     Text="Not Cancer Related" Width="300px" 
                                     onclick="btnProgramRemoved_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="txtfield" style="width: 313px" colspan="2">
                                 <asp:Button ID="btnNoPubYearMonth" runat="server" 
                                     onclick="btnNoPubYearMonth_Click" Text="Pubs Missing Pub Year Or Month" 
                                     Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="txtfield" style="width: 313px" colspan="2">
                                <asp:Button ID="btnNotFinalAuthorship" runat="server" 
                                    onclick="btnNotFinalAuthorship_Click" Text=" Missing Final Authorship" 
                                    Width="300px" />
                                 </td>
                        </tr>
                        <tr>
                            <td align="left" class="txtfield" style="width: 313px" colspan="2">
                                 <asp:Button ID="btnNoPmcid" runat="server" Text="Pubs That Have No PMCID" 
                                     Width="300px" onclick="btnNoPmcid_Click" />
                                 </td>
                        </tr>
                        <tr>
                            <td align="left" class="txtfield" style="width: 313px" colspan="2">
                                <asp:Button ID="btnFGNotSet" runat="server" onclick="btnFGNotSet_Click" 
                                    Text="No Focus Group Set" Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="txtfield" style="width: 313px" colspan="2">
                                <asp:Button ID="btnResource" runat="server" Text="Shared Resources for the Program or Member" 
                                    Width="300px" onclick="btnResource_Click" />
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <br />
            <asp:GridView ID="gvPublication" 
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="publication_id"
                OnRowDataBound="gvPublication_RowDataBound" 
                OnRowCommand="gvPublication_RowCommand" 
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" ForeColor="Black"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="publication_id"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField Visible="true" HeaderText="PMID" ItemStyle-VerticalAlign="Top" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <ItemTemplate> 
                        <asp:LinkButton ID="lnkLink" runat="server" CausesValidation="False" CommandName="Link" Text='<%# Bind("pmid") %>'></asp:LinkButton> 
                    </ItemTemplate> 
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Publication" ItemStyle-VerticalAlign="Top" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="publication"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblPublication" runat="server" Text='<%# Bind("publication") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Authors" Visible="true" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left" SortExpression="authorlist"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Select" Visible="false">
                    <ItemTemplate>
                        <asp:CheckBox ID="chxSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns> 
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>                
            <asp:GridView ID="gvResource" 
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="publication_id"
                OnRowDataBound="gvResource_RowDataBound" 
                OnRowCommand="gvResource_RowCommand" 
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" ForeColor="Black"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="publication_id"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Shared Resource" ItemStyle-VerticalAlign="Top" Visible="true" HeaderStyle-HorizontalAlign="Left"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblResource" runat="server" Text='<%# Bind("resource") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField Visible="true" HeaderText="PMID" ItemStyle-VerticalAlign="Top" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <ItemTemplate> 
                        <asp:LinkButton ID="lnkLink" runat="server" CausesValidation="False" CommandName="Link" Text='<%# Bind("pmid") %>'></asp:LinkButton> 
                    </ItemTemplate> 
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Publication" ItemStyle-VerticalAlign="Top" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="publication"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblPublication" runat="server" Text='<%# Bind("publication") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Authors" Visible="true" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left" SortExpression="authorlist"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Select" Visible="false">
                    <ItemTemplate>
                        <asp:CheckBox ID="chxSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns> 
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>                
            <asp:GridView ID="gvProgPub4Resource" 
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
                <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="publication_id"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Program" ItemStyle-VerticalAlign="Top" Visible="true" HeaderStyle-HorizontalAlign="Left"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblProgram" runat="server" Text='<%# Bind("program") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField Visible="true" HeaderText="PMID" ItemStyle-VerticalAlign="Top" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <ItemTemplate> 
                        <asp:LinkButton ID="lnkLink" runat="server" CausesValidation="False" CommandName="Link" Text='<%# Bind("pmid") %>'></asp:LinkButton> 
                    </ItemTemplate> 
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Publication" ItemStyle-VerticalAlign="Top" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="publication"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblPublication" runat="server" Text='<%# Bind("publication") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Authors" Visible="true" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left" SortExpression="authorlist"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Select" Visible="false">
                    <ItemTemplate>
                        <asp:CheckBox ID="chxSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns> 
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>                
            <br />
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear2column">
        </div>
    </div>
</asp:Content>

