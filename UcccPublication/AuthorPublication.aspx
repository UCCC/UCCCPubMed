<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="AuthorPublication.aspx.cs" Inherits="AuthorPublication" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <a name="pagetop" ></a>
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                    Publication of the author<br />
                <br />
                </font>
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <div>
                    <asp:Button ID="btnAllYes" runat="server" onclick="btnAllYes_Click" Visible="true" 
                        Text="Select all Final to 'Yes'" Width="160px" /><br />
                    <asp:Button ID="btnAllNo" runat="server" onclick="btnAllNo_Click" 
                    Text="Select all Final to 'No'" Width="160px" Visible="true" /><br />
                    <asp:Button ID="btnCheckAddress" runat="server" onclick="btnCheckAddress_Click" 
                        Text="Check Address" Width="160px" />
                        <br /><br />
                    <asp:Button ID="btnSaveAuthorship" runat="server" 
                    onclick="btnSaveAuthorship_Click" Width="160px" Text="Save Final Authorship" 
                    Visible="true" />
                        <br />
                        <asp:Label ID="lblCntStr" runat="server"></asp:Label>
                        <br />
                </div> 
            <asp:GridView ID="gvPublication" 
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="publication_id"
                OnRowCancelingEdit="gvPublication_RowCancelingEdit" 
                OnRowDataBound="gvPublication_RowDataBound" 
                OnRowCommand="gvPublication_RowCommand" 
                OnRowEditing="gvPublication_RowEditing" 
                OnRowUpdating="gvPublication_RowUpdating" 
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
                <asp:TemplateField Visible="true" HeaderText="PMCID" ItemStyle-VerticalAlign="Top" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblPmcid" runat="server" Text='<%# Bind("pmcid") %>'></asp:Label> 
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
                <asp:TemplateField HeaderText="Colorado Address?" Visible="true" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left" SortExpression="addressMatch"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAddressMatch" runat="server" Text='<%# Bind("addressMatch") %>'></asp:Label> 
                    </ItemTemplate> 
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Final Authorship" ItemStyle-VerticalAlign="Top">
                    <HeaderStyle Width="15%" />
                    <ItemStyle Wrap="false" Width="80px" />
                    <ItemTemplate>
                        <asp:RadioButtonList RepeatDirection="Vertical" ID="rblFinal" runat="server">
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Unknown" Value="3"></asp:ListItem>
                        </asp:RadioButtonList>
                    </ItemTemplate>
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
            </asp:GridView>  <br />
                <asp:Label ID="lblSelectedPubIdList" runat="server" Visible="False"></asp:Label>
                <br />
                <a href="#pagetop" >Back to top</a>
                    <br />
                    <asp:HiddenField ID="hdnPublicationId" runat="server" />
                    <div id="onePubDiv" runat="server" visible="false">
                    </div>
            </div>
    </div>
</asp:Content>

