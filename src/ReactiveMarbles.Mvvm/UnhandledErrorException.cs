// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace ReactiveMarbles.Core
{
    /// <summary>
    /// Indicates that an object implementing <see cref="IThrownExceptions"/> has caused an error and nothing is attached
    /// to <see cref="IThrownExceptions.ThrownExceptions"/> to handle that error.
    /// </summary>
    [Serializable]
    public class UnhandledErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledErrorException"/> class.
        /// </summary>
        public UnhandledErrorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledErrorException"/> class.
        /// </summary>
        /// <param name="message">
        /// The exception message.
        /// </param>
        public UnhandledErrorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledErrorException"/> class.
        /// </summary>
        /// <param name="message">
        /// The exception message.
        /// </param>
        /// <param name="innerException">
        /// The exception that caused this exception.
        /// </param>
        public UnhandledErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledErrorException"/> class.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The serialization context.</param>
        protected UnhandledErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
