﻿using Client.Models;
using System;
using System.Text;
using System.Web.Mvc;

namespace Client.HtmlHelpers
{
    /// <summary>
    /// Class that is responsible for implementation of pagination
    /// </summary>
    public static class PagingHelpers
    {
        /// <summary>
        /// Extension method that is responsible for creating pagination HTML helper
        /// </summary>
        public static MvcHtmlString PageLinks(this HtmlHelper html,
              PageInfo pageInfo, Func<int,string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();

                if (i == pageInfo.PageNumber)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}