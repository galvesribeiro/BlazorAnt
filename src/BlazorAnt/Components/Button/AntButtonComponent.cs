using BlazorAnt.Utils;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BlazorAnt.Components
{
    public class AntButtonComponent : BlazorComponent, IDisposable
    {
        private const string BUTTON_CLASS = "btn";

        #region Attributes
        public ButtonType Type { get; set; }
        public ButtonHTMLType HtmlType { get; set; }
        public string Icon { get; set; }
        public ButtonShape Shape { get; set; }
        public ButtonSize Size { get; set; }
        public Action<UIMouseEventArgs> OnClick { get; set; }
        public Action<UIMouseEventArgs> OnMouseUp { get; set; }
        public Action<UIMouseEventArgs> OnMouseDown { get; set; }
        public Action<UIKeyboardEventArgs> OnKeyPress { get; set; }
        public Action<UIKeyboardEventArgs> OnKeyDown { get; set; }
        public Action<UIKeyboardEventArgs> OnKeyUp { get; set; }
        public int TabIndex { get; set; }
        public bool Loading { get; set; }
        public bool Disabled { get; set; }
        public string Style { get; set; }
        public string PrefixCls { get; set; } = "ant";
        public string ClassName { get; set; }
        public bool Ghost { get; set; }
        public string Target { get; set; }
        public string HRef { get; set; }
        public string Download { get; set; }
        #endregion

        public RenderFragment ChildContent { get; set; }

        protected string IconType { get; private set; }
        protected bool Clicked { get; private set; }
        protected string ClassString { get; private set; }

        private Timer _timeout;
        private Timer _delayTimeout;

        public override void SetParameters(ParameterCollection parameters)
        {
            var currentLoadingValue = this.Loading;

            if (currentLoadingValue)
            {
                this._delayTimeout?.Dispose();
            }

            var parametersCopy = parameters.ToDictionary();
            if (parametersCopy.TryGetValue(nameof(this.Loading), out object nextLoadingValue))
            {
                this._delayTimeout?.Dispose();
                this._delayTimeout = new Timer(_ =>
                {
                    this.Loading = false;
                    this.StateHasChanged();
                }, null, 0, 500);
            }

            base.SetParameters(parameters);
        }

        protected override void OnInit() => this.RefreshClasses();

        private void RefreshClasses()
        {
            var classes = ClassNamesBuilder.From(this.PrefixCls,
                    new[] { BUTTON_CLASS, $"{BUTTON_CLASS}-{this.GetTypeClass()}" },
                    new Dictionary<string, Func<bool>>
                    {
                        { $"{BUTTON_CLASS}-{this.GetShapeClass()}", () => this.Shape != ButtonShape.Undefined },
                        { $"{BUTTON_CLASS}-{this.GetSizeClass()}", () => this.Size != ButtonSize.Default },
                        { $"{BUTTON_CLASS}-icon-only", () => this.ChildContent == null && !string.IsNullOrWhiteSpace(this.Icon)},
                        { $"{BUTTON_CLASS}-loading", () => this.Loading },
                        { $"{BUTTON_CLASS}-clicked", () => this.Clicked },
                        { $"{BUTTON_CLASS}-background-ghost", () => this.Ghost }
                    });

            this.IconType = this.Loading && string.IsNullOrWhiteSpace(this.Icon) ? "loading" : this.Icon;

            this.ClassString = string.IsNullOrWhiteSpace(this.ClassName) ? classes : $"{this.ClassName} {classes}";
        }

        protected void HandleClick(UIMouseEventArgs args)
        {
            this.Clicked = true;
            this.RefreshClasses();
            this.StateHasChanged();

            this._timeout?.Dispose();
            this._timeout = new Timer(_ =>
            {
                this.Clicked = false;
                this.RefreshClasses();
                this.StateHasChanged();
                this._timeout.Dispose();
            }, null, 500, 500);

            this.OnClick?.Invoke(args);
        }

        public void Dispose()
        {
            this._delayTimeout?.Dispose();
            this._timeout?.Dispose();
        }

        private string GetShapeClass()
        {
            switch (this.Shape)
            {
                case ButtonShape.Circle:
                    return "circle";
                case ButtonShape.CircleOutline:
                    return "circle-outline";
                default:
                    return "circle-undefined";
            }
        }

        private string GetTypeClass()
        {
            switch (this.Type)
            {
                case ButtonType.Primary:
                    return "primary";
                case ButtonType.Ghost:
                    return "ghost";
                case ButtonType.Dashed:
                    return "dashed";
                case ButtonType.Danger:
                    return "danger";
                default:
                    return "default";
            }
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
