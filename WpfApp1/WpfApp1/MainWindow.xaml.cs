using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var url = TextBoxUrl.Text;
            string message = "You must enter a valid kickasstorrents.to url";
            string title = "Invalid url";

            if (!url.Contains("kickasstorrents.to"))
            {
                MessageBox.Show(message, title);
                return;
            }

            try
            {
                var getHtml = await GrabHtmlAsync(url);

                if (getHtml.Contains("data-hash") || getHtml.Contains("Torrent hash: "))
                {
                    BypassHash(getHtml);
                }
                else
                {
                    TextBoxUrl.Text = "Hash not found.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task<String> GrabHtmlAsync(string url)
        {
            var webClient = new WebClient();
            return await webClient.DownloadStringTaskAsync(url);
        }

        private void BypassHash(string html)
        {
            Regex regex = new Regex(@"([A-Z\d]{40})");

            Match match = regex.Match(html);

            if (match.Success)
            {
                TextBoxHash.Text = match.Value;
            }
        }
    }
}
