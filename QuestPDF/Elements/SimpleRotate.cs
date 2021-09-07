﻿using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    internal class SimpleRotate : ContainerElement
    {
        public int TurnCount { get; set; }
        public int NormalizedTurnCount => (TurnCount % 4 + 4) % 4;
        
        internal override SpacePlan Measure(Size availableSpace)
        {
            if (NormalizedTurnCount == 0 || NormalizedTurnCount == 2)
                return base.Measure(availableSpace);
            
            availableSpace = new Size(availableSpace.Height, availableSpace.Width);
            var childSpace = base.Measure(availableSpace);

            if (childSpace.Type == SpacePlanType.Wrap)
                return SpacePlan.Wrap();

            var targetSpace = new Size(childSpace.Height, childSpace.Width);

            if (childSpace.Type == SpacePlanType.FullRender)
                return SpacePlan.FullRender(targetSpace);
            
            if (childSpace.Type == SpacePlanType.PartialRender)
                return SpacePlan.PartialRender(targetSpace);

            throw new ArgumentException();
        }
        
        internal override void Draw(Size availableSpace)
        {
            var skiaCanvas = (Canvas as Drawing.SkiaCanvasBase)?.Canvas;
            
            if (skiaCanvas == null)
                return;

            var currentMatrix = skiaCanvas.TotalMatrix;

            if (NormalizedTurnCount == 1 || NormalizedTurnCount == 2)
                skiaCanvas.Translate(availableSpace.Width, 0);
            
            if (NormalizedTurnCount == 2  || NormalizedTurnCount == 3)
                skiaCanvas.Translate(0, availableSpace.Height);

            skiaCanvas.RotateDegrees(NormalizedTurnCount * 90);
            
            if (NormalizedTurnCount == 1 || NormalizedTurnCount == 3)
                availableSpace = new Size(availableSpace.Height, availableSpace.Width);
            
            base.Draw(availableSpace);
            skiaCanvas.SetMatrix(currentMatrix);
        }
    }
}