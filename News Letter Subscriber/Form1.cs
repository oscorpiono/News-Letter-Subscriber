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
using Keys = OpenQA.Selenium.Keys;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using Faker;
using System.Diagnostics;
using OpenQA.Selenium.Interactions;
using System.Net;
using System.Net.NetworkInformation;

namespace News_Letter_Subscriber
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        int x, y;
        bool move = false;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            move = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.Location = new Point(this.Location.X + (e.X - x), this.Location.Y + (e.Y - y));
            }
        }

        private void button2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }
        string sites = "";
        string mails = "";
        string[] emailsNews;
        string[] sitesNews;
        private void button2_DragDrop(object sender, DragEventArgs e)
        {
            if (File.Exists(((string[])e.Data.GetData(DataFormats.FileDrop))[0]))
            {
                sites = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                sitesNews = File.ReadAllLines(sites);
                label2.Text = sitesNews.Length.ToString();
                MessageBox.Show("Sites imported successfuly", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("File Doesnt Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }
        private void button3_DragDrop(object sender, DragEventArgs e)
        {
            if (File.Exists(((string[])e.Data.GetData(DataFormats.FileDrop))[0]))
            {
                mails = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                emailsNews = File.ReadAllLines(mails);
                label3.Text = emailsNews.Length.ToString();
                MessageBox.Show("Emails imported successfuly", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("File Doesnt Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        int opened = 0;
        public IWebElement getShadowRootElement(string selector1, string selector2,FirefoxDriver driver)
        {
            IWebElement ele = (IWebElement)((IJavaScriptExecutor)driver)
                .ExecuteScript($"return document.querySelector('{selector1}').shadowRoot.querySelector('{selector2}');");
            return ele;
        }

        private static bool CanPing(string address)
        {
            bool flag = false;
            try
            {
                new WebClient { Proxy = new WebProxy(address.Split(':')[0], int.Parse(address.Split(':')[1])) }.DownloadString("https://google.com");
                flag = true;
            }
            catch
            {

            }
            return flag;
        }

        public void subscribe(string acc, string[] siteds)
        {
            Task.Run(() => {
                FirefoxOptions opts = new FirefoxOptions();
                if (proxies != null)
                {
                    int m = new Random().Next(proxies.Length);
                    while (!CanPing(proxies[m]))
                    {
                        m = new Random().Next(proxies.Length);
                    }
                    Proxy prx = new Proxy();
                    prx.HttpProxy = proxies[m];
                    prx.SslProxy = proxies[m];
                    opts.Proxy = prx;
                }
                //opts.AddArgument("-headless");
                FirefoxDriverService srv = FirefoxDriverService.CreateDefaultService();
                srv.HideCommandPromptWindow = true;
                FirefoxDriver driver = new FirefoxDriver(srv, opts);
                foreach (string site in siteds)
                {
                    driver.SwitchTo().Window(driver.WindowHandles.First());
                    progressBar1.Invoke((MethodInvoker)delegate
                    {
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.ScrollToCaret();
                        richTextBox1.Text += "\nNew Site opened for : " + acc;
                    });

                    try
                    {
                        driver.Navigate().GoToUrl(site.Split('\t')[0]);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                        Thread.Sleep(8000);
                    }
                    catch
                    {

                    }

                    List<string> AreasList = new List<string>();
                    AreasList.AddRange(site.Split('\t'));
                    AreasList.Remove(site.Split('\t')[0]);

                    Thread.Sleep(5000);

                    foreach (var Area in AreasList)
                    {
                        try
                        {
                            string areaType = Area.Split(':')[0];
                            string areaSelectorType = Area.Split(':')[1].Split('|')[0];
                            string areaSelectorValue = Area.Split(':')[1].Split('|')[1];
                            string count = Area.Split(':')[2];

                            try
                            {
                                int wait = int.Parse(Area.Split(':')[3]);
                                Thread.Sleep(wait);
                            }
                            catch
                            {

                            }
                            List<IWebElement> elems = new List<IWebElement>();
                            int errs = 0;

                            if (Area.Split(':')[0].Contains("SHADOW"))
                            {
                                areaType = Area.Split(':')[0].Split(',')[1];
                                IWebElement el = getShadowRootElement(Area.Split(':')[0].Split(',')[0].Split('|')[1], areaSelectorValue, driver);
                                elems.Add(el);
                            }
                            else if (areaSelectorType == "ID")
                            {
                                elems.AddRange(driver.FindElements(By.Id(areaSelectorValue)));
                            }
                            else if (areaSelectorType == "NAME")
                            {
                                elems.AddRange(driver.FindElements(By.Name(areaSelectorValue)));
                            }
                            else if (areaSelectorType == "CLASS")
                            {
                                elems.AddRange(driver.FindElements(By.ClassName(areaSelectorValue)));
                            }
                            else if (areaSelectorType == "TAG")
                            {
                                elems.AddRange(driver.FindElements(By.TagName(areaSelectorValue)));
                            }
                            else if (areaSelectorType == "XPATH")
                            {
                                elems.AddRange(driver.FindElements(By.XPath(areaSelectorValue)));
                            }
                            else if (areaSelectorType == "CSS")
                            {
                                elems.AddRange(driver.FindElements(By.CssSelector(areaSelectorValue)));
                            }
                            else
                            {
                                errs++;
                                progressBar1.Invoke((MethodInvoker)delegate
                                {
                                    richTextBox1.Text += $"\nWrong Selector '{areaSelectorType}' For '{site.Split('\t')[0]}'";
                                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                    richTextBox1.ScrollToCaret();
                                });
                            }
                            if (errs == 0)
                            {
                                int start = 0;
                                int end = 1;
                                if (count.Contains("pos|"))
                                {
                                    start = int.Parse(count.Split('|')[1]) - 1;
                                    end = int.Parse(count.Split('|')[1]);
                                }
                                else
                                {
                                    start = 0;
                                    end = int.Parse(count);
                                }
                                for (int i = start; i < end; i++)
                                {
                                    try
                                    {
                                        if (areaType == "EMAIL")
                                        {
                                            try
                                            {
                                                if (elems[i] != null)
                                                {
                                                    elems[i].SendKeys(acc);
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        else if (areaType == "INPUT")
                                        {
                                            try
                                            {
                                                if (elems[i] != null)
                                                {
                                                    elems[i].SendKeys(Faker.Lorem.Sentences(5).FirstOrDefault().Split(' ')[1]);
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        else if (areaType == "PASSWORD")
                                        {
                                            try
                                            {
                                                if (elems[i] != null)
                                                {
                                                    string rans = "ABCDEFGHIJ.-_@&:!;KLMNOPQRSTUVWXYZ.-_@&:!;abcdefghijklmno.-_@&:!;pqrstuvwxyz.-_@&:!;1234567890";
                                                    string Pass = "";
                                                    for (int m = 0; m < (new Random().Next(8, 10)); m++)
                                                    {
                                                        int r = new Random().Next(0, rans.Length);
                                                        MessageBox.Show(r.ToString());
                                                        Pass += rans.ToCharArray()[r];
                                                    }
                                                    MessageBox.Show(Pass);
                                                    elems[i].SendKeys(Pass);
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        else if (areaType == "CLICK")
                                        {
                                            try
                                            {
                                                if (elems[i] != null) elems[i].Click();
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        else if (areaType == "SELECT")
                                        {
                                            try
                                            {
                                                if (elems[i] != null)
                                                {
                                                    SelectElement dropDown = new SelectElement(elems[i]);
                                                    dropDown.SelectByIndex((new Random().Next(0, dropDown.Options.Count)));
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        Thread.Sleep(2000);
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                        
                    }

                    Thread.Sleep(5000);
                    progressBar1.Invoke((MethodInvoker)delegate
                    {
                        progressBar1.Value++;
                    });
                }
                driver.Close();
                driver.Quit();
                opened--;
                richTextBox1.Invoke((MethodInvoker)delegate
                {
                    if(progressBar1.Value >= progressBar1.Maximum)
                    {
                        MessageBox.Show("DONE !");
                        progressBar1.Value = 0;
                    }
                });
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (emailsNews != null && sitesNews != null)
            {
                progressBar1.Maximum = emailsNews.Length * sitesNews.Length;
                Task.Run(() => {
                    string[] sitess = sitesNews;
                    string[] ems = emailsNews;
                    for (int i = 0; i < emailsNews.Length; i++)
                    {
                        if (opened < int.Parse(textBox1.Text))
                        {
                            if (ems[i] != "")
                            {
                                subscribe(ems[i], sitess);
                                opened++;
                                textBox1.Invoke((MethodInvoker)delegate
                                {
                                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                    richTextBox1.ScrollToCaret();
                                    richTextBox1.Text += "\nNew Window opened for : " + ems[i];
                                });
                                Thread.Sleep(1000);
                            }
                        }
                        else
                        {
                            i--;
                        }
                    }
                });
            }
            else
            {
                MessageBox.Show("Something is missing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var pros = Process.GetProcesses();
            foreach (var pro in pros)
            {
                try
                {
                    if (pro.ProcessName.Contains("firefox") || pro.ProcessName.Contains("waterfox") || pro.ProcessName.Contains("gecko"))
                    {
                        try
                        {
                            pro.Kill();
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {

                }
            }
        }
        string[] proxies;
        private void button4_DragDrop(object sender, DragEventArgs e)
        {
            if (File.Exists(((string[])e.Data.GetData(DataFormats.FileDrop))[0]))
            {
                proxies = File.ReadAllLines(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
                label5.Text = proxies.Length.ToString();
                MessageBox.Show("Proxies imported successfuly", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("File Doesnt Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }
    }
}
