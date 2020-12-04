using System;

namespace MyUtility.Data.Job
{
    public class Vieclam24h
    {
        public static void PostRequest(System.Windows.Forms.WebBrowser wbr, System.Windows.Forms.Timer timer)
        {
            //var url = "http://hcm.vieclam.24h.com.vn/ajax/account/dang_nhap_tk?txt_ten_dang_nhap=" + UserInfo.Email + "&txt_mat_khau=" + UserInfo.Password;
            var url = "http://vieclam.24h.com.vn/taikhoan/login";
            string postData = "username=" + UserInfo.Email + "&password=" + UserInfo.Password;            
                        
            try
            {
                string response = HttpSession.PostRequest(url, postData);
                //System.Threading.Thread.Sleep(5 * 1000);
                if (!string.IsNullOrEmpty(response))
                {                
                    url = "http://vieclam.24h.com.vn/quan-ly-ho-so-tim-viec.html";                    
                    wbr.Navigate(url);
                    timer.Enabled = true;
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error!" + System.Environment.NewLine + ex.Message);
            }
            
        }

        public static void RefreshNews(System.Windows.Forms.WebBrowser wbr)
        {
            string codeString = String.Format("$('.job_detail .col-xs-12 .fwb').val();");
            codeString = codeString.Replace("Mã tin: NTV", "");
            object value = wbr.Document.InvokeScript("eval", new[] { codeString });
            if (value != null)
            {
                string[] arg = { value.ToString() };
                wbr.Document.InvokeScript("lam_moi_ttv", arg);
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
