using ZXing;
using ZXing.Common;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing;

namespace QRCodeGenerator
{
    public partial class Main : Form
    {
        private readonly IBarcodeReader barcodeReader; 

        public Main()
        {
            InitializeComponent();

            barcodeReader = new BarcodeReader(); 
        }

        private void btnEncode_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxData.Text))
            {
                MessageBox.Show("Vui lòng nhập nội dung cần mã hóa", 
                    "QR Code Generator");
                return;
            }

            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 150,
                    Width = 150,
                    Margin = 0
                }
            };

            string content = tbxData.Text;

            using (var bitmap = barcodeWriter.Write(content))
            {
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    var image = Image.FromStream(stream);
                    pictureBox.Image = image;
                }
            }
        }

        private void btnDecode_Click(object sender, System.EventArgs e)
        {
            using (var openDlg = new OpenFileDialog())
            {
                openDlg.Multiselect = false;
                if (openDlg.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = openDlg.FileName;
                    if (!File.Exists(fileName))
                    {
                        return;
                    }

                    var image = (Bitmap)Bitmap.FromFile(fileName);
                    pictureBox.Image = image;

                    var result = barcodeReader.Decode(image);
                    if (result == null)
                    {
                        result = barcodeReader.Decode(image);
                    }

                    if (result == null)
                    {
                        MessageBox.Show("Không nhận diện được mã QR Code",
                        "QR Code Generator");
                    }
                    else
                    {
                        tbxData.Text = result.Text;
                    }
                }
            }
        }
    }
}
