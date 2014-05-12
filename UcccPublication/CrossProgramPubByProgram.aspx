<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="CrossProgramPubByProgram.aspx.cs" Inherits="CrossProgramPubByProgram" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                    Cross Program Publications<br />
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
                OnRowDataBound="gvPublication_RowDataBound" 
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" Font-Size="X-Small" ForeColor="Black"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="Program" Visible="true"  HeaderStyle-HorizontalAlign="Left" SortExpression="l_program_id"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblProgram" runat="server" Text='<%# Bind("program") %>'></asp:Label> 
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

                    <div id="onePubDiv" runat="server">
                    <asp:Chart ID="chartPublication" runat="server" Width="600px" Height="448px">
                        <Titles>
                            <asp:Title Font="Microsoft Sans Serif, 12pt, style=Bold" ForeColor="Maroon" 
                                Name="Publication" Text="Program Publications">
                            </asp:Title>
                        </Titles>
                        <Series>
                            <asp:Series IsValueShownAsLabel="True" Name="Series1" XValueMember="program"  
                                YValueMembers="publications">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                                <AxisY Title="Publications" TitleForeColor="Maroon">
                                </AxisY>
                                <AxisX Title="Program" IsLabelAutoFit="True" TitleForeColor="Maroon">
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
