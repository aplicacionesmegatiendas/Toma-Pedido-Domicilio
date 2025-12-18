using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{

    internal sealed class Splash : System.Windows.Forms.Form
    {

        #region initialisation

        public Splash()
            : base()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //	Size to the image so as to display it fully and position the form in the center screen with no border.
            //this.Size = this.BackgroundImage.Size;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            //	Force the splash to stay on top while the mainform renders but don't show it in the taskbar.
            this.TopMost = true;
            this.ShowInTaskbar = false;

            //	Make the backcolour Fuchia and set that to be transparent
            //	so that the image can be shown with funny shapes, round corners etc.
            //this.BackColor = System.Drawing.Color.Fuchsia;
            //this.TransparencyKey = System.Drawing.Color.Fuchsia;

            //	Initialise a timer to do the fade out
            if (this.components == null)
            {
                this.components = new System.ComponentModel.Container();
            }
            this.fadeTimer = new System.Windows.Forms.Timer(this.components);

        }

        private System.Windows.Forms.Timer fadeTimer;

        #endregion

        #region Static Methods

        internal static Splash Instance = null;
        private Panel panel1;
        private PictureBox pictureBox1;
        internal static System.Threading.Thread splashThread = null;

        public static void ShowSplash()
        {
            //	Show Splash with no fading
            ShowSplash(0);
        }

        public static void ShowSplash(int fadeinTime)
        {
            //	Only show if not showing already
            if (Instance == null)
            {
                Instance = new Splash();

                //	Hide initially so as to avoid a nasty pre paint flicker
                Instance.Opacity = 0;

                Instance.Show();

                //	Process the initial paint events
                Application.DoEvents();

                // Perform the fade in
                if (fadeinTime > 0)
                {
                    //	Set the timer interval so that we fade out at the same speed.
                    int fadeStep = (int)System.Math.Round((double)fadeinTime / 20);
                    Instance.fadeTimer.Interval = fadeStep;

                    for (int i = 0; i <= fadeinTime; i += fadeStep)
                    {
                        System.Threading.Thread.Sleep(fadeStep);
                        Instance.Opacity += 0.05;
                    }
                }
                else
                {
                    //	Set the timer interval so that we fade out instantly.
                    Instance.fadeTimer.Interval = 1;
                }
                Instance.Opacity = 1;
            }
        }

        public static void Fadeout()
        {
            //	Only fadeout if we are currently visible.
            if (Instance != null)
            {
                Instance.BeginInvoke(new MethodInvoker(Instance.Close));

                //	Process the Close Message on the Splash Thread.

                Application.DoEvents();
            }
        }

        #endregion

        #region Close Splash Methods

        protected override void OnClick(System.EventArgs e)
        {
            //	If we are displaying as a about dialog we need to provide a way out.
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            //	Close immediatly is the timer interval is set to 1 indicating no fade.
            if (this.fadeTimer.Interval == 1)
            {
                e.Cancel = false;
                return;
            }

            //	Only use the timer to fade out if we have a mainform running otherwise there will be no message pump
            if (Application.OpenForms.Count > 1)
            {
                if (this.Opacity > 0)
                {
                    e.Cancel = true;
                    this.Opacity -= 0.05;

                    //	use the timer to iteratively call the close method thereby keeping the GUI thread available for other processes.
                    this.fadeTimer.Tick -= new System.EventHandler(this.FadeoutTick);
                    this.fadeTimer.Tick += new System.EventHandler(this.FadeoutTick);
                    this.fadeTimer.Start();
                }
                else
                {
                    e.Cancel = false;
                    this.fadeTimer.Stop();

                    //	Clear the instance variable so we can reshow the splash, and ensure that we don't try to close it twice
                    Instance = null;
                }
            }
            else
            {
                if (this.Opacity > 0)
                {
                    //	Sleep on this thread to slow down the fade as there is no message pump running
                    System.Threading.Thread.Sleep(this.fadeTimer.Interval);
                    Instance.Opacity -= 0.05;

                    //	iteratively call the close method
                    this.Close();
                }
                else
                {
                    e.Cancel = false;

                    //	Clear the instance variable so we can reshow the splash, and ensure that we don't try to close it twice
                    Instance = null;
                }
            }

        }

        void FadeoutTick(object sender, System.EventArgs e)
        {
            this.Close();

        }

        #endregion

        #region Designer stuff

        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 655);
            this.panel1.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 655);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Splash
            // 
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 655);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.Name = "Splash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Splash_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void Splash_Load(object sender, EventArgs e)
        {

        }
    }
}
