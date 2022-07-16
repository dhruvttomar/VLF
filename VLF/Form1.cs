using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;


namespace VLF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            statusStripMain.Text = "Ready";
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            var chromeOptions = new ChromeOptions();
            var downloadDirectory = tbFolder.Text;
            
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");

            statusStripMain.Text = "Connecting to Chrome...";
            IWebDriver driver = new ChromeDriver(chromeOptions);
            driver.Url = "https://cddis.nasa.gov/archive/gnss/data/daily/";


            

            var username = driver.FindElement(By.Id("username"));
            username.SendKeys(tbUser.Text);

            var password = driver.FindElement(By.Id("password"));
            password.SendKeys(tbPass.Text);

            var nextButton = driver.FindElement(By.XPath("//*[@id='login']/p[8]/input"));
            nextButton.Click();

            

            Thread.Sleep(7000);
            // Login done, jump to year and day folder
            driver.Url = "https://cddis.nasa.gov/archive/gnss/data/daily/" + tbYear.Text + "/" + tbDay.Text;
            Thread.Sleep(3000);

            // Get all data folders
            var archiveDirText = driver.FindElements(By.ClassName("archiveDirText"));
            List<String> dataDirectories = new List<string>();
            foreach (var dataDirectory in archiveDirText)
            {
                dataDirectories.Add(dataDirectory.GetAttribute("id"));
            }

            List<string> FilesInsideAllFolders = new List<string>();
            foreach (string folder in dataDirectories)
            {
                driver.Url = "https://cddis.nasa.gov/archive/gnss/data/daily/" + tbYear.Text + "/" + tbDay.Text+"/"+folder;
                var archiveItemTexts = driver.FindElements(By.ClassName("archiveItemText"));
                List<string> FilesInCurrentFolder = new List<string>();
                Thread.Sleep(2000);
                foreach (var archiveItemText in archiveItemTexts)
                {
                    FilesInCurrentFolder.Add("https://cddis.nasa.gov/archive/gnss/data/daily/" + tbYear.Text + "/" + tbDay.Text + "/" + folder+"/"+archiveItemText.GetAttribute("id"));
                }
                

                FilesInsideAllFolders.AddRange(FilesInCurrentFolder);

            }
            foreach (string url in FilesInsideAllFolders)
            {
                driver.Url = url;
                Thread.Sleep(1000);
            }

            

            //.Url = "https://cddis.nasa.gov/archive/gnss/data/daily/" + tbYear.Text + "/" + tbDay.Text + "/" + tbSubDay.Text + "/" + tbFile.Text;
            //https://cddis.nasa.gov/archive/gnss/data/daily/1997/006/97o/albh0060.97o.Z


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            statusStripMain.Text = "Ready";
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbFolder.Text = diag.SelectedPath;
            }
            else
            { tbFolder.Text = "You didn't select any folder!"; }
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            FormHelp formHelp = new FormHelp();
            formHelp.ShowDialog();
        }
    }
}
