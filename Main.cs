using System;
using System.Windows.Forms;

namespace ScreenShot
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "Файлы bmp|*.bmp";
            if (diag.ShowDialog() != DialogResult.OK)
                return;
            // Получаем адрес файла.
            string filename = diag.FileName;
            // Создаем экземпляр класса ScreenShotDll
            ScreenShotDll shoter = new ScreenShotDll();
            // Создаем и сохраняем изображение.
            shoter.CaptureScreen(filename, System.Drawing.Imaging.ImageFormat.Bmp);


        }

  
    }
}
