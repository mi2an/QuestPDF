﻿
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    internal class Translate : ContainerElement
    {
        public float TranslateX { get; set; } = 1;
        public float TranslateY { get; set; } = 1;

        internal override void Draw(Size availableSpace)
        {
            var skiaCanvas = (Canvas as Drawing.SkiaCanvasBase)?.Canvas;
            
            if (skiaCanvas == null)
                return;
            
            skiaCanvas.Translate(TranslateX, TranslateY);
            base.Draw(availableSpace);
            skiaCanvas.Translate(-TranslateX, -TranslateY);
        }
    }
}