using System.Speech.Synthesis;
using System.Windows;

namespace VirtualTtsAudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpeechSynthesizer synthesizer;

        public MainWindow()
        {
            InitializeComponent();
            
            // Initialize the speech synthesizer
            synthesizer = new SpeechSynthesizer();
            
            // Populate the voice combo box with installed voices
            var voices = synthesizer.GetInstalledVoices();
            foreach (var voice in voices)
            {
                VoiceComboBox.Items.Add(voice.VoiceInfo.Name);
            }
            
            if (VoiceComboBox.Items.Count > 0)
            {
                VoiceComboBox.SelectedIndex = 0;
            }

            // Wire up the test button click event
            TestButton.Click += TestButton_Click;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                MessageBox.Show("Please enter some text to speak.", "No Text", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                // Stop any ongoing speech
                synthesizer.SpeakAsyncCancelAll();

                // Set the selected voice
                if (VoiceComboBox.SelectedItem != null)
                {
                    synthesizer.SelectVoice(VoiceComboBox.SelectedItem.ToString()!);
                }

                // Speak the text asynchronously
                synthesizer.SpeakAsync(InputTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing speech: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            // Clean up the synthesizer when the window is closed
            synthesizer?.Dispose();
            base.OnClosed(e);
        }
    }
}