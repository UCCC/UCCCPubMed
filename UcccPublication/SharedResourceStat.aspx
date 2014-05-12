<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="SharedResourceStat.aspx.cs" Inherits="SharedResourceStat" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                    Shared Resource Statistics<br />
            </font>
            <div  class="inventorycrumbs">
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />
                <asp:Label ID="Label1" runat="server">Start Date:</asp:Label>
                <asp:TextBox runat="server" ID="txtStartDate"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="tbxStartDate" runat="server" TargetControlID="txtStartDate" WatermarkText="Start Date">
                </asp:TextBoxWatermarkExtender>
                <asp:Label ID="Label3" runat="server">End Date:</asp:Label>
                <asp:TextBox runat="server" ID="txtEndDate"></asp:TextBox>
                <br /><br />
                <asp:DropDownList ID="ddlProgram" runat="server" Width="200px">
                </asp:DropDownList>
                <br />
                (Select program to see what shared resources were used)<br />
                or<br />
                <asp:DropDownList ID="ddlSharedResource" runat="server" Width="200px">
                </asp:DropDownList>

                <br /> (Select shared resource to see what programs used it)<br />
                <br />
                    <asp:Button id="btnResourceOnProgramStat" runat="server" Text="Get Statistics" onclick="btnResourceOnProgramStat_Click" 
                        ></asp:Button><br /><br />
                <div>
                    <asp:Label ID="lblTotal" runat="server"></asp:Label><br /><br />
                    <asp:GridView ID="gvResource" runat="server" AutoGenerateColumns="true">          
                    </asp:GridView>
                    <br />
                    <asp:HiddenField ID="hdnPublicationId" runat="server" />
                    <div id="onePubDiv" runat="server">
                    <asp:Chart ID="chartPublication" runat="server" Width="600px" Height="448px">
                        <Titles>
                            <asp:Title Font="Microsoft Sans Serif, 12pt, style=Bold" ForeColor="Maroon" 
                                Name="Publication">
                            </asp:Title>
                        </Titles>
                        <Series>
                            <asp:Series IsValueShownAsLabel="True" Name="Series1" 
                                YValueMembers="publications">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                                <AxisY Title="Publications" TitleForeColor="Maroon">
                                </AxisY>
                                <AxisX Title="Shared Resources" IsLabelAutoFit="True" TitleForeColor="Maroon">
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

