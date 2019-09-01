using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using WalkingTec.Mvvm.Core.Extensions;

namespace WalkingTec.Mvvm.TagHelpers.LayUI
{
    public enum ButtonSizeEnum { Big, Normal, Small, Mini }
    public enum ButtonThemeEnum { Primary, Normal, Warm, Danger, Disabled }
    public abstract class BaseButtonTag : BaseElementTag
    {
        /// <summary>
        /// 按钮尺寸,默认为Normal
        /// </summary>
        public ButtonSizeEnum? Size { get; set; }

        /// <summary>
        /// 按钮风格,默认为Normal
        /// </summary>
        public ButtonThemeEnum? Theme { get; set; }

        /// <summary>
        /// 按钮图标
        /// 图标字符串格式参考 http://www.layui.com/doc/element/icon.html
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 是否圆角
        /// </summary>
        public bool IsRound { get; set; }

        /// <summary>
        /// 按钮文字
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 点击事件调用的js方法，如doclick()
        /// </summary>
        public string Click { get; set; }

        public bool Disabled { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToNoSplitString();
            }
            if (output.TagName != "a")
            {
                output.TagName = "button";
                output.TagMode = TagMode.StartTagAndEndTag;
                var btnclass = "layui-btn";
                if (Size != null && Size != ButtonSizeEnum.Normal)
                {
                    switch (Size)
                    {
                        case ButtonSizeEnum.Big:
                            btnclass += " layui-btn-lg";
                            break;
                        case ButtonSizeEnum.Small:
                            btnclass += " layui-btn-sm";
                            break;
                        case ButtonSizeEnum.Mini:
                            btnclass += " layui-btn-xs";
                            break;
                        default:
                            break;
                    }
                }
                if(Disabled == true)
                {
                    Theme = ButtonThemeEnum.Disabled;
                    output.Attributes.SetAttribute(new TagHelperAttribute("disabled"));
                }
                if (Theme != null && Theme != ButtonThemeEnum.Normal)
                {
                    btnclass += " layui-btn-" + Theme.Value.ToString().ToLower();
                }
                output.Attributes.SetAttribute("class", btnclass);
                if (string.IsNullOrEmpty(Icon) == false)
                {
                    output.Content.SetHtmlContent($@"<i class=""{Icon}""></i> {Text ?? ""}");
                }
                else
                {
                    output.Content.SetHtmlContent(Text ?? string.Empty);
                }
            }
            else
            {
                output.Content.SetHtmlContent(Text ?? string.Empty);
            }
            if (string.IsNullOrEmpty(Click) == false && Disabled == false)
            {
                output.PostElement.AppendHtml($@"
<script>
  $('#{Id}').on('click',function(){{
    {Click};
    return false;
}});
</script>
");
            }
            base.Process(context, output);
        }

    }
}
