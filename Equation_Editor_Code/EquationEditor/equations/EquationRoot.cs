using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace Editor
{
    public class EquationRoot : EquationContainer
    {
        Caret vCaret;
        Caret hCaret;

        public EquationRoot(Caret vCaret, Caret hCaret)
            : base(null)
        {
            this.vCaret = vCaret;
            this.hCaret = hCaret;
            ActiveChild = new RowContainer(this, 0.3);
            childEquations.Add(ActiveChild);
            ActiveChild.Location = Location = new Point(15, 15);
            AdjustCarets();
        }

        public override bool ConsumeMouseClick(Point mousePoint)
        {
            ActiveChild.ConsumeMouseClick(mousePoint);
            AdjustCarets();
            return true;
        }
        
        public void HandleUserCommand(CommandDetails commandDetails)
        {
            if (commandDetails.CommandType == CommandType.Text)
            {
                ConsumeText(commandDetails.UnicodeString); //ConsumeText() will call DeSelect() itself. No worries here
            }
            else
            {
                ((EquationContainer)ActiveChild).ExecuteCommand(commandDetails.CommandType, commandDetails.CommandParam);
                CalculateSize();
                AdjustCarets();
            }
        }

        public void AdjustCarets()
        {
            vCaret.Location = ActiveChild.GetVerticalCaretLocation();
            vCaret.CaretLength = ActiveChild.GetVerticalCaretLength();
            EquationContainer innerMost = ((RowContainer)ActiveChild).GetInnerMostEquationContainer();
            hCaret.Location = innerMost.GetHorizontalCaretLocation();
            hCaret.CaretLength = innerMost.GetHorizontalCaretLength();
        }


        public override void ConsumeText(string text)
        {
            ActiveChild.ConsumeText(text);
            CalculateSize();
            AdjustCarets();
        }
        
        public void DrawVisibleRows(DrawingContext dc, double top, double bottom)
        {
            ((RowContainer)ActiveChild).DrawVisibleRows(dc, top, bottom);
        }

        public void SaveImageToFile(string path, RichTextBox richTextBox)
        {
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, Math.Ceiling(Width + Location.X * 2), Math.Ceiling(Height + Location.Y * 2)));
                ActiveChild.DrawEquation(dc);
            }
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)(Math.Ceiling(Width + Location.X * 2)), (int)(Math.Ceiling(Height - 20 + Location.Y * 2)), 96, 96, PixelFormats.Default);
            bitmap.Render(dv);
            BitmapEncoder encoder = new BmpBitmapEncoder();
            try
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                MemoryStream ms = new MemoryStream();
                using (Stream s = File.Create(path))
                {
                    encoder.Save(s);
                }
                using (Stream s = File.OpenRead(path))
                {
                    System.Drawing.Bitmap b = new System.Drawing.Bitmap(System.Drawing.Image.FromStream(s));
                    int h = b.Height;
                    int w = b.Width;
                    System.Drawing.Size size = new System.Drawing.Size(w, h);
                    w = (int)((w * 2f) / 3f);
                    h = (int)((h * 2f) / 3f);
                    size = new System.Drawing.Size(w, h);
                    b = new System.Drawing.Bitmap(System.Drawing.Image.FromStream(s), size);
                    System.Drawing.Image im = (System.Drawing.Image)b;
                    string str = System.Windows.Forms.Clipboard.GetText();
                    System.Windows.Forms.Clipboard.SetText(" ");
                    richTextBox.Paste();
                    System.Windows.Forms.Clipboard.SetImage(im);
                    richTextBox.Paste();
                    try { System.Windows.Forms.Clipboard.SetText(str); } catch { }
                }
                File.Delete(path);
            }
            catch
            {
                MessageBox.Show("Fatal error!", "Error");
            }
        }

        public override bool ConsumeKey(Key key)
        {            
            Key[] handledKeys = { Key.Left, Key.Right, Key.Delete, Key.Up, Key.Down, Key.Enter, Key.Escape, Key.Back, Key.Home, Key.End };
            bool result = false;
            if (handledKeys.Contains(key))
            {
                result = true;
                ActiveChild.ConsumeKey(key);
                CalculateSize();
                AdjustCarets();
            }
            return result;
        }

        protected override void CalculateWidth()
        {
            Width = ActiveChild.Width;
        }

        protected override void CalculateHeight()
        {
            Height = ActiveChild.Height;
        }

        public void ZoomOut(int difference)
        {
            FontSize -= difference;
        }

        public void ZoomIn(int difference)
        {
            FontSize += difference;
        }

        public override double FontSize
        {
            get { return base.FontSize; }
            set
            {
                base.FontSize = value;
                AdjustCarets();
            }
        }
    }
}
