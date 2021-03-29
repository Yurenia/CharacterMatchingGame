using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picture_Test
{
    public partial class Form1 : Form
    {
        //Game System        
        bool[] number_duplication_check = new bool[143];
        int[] selected_number = new int[40];
        int entry_number = 0;
        int total_picture_count = 0;
        int current_picture_count = 1;
        const int total_picture_number = 142;
        Random random = new Random();
        Image image;

        bool game_start_flag = false;
        bool game_screen_flag = false;
        bool is_screen_full = true;

        //File path string

        const string image_file_path = @"\picturelist\";
        const string image_file_extension = ".png";

        //Main Screen Controls
        public Button start_button;
        public Button entry_set_button;
        public Button quit_button;

        public Label entry_number_label;
        public TextBox entry_set_textbox;

        //Game Screen Controls
        public PictureBox picture_box;

        //Result Screen Controls
        public Label game_result_text;
        public Button back_to_main_button;
        public Button restart_game_button;

        public Form1()
        {
            InitializeComponent();

            //Window form setting
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            //Main screen setting
            Size main_screen_button_size = new Size(300, 75);
            Size main_screen_label_size = new Size(300, 75);
            Font main_screen_font = new Font(FontFamily.GenericSansSerif, 20.0f);

            start_button = new Button
            {
                Text = "게임 시작",
                Font = main_screen_font,
                Size = main_screen_button_size
            };
            entry_set_button = new Button
            {
                Text = "참가자 수 설정",
                Font = main_screen_font,
                Size = main_screen_button_size
            };
            quit_button = new Button
            {
                Text = "바탕화면으로 돌아가기",
                Font = main_screen_font,
                Size = main_screen_button_size
            };
            entry_number_label = new Label
            {
                Text = "참가자 수를 입력해주십시오",
                Font = main_screen_font,
                Size = main_screen_label_size
            };
            entry_set_textbox = new TextBox
            {
                Font = main_screen_font,
                Size = main_screen_label_size,
                TextAlign = HorizontalAlignment.Center
            };

            start_button.Location = new Point(this.Size.Width / 2 - start_button.Size.Width / 2, this.Size.Height / 2 - start_button.Size.Height / 2);
            entry_set_button.Location = new Point(this.Size.Width / 2 - entry_set_button.Size.Width / 2, start_button.Location.Y + start_button.Size.Height + 30);
            quit_button.Location = new Point(this.Size.Width / 2 - quit_button.Size.Width / 2, entry_set_button.Location.Y + entry_set_button.Size.Height + 30);
            entry_number_label.Location = new Point(this.Size.Width / 2 - entry_number_label.Size.Width, entry_set_button.Location.Y);
            entry_set_textbox.Location = new Point(this.Size.Width / 2, entry_set_button.Location.Y);

            start_button.Click += Start_Button_Click_Event;
            entry_set_button.Click += Entry_Set_Button_Click_Event;
            quit_button.Click += Quit_Button_Click_Event;
            entry_set_textbox.KeyDown += Entry_Set_Key_Event;

            // Game screen setting
            picture_box = new PictureBox();
            this.KeyDown += Picture_Box_Key_Event;

            // Result screen setting
            Size result_screen_button_size = new Size(300, 75);
            Size result_screen_label_size = new Size(900, 300);
            Font result_screen_button_font = new Font(FontFamily.GenericSansSerif, 20.0f);
            Font result_screen_label_font = new Font(FontFamily.GenericSansSerif, 150.0f);
            game_result_text = new Label
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Font = result_screen_label_font,
                Size = result_screen_label_size
            };
            back_to_main_button = new Button
            {
                Text = "메인으로",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = result_screen_button_font,
                Size = result_screen_button_size
            };
            restart_game_button = new Button
            {
                Text = "재시작",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = result_screen_button_font,
                Size = result_screen_button_size
            };
            game_result_text.Location = new Point(this.Size.Width / 2 - game_result_text.Size.Width / 2, this.Size.Height / 2 - game_result_text.Size.Height / 2);
            back_to_main_button.Location = new Point(this.Size.Width / 2 - back_to_main_button.Size.Width - 100, game_result_text.Location.Y + game_result_text.Size.Height + 100);
            restart_game_button.Location = new Point(this.Size.Width / 2 + 100, game_result_text.Location.Y + game_result_text.Size.Height + 100);

            back_to_main_button.Click += Back_To_Main_Click_Event;
            restart_game_button.Click += Restart_Game_Click_Event;

            Main_Screen_Load();
        }

        private void Main_Screen_Load()
        {
            this.Controls.Clear();
            this.Controls.Add(start_button);
            this.Controls.Add(entry_set_button);
            this.Controls.Add(quit_button);
        }

        private void Game_Screen_Load()
        {
            game_screen_flag = true;
            this.Controls.Clear();
            this.Controls.Add(picture_box);
        }

        private void Result_Screen_Load()
        {
            this.Controls.Clear();
            this.Controls.Add(game_result_text);
            this.Controls.Add(back_to_main_button);
            this.Controls.Add(restart_game_button);
            game_start_flag = false;
            game_screen_flag = false;
            current_picture_count = 1;
        }

        private void Picture_Refresh(int picture_number)
        {
            const string message = "파일을 찾을 수 없습니다\n파일을 확인 후 다시 실행해주십시오";
            const string caption = "파일 오류";
            try
            {
                image = new Bitmap(image_file_path + picture_number.ToString() + image_file_extension);
                picture_box.Image = image;
                picture_box.Size = image.Size;
                picture_box.Location = new Point(this.Size.Width / 2 - picture_box.Size.Width / 2, this.Size.Height / 2 - picture_box.Size.Height / 2);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            catch (Exception exception)
            {
                Exception_Messsage(exception);
                Application.Exit();
            }
        }

        private void Picture_Suffle()
        {
            Array.Clear(number_duplication_check, 0, number_duplication_check.Length);
            Array.Clear(selected_number, 0, selected_number.Length);
            for (int i = 1; i <= total_picture_count; i++)
            {
                selected_number[i] = random.Next(1, total_picture_number);
                if (number_duplication_check[selected_number[i]])
                {
                    selected_number[i] = 0;
                    i--;
                }
                else
                {
                    number_duplication_check[selected_number[i]] = true;
                }
            }
        }

        private void Start_Button_Click_Event(object sender, EventArgs e)
        {
            if (!game_start_flag)
            {
                const string message = "참가자 수가 설정되지 않았습니다.\n참가자 수를 설정해 주십시오.";
                const string caption = "게임 시작 불가";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Game_Screen_Load();
                Picture_Refresh(selected_number[current_picture_count]);
            }
        }

        private void Entry_Set_Button_Click_Event(object sender, EventArgs e)
        {
            this.Controls.Remove(entry_set_button);
            this.Controls.Add(entry_number_label);
            this.Controls.Add(entry_set_textbox);
            entry_set_textbox.Focus();
        }

        private void Quit_Button_Click_Event(object sender, EventArgs e)
        {
            const string message = "정말 바탕화면으로 돌아가시겠습니까?";
            const string caption = "게임 종료";

            try
            {
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                    this.Close();
                else
                    quit_button.Focus();
            }
            catch (Exception exception)
            {
                Exception_Messsage(exception);
            }
        }

        private void Entry_Set_Key_Event(object sender, KeyEventArgs e)
        {
            string message = "";
            string caption = "";
            int temp = 0;
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    temp = int.Parse(entry_set_textbox.Text);
                    if (temp > 20 | temp < 1)
                    {
                        message = "1~20 사이의 정수를 입력해주십시오";
                        caption = "입력 범위 초과";
                        MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        entry_set_textbox.Text = "";
                    }
                    else
                    {
                        entry_number = temp;
                        total_picture_count = entry_number * 2 - 1;
                        Picture_Suffle();

                        this.Controls.Add(entry_set_button);
                        this.Controls.Remove(entry_number_label);
                        this.Controls.Remove(entry_set_textbox);

                        game_start_flag = true;

                        message = entry_number.ToString() + "명이 참가합니다";
                        caption = "설정 완료";
                        MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    message = "취소되었습니다.";
                    caption = "설정 취소";

                    this.Controls.Add(entry_set_button);
                    this.Controls.Remove(entry_number_label);
                    this.Controls.Remove(entry_set_textbox);

                    entry_set_button.Focus();
                    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException)
            {
                message = "1~20 사이의 정수를 입력해주십시오.";
                caption = "입력 오류";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                entry_set_textbox.Text = "";
            }
            catch (Exception exception)
            {
                Exception_Messsage(exception);
            }
        }

        private void Picture_Box_Key_Event(object sender, KeyEventArgs e)
        {
            if (game_start_flag && game_screen_flag)
            {
                try
                {
                    if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Enter)
                    {
                        if (current_picture_count != total_picture_count)
                        {
                            current_picture_count++;
                            Picture_Refresh(selected_number[current_picture_count]);
                        }
                        else if (current_picture_count == total_picture_count)
                        {
                            game_result_text.Text = "승리";
                            Result_Screen_Load();
                        }
                    }
                    else if (e.KeyCode == Keys.Left)
                    {
                        if (current_picture_count != 1)
                        {
                            current_picture_count--;
                            Picture_Refresh(selected_number[current_picture_count]);
                        }
                    }
                    else if (e.KeyCode == Keys.Escape)
                    {
                        game_result_text.Text = "패배";
                        Result_Screen_Load();
                    }
                }
                catch (Exception exception)
                {
                    Exception_Messsage(exception);
                }
            }
        }

        private void Back_To_Main_Click_Event(object sender, EventArgs e)
        {
            Main_Screen_Load();
        }

        private void Restart_Game_Click_Event(object sender, EventArgs e)
        {
            Picture_Suffle();
            Picture_Refresh(selected_number[current_picture_count]);
            game_start_flag = true;
            Game_Screen_Load();
        }

        private void Exception_Messsage(Exception exception)
        {
            const string message = "오류가 발생했습니다.\n다음의 내용을 시스템 관리자에게 문의해주십시오.\n";
            const string caption = "예외 발생";
            MessageBox.Show(message + exception.ToString(), caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}
