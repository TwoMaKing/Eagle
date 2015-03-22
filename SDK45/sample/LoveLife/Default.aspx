<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LoveLife._Default" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="DefaultHeadContent">
    
    <script type="text/javascript" src="Scripts/jquery.signalR-2.2.0.js"></script>
    
    <script src="/signalr/hubs"></script>

    <script type="text/javascript">

        $(function(){

            var conn = $.connection('/PostPush');

            conn.received(function (data) {
                //debugger;
                var posts = JSON.parse(data);

                var cs;
                $.each(posts, function (i, p) {
                    //debugger;
                    cs += p.Content;
                });

                $('#txtMessage').val(cs);
            });

            conn.start().done(function () {
                conn.send("");
            });

            $("#btnPublish").click = function () {
                conn.send($("#txtMessage").val());
            }

            //var pushHub = $.connection.PostPushHub;

            //$.connection.hub.start().done(function(){

            //    pushHub.server.connect();

            //});


        });


        //function registerSignalR(hubClient) {

        //    hubClient.client.

        //}

    </script>

</asp:Content>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <p>
                Xpress Love Life
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>文章列表</h3>

    <div>
        <p>
            SignalR 实现实时通信 让客户端（Web页面）和服务器端可以互相通知消息及调用方法
        </p>
        <input id="txtMessage" type="text" style="width:100px;" />
        <button id="btnPublish">Publish</button>
    </div>


    <%  
        foreach(var p in this.AllPosts)
        { 
    %>
            <ul>
                <li>
                    <h5>
                        <%: p.Author.Name %>
                    </h5>
                    <p>
                        <%: p.Content %>
                    </p>
                    <a href="Post/<%: p.CreationDateTime.ToString("yyyy-MM-dd") %>/<%: p.Id %>">Details</a>
                </li>
            </ul>         
   <%
        }
        
   %>


    <ul class="round">
        <asp:Repeater ID="Posts" runat="server">
            <ItemTemplate>
                <li class="one">
                    <h5><%# Eval("Author.Name") %></h5>
                    <h5><%# Eval("Content") %></h5>
                    <a href="Contact.aspx?date=<%# Eval("CreationDateTime", "yyyy-MM-dd") %>&id=<%# Eval("Id") %>">Details</a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>


    <ol class="round">
        <li class="one">
            <h5>Getting Started</h5>
            <a href="http://go.microsoft.com/fwlink/?LinkId=245146">Learn more…</a>
        </li>
        <li class="two">
            <h5>Add NuGet packages and jump-start your coding</h5>
            <a href="http://go.microsoft.com/fwlink/?LinkId=245147">Learn more…</a>
        </li>
        <li class="three">
            <h5>Find Web Hosting</h5>
            <a href="http://go.microsoft.com/fwlink/?LinkId=245143">Learn more…</a>
        </li>
    </ol>
</asp:Content>
