namespace SML.Extensions
{
    public partial class ImageViewer : Form
    {
        private Image _origin;

        public ImageViewer(Image image)
        {
            this._origin = image;
            this.InitializeComponent();
            this.ShowImage();
        }

        private void ShowImage() =>
            this.BackgroundImage = new Bitmap(this._origin, this.Size);

        private void OnResize(object sender, EventArgs e) =>
            this.ShowImage();
    }
}
