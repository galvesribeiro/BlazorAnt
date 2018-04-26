using BlazorAnt.Utils;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;

namespace BlazorAnt.Components
{
    public class AntDividerComponent : BlazorComponent
    {
        private const string DIVIDER_CLASS = "divider";
        public string PrefixCls { get; set; } = "ant";
        public DividerType Type { get; set; } = DividerType.Horizontal;
        public DividerOrientation Orientation { get; set; } = DividerOrientation.Undefined;
        public string ClassName { get; set; }
        public RenderFragment ChildContent { get; set; }
        public bool Dashed { get; set; }
        public string Style { get; set; }

        protected string ClassString { get; private set; }

        protected override void OnInit()
        {
            var classes = ClassNamesBuilder.From(this.PrefixCls,
                new[] { DIVIDER_CLASS, this.GetTypeClass() },
                new Dictionary<string, Func<bool>>
                {
                    { $"{DIVIDER_CLASS}-with-text{this.GetOrientationClass()}", () => this.ChildContent != null },
                    { $"{DIVIDER_CLASS}-dashed", () => this.Dashed }
                });
            this.ClassString = string.IsNullOrWhiteSpace(this.ClassName) ? classes : $"{this.ClassName} {classes}";
        }

        private string GetTypeClass()
        {
            if (this.Type == DividerType.Vertical)
                return $"{DIVIDER_CLASS}-vertical";

            return $"{DIVIDER_CLASS}-horizontal";
        }

        private string GetOrientationClass()
        {
            switch (this.Orientation)
            {
                case DividerOrientation.Left:
                    return $"-left";
                case DividerOrientation.Right:
                    return $"-right";
                default:
                    return string.Empty;
            }
        }
    }

    public enum DividerType
    {
        Horizontal,
        Vertical
    }

    public enum DividerOrientation
    {
        Undefined,
        Left,
        Right
    }
}
