<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ForEndNote.aspx.cs" Inherits="ForEndNote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                Publications for EndNote<br />
                    <br />
                </font>
                <table style="width: 63%" align="center">
                    <tr>
                        <td align="left" width="50%">
                            Start Date:<asp:TextBox id="txtStartDate" Runat="server" Width="71px" />
                        </td>
                        <td align="left">
                    <asp:Button id="btnGetPublication" runat="server" Text="List Publications" 
                        onclick="btnGetPublication_Click" Width="160px"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            End Date: <asp:TextBox ID="txtEndDate" runat="server" Width="71px"></asp:TextBox>
                        </td>
                        <td align="left">
                    <asp:Button ID="btnExportToExcel" runat="server" onclick="btnExportToExcel_Click" Visible="false" 
                        Text="Export to Excel" Width="160px" />
                        </td>
                    </tr>
                    </table>
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>  <br />              

            <div  class="inventorycrumbs">
                    <asp:GridView ID="gvPublication" 
                        runat="server" 
                        CssClass="clsFormLabelLeft"
                        AutoGenerateColumns="False" 
                        DataKeyNames="publication_id"
                        OnRowDataBound="gvPublication_RowDataBound" 
                        ShowFooter="True" 
                        BackColor="White" BorderColor="#DEDFDE" 
                        BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                        GridLines="Vertical" ForeColor="Black" Width="688px"> 
                        <RowStyle BackColor="#F7F7DE" />
                    <Columns> 
                        <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="publication_id"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_id") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Authors" ItemStyle-VerticalAlign="Top" Visible="true" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblAuthorlist" runat="server"></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Year" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblYear" runat="server" Text='<%# Bind("pub_year") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Epub Date" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblEpubDate" runat="server" Text='<%# Bind("epub_date") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Title" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("article_title") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Journal" Visible="true" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblJournal" runat="server" Text='<%# Bind("journal") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="CCSG Authors w/ Programs" ItemStyle-VerticalAlign="Top" Visible="true" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblAuthorlistNoInst" runat="server" Text='<%# Bind("authorlist_no_inst") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="CCSG Authors w/Programs & Inst" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblAuthorlistWInst" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Volume" ItemStyle-VerticalAlign="Top"  HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblVolume" runat="server" Text='<%# Bind("volume") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Number" ItemStyle-VerticalAlign="Top"  HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblIssue" runat="server" Text='<%# Bind("issue") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Pages" ItemStyle-VerticalAlign="Top"  HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblMedlinePgn" runat="server" Text='<%# Bind("MedlinePgn") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Month of Print Publication" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblPubMonth" runat="server" Text='<%# Bind("pub_month") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="CCSG Programs Code" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblProgram" runat="server"></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="CCSG Focus Group Code" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblFocusGroup" runat="server"></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Shared Resource" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblResource" runat="server"></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="PMCID" ItemStyle-VerticalAlign="Top"  HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblPmcid" runat="server" Text='<%# Bind("pmcid") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Accession Number" ItemStyle-VerticalAlign="Top" Visible="true"  HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblPmid" runat="server" Text='<%# Bind("pmid") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Full Text URL" ItemStyle-VerticalAlign="Top"  HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblFullTextUrl" runat="server" Text='<%# Bind("full_text_url") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Members" ItemStyle-VerticalAlign="Top" Visible="true" HeaderStyle-HorizontalAlign="Left"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblMemberlist" runat="server"></asp:Label> 
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
                    <br />
                </div>
            </div>
    </div>
</asp:Content>

