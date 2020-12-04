using System;

namespace MyUtility.Data.News
{
    public class Rongbay
    {
        public static void PostRequest(System.Windows.Forms.WebBrowser wbr, System.Windows.Forms.Timer timer)
        {
            var url = "http://vietid.net/OauthServerV2/RegisterV2?app_key=07234206219b51690a3bc234115a34f2&clearsession=1&oauth_token=";
            wbr.Url = new Uri(url);
            wbr.ScriptErrorsSuppressed = true;
            //wbr.DocumentCompleted += wbr_DocumentCompleted_PostRequest;
            //System.Threading.Thread.Sleep(3000);
            AccountConfirm(wbr);                         
            //timer.Enabled = true;
            //timer.Start();
        }

        private static void AccountConfirm(System.Windows.Forms.WebBrowser wbr)
        {
            var url = "https://accounts.google.com/ServiceLogin?service=mail&continue=https://mail.google.com/mail/&hl=vi";
            wbr.Url = new Uri(url);
            wbr.ScriptErrorsSuppressed = true;
            wbr.DocumentCompleted += wbr_DocumentCompleted_AccountConfirm;                        
        }

        private static void RedirectActiveLink(System.Windows.Forms.WebBrowser wbr)
        {            
            string codeString = String.Format("$('.Cp .xS .y6 span:first-child').html();");
            object title = wbr.Document.InvokeScript("eval", new[] { codeString });
            if (title != null)
            {
                if (title.ToString().IndexOf("Kích hoạt tài khoản VietID") != -1)
                {
                    string script = "<script> function ShowEmail() { $('.aqw').click(function(){ alert('111'); }); } </script>";
                    wbr.Document.CreateElement(script);
                    wbr.Document.InvokeScript("ShowEmail()");
                }    
            }            
        }

        private static void wbr_DocumentCompleted_AccountConfirm(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            System.Windows.Forms.WebBrowser wbr = sender as System.Windows.Forms.WebBrowser;
            try
            {
                if (wbr.ReadyState == System.Windows.Forms.WebBrowserReadyState.Complete)
                {
                    System.Windows.Forms.HtmlElement form = wbr.Document.GetElementById("gaia_loginform");
                    if (form != null)
                    {
                        wbr.Document.GetElementById("Email").SetAttribute("value", UserInfo.Email = "buitran986@gmail.com");
                        wbr.Document.GetElementById("Passwd").SetAttribute("value", UserInfo.Password = "yeuem1234567890");
                        form.InvokeMember("submit");                        
                    }

                    if (wbr.Url.AbsoluteUri == "https://mail.google.com/mail/u/0/#inbox")
                    {
                        RedirectActiveLink(wbr);                        
                        wbr.DocumentCompleted -= wbr_DocumentCompleted_AccountConfirm;
                        Login();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error!" + System.Environment.NewLine + ex.Message);
            }
        }

        private static void Login()
        {

        }

        private static void wbr_DocumentCompleted_PostRequest(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            System.Windows.Forms.WebBrowser wbr = sender as System.Windows.Forms.WebBrowser;
            try
            {
                if (wbr.ReadyState == System.Windows.Forms.WebBrowserReadyState.Complete)
                {
                    System.Windows.Forms.HtmlElement form = wbr.Document.GetElementById("show_connect");
                    if (form != null)
                    {
                        wbr.Document.GetElementById("email").SetAttribute("value", UserInfo.Email = "buitran986@gmail.com");
                        wbr.Document.GetElementById("password").SetAttribute("value", UserInfo.Password = "12345678");
                        wbr.Document.GetElementById("confirm_password").SetAttribute("value", "12345678");
                        wbr.Document.GetElementById("full_name").SetAttribute("value", UserInfo.Fullname = "tester");
                        form.InvokeMember("submit");
                        //string script = "<script> function HideForm() { $('#myModal').hide(); } </script>";                        
                        //wbr.Document.CreateElement(script);
                        //wbr.Document.InvokeScript("HideForm()");                        

                        wbr.DocumentCompleted -= wbr_DocumentCompleted_PostRequest;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error!" + System.Environment.NewLine + ex.Message);
            }
        }      

        public static void RefreshNews(System.Windows.Forms.WebBrowser wbr)
        {
            int sumPostedNews = GetSumPostedNews(wbr);
            for (int i = 0; i < sumPostedNews; i++)
            {
                string codeString1 = String.Format("$('input[id=checkbox_ttv[" + i + "]]').val();");
                object value1 = wbr.Document.InvokeScript("eval", new[] { codeString1 });
                string[] arg = { value1.ToString() };
                wbr.Document.InvokeScript("ntv_quan_tri_lam_moi_1_ttv", arg);
                wbr.Refresh();
            }            
        }

        static int GetSumPostedNews(System.Windows.Forms.WebBrowser wbr)
        {
            int sumPostedNews;
            string codeString = String.Format("$('#tong_so_ttv_da_dang').val();");
            object value = wbr.Document.InvokeScript("eval", new[] { codeString });

            if (int.TryParse(value.ToString(), out sumPostedNews))
            {
                return sumPostedNews;
            }
            return 0;
        }
    }    
}
