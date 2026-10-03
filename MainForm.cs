using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToeApp
{
    public class MainForm : Form
    {
        private Button[,] boardButtons = new Button[3, 3];
        private char[,] boardState = new char[3, 3];
        
        private Player playerX;
        private Player playerO;
        private Player currentPlayer;
        
        private Label statusLabel;
        private Label scoreLabel;
        private Button resetButton;

        // Color Palette
        private readonly Color BgColor = Color.FromArgb(30, 30, 47);          // Dark Slate
        private readonly Color CardColor = Color.FromArgb(45, 45, 68);        // Light Slate
        private readonly Color TextColor = Color.FromArgb(240, 240, 245);     // White
        private readonly Color AccentX = Color.FromArgb(0, 180, 216);         // Cyan
        private readonly Color AccentO = Color.FromArgb(255, 75, 110);        // Coral
        private readonly Color ButtonBg = Color.FromArgb(60, 63, 84);         // Cell Background
        private readonly Color ButtonHover = Color.FromArgb(75, 79, 105);     // Cell Hover

        public MainForm()
        {
            this.Text = "C# Tic-Tac-Toe";
            this.Size = new Size(440, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = BgColor;

            playerX = new Player("Player 1", 'X');
            playerO = new Player("Player 2", 'O');
            currentPlayer = playerX;

            var (xWins, oWins) = ScoreManager.LoadScores();
            playerX.Score.TotalWins = xWins;
            playerO.Score.TotalWins = oWins;

            InitializeComponent();
            ResetGame();
        }

        private void InitializeComponent()
        {
            // Top Header Panel for Status & Scores
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = CardColor,
                Padding = new Padding(10)
            };

            statusLabel = new Label
            {
                Text = $"{currentPlayer.Name}'s Turn (X)",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter
            };

            scoreLabel = new Label
            {
                Text = GetScoreText(),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 180, 200),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            topPanel.Controls.Add(scoreLabel);
            topPanel.Controls.Add(statusLabel);

            // Bottom Panel for Reset button
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 65,
                BackColor = CardColor,
                Padding = new Padding(12)
            };

            resetButton = new Button
            {
                Text = "Reset Game",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(90, 105, 200),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand
            };
            resetButton.FlatAppearance.BorderSize = 0;
            resetButton.Click += (sender, e) => ResetGame();

            bottomPanel.Controls.Add(resetButton);

            // Center Board Layout Panel
            TableLayoutPanel gridPanel = new TableLayoutPanel
            {
                RowCount = 3,
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                BackColor = BgColor,
                Padding = new Padding(15)
            };

            for (int i = 0; i < 3; i++)
            {
                gridPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
                gridPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            }
            
            // Add buttons in grid layout
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    Button btn = new Button
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Segoe UI", 32, FontStyle.Bold),
                        Tag = new BoardPosition(r, c),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = ButtonBg,
                        ForeColor = TextColor,
                        Cursor = Cursors.Hand
                    };
                    btn.FlatAppearance.BorderSize = 2;
                    btn.FlatAppearance.BorderColor = BgColor;

                    // Smooth Hover Effect Logic
                    btn.MouseEnter += (s, e) => { if (string.IsNullOrEmpty(btn.Text)) btn.BackColor = ButtonHover; };
                    btn.MouseLeave += (s, e) => { if (string.IsNullOrEmpty(btn.Text)) btn.BackColor = ButtonBg; };

                    btn.Click += OnCellClicked;
                    boardButtons[r, c] = btn;
                    
                    // Wrapper panel to give spacing margins between buttons
                    Panel cellWrapper = new Panel
                    {
                        Dock = DockStyle.Fill,
                        Padding = new Padding(4),
                        BackColor = BgColor
                    };
                    cellWrapper.Controls.Add(btn);
                    gridPanel.Controls.Add(cellWrapper, c, r);
                }
            }

            this.Controls.Add(gridPanel);
            this.Controls.Add(topPanel);
            this.Controls.Add(bottomPanel);
        }

        // Return the scores by player
        private string GetScoreText()
        {
            return $"{playerX.Name} (X): {playerX.Score.TotalWins}   |   {playerO.Name} (O): {playerO.Score.TotalWins}";
        }

        // Event handler for when a cell button is clicked
        private void OnCellClicked(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null || !string.IsNullOrEmpty(clickedButton.Text))
                return;

            BoardPosition pos = (BoardPosition)clickedButton.Tag;

            boardState[pos.Row, pos.Column] = currentPlayer.Symbol;
            clickedButton.Text = currentPlayer.Symbol.ToString();
            
            // Assign custom colors per player
            clickedButton.ForeColor = (currentPlayer.Symbol == 'X') ? AccentX : AccentO;
            clickedButton.BackColor = CardColor;

            // When a player wins, update the score and save to file
            if (CheckWin(currentPlayer.Symbol))
            {
                currentPlayer.Score.TotalWins += 1;
                statusLabel.Text = $"{currentPlayer.Name} Wins!";
                scoreLabel.Text = GetScoreText();
                
                ScoreManager.SaveScores(playerX.Score.TotalWins, playerO.Score.TotalWins);
                DisableBoard();
            }
            // Tie: board full and no winner
            else if (IsBoardFull())
            {
                statusLabel.Text = "It's a Tie!";
            }
            // Regular turn: switch players
            else
            {
                currentPlayer = (currentPlayer == playerX) ? playerO : playerX;
                statusLabel.Text = $"{currentPlayer.Name}'s Turn ({currentPlayer.Symbol})";
            }
        }

        // Check if the current player has won by checking rows, columns, and diagonals
        private bool CheckWin(char s)
        {
            for (int i = 0; i < 3; i++)
            {
                if ((boardState[i, 0] == s && boardState[i, 1] == s && boardState[i, 2] == s) ||
                    (boardState[0, i] == s && boardState[1, i] == s && boardState[2, i] == s))
                    return true;
            }

            if ((boardState[0, 0] == s && boardState[1, 1] == s && boardState[2, 2] == s) ||
                (boardState[0, 2] == s && boardState[1, 1] == s && boardState[2, 0] == s))
                return true;

            return false;
        }

        // If there's any empty cell, the board is not full
        private bool IsBoardFull()
        {
            foreach (char cell in boardState)
            {
                if (cell == '\0') return false;
            }
            return true;
        }

        // Disable all buttons on the board after a win or tie
        private void DisableBoard()
        {
            foreach (Button btn in boardButtons)
            {
                btn.Enabled = false;
            }
        }

        // Reset the game state, clear the board, and set the current player to X
        private void ResetGame()
        {
            boardState = new char[3, 3];
            currentPlayer = playerX;
            statusLabel.Text = $"{currentPlayer.Name}'s Turn (X)";

            foreach (Control control in boardButtons)
            {
                if (control is Button btn)
                {
                    btn.Text = "";
                    btn.Enabled = true;
                    btn.BackColor = ButtonBg;
                    btn.ForeColor = TextColor;
                }
            }
        }
    }
}