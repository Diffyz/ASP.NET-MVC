<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryList.ascx.cs" 
   Inherits="GameStore.Controls.CategoryList" %>

    <link rel="stylesheet" href="/Content/Styles.css" />

<%= CreateHomeLinkHtml() %>

<% foreach (string category in GetCategories()) {
       Response.Write(CreateLinkHtml(category));       
}%>