using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ReturnToSender
{
    /// <summary>
    /// A base value converter that allows direct XAML usage
    /// </summary>
    /// <typeparam name="T">The type of this converter</typeparam>
    public abstract class BaseMultiValueConverter<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {
        #region Private Members
        private static T mConverter = null;
        #endregion

        #region Markup Extension Methods
        /// <summary>
        /// Provides a static instance of the value converter
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return mConverter ?? (mConverter = new T());
        }
        #endregion

        #region Value Converter Methods
        /// <summary>
        /// The method for converting the type to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        public abstract object Convert(object[] value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The method for converting back to the source type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        public abstract object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture);
        #endregion
    }
}
