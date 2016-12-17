<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ReSTWebClient._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to wcf rest client test!
    </h2>
    <p>
        Enter some text and hit the button <br />
         <asp:TextBox ID="txtInput" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtInput" ForeColor="Red"
            runat="server" ErrorMessage="Enter something in text box"></asp:RequiredFieldValidator>
       <br />
       <asp:Button ID="btnCallReSTService" runat="server" Text="Button" 
            onclick="btnCallReSTService_Click" />
    </p>
    <p>
        <asp:Label ID="lblCaption" runat="server" Text="Response message: "></asp:Label>
        <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
    </p>
    <p>
        &nbsp;</p>
   
</asp:Content>
