using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;



namespace PersianCoder
{
    
    public partial class Form1 : Form 
    {
       private static string Token = "609621311:AAG_SPpuoYvVUyA6xAnldnYqrH7UjBlt0rc";  
       private Thread botThread; 
       private Telegram.Bot.TelegramBotClient Bot; 
        private ReplyKeyboardMarkup mainKeyboardMarkup;
        int bale1, bale2, bale3 = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Token = txtToken.Text;
            botThread = new Thread(new ThreadStart (runBot));
            botThread.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mainKeyboardMarkup = new ReplyKeyboardMarkup();
            KeyboardButton[] row1 =
            {
                new KeyboardButton("درباره ما"+" \U0000270F"), new KeyboardButton("ارتباط با ما"+"  \U00002709")
            };
            KeyboardButton[] row2 =
            {
                new KeyboardButton("آدرس ما"+" \U0001F170"), new KeyboardButton("نظر سنجی"+" \U00002705")
            };
            mainKeyboardMarkup.Keyboard = new KeyboardButton[][] 
            {
                row1,row2
            };
        }

        
        void runBot()
        {
            //nemone sazi az bot
            Bot = new Telegram.Bot.TelegramBotClient(Token);
            this.Invoke(new Action(() =>
            {
                lblStatus.Text = "Online";
                lblStatus.ForeColor = Color.Green;
                 
            } ));

            int offset = 0;
            // controling orders and message of users  
            while (true)
            {
                Telegram.Bot.Types.Update[] update = Bot.GetUpdatesAsync(offset).Result;
                foreach (var upd in update)
                {

                    offset = upd.Id + 1;
                    if(upd.CallbackQuery != null)
                    {
                        switch (upd.CallbackQuery.Data)
                        {
                            case "1":
                                {
                                    bale1 += 1;
                                    break;
                                };
                            case "2":
                                {
                                    bale2 += 1;
                                    break;
                                };
                            case "3":
                                {
                                    bale3 += 1;
                                    break;
                                };
                        }
                    }
                    //agar karbar chizi vared nakard
                    if (upd.Message == null)
                    {
                        continue;
                    }
                    //text ke karbar ferstade

                    var text = upd.Message.Text.ToLower();
                    //user who sent message
                    var from = upd.Message.From;
                    // user id is unique and work until user do not block the bot
                    var chatId = upd.Message.Chat.Id;

                    if (text.Contains("/start"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(from.Username + "به ربات ما خوش آمدید");
                        sb.AppendLine("شما میتوانید از محتوای با کیفیت ما استفاده کنید ");
                        //      sb.AppendLine(" درباره ما: /AboutUs");
                        //      sb.AppendLine(" ارتباط با ما: /ContactUs");
                        //      sb.AppendLine(" آدرس بات: /Address");
                        Bot.SendTextMessageAsync(chatId, sb.ToString(), ParseMode.Default, false, false, 0, mainKeyboardMarkup);
                    } else if (text.Contains("/aboutus") || text.Contains("درباره ما"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("بهترین بات آموزشی کامپیوتر");
                        Bot.SendTextMessageAsync(chatId, sb.ToString());
                    } else if (text.Contains("/contactus") || text.Contains("ارتباط با ما"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Email:hamidrezashahmazari.99@gmail.com");
                        sb.AppendLine("Phone:031534325");
                        sb.AppendLine("Id:@hamidjoon1999");
                        Bot.SendTextMessageAsync(chatId, sb.ToString());

                        ReplyKeyboardMarkup contactKeyboardMarkup = new ReplyKeyboardMarkup();
                        KeyboardButton[] row1 =
                            {

                         new KeyboardButton("تماس با مدیریت"),  new KeyboardButton("تماس با پشتیبانی"), new KeyboardButton("تماس با واحد فروش")


                        };
                        KeyboardButton[] row2 =
                        {

                           new KeyboardButton("بازگشت ")

                        };
                        contactKeyboardMarkup.Keyboard = new KeyboardButton[][]
                        {
                            row1,row2
                        };
                        Bot.SendTextMessageAsync(chatId, text, ParseMode.Default, false, false, 0, contactKeyboardMarkup);
                    } else if (text.Contains("/address") || text.Contains("آدرس ما"))
                    {

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("www.PersianCoder.com");
                        InlineKeyboardMarkup inline = new InlineKeyboardMarkup();
                        InlineKeyboardUrlButton[] row1 =
                        {
                            new InlineKeyboardUrlButton("گوگل","http://google.com")
                        };
                        inline.InlineKeyboard = new InlineKeyboardButton[][]
                        {
                            row1
                        };
                        Bot.SendTextMessageAsync(chatId, sb.ToString(), ParseMode.Default, false, false, 0, inline);
                         
                        Bot.SendTextMessageAsync(chatId, sb.ToString());

                    } else if (text.Contains("بازگشت"))
                    {
                        Bot.SendTextMessageAsync(chatId,"بازگشت به منوی اصلی",ParseMode.Default,false,false,0,mainKeyboardMarkup);
                    } else if(text.Contains("نظر سنجی"))
                    {
                        InlineKeyboardMarkup inline = new InlineKeyboardMarkup();
                        InlineKeyboardCallbackButton[] row1 =
                        {
                            new InlineKeyboardCallbackButton("بله راضی هستم("+bale1+")","1") 
                        };
                        InlineKeyboardCallbackButton[] row2 = 
                        {
                            new InlineKeyboardCallbackButton("نه اصلا("+bale2+")","2")
                        }; 
                        InlineKeyboardCallbackButton[] row3 =
                        {
                            new InlineKeyboardCallbackButton("گمشو("+bale3+")","3")
                        };
                        inline.InlineKeyboard = new InlineKeyboardButton[][]
                        {
                            row1,row2,row3
                        };
                        Bot.SendTextMessageAsync(chatId, "آیا از کانال ما راضی هستید", ParseMode.Default, false, false, 0, inline);
                        

                    }


                    dgReport.Invoke(new Action(() =>
                    {
                        dgReport.Rows.Add(chatId, from.Username, text, upd.Message.MessageId, upd.Message.Date.ToString("yyy/MM/dd - HH:mm"));

                    }));

                }

            }
          
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            botThread.Abort();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
           //چک کردن این که آیا خطی از گیرید انتخاب شده یا نه
           if(dgReport.CurrentRow !=null)
            {
                int chatId = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                Bot.SendTextMessageAsync(chatId,txtMessage.Text,ParseMode.Html,true);
                txtMessage.Text = "";
            }

        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if(openFile.ShowDialog()==DialogResult.OK)
            {
                txtFilePath.Text = openFile.FileName;
            }
        }

        private void btnPhoto_Click(object sender, EventArgs e)
        {
           
            if (dgReport.CurrentRow != null)
            {
                int chatId = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());

                FileStream imageFile = System.IO.File.Open(txtFilePath.Text, FileMode.Open);

                Bot.SendPhotoAsync(chatId, new FileToSend("1234.jpg", imageFile), txtMessage.Text);
            }

        }

        private void btnSendPhoto_Click(object sender, EventArgs e)
        {

        }

        private void groupbox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dgReport.CurrentRow !=null)
            {
                FileStream videoFile = System.IO.File.Open(txtFilePath.Text,FileMode.Open);

                int chatId = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());

                Bot.SendVideoAsync(chatId,new FileToSend("hamid.mp4",videoFile));
            }
        }

        private void btnSendText_Click(object sender, EventArgs e)
        {
            Bot.SendTextMessageAsync(txtChannel.Text, txtMessage.Text, ParseMode.Html);
        }
    }
}
