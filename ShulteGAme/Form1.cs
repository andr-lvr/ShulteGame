using System;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace ShulteGAme
{
    public partial class Form1 : Form
    {
        private int[] shuffledNumbers;
        private int currentIndex;
        private int totalTime;
        private DateTime startTime;
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Initialize timer
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Tick;

            // Shuffle numbers
            shuffledNumbers = GenerateShuffledNumbers(40);
            currentIndex = 0;

            // Display shuffled numbers on buttons and assign random colors
            DisplayShuffledNumbers();

            // Attach button events
            AttachButtonEvents();
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            totalTime = (int)(DateTime.Now - startTime).TotalSeconds;
            label1.Text = "Timer: " + totalTime.ToString() + "s";

            if (totalTime >= progressBar1.Maximum)
            {
                MessageBox.Show("Time's up! You Lose!");
                timer.Stop();
                ResetGame();
            }

            if (totalTime % 5 == 0) // Adjust the frequency of shaking as needed
            {
                ShakeButtons();
            }
        }

        // Declare a random number generator for shaking effect
        private Random rand = new Random();

        private void ShakeButtons()
        {
            const int shakeMagnitude = 5; // Adjust the shake magnitude as needed

            foreach (Control control in panel1.Controls)
            {
                if (control is Button button)
                {
                    // Shake button position
                    int offsetX = rand.Next(-shakeMagnitude, shakeMagnitude + 1);
                    int offsetY = rand.Next(-shakeMagnitude, shakeMagnitude + 1);
                    button.Location = new Point(button.Location.X + offsetX, button.Location.Y + offsetY);

                    // Shake button size (optional)
                    int sizeChange = rand.Next(-shakeMagnitude, shakeMagnitude + 1);
                    button.Size = new Size(button.Size.Width + sizeChange, button.Size.Height + sizeChange);
                }
            }
        }


        private int[] GenerateShuffledNumbers(int count)
        {
            int[] numbers = new int[count];
            for (int i = 0; i < count; i++)
            {
                numbers[i] = i + 1;
            }

            // Fisher-Yates shuffle algorithm
            Random rand = new Random();
            for (int i = count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                int temp = numbers[i];
                numbers[i] = numbers[j];
                numbers[j] = temp;
            }

            return numbers;
        }

        private void DisplayShuffledNumbers()
        {
            for (int i = 0; i < 40; i++)
            {
                Control[] controls = panel1.Controls.Find("button" + (i + 1).ToString(), true);
                if (controls.Length > 0 && controls[0] is Button)
                {
                    Button button = (Button)controls[0];
                    button.Text = shuffledNumbers[i].ToString();
                    button.Enabled = true; // Enable all buttons

                    // Preserve button's existing color
                }
            }
        }

        private void DisplayShuffledNumbersWithRandomColors()
        {
            Random rand = new Random();

            for (int i = 0; i < 40; i++)
            {
                Control[] controls = panel1.Controls.Find("button" + (i + 1).ToString(), true);
                if (controls.Length > 0 && controls[0] is Button)
                {
                    Button button = (Button)controls[0];
                    button.Text = shuffledNumbers[i].ToString();
                    button.Enabled = true; // Enable all buttons

                    // Assign random color
                    button.BackColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                }
            }
        }


        private void button41_Click(object sender, EventArgs e)
        {
            // Start the game
            progressBar1.Maximum = 40; // Total buttons
            progressBar1.Value = 0;
            startTime = DateTime.Now;
            timer.Start();

            // Shuffle buttons
            shuffledNumbers = GenerateShuffledNumbers(40);
            DisplayShuffledNumbers();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int buttonNumber = int.Parse(clickedButton.Text);

            // Disable clicked button
            clickedButton.Enabled = false;

            // Update progress bar
            progressBar1.Value++;

            // Check if the clicked button is in order
            if (buttonNumber == currentIndex + 1)
            {
                currentIndex++;
                if (currentIndex == 40)
                {
                    MessageBox.Show($"Congratulations! You completed the game in {totalTime} seconds.");
                    ResetGame();
                }
            }
            else
            {
                MessageBox.Show("You Lose!");
                ResetGame();
            }
        }

        private void ResetGame()
        {
            // Reset timer and progress bar
            timer.Stop();
            progressBar1.Value = 0;
            label1.Text = "Timer: ";

            // Reset shuffled numbers and display
            shuffledNumbers = GenerateShuffledNumbers(40);
            currentIndex = 0;
            DisplayShuffledNumbers();
        }

        // Attach Button_Click event to all buttons
        private void AttachButtonEvents()
        {
            foreach (Control control in panel1.Controls)
            {
                if (control is Button button)
                {
                    button.Click += Button_Click;
                }
            }
        }

        // Detach Button_Click event from all buttons
        private void DetachButtonEvents()
        {
            foreach (Control control in panel1.Controls)
            {
                if (control is Button button)
                {
                    button.Click -= Button_Click;
                }
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            AttachButtonEvents();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DetachButtonEvents();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void ColorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ColorCheckBox.Checked)
            {
                // Display shuffled numbers on buttons and assign random colors
                DisplayShuffledNumbersWithRandomColors();
            }
            else
            {
                // Display shuffled numbers on buttons without changing colors
                DisplayShuffledNumbers();
            }
        }


        private void ShakingCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
