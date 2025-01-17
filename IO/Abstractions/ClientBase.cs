﻿// ******************************************************************************************
//     Assembly:                Bobo
//     Author:                  Terry D. Eppler
//     Created:                 10-16-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-16-2024
// ******************************************************************************************
// <copyright file="ClientBase.cs" company="Terry D. Eppler">
//    A windows presentation foundation (WPF) app to communicate with the Chat GPT-3.5 Turbo API
// 
//    Copyright ©  2020-2024 Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the “Software”),
//    to deal in the Software without restriction,
//    including without limitation the rights to use,
//    copy, modify, merge, publish, distribute, sublicense,
//    and/or sell copies of the Software,
//    and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
// 
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
// 
//    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT.
//    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//    DEALINGS IN THE SOFTWARE.
// 
//    You can contact me at:  terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   ClientBase.cs
// </summary>
// ******************************************************************************************

namespace Bobo
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" ) ]
    public abstract class ClientBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The is connected
        /// </summary>
        private protected bool _connected;

        /// <summary>
        /// The bytes
        /// </summary>
        private protected int _count;

        /// <summary>
        /// The data
        /// </summary>
        private protected byte[ ] _data;

        /// <summary>
        /// The ip address
        /// </summary>
        private protected IPAddress _ipAddress;

        /// <summary>
        /// The ip end point
        /// </summary>
        private protected IPEndPoint _ipEndPoint;

        /// <summary>
        /// The message
        /// </summary>
        private protected string _message;

        /// <summary>
        /// The locked object
        /// </summary>
        private protected object _entry = new object( );

        /// <summary>
        /// The port
        /// </summary>
        private protected int _port;

        /// <summary>
        /// The socket
        /// </summary>
        private protected Socket _socket;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Pings the network.
        /// </summary>
        /// <param name="ipAddress">
        /// The host name or address.
        /// </param>
        /// <returns>
        /// bool
        /// </returns>
        private protected bool PingNetwork( string ipAddress )
        {
            var _status = false;
            try
            {
                ThrowIf.Null( ipAddress, nameof( ipAddress ) );
                using var _ping = new Ping( );
                var _buffer = Encoding.ASCII.GetBytes( "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" );
                var _timeout = 5000;// 5 sec
                var _reply = _ping.Send( ipAddress, _timeout, _buffer );
                if( _reply != null )
                {
                    _status = _reply.Status == IPStatus.Success;
                }
            }
            catch( Exception ex )
            {
                _status = false;
                Fail( ex );
            }

            return _status;
        }

        /// <summary>
        /// Notifies this instance.
        /// </summary>
        private protected void SendNotification( string message )
        {
            try
            {
                ThrowIf.Null( message, nameof( message ) );
                var _notify = new SplashMessage( message );
                _notify.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Begins the initialize.
        /// </summary>
        private protected void Busy( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = true;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Ends the initialize.
        /// </summary>
        private protected void Chill( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = false;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }

        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value>
        /// <c> true </c>
        /// if this instance is busy; otherwise,
        /// <c> false </c>
        /// </value>
        public bool IsBusy
        {
            get
            {
                lock( _entry )
                {
                    return _busy;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// this instance is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is connected;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get
            {
                return _connected;
            }
            private protected set
            {
                _connected = value;
            }
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}