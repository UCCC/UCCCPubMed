<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ProgramPubType.aspx.cs" Inherits="ProgramPubType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                    Program Publication Type Statistics<br />
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
                    <asp:Button id="btnPubtypeStat" runat="server" Text="Get Statistics" onclick="btnPubtypeStat_Click" 
                        ></asp:Button><br /><br />
                </div> 
                <div>
                    <asp:Label ID="lblTotal" runat="server"></asp:Label><br /><br />
                    <asp:GridView ID="gvPubtype" runat="server" AutoGenerateColumns="false">          
                        <Columns>
                            <asp:BoundField ItemStyle-HorizontalAlign="Left" DataField="pubtype" HeaderText="Publication Type" SortExpression="pubtype" />
                            <asp:BoundField ItemStyle-HorizontalAlign="Left" DataField="publications" HeaderText="Publications" SortExpression="publications" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:HiddenField ID="hdnPublicationId" runat="server" />
                </div>
                </div>
            </div>
        <div class="clear2column">
        </div>
    </div>
</asp:Content>

