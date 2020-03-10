// -----------------------------------------------------------------------
// <copyright file="BlinkingService.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WPF.Blinkers.Services
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    ///     Static Blinking Service class.
    /// </summary>
    public static class BlinkingService
    {
        /// <summary>
        ///     The default key
        /// </summary>
        [NotNull]
        private const string DefaultKey = "default";

        /// <summary>
        ///     The InternalProviders
        /// </summary>
        [NotNull]
        private static readonly Dictionary<string, IBlinkingProvider> InternalProviders =
            new Dictionary<string, IBlinkingProvider>();

        /// <summary>
        ///     The default provider
        /// </summary>
        [NotNull]
        private static IBlinkingProvider defaultProvider = new BlinkingProvider();

     
        /// <summary>
        ///     Gets or sets the default provider.
        /// </summary>
        /// <value>
        ///     The default provider.
        /// </value>
        [NotNull]
        public static IBlinkingProvider DefaultProvider
        {
            get => defaultProvider;
            set
            {
                if (InternalProviders.ContainsKey(DefaultKey))
                {
                    InternalProviders.Remove(DefaultKey);
                }

                AddProvider(DefaultKey, value);
                defaultProvider = value;
            }
        }

        /// <summary>
        ///     Gets the InternalProviders.
        /// </summary>
        /// <value>
        ///     The InternalProviders.
        /// </value>
        [NotNull]
        public static IReadOnlyDictionary<string, IBlinkingProvider> Providers => InternalProviders;

        /// <summary>
        ///     Adds the provider.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="provider">The provider.</param>
        public static void AddProvider([NotNull] string name, [NotNull] IBlinkingProvider provider)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (!InternalProviders.ContainsKey(name.ToLower()))
            {            InternalProviders.Add(name.ToLower(), provider);

            }

        }

        /// <summary>
        ///     Removes the provider.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Is provider removed.</returns>
        public static bool RemoveProvider([NotNull] string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return InternalProviders.Remove(name.ToLower());
        }
    }
}