using System;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
namespace ScreenShot
{
    /// <summary>
    /// Класс для создания снимков экрана
    /// </summary>
    public class ScreenShotDll
    {
        /// <summary>
        ///Создание снимка
        /// </summary>
        /// <param name="fileName">Название файла.</param>
        /// <param name="imageFormat">Формат изображения.</param>
        public void CaptureScreen(string fileName, ImageFormat imageFormat)
        {
            int hdcSrc = User32.GetWindowDC(User32.GetDesktopWindow()),
            hdcDest = GDI32.CreateCompatibleDC(hdcSrc),
            hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc,
            GDI32.GetDeviceCaps(hdcSrc, 8), GDI32.GetDeviceCaps(hdcSrc, 10));
            GDI32.SelectObject(hdcDest, hBitmap);
            GDI32.BitBlt(hdcDest, 0, 0, GDI32.GetDeviceCaps(hdcSrc, 8),
            GDI32.GetDeviceCaps(hdcSrc, 10),
            hdcSrc, 0, 0, 0x00CC0020);
            SaveImageAs(hBitmap, fileName, imageFormat);
            Cleanup(hBitmap, hdcSrc, hdcDest);
        }
        /// <summary>
        /// Получение снимка в формате Windows Bitmap
        /// </summary>
        /// <returns></returns>
        public Bitmap CaptureScreen()
        {
            int hdcSrc = User32.GetWindowDC(User32.GetDesktopWindow()),
            hdcDest = GDI32.CreateCompatibleDC(hdcSrc),
            hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc,
            GDI32.GetDeviceCaps(hdcSrc, 8), GDI32.GetDeviceCaps(hdcSrc, 10));
            GDI32.SelectObject(hdcDest, hBitmap);
            GDI32.BitBlt(hdcDest, 0, 0, GDI32.GetDeviceCaps(hdcSrc, 8),
            GDI32.GetDeviceCaps(hdcSrc, 10),
            hdcSrc, 0, 0, 0x00CC0020);
            Bitmap btm = GetImage(hBitmap);
            Cleanup(hBitmap, hdcSrc, hdcDest);
            return btm;
        }
        /// <summary>
        /// Освобождение ресурсов системы.
        /// </summary>
        /// <param name="hBitmap">Указатель на структуру данных.</param>
        /// <param name="hdcSrc">Указатель на структуру данных источника.</param>
        /// <param name="hdcDest">Указатель на структуру данных назначения.</param>
        private void Cleanup(int hBitmap, int hdcSrc, int hdcDest)
        {
            User32.ReleaseDC(User32.GetDesktopWindow(), hdcSrc);
            GDI32.DeleteDC(hdcDest);
            GDI32.DeleteObject(hBitmap);
        }
        /// <summary>
        /// Сохранение снимка
        /// </summary>
        /// <param name="hBitmap">Указатель на изображение.</param>
        /// <param name="fileName">Название файла.</param>
        /// <param name="imageFormat">Формат изображения.</param>
        private void SaveImageAs(int hBitmap, string fileName, ImageFormat imageFormat)
        {
            Bitmap image = new Bitmap(Image.FromHbitmap(new IntPtr(hBitmap)),
            Image.FromHbitmap(new IntPtr(hBitmap)).Width,
            Image.FromHbitmap(new IntPtr(hBitmap)).Height);
            image.Save(fileName, imageFormat);
        }
        /// <summary>
        /// Возвращение изображения по указателю.
        /// </summary>
        /// <param name="hBitmap">Указатель на изображение.</param>
        /// <returns></returns>
        private Bitmap GetImage(int hBitmap)
        {
            Bitmap image =
            new Bitmap(Image.FromHbitmap(new IntPtr(hBitmap)),
            Image.FromHbitmap(new IntPtr(hBitmap)).Width,
            Image.FromHbitmap(new IntPtr(hBitmap)).Height);
            return image;
        }
    }
    /// <summary>
    /// Реализация методов библиотеки User32
    /// </summary>
    class User32
    {
        /// <summary>
        /// Возвращение указателя на рабочий стол.
        /// </summary>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int GetDesktopWindow();
        /// <summary>
        /// Получение структуры данных.
        /// </summary>
        /// <param name="hWnd">Указатель на окно.</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int GetWindowDC(int hWnd);
        /// <summary>
        /// Освобождение структуры данных.
        /// </summary>
        /// <param name="hWnd">Указатель на окно.</param>
        /// <param name="hDC">Указатель на структуру данных.</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int ReleaseDC(int hWnd, int hDC);
    }
    /// <summary>
    /// Реализация методов библиотеки GDI32
    /// </summary>
    class GDI32
    {
        /// <summary>
        /// Передача изображения.
        /// </summary>
        /// <param name="hdcDest">Указатель на назначение передачи.</param>
        /// <param name="nXDest">Х координата верхнего левого угла назначения.</param>
        /// <param name="nYDest">У координата верхнего левого угла назначения.</param>
        /// <param name="nWidth">Ширина прямоугольника.</param>
        /// <param name="nHeight">Высота прямоугольника.</param>
        /// <param name="hdcSrc">Указатель на источник.</param>
        /// <param name="nXSrc">Х координата верхнего левого угла источника.</param>
        /// <param name="nYSrc">У координата верхнего левого угла источника.</param>
        /// <param name="dwRop">Код растровой операции.</param>
        /// <returns></returns>
        [DllImport("GDI32.dll")]
        public static extern bool BitBlt(int hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, int hdcSrc,
        int nXSrc, int nYSrc, int dwRop);
        /// <summary>
        /// Создание и запись в структуру данных изображения.
        /// </summary>
        /// <param name="hdc">Указатель на структуру данных.</param>
        /// <param name="nWidth">Ширина изображения.</param>
        /// <param name="nHeight">Высота изображения.</param>
        /// <returns></returns>
        [DllImport("GDI32.dll")]
        public static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);
        /// <summary>
        /// Создание и сохранение в памяти структуры данных, совместимой с указанным устройством вывода.
        /// Для помещения изображения в структуру данных
        /// необходимо указать высоту, ширину и цветовой режим изображения.
        /// Это можно сделать с помощью функции CreateCompatibleBitmap, поддерживаемой устройствами с растровым выводом.
        
         /// Для получения информации об этих устройствах используется функция GetDeviceCaps.
         /// После использования структуры данных ее нужно удалить с помощью функции DeleteDC.
         /// </summary>
         /// <param name="hdc">Указатель на существующую структуру данных. Если указатель равен null,
         /// то функция создает структуру для экрана текущего приложения.</param>
         /// <returns></returns>
 [DllImport("GDI32.dll")]
    public static extern int CreateCompatibleDC(int hdc);
        /// <summary>
        /// Удаление указанной структуры данных.
        /// </summary>
        /// <param name="hdc">Структура данных.</param>
        /// <returns></returns>
        [DllImport("GDI32.dll")]
        public static extern bool DeleteDC(int hdc);
        /// <summary>
        /// Удаление графических объектов с освобождением системных ресурсов.
        /// </summary>
        /// <param name="hObject">Указатель на графический объект.</param>
        /// <returns></returns>
        [DllImport("GDI32.dll")]
        public static extern bool DeleteObject(int hObject);
        /// <summary>
        /// Возвращение информации об указанной структуре.
        /// </summary>
        /// <param name="hdc">Указатель на структуру данных.</param>
        /// <param name="nIndex">Индекс совместимости. .</param>
        /// <returns></returns>
        [DllImport("GDI32.dll")]
        public static extern int GetDeviceCaps(int hdc, int nIndex);
        /// <summary>
        /// Выбор объекта, находящегося в указанной структуре данных.
        /// </summary>
        /// <param name="hdc">Указатель на структуру данных.</param>
        /// <param name="hgdiobj">Указатель на объект.</param>
        /// <returns></returns>
        [DllImport("GDI32.dll")]
        public static extern int SelectObject(int hdc, int hgdiobj);
    }
}