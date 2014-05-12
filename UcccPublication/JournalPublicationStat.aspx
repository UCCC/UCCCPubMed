<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="JournalPublicationStat.aspx.cs" Inherits="JournalPublicationStat" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                     Publication Journals<br />
            </font>
            <div  class="inventorycrumbs">
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label><br />                
                Start Date:<asp:TextBox runat="server" ID="txtStartDate"></asp:TextBox>
                End Date:<asp:TextBox runat="server" ID="txtEndDate"></asp:TextBox><br /><br />
                Only show journals with at least the following number of publications in this time period:<br />
                <asp:TextBox runat="server" ID="txtLeast" Width="51px"></asp:TextBox><br />
                <div><br />
                    <asp:Button id="btnPublicationStat" runat="server" Text="Get Statistics" onclick="btnPublicationStat_Click" 
                        ></asp:Button><br /><br />
                </div> 
                <div>
                <br />
                    <asp:Label ID="lblTotal" runat="server"></asp:Label><br /><br />
                      <asp:GridView ID="gvPublication" ShowHeaderWhenEmpty="True"
                        runat="server" 
                        CssClass="clsFormLabelLeft"
                        AutoGenerateColumns="False" 
                        ShowFooter="True" 
                        BackColor="White" BorderColor="#DEDFDE" 
                        BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                        GridLines="Vertical" Font-Size="X-Small" ForeColor="Black"> 
                        <RowStyle BackColor="#F7F7DE" />
                    <Columns> 
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chxSelect" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField    
                            HeaderText="Journal"  
                            DataNavigateUrlFields="journal_link"  
                            DataNavigateUrlFormatString="~/PublicationOfOneJournal.aspx?journal={0}"
                            DataTextField="journal_name"  
                        />  
                        <asp:TemplateField HeaderText="Journal Name" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="journal_name"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblJournalName" runat="server" Text='<%# Bind("journal_name") %>'></asp:Label> 
                            </ItemTemplate> 
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Publications" Visible="true"  HeaderStyle-HorizontalAlign="Left" SortExpression="publications"> 
                            <ItemTemplate> 
                                <asp:Label ID="lblPublications" runat="server" Text='<%# Bind("publications") %>'></asp:Label> 
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
                <font color="maroon" size="2" style="font-weight:bold">
                    <asp:Button ID="btnPubsSelectedJournals" runat="server" 
                    onclick="btnPubsSelectedJournals_Click" Text="Publications of Selected Journals" 
                    Visible="False" />
                    <br />
                </font>
                    <br />
                    <div id="onePubDiv" runat="server">
                    <asp:Chart ID="chartPublication" runat="server" Width="600px" Height="448px">
                        <Titles>
                            <asp:Title Font="Microsoft Sans Serif, 12pt, style=Bold" ForeColor="Maroon" 
                                Name="Publication" Text="Journal Publications">
                            </asp:Title>
                        </Titles>
                        <Series>
                            <asp:Series IsValueShownAsLabel="True" Name="Series1" XValueMember="journal_name" 
                                YValueMembers="publications">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                                <AxisY Title="Publications" TitleForeColor="Maroon">
                                </AxisY>
                                <AxisX Title="Journal" IsLabelAutoFit="True" TitleForeColor="Maroon">
                                    <LabelStyle Angle="-90" Interval="1" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                    </div>
                </div>
                </div>
            </div>
        <div class="clear2column">
        </div>
    </div>
</asp:Content>

