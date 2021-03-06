﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="RetrievePub.aspx.cs" Inherits="RetrievePub" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" language="javascript">
    function do_totals1() {
        document.all.pleasewaitScreen.style.visibility = "visible";
        window.setTimeout('do_totals2()', 3000);
    }

    function do_totals2() {
        document.all.pleasewaitScreen.style.visibility = "hidden";
    }

    function hourglass() {
        document.body.style.cursor = "wait";
    }
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<asp:HiddenField ID="hdnPubCnt" runat="server" />
    <asp:HiddenField ID="hdnMemberCnt" runat="server" />
    <div id="body">
        <div class="fullblock" id="Div1" runat="server">
        <font color="maroon" size="2" style="font-weight:bold">
                    Retrieve Publications<asp:HiddenField 
                ID="hdnClientWithPlusSign" runat="server" />
            <br />
                </font>
        <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>    
            <br />
            <table style="width: 100%" border="1">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td align="center">
                                    <font color="maroon" size="2" style="font-weight:bold">
                                            Import 
                                    Specific Publications<br /><br />
                                    </font>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="txtfield" style="height: 28px">
                                    Whole or partial title:<asp:TextBox ID="txtTitle" runat="server" Width="500px"></asp:TextBox>
                                    </td>
                            </tr>
                            <tr>
                                <td align="left" class="txtfield" style="height: 28px">
                                    <asp:Button ID="btnSearchOnTitle" runat="server" onclick="btnSearchOnTitle_Click" 
                                        Text="Search in PubMed" Width="170px" Height="22px" />
                                    </td>
                            </tr>
                            <tr>
                                <td align="left" class="txtfield">
                                    Last Name:<asp:TextBox ID="txtLastName" runat="server" Width="120px"></asp:TextBox>
                                    &nbsp;&nbsp;
                                    First Name:<asp:TextBox 
                                        ID="txtFirstName" runat="server" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="txtfield">
                                    <asp:Button ID="btnImportBySpecifiedName" runat="server" 
                                        onclick="btnImportBySpecifiedName_Click" Text="Search in PubMed" 
                                        Width="171px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="txtfield">
                                    PMID List:<asp:TextBox ID="txtPmidSearch" runat="server" Width="500px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="txtfield">
                                    <asp:Button ID="btnSearchPmid" runat="server" onclick="btnSearchPmid_Click" 
                                        Text="Search in PubMed" Width="171px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="height: 128px">
                            <div style="text-align:center">
                                <asp:GridView ID="gvPublication" 
                                    runat="server" 
                                    AutoGenerateColumns="false" 
                                    Visible="false" 
                                    OnRowCommand="gvPublication_RowCommand" 
                                    OnRowDataBound="gvPublication_RowDataBound" 
                                    >
                                    <Columns>
                                        <asp:TemplateField HeaderText="PMID" Visible="false" ItemStyle-HorizontalAlign="Left" SortExpression="title"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblPmid" runat="server" Text='<%# Bind("PMID") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Publication" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="Publication"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblPublication" runat="server" Text='<%# Bind("Publication") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Authors" Visible="true" ItemStyle-HorizontalAlign="Left" SortExpression="authorlist"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("Authors") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField> 
                                        <asp:HyperLinkField    
                                            HeaderText="PMID"  
                                            DataNavigateUrlFields="pmid"  
                                            DataNavigateUrlFormatString="http://www.ncbi.nlm.nih.gov/pubmed/{0}"
                                            DataTextField="pmid"  
                                        />  
                                        <asp:TemplateField HeaderText="Imported?" Visible="true" ItemStyle-HorizontalAlign="center" SortExpression="Existing"> 
                                            <ItemTemplate> 
                                                <asp:Label ID="lblExisting" runat="server" Text='<%# Bind("Existing") %>'></asp:Label> 
                                            </ItemTemplate> 
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:TemplateField> 
                                        <asp:TemplateField Visible="true" HeaderText="" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                                            <ItemTemplate> 
                                                <asp:LinkButton ID="lnkImport" runat="server" CausesValidation="False" CommandName="Link" Text="Import"></asp:LinkButton> 
                                            </ItemTemplate> 
                                        </asp:TemplateField> 
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                                </td>
                            </tr>
                            </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td align="center" colspan="3">
                                    <font color="maroon" size="2" style="font-weight:bold">
                                            General
                                            Retrieve Publications<br />
                                    </font>
                                </td>
                            </tr>
<!-- 
                            <tr>
                                <td align="right" colspan="3">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2" class="txtfield" style="height: 25px">
                                    Number of publications to retrieve for each member</td>
                                <td align="left" style="height: 25px">
                                    <asp:DropDownList ID="ddlPubMax" runat="server" Enabled="False">
                                    </asp:DropDownList>
                                    </td>
                            </tr>
