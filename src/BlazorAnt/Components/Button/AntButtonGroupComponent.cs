using BlazorAnt.Utils;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;

namespace BlazorAnt.Components.Button
{
    public class AntButtonGroupComponent : BlazorComponent
    {
        private const string BUTTON_GROUP_CLASS = "btn-group";
        public ButtonSize Size { get; set; }
        public string Style { get; set; }
        public string ClassName { get; set; }
        public string PrefixCls { get; set; }

        public RenderFragment ChildContent { get; set; }

        protected string ClassString { get; private set; }

        protected override void OnInit()
        {
            var classes = ClassNamesBuilder.From(this.PrefixCls,
                new[] { BUTTON_GROUP_CLASS },
                new Dictionary<string, Func<bool>>
                {
                    { $"{BUTTON_GROUP_CLASS}-{this.GetSizeClass()}", () => this.Size != ButtonSize.Default },
                });

            this.ClassString = string.IsNullOrWhiteSpace(this.ClassName) ? classes : $"{this.ClassName} {classes}";
        }

        private string GetSizeClass()
        {
            switch (this.Size)
            {
                case ButtonSize.Small:
                    return "sm";
                case ButtonSize.Large:
                    return "lg";
                default:
                    return "size-undefined";
            }
        }
    }
}
