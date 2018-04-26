using BlazorAnt.Utils;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;

namespace BlazorAnt.Components
{
    public class AntIconComponent : BlazorComponent
    {
        private const string ICON_CLASS = "anticon";
        public string Type { get; set; } = "loading";
        public string ClassName { get; set; }
        public string Title { get; set; }
        public Action<UIMouseEventArgs> OnClick { get; set; }
        public bool Spin { get; set; }
        public string Style { get; set; }

        protected string ClassString { get; set; }

        protected override void OnInit()
        {
            this.RefreshClasses();
        }

        private void RefreshClasses()
        {
            var classes = ClassNamesBuilder.From(null,
                new[] { ICON_CLASS },
                new Dictionary<string, Func<bool>>
                {
                    { $"{ICON_CLASS}-spin", () => this.Spin || this.Type == "loading" },
                    { $"{ICON_CLASS}-{this.Type}", () => true }
                });

            this.ClassString = string.IsNullOrWhiteSpace(this.ClassName) ? classes : $"{this.ClassName} {classes}";
        }
    }
}
