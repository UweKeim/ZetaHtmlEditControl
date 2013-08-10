namespace Test
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.ThreadException += applicationThreadException;
            Application.SetUnhandledExceptionMode( UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += currentDomainUnhandledException;

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TestFormForScreenshots());
            }
            catch (Exception e)
            {
                doHandleException(e);
            }
        }

        private static void doHandleException(Exception x)
        {
            if (x is ObjectDisposedException)
            {
                // Eat.
            }
            else
            {
                MessageBox.Show(x.Message);
            }
        }

        private static void currentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            doHandleException((Exception)e.ExceptionObject);
        }

        private static void applicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            doHandleException(e.Exception);
        }
    }
}