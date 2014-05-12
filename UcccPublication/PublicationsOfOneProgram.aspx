<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="PublicationsOfOneProgram.aspx.cs" Inherits="PublicationsOfOneProgram" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                    Member Publications of the Program <br />
                    <asp:Label ID="lblProgram" runat="server" Text=""></asp:Label><br /></font>
                    During the time period <asp:Label ID="lblStartDate" runat="server" Text=""></asp:Label> and <asp:Label ID="lblEndDate" runat="server" Text=""></asp:Label>
            
            <div  class="inventorycrumbs">
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />
                <div>
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
                        <asp:HyperLinkField    
                            HeaderText="Member"  
                            DataNavigateUrlFields="client_id"  
                            DataNavigateUrlFormatString="~/PublicationOfOneMember.aspx?clientId={0}"
                            DataTextField="member"  
                        />  
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

