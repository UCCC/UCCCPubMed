<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="PublicationOfOneMember.aspx.cs" Inherits="PublicationOfOneMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                Publications of the Member <br />
                    <asp:Label ID="lblMember" runat="server"></asp:Label><br /></font>
                    During the time period <asp:Label ID="lblStartDate" runat="server" Text=""></asp:Label> and <asp:Label ID="lblEndDate" runat="server" Text=""></asp:Label>
            
            <div  class="inventorycrumbs">
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />
                <div>
                    <asp:Label ID="lblTotal" runat="server"></asp:Label><br /><br />
                    <asp:GridView ID="gvPublication" runat="server" AutoGenerateColumns="false">          
                        <Columns>
                            <asp:BoundField ItemStyle-HorizontalAlign="Left" DataField="the_year" HeaderText="Year" SortExpression="the_year" />
                            <asp:BoundField ItemStyle-HorizontalAlign="Left" DataField="publications" HeaderText="Publications" SortExpression="publications" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:HiddenField ID="hdnPublicationId" runat="server" />
                    <div id="onePubDiv" runat="server" visible="false">
                        <table style="width: 100%">
                        </table>
                    </div>
                </div>
                </div>
            </div>
        <div class="clear2column">
        </div>
    </div>
</asp:Content>

