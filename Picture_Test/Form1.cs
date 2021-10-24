using System;
using System.Drawing;
using System.IO;

using System.Windows.Forms;

namespace Picture_Test
{
    public partial class Form1 : Form
    {
        //게임 시스템 관련 변수
        
        bool[] NumberDuplicationCheck = new bool[143];
        int[] SelectedNumber = new int[40];
        int EntryNumber = 0;
        int TotalPictureCount = 0;
        int CurrentPictureCount = 1;
        const int TotalPitureNumber = 142;
        Random Suffle = new Random();
        Image GameImage;

        bool flagGameStart = false;
        bool flagWindowScreenState = false;

        //파일 경로 스트링 변수

        const string IMAGE_FILE_PATH = @"\picturelist\";
        const string IMAGE_FILE_EXTENSION = ".png";

        //메인 화면 컨트롤을 위한 변수

        public Button StartButton;
        public Button EntrySettingButton;
        public Button QuitButton;

        public Label EntryNumberLabel;
        public TextBox EntrySettingTextBox;

        //게임 화면 컨트롤을 위한 변수

        public PictureBox picture_box;

        //결과 화면 컨트롤을 위한 변수

        public Label GameResultLabel;
        public Button BacktoMainButton;
        public Button GameRestartButton;

        // 클래스 생성자 정의 (윈도우를 구성하는 오브젝트 초기화)

        public Form1()
        {
            InitializeComponent();

            //윈도우 폼 설정

            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            //메인 화면 설정

            Size MainScreenButtonSize = new Size(300, 75);
            Size MainScreenLabelSize = new Size(300, 75);
            Font MainScreenFont = new Font(FontFamily.GenericSansSerif, 20.0f);

            StartButton = new Button
            {
                Text = "게임 시작",
                Font = MainScreenFont,
                Size = MainScreenButtonSize
            };
            EntrySettingButton = new Button
            {
                Text = "참가자 수 설정",
                Font = MainScreenFont,
                Size = MainScreenButtonSize
            };
            QuitButton = new Button
            {
                Text = "바탕화면으로 돌아가기",
                Font = MainScreenFont,
                Size = MainScreenButtonSize
            };
            EntryNumberLabel = new Label
            {
                Text = "참가자 수를 입력해주십시오",
                Font = MainScreenFont,
                Size = MainScreenLabelSize
            };
            EntrySettingTextBox = new TextBox
            {
                Font = MainScreenFont,
                Size = MainScreenLabelSize,
                TextAlign = HorizontalAlignment.Center
            };

            StartButton.Location = new Point(this.Size.Width / 2 - StartButton.Size.Width / 2, this.Size.Height / 2 - StartButton.Size.Height / 2);
            EntrySettingButton.Location = new Point(this.Size.Width / 2 - EntrySettingButton.Size.Width / 2, StartButton.Location.Y + StartButton.Size.Height + 30);
            QuitButton.Location = new Point(this.Size.Width / 2 - QuitButton.Size.Width / 2, EntrySettingButton.Location.Y + EntrySettingButton.Size.Height + 30);
            EntryNumberLabel.Location = new Point(this.Size.Width / 2 - EntryNumberLabel.Size.Width, EntrySettingButton.Location.Y);
            EntrySettingTextBox.Location = new Point(this.Size.Width / 2, EntrySettingButton.Location.Y);

            StartButton.Click += StartButtonClickEvent;
            EntrySettingButton.Click += EntrySettingClickEvent;
            QuitButton.Click += QuitbuttonClickEvent;
            EntrySettingTextBox.KeyDown += EntrySettingKeyEvent;

            // 게임 화면 설정

            picture_box = new PictureBox();
            this.KeyDown += PictureBoxKeyEvent;

            // 결과 화면 설정

            Size ResultScreenButtonSize = new Size(300, 75);
            Size ResultScreenLabelSize = new Size(900, 300);
            Font ResultScreenButtonFont = new Font(FontFamily.GenericSansSerif, 20.0f);
            Font ResultScreenLabelFont = new Font(FontFamily.GenericSansSerif, 150.0f);
            GameResultLabel = new Label
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Font = ResultScreenLabelFont,
                Size = ResultScreenLabelSize
            };
            BacktoMainButton = new Button
            {
                Text = "메인으로",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = ResultScreenButtonFont,
                Size = ResultScreenButtonSize
            };
            GameRestartButton = new Button
            {
                Text = "재시작",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = ResultScreenButtonFont,
                Size = ResultScreenButtonSize
            };
            GameResultLabel.Location = new Point(this.Size.Width / 2 - GameResultLabel.Size.Width / 2, this.Size.Height / 2 - GameResultLabel.Size.Height / 2);
            BacktoMainButton.Location = new Point(this.Size.Width / 2 - BacktoMainButton.Size.Width - 100, GameResultLabel.Location.Y + GameResultLabel.Size.Height + 100);
            GameRestartButton.Location = new Point(this.Size.Width / 2 + 100, GameResultLabel.Location.Y + GameResultLabel.Size.Height + 100);

