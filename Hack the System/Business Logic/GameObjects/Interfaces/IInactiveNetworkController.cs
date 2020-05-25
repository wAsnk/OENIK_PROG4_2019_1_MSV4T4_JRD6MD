// <copyright file="IInactiveNetworkController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.GameObjects.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Parameters of inactiveetworkcontroller
    /// </summary>
    public interface IInactiveNetworkController : INetworkController
    {
        /// <summary>
        /// Gets actualy capture point
        /// </summary>
        int CapturePoint { get; }

        /// <summary>
        /// Gets limit to capture networcontroller
        /// </summary>
        int CaptureLimit { get; }

        /// <summary>
        /// This method will call when networkcontroller captured
        /// </summary>
        void Captured();
    }
}
