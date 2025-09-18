using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dyno.ViewModels;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.FormControls
{
    [ProtoContract]
    public class FormImage : FormControl
    {
        static FormImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormImage), new FrameworkPropertyMetadata(typeof(FormImage)));
        }

        public FormImage()
        {
            Width = 100;
            Height = 100;
        }



        protected override void OnRender(DrawingContext drawingContext)
        {
            VisualBitmapScalingMode = BitmapScalingMode.HighQuality;
            base.OnRender(drawingContext);
        }

        [ProtoMember(3)]
        public byte[] ImBytes;

        public static readonly DependencyProperty EditorImageProperty = DependencyProperty.Register("EditorImage", typeof(string), typeof(FormImage), new FrameworkPropertyMetadata(OnImChanged));

        private static void OnImChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FormImage).ImBytes = null;

            OnChanged(d, e);
        }

        [Category(FormControlHelper.PropCategory.Binding)]
        [DisplayName("Image")]
        [Editor("ImagePath", "")]
        [ProtoMember(1)]
        public string EditorImage
        {
            get { return GetValue(EditorImageProperty) as string; }
            set { SetValue(EditorImageProperty, value); }
        }

        public static readonly DependencyProperty EditorStretchProperty = DependencyProperty.Register("EditorStretch", typeof(Stretch), typeof(FormImage));
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName("Image Stretch Mode")]
        [ProtoMember(2)]
        public Stretch EditorStretch
        {
            get { return (Stretch)GetValue(EditorStretchProperty); }
            set { SetValue(EditorStretchProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(BitmapSource), typeof(FormImage));
        public BitmapSource Source
        {
            get { return GetValue(SourceProperty) as BitmapSource; }
            set { SetValue(SourceProperty, value); }
        }

        public override void Update()
        {

            if (DynoManagerBase.SelectedWorkspacePreset != null && EditorImage != null)
            {
                var dir = new FileInfo(DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspacePath).DirectoryName;
                if (dir != null)
                {
                    var path = Path.Combine(dir, EditorImage);
                    if (ImBytes == null && File.Exists(path))
                    {
                        Source = new BitmapImage(new Uri(path, UriKind.Absolute));
                        ImBytes = ConvertToBytes(Source as BitmapImage);
                    }
                    else if (ImBytes != null)
                    {
                        Source = ConvertFromBytes(ImBytes);
                    }


                }
            }

            base.Update();
        }

        public static byte[] ConvertToBytes(BitmapImage bitmapImage)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        public static BitmapSource ConvertFromBytes(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var decoder = BitmapDecoder.Create(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                var frame = decoder.Frames[0];

                return frame;
            }
        }
    }
}
