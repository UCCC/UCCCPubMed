<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="MemberInstitution.aspx.cs" Inherits="MemberInstitution" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server" style="text-align:center">
                <font color="maroon" size="2" style="font-weight:bold">
                    Member Institution<br />
                </font>
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />
                <br />
				<DIV align="center">
                    <asp:DropDownList ID="ddlMember" runat="server" AutoPostBack="true" 
                                onselectedindexchanged="ddlMember_SelectedIndexChanged">
                    </asp:DropDownList></DIV><br />
                    &nbsp;<asp:HiddenField ID="hdnPublicationId" runat="server" />
                <div class="dashedline">
				<DIV align="center">
                    <asp:Label ID="lblNameLbl" runat="server"></asp:Label>
                    <asp:label id="lblName" runat="server" CssClass="clsAlert"></asp:label></DIV><br />

                <asp:datagrid id="dgPrimaryInstitution" runat="server" Font-Size="X-Small" 
                        GridLines="Vertical" CellPadding="4"
						BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#DEDFDE" AutoGenerateColumns="False" 
                        OnDeleteCommand="dgPrimaryInstitution_DeleteCommand"
						OnEditCommand="dgPrimaryInstitution_OnEditCommand" 
                        OnCancelCommand="dgPrimaryInstitution_CancelCommand" 
                        OnItemDataBound="dgPrimaryInstitution_ItemDataBound" 
                        OnUpdateCommand="dgPrimaryInstitution_UpdateCommand"
						OnItemCommand="DoInsert" ShowFooter="True" ForeColor="Black">
						<FooterStyle BackColor="#CCCC99"></FooterStyle>
						<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#CE5D5A"></SelectedItemStyle>
						<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
						<ItemStyle BackColor="#F7F7DE"></ItemStyle>
						<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#6B696B"></HeaderStyle>
						<Columns>
							<asp:BoundColumn Visible="False" DataField="client_institution_id" ReadOnly="True" HeaderText="client_institution_id"></asp:BoundColumn>
							<asp:TemplateColumn ItemStyle-Width="260px" HeaderText="Institution">
								<ItemTemplate>
									<asp:Label id="lblOrigInstitution" Width="240px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.institution") %>'>
									</asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:DropDownList id="ddlInstitution" Enabled="false" DataValueField="l_institution_id" DataTextField="institution"
										Width="240px" runat="server"></asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList id="ddlAddNewInstitution" DataValueField="l_institution_id" DataTextField="institution"
										Width="240px" runat="server"></asp:DropDownList>
								</FooterTemplate>
                                <ItemStyle Width="260px"></ItemStyle>
							</asp:TemplateColumn>
						    <asp:TemplateColumn ItemStyle-Width="80px" HeaderText="Primary?">
							    <ItemTemplate>
								    <asp:Label id="lblOrigYesNo" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.yes_no") %>'>
								    </asp:Label>
							    </ItemTemplate>
							    <EditItemTemplate>
								    <asp:DropDownList id="ddlYesNo" DataValueField="l_yes_no_id" DataTextField="yes_no"
									    Width="80px" runat="server"></asp:DropDownList>
							    </EditItemTemplate>
							    <FooterTemplate>
								    <asp:DropDownList id="ddlAddNewYesNo" DataValueField="l_yes_no_id" DataTextField="yes_no"
									    Width="80px" runat="server"></asp:DropDownList>
							    </FooterTemplate>
                                <ItemStyle Width="80px"></ItemStyle>
						    </asp:TemplateColumn>
							<asp:TemplateColumn ItemStyle-Width="80px" HeaderText="Start Date">
								<ItemTemplate>
									<asp:Label id=lblOrigStartDate Width="80px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.start_date") %>'>
									</asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox id="txtStartDate" onblur="DateFormat(this,this.value,event,true,'1')" onkeyup="DateFormat(this,this.value,event,false,'1')" onfocus="javascript:vDateType='1'" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.start_date") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="txtAddNewStartDate" onblur="DateFormat(this,this.value,event,true,'1')" onkeyup="DateFormat(this,this.value,event,false,'1')"
										onfocus="javascript:vDateType='1'" Columns="5" Width="80px" Runat="Server" />
								</FooterTemplate>
                                <ItemStyle Width="80px"></ItemStyle>
							</asp:TemplateColumn>
							<asp:TemplateColumn ItemStyle-Width="80px" HeaderText="End Date">
								<ItemTemplate>
									<asp:Label id="lblOrigEndDate" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.end_date") %>'>
									</asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox id="txtEndDate" onblur="DateFormat(this,this.value,event,true,'1')" onkeyup="DateFormat(this,this.value,event,false,'1')" onfocus="javascript:vDateType='1'" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.end_date") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="txtAddNewEndDate" onblur="DateFormat(this,this.value,event,true,'1')" onkeyup="DateFormat(this,this.value,event,false,'1')"
										onfocus="javascript:vDateType='1'" Columns="5" Width="80px" Runat="Server" />
								</FooterTemplate>
                                <ItemStyle Width="80px"></ItemStyle>
							</asp:TemplateColumn>
							<asp:TemplateColumn ItemStyle-Width="80px" HeaderText="Note">
								<ItemTemplate>
									<asp:Label id="lblNote" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.note") %>'>
									</asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox id="txtNote" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.note") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="txtAddNewNote" Columns="5" Width="80px" Runat="Server" />
								</FooterTemplate>
                                <ItemStyle Width="80px"></ItemStyle>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="">
								<FooterTemplate>
									<asp:LinkButton CommandName="Insert" Text="Add" ID="btnAdd" Runat="server" />
								</FooterTemplate>
								<ItemTemplate>
									<asp:LinkButton CommandName="Delete" Text="Delete" ID="btnDel" Runat="server" />
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn Visible="False" DataField="client_id" ReadOnly="True" HeaderText="client_id"></asp:BoundColumn>
						</Columns>
						<PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#F7F7DE" 
                            Mode="NumericPages"></PagerStyle>
					</asp:datagrid>
                    <br />


                </div>
            </div>
        </div>
</asp:Content>

