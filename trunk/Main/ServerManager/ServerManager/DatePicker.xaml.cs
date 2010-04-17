using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Globalization;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para seleccionar una fecha.
    /// </summary>
    public partial class DatePicker
    {
        #region Instance Variables and Properties

        /// <summary>
        /// Returna verdadero si el mes es el primer campo, de acuerdo a la localización.
        /// </summary>
        public static bool IsMonthFirst
        {
            get { return System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern.StartsWith("M", StringComparison.Ordinal); }
        }

        /// <summary>
        /// El combo de día.
        /// </summary>
        public ComboBox Day
        {
            get { return (DatePicker.IsMonthFirst) ? Second : First; }
        }

        /// <summary>
        /// El combo de mes.
        /// </summary>
        public ComboBox Month
        {
            get { return (DatePicker.IsMonthFirst) ? First : Second; }
        }

        /// <summary>
        /// El combo de año.
        /// </summary>
        public ComboBox Year
        {
            get { return Third; }
        }

        /// <summary>
        /// El contenido del componente como fecha.
        /// </summary>
        public DateTime Date
        {
            get
            {
                try
                {
                    return new DateTime(SelectedYear, SelectedMonth, SelectedDay);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return new DateTime(0);
                }
            }

            // Si el año no está en el rando, lanzará una excepción.
            set
            {
                Day.SelectedIndex = value.Day - 1;
                Month.SelectedIndex = value.Month - 1;
                Year.SelectedIndex = value.Year - Int32.Parse((string)Year.Items[0], CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Verdadero si la fecha es válida.
        /// </summary>
        public bool IsValidDate
        {
            get
            {
                try
                {
                    DateTime time = new DateTime(SelectedYear, SelectedMonth, SelectedDay);
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// El día seleccionado.
        /// </summary>
        public int SelectedDay
        {
            get { return Int32.Parse((string)Day.SelectedItem, CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// El mes seleccionado.
        /// </summary>
        public int SelectedMonth
        {
            get { return Int32.Parse((string)Month.SelectedItem, CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// El año seleccionado.
        /// </summary>
        public int SelectedYear
        {
            get { return Int32.Parse((string)Year.SelectedItem, CultureInfo.InvariantCulture); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public DatePicker()
        {
            this.InitializeComponent();
            Fill();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia los campos.
        /// </summary>
        public void Reset()
        {
            First.SelectedIndex = 0;
            Second.SelectedIndex = 0;
            Third.SelectedIndex = 0;
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Llena el campo de día con el rango 1-31.
        /// </summary>
        private void FillDay()
        {
            for (int i = 1; i < 32; i++)
            {
                Day.Items.Add("" + i);
            }

            Day.SelectedIndex = 0;
        }

        /// <summary>
        /// Llena el campo mes con el rango 1-12.
        /// </summary>
        private void FillMonth()
        {
            for (int i = 1; i < 13; i++)
            {
                Month.Items.Add("" + i);
            }

            Month.SelectedIndex = 0;
        }

        /// <summary>
        /// Llena el campo año con el año actual y los próximos 3 años.
        /// </summary>
        private void FillYear(int startYear, int endYear)
        {
            for (int i = startYear; i < endYear; i++)
            {
                Year.Items.Add("" + i);
            }

            Year.SelectedIndex = 0;
        }

        /// <summary>
        /// Llena todos los campos.
        /// </summary>
        public void Fill()
        {
            FillDay();
            FillMonth();
            FillYear(2000, DateTime.Today.Year + 3);
        }

        /// <summary>
        /// Llena todos los campos, para un cierto intervalo de años.
        /// </summary>
        public void Fill(int startYear, int endYear)
        {
            FillDay();
            FillMonth();
            FillYear(startYear, endYear);
        }

        #endregion

        #endregion
    }
}