using System;
using System.Windows;

namespace ReturnToSender
{
    /// <summary>
    /// Base attached property to replace the basic wpf attached property
    /// </summary>
    /// <typeparam name="Parent">The parent class to be the attached property</typeparam>
    /// <typeparam name="Property">The type of this attached property</typeparam>
    public abstract class BaseAttachedProperty<Parent, Property>
        where Parent : BaseAttachedProperty<Parent, Property>, new()
    {
        #region Public Events
        /// <summary>
        /// Fired when the value changes
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };
        #endregion

        #region Public Properties
        /// <summary>
        /// Singleton instance of our parent class
        /// </summary>
        public static Parent Instance { get; private set; } = new Parent();
        #endregion

        #region Attached Properties
        /// <summary>
        /// The attached property for this class
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(Property), typeof(BaseAttachedProperty<Parent, Property>), new PropertyMetadata(new PropertyChangedCallback(OnValuePropertyChanged)));
        /// <summary>
        /// The call back event when the value property is changed
        /// </summary>
        /// <param name="d">The UI element that had its property changed</param>
        /// <param name="e">Event args</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Call the parent function
            Instance.OnValueChanged(d, e);

            // Call event listeners
            Instance.ValueChanged(d, e);
        }

        /// <summary>
        /// Get's the attached property
        /// </summary>
        /// <param name="d">The element to get the property from</param>
        /// <returns></returns>
        public static Property GetValue(DependencyObject d) => (Property)d.GetValue(ValueProperty);

        /// <summary>
        /// Set's the attached property
        /// </summary>
        /// <param name="d">The element to set the property on</param>
        /// <param name="value">The value to set the property to</param>
        public static void SetValue(DependencyObject d, Property value) => d.SetValue(ValueProperty, value);
        #endregion

        #region Event Methods
        /// <summary>
        /// The method that is called when any attached property has changed
        /// </summary>
        /// <param name="sender">The UI element that this property change on</param>
        /// <param name="e">Event args</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
    }
}
