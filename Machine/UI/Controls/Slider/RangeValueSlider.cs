

namespace Bobo
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Xml.Linq;

    [ DefaultEvent( "RangeSelectionChanged" ),
      TemplatePart( Name = "PART_RangeSliderContainer", Type = typeof( StackPanel ) ),
      TemplatePart( Name = "PART_LeftEdge", Type = typeof( RepeatButton ) ),
      TemplatePart( Name = "PART_RightEdge", Type = typeof( RepeatButton ) ),
      TemplatePart( Name = "PART_LeftThumb", Type = typeof( Thumb ) ),
      TemplatePart( Name = "PART_MiddleThumb", Type = typeof( Thumb ) ),
      TemplatePart( Name = "PART_RightThumb", Type = typeof( Thumb ) ) ]
    public sealed class RangeSlider : Control
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static RangeSlider( ) =>
            DefaultStyleKeyProperty.OverrideMetadata( typeof( RangeSlider ),
                new FrameworkPropertyMetadata( typeof( RangeSlider ) ) );

        /// <summary>
        /// Default constructor
        /// </summary>
        public RangeSlider( )
        {
            CommandBindings.Add( new CommandBinding( MoveBack, MoveBackHandler ) );
            CommandBindings.Add( new CommandBinding( MoveForward, MoveForwardHandler ) );
            CommandBindings.Add( new CommandBinding( MoveAllForward, MoveAllForwardHandler ) );
            CommandBindings.Add( new CommandBinding( MoveAllBack, MoveAllBackHandler ) );

            //hook to the size change event of the range slider
            DependencyPropertyDescriptor.FromProperty( ActualWidthProperty, typeof( RangeSlider ) )
                .AddValueChanged( this, delegate { ReCalculateWidths( ); } );
        }

        bool _internalUpdate = false;

        const double
            REPEAT_BUTTON_MOVE_RATIO =
                0.1;//used to move the selection by x ratio when click the repeat buttons

        const double DEFAULT_SPLITTERS_THUMB_WIDTH = 10;

        Thumb _centerThumb;//the center thumb to move the range around

        Thumb _leftThumb;//the left thumb that is used to expand the range selected

        Thumb _rightThumb;//the right thumb that is used to expand the range selected

        RepeatButton _leftButton;//the left side of the control (movable left part)

        RepeatButton _rightButton;//the right side of the control (movable right part)

        StackPanel
            _visualElementsContainer;//stackpanel to store the visual elements for this control
        /// <summary>
        /// The min value for the range of the range slider
        /// </summary>
        public long RangeStart
        {
            get =>
                ( long )GetValue( RangeStartProperty );
            set =>
                SetValue( RangeStartProperty, value );
        }

        /// <summary>
        /// The min value for the range of the range slider
        /// </summary>
        public static readonly DependencyProperty RangeStartProperty = DependencyProperty.Register(
            "RangeStart", typeof( long ), typeof( RangeSlider ), new UIPropertyMetadata( ( long )0,
                delegate( DependencyObject sender, DependencyPropertyChangedEventArgs e )
                {
                    var _slider = ( RangeSlider )sender;
                    if( _slider._internalUpdate == false )//check if the property is set internally
                    {
                        _slider.ReCalculateRanges( );
                        _slider.ReCalculateWidths( );
                    }
                } ) );


        /// <summary>
        /// The max value for the range of the range slider
        /// </summary>
        public long RangeStop
        {
            get =>
                ( long )GetValue( RangeStopProperty );
            set =>
                SetValue( RangeStopProperty, value );
        }

        /// <summary>
        /// The max value for the range of the range slider
        /// </summary>
        public static readonly DependencyProperty RangeStopProperty = DependencyProperty.Register(
            "RangeStop", typeof( long ), typeof( RangeSlider ), new UIPropertyMetadata( ( long )1,
                delegate( DependencyObject sender, DependencyPropertyChangedEventArgs e )
                {
                    var _slider = ( RangeSlider )sender;
                    if( _slider._internalUpdate == false )//check if the property is set internally
                    {
                        _slider.ReCalculateRanges( );
                        _slider.ReCalculateWidths( );
                    }
                } ) );


        /// <summary>
        /// The min value of the selected range of the range slider
        /// </summary>
        public long RangeStartSelected
        {
            get =>
                ( long )GetValue( RangeStartSelectedProperty );
            set =>
                SetValue( RangeStartSelectedProperty, value );
        }

        /// <summary>
        /// The min value of the selected range of the range slider
        /// </summary>
        public static readonly DependencyProperty RangeStartSelectedProperty =
            DependencyProperty.Register( "RangeStartSelected", typeof( long ),
                typeof( RangeSlider ), new UIPropertyMetadata( ( long )0,
                    delegate( DependencyObject sender, DependencyPropertyChangedEventArgs e )
                    {
                        var _slider = ( RangeSlider )sender;
                        if( _slider._internalUpdate
                            == false )//check if the property is set internally
                        {
                            _slider.ReCalculateWidths( );
                            _slider.OnRangeSelectionChanged(
                                new RangeSelectionChangedEventArgs( _slider ) );
                        }
                    } ) );


        /// <summary>
        /// The max value of the selected range of the range slider
        /// </summary>
        public long RangeStopSelected
        {
            get =>
                ( long )GetValue( RangeStopSelectedProperty );
            set =>
                SetValue( RangeStopSelectedProperty, value );
        }

        /// <summary>
        /// The max value of the selected range of the range slider
        /// </summary>
        public static readonly DependencyProperty RangeStopSelectedProperty =
            DependencyProperty.Register( "RangeStopSelected", typeof( long ), typeof( RangeSlider ),
                new UIPropertyMetadata( ( long )1,
                    delegate( DependencyObject sender, DependencyPropertyChangedEventArgs e )
                    {
                        var _slider = ( RangeSlider )sender;
                        if( _slider._internalUpdate
                            == false )//check if the property is set internally
                        {
                            _slider.ReCalculateWidths( );
                            _slider.OnRangeSelectionChanged(
                                new RangeSelectionChangedEventArgs( _slider ) );
                        }
                    } ) );


        /// <summary>
        /// The min range value that you can have for the range slider
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when MinRange is set less than 0</exception>
        public long MinRange
        {
            get =>
                ( long )GetValue( MinRangeProperty );
            set =>
                SetValue( MinRangeProperty, value );
        }

        /// <summary>
        /// The min range value that you can have for the range slider
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when MinRange is set less than 0</exception>
        public static readonly DependencyProperty MinRangeProperty = DependencyProperty.Register(
            "MinRange", typeof( long ), typeof( RangeSlider ), new UIPropertyMetadata( ( long )0,
                delegate( DependencyObject sender, DependencyPropertyChangedEventArgs e )
                {
                    if( ( long )e.NewValue < 0 )
                        throw new ArgumentOutOfRangeException( "value",
                            "value for MinRange cannot be less than 0" );

                    var _slider = ( RangeSlider )sender;
                    if( !_slider._internalUpdate )//check if the property is set internally
                    {
                        _slider._internalUpdate =
                            true;//set flag to signal that the properties are being set by the object itself

                        _slider.RangeStopSelected = Math.Max( _slider.RangeStopSelected,
                            _slider.RangeStartSelected + ( long )e.NewValue );

                        _slider.RangeStop = Math.Max( _slider.RangeStop, _slider.RangeStopSelected );
                        _slider._internalUpdate =
                            false;//set flag to signal that the properties are being set by the object itself

                        _slider.ReCalculateRanges( );
                        _slider.ReCalculateWidths( );
                    }
                } ) );

        /// <summary>
        /// Event raised whenever the selected range is changed
        /// </summary>
        public static readonly RoutedEvent RangeSelectionChangedEvent =
            EventManager.RegisterRoutedEvent( "RangeSelectionChanged", RoutingStrategy.Bubble,
                typeof( RangeSelectionChangedEventHandler ), typeof( RangeSlider ) );

        /// <summary>
        /// Event raised whenever the selected range is changed
        /// </summary>
        public event RangeSelectionChangedEventHandler RangeSelectionChanged
        {
            add =>
                AddHandler( RangeSelectionChangedEvent, value );
            remove =>
                RemoveHandler( RangeSelectionChangedEvent, value );
        }


        /// <summary>
        /// Command to move back the selection
        /// </summary>
        public static RoutedUICommand MoveBack = new( "MoveBack", "MoveBack", typeof( RangeSlider ),
            new InputGestureCollection( new InputGesture[ ]
            {
                new KeyGesture( Key.B, ModifierKeys.Control )
            } ) );

        /// <summary>
        /// Command to move forward the selection
        /// </summary>
        public static RoutedUICommand MoveForward = new( "MoveForward", "MoveForward",
            typeof( RangeSlider ), new InputGestureCollection( new InputGesture[ ]
            {
                new KeyGesture( Key.F, ModifierKeys.Control )
            } ) );

        /// <summary>
        /// Command to move all forward the selection
        /// </summary>
        public static RoutedUICommand MoveAllForward = new( "MoveAllForward", "MoveAllForward",
            typeof( RangeSlider ), new InputGestureCollection( new InputGesture[ ]
            {
                new KeyGesture( Key.F, ModifierKeys.Alt )
            } ) );

        /// <summary>
        /// Command to move all back the selection
        /// </summary>
        public static RoutedUICommand MoveAllBack = new( "MoveAllBack", "MoveAllBack",
            typeof( RangeSlider ), new InputGestureCollection( new InputGesture[ ]
            {
                new KeyGesture( Key.B, ModifierKeys.Alt )
            } ) );





        void MoveAllBackHandler( object sender, ExecutedRoutedEventArgs e )
        {
            ResetSelection( true );
        }

        void MoveAllForwardHandler( object sender, ExecutedRoutedEventArgs e )
        {
            ResetSelection( false );
        }

        void MoveBackHandler( object sender, ExecutedRoutedEventArgs e )
        {
            MoveSelection( true );
        }

        void MoveForwardHandler( object sender, ExecutedRoutedEventArgs e )
        {
            MoveSelection( false );
        }
        //drag thumb from the right splitter
        private void RightThumbDragDelta( object sender, DragDeltaEventArgs e )
        {
            MoveThumb( _centerThumb, _rightButton, e.HorizontalChange );
            ReCalculateRangeSelected( false, true );
        }

        //drag thumb from the left splitter
        private void LeftThumbDragDelta( object sender, DragDeltaEventArgs e )
        {
            MoveThumb( _leftButton, _centerThumb, e.HorizontalChange );
            ReCalculateRangeSelected( true, false );
        }

        //left repeat button clicked
        private void LeftButtonClick( object sender, RoutedEventArgs e ) => MoveSelection( true );

        //right repeat button clicked
        private void RightButtonClick( object sender, RoutedEventArgs e ) => MoveSelection( false );

        //drag thumb from the middle
        private void CenterThumbDragDelta( object sender, DragDeltaEventArgs e )
        {
            MoveThumb( _leftButton, _rightButton, e.HorizontalChange );
            ReCalculateRangeSelected( true, true );
        }
        //resizes the left column and the right column
        private static void MoveThumb( FrameworkElement x, FrameworkElement y,
            double horizonalChange )
        {
            double _change = 0;
            if( horizonalChange < 0 )//slider went left
                _change = GetChangeKeepPositive( x.Width, horizonalChange );
            else if( horizonalChange > 0 )//slider went right if(horizontal change == 0 do nothing)
                _change = -GetChangeKeepPositive( y.Width, -horizonalChange );

            x.Width += _change;
            y.Width -= _change;
        }

        //ensures that the new value (newValue param) is a valid value. returns false if not
        private static double GetChangeKeepPositive( double width, double increment ) =>
            Math.Max( width + increment, 0 ) - width;
        long _movableRange = 0;

        double _movableWidth = 0;

        //recalculates the movableRange. called from the RangeStop setter, RangeStart setter and MinRange setter
        private void ReCalculateRanges( ) => _movableRange = RangeStop - RangeStart - MinRange;

        //recalculates the movableWidth. called whenever the width of the control changes
        private void ReCalculateWidths( )
        {
            if( _leftButton != null
                && _rightButton != null
                && _centerThumb != null )
            {
                _movableWidth =
                    Math.Max(
                        ActualWidth - _rightThumb.ActualWidth - _leftThumb.ActualWidth
                        - _centerThumb.MinWidth, 1 );

                _leftButton.Width =
                    Math.Max( _movableWidth * ( RangeStartSelected - RangeStart ) / _movableRange,
                        0 );

                _rightButton.Width =
                    Math.Max( _movableWidth * ( RangeStop - RangeStopSelected ) / _movableRange, 0 );

                _centerThumb.Width =
                    Math.Max(
                        ActualWidth - _leftButton.Width - _rightButton.Width - _rightThumb.ActualWidth
                        - _leftThumb.ActualWidth, 0 );
            }
        }

        //recalculates the rangeStartSelected called when the left thumb is moved and when the middle thumb is moved
        //recalculates the rangeStopSelected called when the right thumb is moved and when the middle thumb is moved
        private void ReCalculateRangeSelected( bool reCalculateStart, bool reCalculateStop )
        {
            _internalUpdate =
                true;//set flag to signal that the properties are being set by the object itself

            if( reCalculateStart )
            {
                // Make sure to get exactly rangestart if thumb is at the start
                if( _leftButton.Width == 0.0 )
                    RangeStartSelected = RangeStart;
                else
                    RangeStartSelected = Math.Max( RangeStart,
                        ( long )( RangeStart + _movableRange * _leftButton.Width / _movableWidth ) );
            }

            if( reCalculateStop )
            {
                // Make sure to get exactly rangestop if thumb is at the end
                if( _rightButton.Width == 0.0 )
                    RangeStopSelected = RangeStop;
                else
                    RangeStopSelected = Math.Min( RangeStop,
                        ( long )( RangeStop - _movableRange * _rightButton.Width / _movableWidth ) );
            }

            _internalUpdate =
                false;//set flag to signal that the properties are being set by the object itself

            if( reCalculateStart || reCalculateStop )

                //raise the RangeSelectionChanged event
                OnRangeSelectionChanged( new RangeSelectionChangedEventArgs( this ) );
        }

        /// <summary>
        /// moves the current selection with x value
        /// </summary>
        /// <param name="isLeft">True if you want to move to the left</param>
        public void MoveSelection( bool isLeft )
        {
            var _widthChange = REPEAT_BUTTON_MOVE_RATIO * ( RangeStopSelected - RangeStartSelected )
                * _movableWidth / _movableRange;

            _widthChange = isLeft
                ? -_widthChange
                : _widthChange;

            MoveThumb( _leftButton, _rightButton, _widthChange );
            ReCalculateRangeSelected( true, true );
        }

        /// <summary>
        /// Reset the Slider to the Start/End
        /// </summary>
        /// <param name="isStart">Pass true to reset to start point</param>
        public void ResetSelection( bool isStart )
        {
            double _widthChange = RangeStop - RangeStart;
            _widthChange = isStart
                ? -_widthChange
                : _widthChange;

            MoveThumb( _leftButton, _rightButton, _widthChange );
            ReCalculateRangeSelected( true, true );
        }

        ///<summary>
        /// Change the range selected 
        ///</summary>
        ///<param name="span">The steps</param>
        public void MoveSelection( long span )
        {
            if( span > 0 )
            {
                if( RangeStopSelected + span > RangeStop )
                {
                    span = RangeStop - RangeStopSelected;
                }
            }
            else
            {
                if( RangeStartSelected + span < RangeStart )
                {
                    span = RangeStart - RangeStartSelected;
                }
            }

            if( span != 0 )
            {
                _internalUpdate =
                    true;//set flag to signal that the properties are being set by the object itself

                RangeStartSelected += span;
                RangeStopSelected += span;
                ReCalculateWidths( );
                _internalUpdate =
                    false;//set flag to signal that the properties are being set by the object itself

                OnRangeSelectionChanged( new RangeSelectionChangedEventArgs( this ) );
            }
        }

        /// <summary>
        /// Sets the selected range in one go. If the selection is invalid, nothing happens.
        /// </summary>
        /// <param name="selectionStart">New selection start value</param>
        /// <param name="selectionStop">New selection stop value</param>
        public void SetSelectedRange( long selectionStart, long selectionStop )
        {
            var _start = Math.Max( RangeStart, selectionStart );
            var _stop = Math.Min( selectionStop, RangeStop );
            _start = Math.Min( _start, RangeStop - MinRange );
            _stop = Math.Max( RangeStart + MinRange, _stop );
            if( _stop >= _start + MinRange )
            {
                _internalUpdate =
                    true;//set flag to signal that the properties are being set by the object itself

                RangeStartSelected = _start;
                RangeStopSelected = _stop;
                ReCalculateWidths( );
                _internalUpdate =
                    false;//set flag to signal that the properties are being set by the object itself

                OnRangeSelectionChanged( new RangeSelectionChangedEventArgs( this ) );
            }
        }

        /// <summary>
        /// Changes the selected range to the supplied range
        /// </summary>
        /// <param name="span">The span to zoom</param>
        public void ZoomToSpan( long span )
        {
            _internalUpdate =
                true;//set flag to signal that the properties are being set by the object itself

            // Ensure new span is within the valid range
            span = Math.Min( span, RangeStop - RangeStart );
            span = Math.Max( span, MinRange );
            if( span == RangeStopSelected - RangeStartSelected )
            {
                return;// No change
            }

            // First zoom half of it to the right
            var _rightChange = ( span - ( RangeStopSelected - RangeStartSelected ) ) / 2;
            var _leftChange = _rightChange;

            // If we will hit the right edge, spill over the leftover change to the other side
            if( _rightChange > 0
                && RangeStopSelected + _rightChange > RangeStop )
            {
                _leftChange += _rightChange - ( RangeStop - RangeStopSelected );
            }

            RangeStopSelected = Math.Min( RangeStopSelected + _rightChange, RangeStop );
            _rightChange = 0;

            // If we will hit the left edge and there is space on the right, add the leftover change to the other side
            if( _leftChange > 0
                && RangeStartSelected - _leftChange < RangeStart )
            {
                _rightChange = RangeStart - ( RangeStartSelected - _leftChange );
            }

            RangeStartSelected = Math.Max( RangeStartSelected - _leftChange, RangeStart );
            if( _rightChange > 0 )// leftovers to the right
            {
                RangeStopSelected = Math.Min( RangeStopSelected + _rightChange, RangeStop );
            }

            ReCalculateWidths( );
            _internalUpdate =
                false;//set flag to signal that the properties are being set by the object itself

            OnRangeSelectionChanged( new RangeSelectionChangedEventArgs( this ) );
        }
        //Raises the RangeSelectionChanged event
        private void OnRangeSelectionChanged( RangeSelectionChangedEventArgs e )
        {
            e.RoutedEvent = RangeSelectionChangedEvent;
            RaiseEvent( e );
        }

        /// <summary>
        /// Overide to get the visuals from the control template
        /// </summary>
        public override void OnApplyTemplate( )
        {
            base.OnApplyTemplate( );
            _visualElementsContainer = EnforceInstance<StackPanel>( "PART_RangeSliderContainer" );
            _centerThumb = EnforceInstance<Thumb>( "PART_MiddleThumb" );
            _leftButton = EnforceInstance<RepeatButton>( "PART_LeftEdge" );
            _rightButton = EnforceInstance<RepeatButton>( "PART_RightEdge" );
            _leftThumb = EnforceInstance<Thumb>( "PART_LeftThumb" );
            _rightThumb = EnforceInstance<Thumb>( "PART_RightThumb" );
            InitializeVisualElementsContainer( );
            ReCalculateWidths( );
        }

        T EnforceInstance<T>( string partName )
            where T : FrameworkElement, new( )
        {
            if( GetTemplateChild( partName ) is not T _element )
            {
                _element = new T( );
            }

            return _element;
        }

        //adds all visual element to the conatiner
        private void InitializeVisualElementsContainer( )
        {
            _visualElementsContainer.Orientation = Orientation.Horizontal;
            _leftThumb.Width = DEFAULT_SPLITTERS_THUMB_WIDTH;
            _leftThumb.Tag = "left";
            _rightThumb.Width = DEFAULT_SPLITTERS_THUMB_WIDTH;
            _rightThumb.Tag = "right";

            //handle the drag delta
            _centerThumb.DragDelta += CenterThumbDragDelta;
            _leftThumb.DragDelta += LeftThumbDragDelta;
            _rightThumb.DragDelta += RightThumbDragDelta;
            _leftButton.Click += LeftButtonClick;
            _rightButton.Click += RightButtonClick;
        }
    }

    /// <summary>
    /// Delegate for the RangeSelectionChanged event
    /// </summary>
    /// <param name="sender">The object raising the event</param>
    /// <param name="e">The event arguments</param>
    public delegate void RangeSelectionChangedEventHandler( object sender,
        RangeSelectionChangedEventArgs e );

    /// <summary>
    /// Event arguments for the Range slider RangeSelectionChanged event
    /// </summary>
    public class RangeSelectionChangedEventArgs : RoutedEventArgs
    {

        /// <summary>
        /// sets the range start and range stop for the event args by using the slider RangeStartSelected and RangeStopSelected properties
        /// </summary>
        /// <param name="slider">The slider to get the info from</param>
        internal RangeSelectionChangedEventArgs( RangeSlider slider )
            : this( slider.RangeStartSelected, slider.RangeStopSelected )
        {
        }

        /// <summary>
        /// The new range start selected in the range slider
        /// </summary>
        public long NewRangeStart { get; set; }

        /// <summary>
        /// The new range stop selected in the range slider
        /// </summary>
        public long NewRangeStop { get; set; }

        /// <summary>
        /// sets the range start and range stop for the event args
        /// </summary>
        /// <param name="newRangeStart">The new range start set</param>
        /// <param name="newRangeStop">The new range stop set</param>
        internal RangeSelectionChangedEventArgs( long newRangeStart, long newRangeStop )
        {
            this.NewRangeStart = newRangeStart;
            this.NewRangeStop = newRangeStop;
        }
    }
}