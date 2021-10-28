using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MyStopWatch2
{
    public partial class KronoTest : Control
    {
        /// <summary>
        /// zaman ile başlayan tüm değerler kronometre değerleridir. gecen ile baslayanlar ise elapsed time içindir. 
        /// </summary>
        public int gecenMs { get; set; } = 0;
        /// <summary>
        /// ileri sayım veya geri sayım modu. İleri sayım= true
        /// </summary>
        public bool mod { get; set; } = true;
        /// <summary>
        /// Stopwatch'ın zaman değerleri bu değerlerde tutulur.
        /// </summary>
        public int zamanSaat { get; set; } = 0;

        public int zamanDakika { get; set; } = 0;

        public int zamanSaniye { get; set; } = 0;
        public int zamanMs { get; set; } = 0;

        /// <summary>
        /// Elapsed zamanlar bu değişkenlerde tutulur
        /// </summary>
        public int gecenSaat { get; set; } = 0;

        public int gecenSaniye { get; set; } = 0;

        public int gecenDakika { get; set; } = 0;

        /// <summary>
        /// Calisma normal kronometre'nin çalışmasını kontrol eder. elapsedCalisma geçen zaman sayacını kontrol eder. bunlar birbirinden
        /// bağımsızdır.
        /// </summary>

        public bool calisma { get; set; } = false;

        public bool elapsedCalisma { get; set; } = false;
        /// <summary>
        /// Gösterge tipi ya geçen zamandır ya da kronometredir. True değeri kronometreyi temsil eder.
        /// </summary>
        public bool gostergeTipi { get; set; } = true;
        /// <summary>
        /// elapsed sayacı farklı değerler olarak gösterebilir.
        /// </summary>
        public enum elapsedgosterge { millisaniye, saniye, dakika, saat };

        private elapsedgosterge ElapsedGosterge = elapsedgosterge.millisaniye;

        public elapsedgosterge ElapsedGostergeTipi { get=>ElapsedGosterge; set { ElapsedGosterge = value; Invalidate(); } }

        private Timer t = new Timer();
        
        /// <summary>
        /// Constructor
        /// </summary>
        public KronoTest()
        {
            InitializeComponent();
            t.Interval = 10;
            t.Enabled = true;
            t.Tick += StopwatchTick;
            DoubleBuffered = true;



        }
        /// <summary>
        /// çizimler burada gerçekleşir.
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            string text = "";

            switch (gostergeTipi)
            {
                case true:


                    text = ( string.Format("{0:00}:{1:00}:{2:00}.{3:00}",zamanSaat,zamanDakika,zamanSaniye,zamanMs)) ;




                    break;

                case false:

                    switch (ElapsedGostergeTipi)
                    {

                        case elapsedgosterge.millisaniye:

                            text = gecenMs.ToString();


                            break;


                        case elapsedgosterge.saniye:
                            text = gecenSaniye.ToString();

                            break;

                       
                        case elapsedgosterge.dakika:

                            text = gecenDakika.ToString();

                            break;


                        case elapsedgosterge.saat:

                            text = gecenSaat.ToString();

                            break;

                    }




                    break;



            }



            pe.Graphics.DrawString(text,Font,new Pen(ForeColor).Brush, new PointF(0f,0f));



        }

        /// <summary>
        /// Tekrarlanan tüm işlemler bu fonksiyonda işlenir.
        /// </summary>
        private void StopwatchTick(object sender,EventArgs e)
        {
            //ileri sayma modu.
            if (mod == true && calisma == true && !DesignMode)
            {
                //Her millisaniye 1 arttır, 100'e ulaşıldığında saniyeyi arttır ve Ms değerini 0 yap
                zamanMs++;
                if (zamanMs >= 100)
                {   //her Saniyede 1 arttır, 60'a ulaşıldığında dakika arttır ve Saniye değerini 0 yap
                    zamanSaniye++;
                    zamanMs = 0;
                    if (zamanSaniye >= 60)
                    {   //her Dakika'da 1 arttır, 60'a ulaşıldığında saat arttır ve Saniye değerini 0 yap
                        zamanDakika++;
                        zamanSaniye = 0;

                        if (zamanDakika >= 60)
                        {  //her saatte 1 arttır
                            zamanSaat++;
                            zamanDakika = 0;

                        }

                    }

                }

            }

            //Geri sayma modu.
            if (mod == false && calisma == true && !DesignMode )
            {
                //her millisaniyede 1 azalt, 
                if (zamanMs > 0)
                {
                    zamanMs--;


                }
                //eğer 0 ise saniyeden bir eksilt ve millisaniye değerini 100 yap.
                if (zamanMs <= 0)
                {

                    zamanMs = 0;
                    //Saniyeden bir azalt
                    if (zamanSaniye > 0)
                    {
                        zamanSaniye--;
                        zamanMs = 100;

                    }
                    //Saniye yoksa dakikadan bir azalt ve saniyeyi 60 yap
                    if (zamanSaniye <= 0)
                    {

                        zamanSaniye = 0;
                        //Dakikadan bir azalt
                        if (zamanDakika > 0)
                        {
                            zamanDakika--;
                            zamanSaniye = 60;


                        }
                        //Dakika yoksa saati bir azalt ve dakikayı 60 yap.
                        if (zamanDakika <= 0)
                        {
                            zamanDakika = 0;
                            if (zamanSaat > 0)
                            {
                                //Saati 1 azalt.
                                zamanSaat--;
                                zamanDakika = 60;


                            }

                        }


                    }


                }
                //Eeğer tüm değerler 0'a ulaşırsa saymayı durdur.
                if (zamanSaat == 0 && zamanDakika == 0 && zamanSaniye == 0 && zamanMs == 0)
                {
                    calisma = false;

                }



            }
            //Bir işlemin yapılmasında geçen zamanı kontrol etmek için bağımsız sayaç

            if (elapsedCalisma == true && !DesignMode)
            {
                gecenMs++;

                gecenSaniye = gecenMs / 100;

                gecenDakika = gecenSaniye / 60;

                gecenSaat = gecenDakika / 60;


            }

            Invalidate();

        }
        /// <summary>
        /// Dakikayı değiştiren fonksiyon
        /// </summary>
        /// <param name="s"></param>
        public void dakikaDegistir(string s)
        {
            int islem;


            if (!calisma)
            {

                if (int.TryParse(s, out int i))
                {
                    islem = int.Parse(s);






                }
                else
                {

                    islem = 0;


                }
                zamanDakika = islem;

                parametreKontrol();
            }


        }
        /// <summary>
        /// saniyeyi değiştiren fonksiyon
        /// </summary>
        /// <param name="s"></param>
        public void saniyeDegistir(string s)

        {
            int islem;
            string cikti;
            if (!calisma)
            {

                if (int.TryParse(s, out int i))
                {
                    islem = int.Parse(s);


                    cikti = islem.ToString();



                }
                else
                {
                    cikti = "0";
                    islem = 0;


                }
                zamanSaniye = islem;

                parametreKontrol();

            }



        }
        /// <summary>
        /// elapsed timeri başlat
        /// </summary>
        public void GecenBaslat()
        {
            elapsedCalisma = true;


        }


        /// <summary>
        /// Elapsed'in değerlerini silmeden durdur
        /// </summary>
        public void GecenDuraklat()
        {
            elapsedCalisma = false;

        }


        /// <summary>
        /// Stopwatch ileri veya geri sayımını değiştir. Varsayılan değer true
        /// </summary>
        /// <param name="s"></param>
        public void modDegistir(bool s = true)
        {



            mod = s;




        }



        /// <summary>
        /// Stopwatchı durdurur ve değerleri siler
        /// </summary>
        public void StopwatchDurdur()
        {

            zamanSaat = 0;
            zamanDakika = 0;
            zamanSaniye = 0;
            zamanMs = 0;
            calisma = false;


        }

        /// <summary>
        /// Stopwatchı durdurur ama değerleri silmez
        /// </summary>
        public void StopwatchDuraklat()
        {
            calisma = false;


        }
        /// <summary>
        /// Bu değişen değerleri düzenler. 60 sn = 1 dk ör.
        /// </summary>
        private void parametreKontrol()
        {
            if (!calisma)
            {
                if (zamanMs >= 100)
                {

                    zamanSaniye += zamanMs / 100;
                    zamanMs = zamanMs % 100;

                }
                if (zamanMs < 0)
                {
                    zamanMs = 0;

                }

                if (zamanSaniye >= 60)
                {
                    zamanDakika += zamanSaniye / 60;
                    zamanSaniye = zamanSaniye % 60;



                }
                if (zamanSaniye < 0)
                {
                    zamanSaniye = 0;

                }
                if (zamanDakika >= 60)
                {
                    zamanSaat += zamanDakika / 60;
                    zamanDakika = zamanDakika % 60;


                }
                if (zamanDakika < 0)
                {
                    zamanDakika = 0;

                }
                if (zamanSaat >= 100)
                {
                    zamanSaat = 100;

                }
                if (zamanSaat < 0)
                {
                    zamanSaat = 0;

                }



            }

        }

        /// <summary>
/// saat değerini değiştirir.
/// </summary>
/// <param name="s"></param>
        public void saatDegistir(string s)

        {
            int islem;
            string cikti;

            if (!calisma)
            {
                if (int.TryParse(s, out int i))
                {
                    islem = int.Parse(s);


                    cikti = islem.ToString();



                }
                else
                {
                    cikti = "0";
                    islem = 0;


                }
                zamanSaat = islem;

                parametreKontrol();

            }


        }
        /// <summary>
        /// Stopwatch'ı başlatır. Duraklatmak için kullanılabilinir. varsayılan değer true.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool StopwatchCalistir(bool b = true)
        {
            calisma = b;
            return calisma;

        }
        /// <summary>
        /// Ms değerini değiştir
        /// </summary>
        /// <param name="s"></param>
        public void MsDegistir(string s)
        {
            int islem;
            string cikti;

            if (!calisma)
            {
                if (int.TryParse(s, out int i))
                {
                    islem = int.Parse(s);


                    cikti = islem.ToString();



                }
                else
                {
                    cikti = "0";
                    islem = 0;


                }
                zamanDakika = islem;

                parametreKontrol();
            }



        }

       

    }
}
