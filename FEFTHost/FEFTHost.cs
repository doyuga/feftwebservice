using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.Threading;
using FEFTRest;

//references
//http://www.codeproject.com/Articles/16299/Hosting-WCF-services-in-a-Windows-Forms-Applicatio

namespace FEFTHost
{
    public partial class frmFEFTHost : Form
    {
        public frmFEFTHost()
        {
            InitializeComponent();
        }

        #region PRIVATE VARIABLES
        //WCF SERVICE THREAD
        const int SleepTime = 100;  
        private bool m_running; 
        private Thread m_thread;
        private ServiceHost serviceHost = null;

        //NOTIFICATION ICONS
        private System.Windows.Forms.MenuItem menuExit;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        
        //LOGGER
        private static Logger log = new Logger();
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //WCF SERVICE THREAD
                this.m_thread = new Thread(new ThreadStart(ThreadMethod));
                this.m_thread.Start();

                #region NOTIFICATION ICON
                //http://msdn.microsoft.com/en-us/library/system.windows.forms.notifyicon.icon(v=vs.90).aspx
                this.components = new System.ComponentModel.Container();
                this.contextMenu1 = new System.Windows.Forms.ContextMenu();
                this.menuExit = new System.Windows.Forms.MenuItem();


                // Initialize contextMenu1 
                this.contextMenu1.MenuItems.AddRange(
                            new System.Windows.Forms.MenuItem[] { this.menuExit });

                // Initialize menuExit 
                this.menuExit.Index = 0;
                this.menuExit.Text = "E&xit";
                this.menuExit.Click += new EventHandler(menuExit_Click);

                //// Set up how the form should be displayed. 
                //this.ClientSize = new System.Drawing.Size(292, 266);
                //this.Text = "Notify Icon Example";

                // Create the NotifyIcon. 
                this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

                // The Icon property sets the icon that will appear 
                // in the systray for this application.
                notifyIcon1.Icon = FEFTHost.Properties.Resources.feft_00;

                // The ContextMenu property sets the menu that will 
                // appear when the systray icon is right clicked.
                notifyIcon1.ContextMenu = this.contextMenu1;

                // The Text property sets the text that will be displayed, 
                // in a tooltip, when the mouse hovers over the systray icon.
                notifyIcon1.Text = "FORTIFIED EFT Web Service V3.0";
                notifyIcon1.Visible = true;

                // Handle the DoubleClick event to activate the form.
                notifyIcon1.DoubleClick += new EventHandler(notifyIcon1_DoubleClick);
                #endregion

                log.LogMsg(LogModes.FILE_LOG_COMMON, LogLevel.INFO, "Service successfully started!");
            }
            catch (Exception ex) {
                log.LogMsg(LogModes.FILE_LOG_COMMON, LogLevel.ERROR, ex.Message);
            }
        }

        void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //MEANT TO POPUP A WINDOWS. DO NOTHING FOR NOW
        }

        void menuExit_Click(object sender, EventArgs e)
        {
            //try stop the thread
            this.m_running = false;

            //stop the application
            Application.Exit();
        }

        void ThreadMethod()
        {
            try
            {
                m_running = true;

                // Start the host
                this.serviceHost = new ServiceHost(typeof(FEFTRest.FEFTService));
                foreach (var endpoint in serviceHost.Description.Endpoints)
                {
                    endpoint.Behaviors.Add(new FEFTRest.utilies.IncomingMessageLogger());
                }

                this.serviceHost.Open();

                while (m_running)
                {
                    // Wait until thread is stopped
                    Thread.Sleep(SleepTime);
                }

                // Stop the host
                this.serviceHost.Close();
            }
            catch (Exception ex1)
            {
                //LOG LEVEL 1
                log.LogMsg(LogModes.FILE_LOG_COMMON, LogLevel.ERROR, ex1.Message);

                if (this.serviceHost != null)
                {
                    try
                    {
                        this.serviceHost.Close();
                    }
                    catch (Exception ex2) {
                        //LOG LEVEL 2
                        log.LogMsg(LogModes.FILE_LOG_COMMON, LogLevel.ERROR, ex2.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Request the end of the thread method.
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                m_running = false;
            }
        }
    }
}