-->
                            <tr>
                                <td align="center" colspan="3">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    For member</td>
                                <td align="left" colspan="2">
                                    <asp:DropDownList ID="ddlMember" runat="server">
                                    </asp:DropDownList>
                                    </td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    &nbsp;</td>
                                <td align="left" colspan="2">
                                    <asp:Button ID="btnGetPubByMember" runat="server" 
                                         Text="Get Publications" 
                                        onclick="btnGetPubByMember_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    &nbsp;</td>
                                <td align="left" colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr style="visibility: hidden">
                                <td align="right" class="txtfield">
                                    Member Name Start</td>
                                <td align="left" colspan="2">
                                    <asp:DropDownList ID="ddlStartWith" runat="server" AutoPostBack="True" 
                                        Width="198px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="visibility: hidden">
                                <td align="right" class="txtfield">
                                    &nbsp;</td>
                                <td align="left" colspan="2">
                                    <asp:Button ID="btnGetPubByStartLetter" runat="server" 
                                         Text="Get Publications" 
                                        onclick="btnGetPubByStartLetter_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    &nbsp;</td>
                                <td align="left" colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    For program</td>
                                <td align="left" colspan="2">
                                    <asp:DropDownList ID="ddlProgram" runat="server">
                                    </asp:DropDownList>
                                    </td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    &nbsp;</td>
                                <td align="left" colspan="2">
                                    <asp:Button ID="btnGetPubByProgram" runat="server" 
                                        onclick="btnGetPubByProgram_Click" 
                                         Text="Get Publications" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    &nbsp;</td>
                                <td align="left" colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    For all programs and members</td>
                                <td align="left" colspan="2">
                                    <asp:Button ID="btnGetPubAll" runat="server" 
                                        onclick="btnGetPubAll_Click" 
                                         Text="Get Publications" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    &nbsp;</td>
                                <td align="left" colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    Results</td>
                                <td align="left" colspan="2">
                                    <div style="text-align:center">
                                        <asp:GridView ID="gvResults" 
                                            runat="server" 
                                            AutoGenerateColumns="False" 
                                            >
                                            <Columns>
                                                <asp:TemplateField HeaderText="Program" ItemStyle-HorizontalAlign="Left"> 
                                                    <ItemTemplate> 
                                                        <asp:Label ID="lblProgram" runat="server" Text='<%# Bind("Program") %>'></asp:Label> 
                                                    </ItemTemplate> 
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField> 
                                                <asp:TemplateField HeaderText="Member" Visible="true" ItemStyle-HorizontalAlign="Left"> 
                                                    <ItemTemplate> 
                                                        <asp:Label ID="lblMember" runat="server" Text='<%# Bind("Member") %>'></asp:Label> 
                                                    </ItemTemplate> 
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField> 
                                                <asp:TemplateField HeaderText="No. of Pub" Visible="true" ItemStyle-HorizontalAlign="Left"> 
                                                    <ItemTemplate> 
                                                        <asp:Label ID="lblNoOfPub" runat="server" Text='<%# Bind("NoOfPub") %>'></asp:Label> 
                                                    </ItemTemplate> 
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField> 
                                                <asp:TemplateField HeaderText="No. Imported" Visible="true" ItemStyle-HorizontalAlign="center"> 
                                                    <ItemTemplate> 
                                                        <asp:Label ID="lblImport" runat="server" Text='<%# Bind("NoImport") %>'></asp:Label> 
                                                    </ItemTemplate> 
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField> 
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                 </td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    &nbsp;</td>
                                <td align="left" colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right" class="txtfield">
                                    &nbsp;</td>
                                <td align="left" colspan="2">
                                    <asp:Button ID="btnSpecialImport" runat="server" Text="Special Imports" 
                                        Width="136px" onclick="btnSpecialImport_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                                <tr>
                                    <td>
                        <table style="width: 100%" id="specialImportTable" visible="false" runat="server">
                            <tr>
                                <td align="center">
                                    <font color="maroon" size="2" style="font-weight:bold">
                                            Re-import special items<br /><br />
                                    </font>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnReImportTitle" runat="server" 
                                        onclick="btnReImportTitle_Click" Text="Re-import Title" Width="160px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnImportEntrezDate" runat="server" 
                                        onclick="btnImportEntrezDate_Click" Text="Import Entrez Date" Width="160px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnImportArticleEDate" runat="server" Text="Import Article E Date" 
                                        Width="160px" onclick="btnImportArticleEDate_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnImportCollectiveName" runat="server" 
                                        onclick="btnImportCollectiveName_Click" Text="Import CollectiveName" Width="160px" />
                                </td>
                            </tr>
                         </table>
                    </td>
                </tr>
        </table>
        <div class="clear2column">
        </div>
        </div>
    </div>
</asp:Content>


