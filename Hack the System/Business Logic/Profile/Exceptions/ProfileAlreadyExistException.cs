// <copyright file="ProfileAlreadyExistException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Profile.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This exception throw if player want to create a new profile with already exist name
    /// </summary>
    [Serializable]
    public class ProfileAlreadyExistException : Exception
    {
    }
}
