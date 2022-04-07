using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using System.IO;
using System.Xml.Serialization;
using System.Drawing.Imaging;

namespace HW8.Models
{
    [Serializable]
    public class Notes : INotifyPropertyChanged
    {

        private string h1;
        private string txt;
        private Bitmap? ffil;
        
        [XmlIgnore]
        public Bitmap? PathToFile
        {
            get => ffil;
            set
            {
                ffil = value;
                RaisePropertyChangedEvent("PathToFile");
            }
        }
        public string Text
        {
            get => txt;
            set
            {
                txt = value;
                RaisePropertyChangedEvent("Text");
            }
        }
        
        
        public Notes(string Header)
        {
            this.Header = Header;
            Text = "";
            PathToFile = null;
        }
        public Notes()
        {
            Header = "";
            Text = "";
            PathToFile = null;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }
        public string Header
        {
            get => h1;
            set
            {
                h1 = value;
                RaisePropertyChangedEvent("Header");
            }
        }
        public async void GetFile(Window parent)
        {
            var fileopen = new OpenFileDialog().ShowAsync(parent);
            string[]? path = await fileopen;
            if (path is not null)
            {
                string sourcePath = String.Join("/", path);
                FileInfo fileInfo = new FileInfo(sourcePath);
                using (FileStream fs = fileInfo.OpenRead())
                {
                    try
                    {
                        PathToFile = Bitmap.DecodeToWidth(fs, 100);
                    }
                    catch (Exception e)
                    {
                        PathToFile = null;
                    }
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("PathToFile")]
        public byte[] PathToFileSerialized
        {
            get
            {
                if (PathToFile == null) return null;
                using (MemoryStream s = new MemoryStream())
                {
                    PathToFile.Save(s);
                    return s.ToArray();
                }
            }
            set
            {
                if (value == null)
                {
                    PathToFile = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        PathToFile = new Bitmap(ms);
                    }
                }
            }
        }
    }
}
