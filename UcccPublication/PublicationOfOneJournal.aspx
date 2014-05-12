<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="PublicationOfOneJournal.aspx.cs" Inherits="PublicationOfOneJournal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server">
                <font color="maroon" size="2" style="font-weight:bold">
                Articles in journal(s) <br /><br />
                    <asp:Label ID="lblJournal" runat="server"></asp:Label><br /></font>
                    <asp:Label ID="lblStartDate" runat="server" Text=""></asp:Label> to <asp:Label ID="lblEndDate" runat="server" Text=""></asp:Label>
            <div  class="inventorycrumbs">
                <div>
                    <asp:Label ID="lblTotal" runat="server"></asp:Label><br /><br />
                    <asp:GridView ID="gvPublication" runat="server" AutoGenerateColumns="false">          
                        <Columns>
                            <asp:BoundField ItemStyle-HorizontalAlign="Left" DataField="journal" HeaderText="Journal" SortExpression="journal" />
                            <asp:BoundField ItemStyle-HorizontalAlign="Left" DataField="article" HeaderText="Articles" SortExpression="article" />
                            <asp:TemplateField HeaderText="Authors" ItemStyle-HorizontalAlign="Left" SortExpression="authorlist"> 
                                <ItemTemplate> 
                                    <asp:Label ID="lblAuthorList" runat="server" Text='<%# Bind("authorlist") %>'></asp:Label> 
                                </ItemTemplate> 
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField> 
                            <asp:HyperLinkField    
                                HeaderText="PMID"  
                                DataNavigateUrlFields="pmid"  
                                DataNavigateUrlFormatString="http://www.ncbi.nlm.nih.gov/pubmed/{0}"
                                DataTextField="pmid"  
                            />  
                        </Columns>
                    </asp:GridView>
                    <br />
                    <div id="onePubDiv" runat="server">
                    </div>
                </div>
                </div>
            </div>
        <div class="clear2column">
        </div>
    </div>
</asp:Content>

