﻿using Microsoft.AspNetCore.Components;
using System;
using Microsoft.JSInterop;

namespace Elevenworks.Graphics.Blazor
{
    public class CanvasComponentBase : ComponentBase
    {
        private BlazorCanvas _blazorCanvas;

        [Parameter]
        protected long width { get; set; }

        [Parameter]
        protected long height { get; set; }

        [Inject] 
        public IJSRuntime JSRuntime { get; set; }
        
        protected readonly string id = Guid.NewGuid().ToString();

        protected ElementRef canvas;

        internal ElementRef CanvasReference => canvas;

        public string Id => id;

        private bool _initialized;
        private float _displayScale = 1;
        
        public BlazorCanvas BlazorCanvas
        {
            get
            {
                if (_blazorCanvas == null)
                {
                    _blazorCanvas = new BlazorCanvas
                    {
                        Context = new Canvas2D.CanvasRenderingContext2D(this),
                        DisplayScale = _displayScale
                    };
                }

                return _blazorCanvas;
            }
        }
        
        protected override async void OnAfterRender()
        {
            if (!_initialized)
            {
                _initialized = true;
                _displayScale = await JSRuntime.SetupCanvas(Id);
            }
        }
    }
}