            BacktoMainButton.Click += BacktoMainClickEvent;
            GameRestartButton.Click += RestartGameClickEvent;

            MainScreenLoad();
        }

        private void MainScreenLoad()
        {
            this.Controls.Clear();
            this.Controls.Add(StartButton);
            this.Controls.Add(EntrySettingButton);
            this.Controls.Add(QuitButton);
        }

        private void GameScreenLoad()
        {
            flagWindowScreenState = true;
            this.Controls.Clear();
            this.Controls.Add(picture_box);
        }

        private void ResultScreenLoad()
        {
            this.Controls.Clear();
            this.Controls.Add(GameResultLabel);
            this.Controls.Add(BacktoMainButton);
            this.Controls.Add(GameRestartButton);
            flagGameStart = false;
            flagWindowScreenState = false;
            CurrentPictureCount = 1;
        }

        private void PictureRefresh(int PictureNumber)
        {
            const string MESSAGE = "파일을 찾을 수 없습니다\n파일을 확인 후 다시 실행해주십시오\n";
            const string CAPTION = "파일 오류";
            string FilePath = "";
            try
            {
                FilePath = System.Windows.Forms.Application.StartupPath + IMAGE_FILE_PATH + PictureNumber.ToString() + IMAGE_FILE_EXTENSION;
                GameImage = new Bitmap(FilePath);
                picture_box.Image = GameImage;
                picture_box.Size = GameImage.Size;
                picture_box.Location = new Point(this.Size.Width / 2 - picture_box.Size.Width / 2, this.Size.Height / 2 - picture_box.Size.Height / 2);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(MESSAGE, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            catch (Exception exception)
            {
                ExceptionMesssage(exception);
                Application.Exit();
            }
        }

        private void PictureSuffle()
        {
            Array.Clear(NumberDuplicationCheck, 0, NumberDuplicationCheck.Length);
            Array.Clear(SelectedNumber, 0, SelectedNumber.Length);
            for (int i = 1; i <= TotalPictureCount; i++)
            {
                SelectedNumber[i] = Suffle.Next(1, TotalPitureNumber);
                if (NumberDuplicationCheck[SelectedNumber[i]])
                {
                    SelectedNumber[i] = 0;
                    i--;
                }
                else
                {
                    NumberDuplicationCheck[SelectedNumber[i]] = true;
                }
            }
        }

        private void StartButtonClickEvent(object sender, EventArgs e)
        {
            if (!flagGameStart)
            {
                const string MESSAGE = "참가자 수가 설정되지 않았습니다.\n참가자 수를 설정해 주십시오.";
                const string CAPTION = "게임 시작 불가";
                MessageBox.Show(MESSAGE, CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                GameScreenLoad();
                PictureRefresh(SelectedNumber[CurrentPictureCount]);
            }
        }

        private void EntrySettingClickEvent(object sender, EventArgs e)
        {
            this.Controls.Remove(EntrySettingButton);
            this.Controls.Add(EntryNumberLabel);
            this.Controls.Add(EntrySettingTextBox);
            EntrySettingTextBox.Focus();
        }

        private void QuitbuttonClickEvent(object sender, EventArgs e)
        {
            const string MESSAGE = "정말 바탕화면으로 돌아가시겠습니까?";
            const string CAPTION = "게임 종료";

            try
            {
                var result = MessageBox.Show(MESSAGE, CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                    this.Close();
                else
                    QuitButton.Focus();
            }
            catch (Exception exception)
            {
                ExceptionMesssage(exception);
            }
        }

        private void EntrySettingKeyEvent(object sender, KeyEventArgs e)
        {
            string Message = "";
            string Caption = "";
            int temp = 0;
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    temp = int.Parse(EntrySettingTextBox.Text);
                    if (temp > 20 | temp < 1)
                    {
                        Message = "1~20 사이의 정수를 입력해주십시오";
                        Caption = "입력 범위 초과";
                        MessageBox.Show(Message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        EntrySettingTextBox.Text = "";
                    }
                    else
                    {
                        EntryNumber = temp;
                        TotalPictureCount = EntryNumber * 2 - 1;
                        PictureSuffle();

                        this.Controls.Add(EntrySettingButton);
                        this.Controls.Remove(EntryNumberLabel);
                        this.Controls.Remove(EntrySettingTextBox);

                        flagGameStart = true;

                        Message = EntryNumber.ToString() + "명이 참가합니다";
                        Caption = "설정 완료";
                        MessageBox.Show(Message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    Message = "취소되었습니다.";
                    Caption = "설정 취소";

                    this.Controls.Add(EntrySettingButton);
                    this.Controls.Remove(EntryNumberLabel);
                    this.Controls.Remove(EntrySettingTextBox);

                    EntrySettingButton.Focus();
                    MessageBox.Show(Message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException)
            {
                Message = "1~20 사이의 정수를 입력해주십시오.";
                Caption = "입력 오류";
                MessageBox.Show(Message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                EntrySettingTextBox.Text = "";
            }
            catch (Exception exception)
            {
                ExceptionMesssage(exception);
            }
        }

        private void PictureBoxKeyEvent(object sender, KeyEventArgs e)
        {
            if (flagGameStart && flagWindowScreenState)
            {
                try
                {
                    if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Enter)
                    {
                        if (CurrentPictureCount != TotalPictureCount)
                        {
                            CurrentPictureCount++;
                            PictureRefresh(SelectedNumber[CurrentPictureCount]);
                        }
                        else if (CurrentPictureCount == TotalPictureCount)
                        {
                            GameResultLabel.Text = "승리";
                            ResultScreenLoad();
                        }
                    }
                    else if (e.KeyCode == Keys.Left)
                    {
                        if (CurrentPictureCount != 1)
                        {
                            CurrentPictureCount--;
                            PictureRefresh(SelectedNumber[CurrentPictureCount]);
                        }
                    }
                    else if (e.KeyCode == Keys.Escape)
                    {
                        GameResultLabel.Text = "패배";
                        ResultScreenLoad();
                    }
                }
                catch (Exception exception)
                {
                    ExceptionMesssage(exception);
                }
            }
        }

        private void BacktoMainClickEvent(object sender, EventArgs e)
        {
            MainScreenLoad();
        }

        private void RestartGameClickEvent(object sender, EventArgs e)
        {
            PictureSuffle();
            PictureRefresh(SelectedNumber[CurrentPictureCount]);
            flagGameStart = true;
            GameScreenLoad();
        }

        private void ExceptionMesssage(Exception exception)
        {
            const string message = "오류가 발생했습니다.\n다음의 내용을 시스템 관리자에게 문의해주십시오.\n";
            const string caption = "예외 발생";
            MessageBox.Show(message + exception.ToString(), caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
