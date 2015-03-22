<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="LoveLife.Contact" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1></h1>
    </hgroup>

    <section class="contact">
        <header>
            <h3>Post</h3>
        </header>
        <div>
            <span class="label">Author:</span>
            <p>
                <%= this.CurrentPost.Author.Name %>
            </p>
        </div>
        <div>
            <span class="label">Content:</span>
            <p>
                <%= this.CurrentPost.Content %>
            </p>
        </div>
    </section>
</asp:Content>
