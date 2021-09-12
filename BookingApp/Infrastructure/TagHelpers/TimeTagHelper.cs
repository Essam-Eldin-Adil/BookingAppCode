using Domain;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Infrastructure.TagHelpers
{
    [HtmlTargetElement("distime")]
    public class TimeTagHelper : TagHelper
    {
        public string Mode { get; set; }
        public string Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            var sb = new StringBuilder();
            

            if (Mode == "12")
            {
                sb.AppendFormat("<span>Hi! {0}</span>", DateTimeHelper.Convert24HourTo12Hour(Value));
            }
            else
            {
                sb.AppendFormat("<span>Hi! {0}</span>", DateTimeHelper.Convert12HourTo24Hour(Value));
            }
            output.Content.SetContent(sb.ToString());

           // output.PreContent.SetHtmlContent(sb.ToString());

        }
    }
}
