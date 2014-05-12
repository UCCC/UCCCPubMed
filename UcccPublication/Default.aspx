<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Title="Publication Home Page" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="body">
            <!-- Start of news list functionality -->
            <div class="fullblock" style="text-align:center">
                                        Start Date:
                    <asp:TextBox id="txtStartDate" Runat="server" Width="71px" />
                    &nbsp;&nbsp;&nbsp;
                    End Date:<asp:TextBox ID="txtEndDate" runat="server" Width="71px"></asp:TextBox>
                                        <asp:Button ID="btnShowGraph" runat="server" onclick="btnShowGraph_Click" 
                                            Text="Show Graph" />
                    <br /><br />
                    <font color="maroon" size="2" style="font-weight:bold">
                    <asp:Label ID="lblTotal" runat="server"></asp:Label><br /><br />
                    </font>
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
        <div class="clear2column"></div>
    </div>
</asp:Content>

