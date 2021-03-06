// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace McMaster.Extensions.CommandLineUtils
{
    /// <summary>
    /// Represents a command line application using attributes to define options and arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandAttribute : Attribute
    {
        private string[] _names = Util.EmptyArray<string>();

        /// <summary>
        /// Initializes a new <see cref="CommandAttribute"/>.
        /// </summary>
        public CommandAttribute()
        { }

        /// <summary>
        /// Initializes a new <see cref="CommandAttribute"/>.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        public CommandAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new <see cref="CommandAttribute"/>.
        /// </summary>
        /// <param name="names">The names of the command. The first name given is the primary name</param>
        public CommandAttribute(params string[] names)
        {
            _names = names;
        }

        /// <summary>
        /// The name of the command line application. When this is a subcommand, it is the name of the word used to invoke the subcommand.
        /// <seealso cref="CommandLineApplication.Name" />
        /// </summary>
        public string Name
        {
            get => _names.Length > 0 ? _names[0] : null;
            set => _names = new[] { value };
        }

        /// <summary>
        /// THe names of the command. The first is the primary name. All other names are aliases.
        /// </summary>
        public IEnumerable<string> Names => _names;

        /// <summary>
        /// The full name of the command line application to show in help text. <seealso cref="CommandLineApplication.FullName" />
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// A description of the command. <seealso cref="CommandLineApplication.Description"/>
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Determines if this command appears in generated help text. <seealso cref="CommandLineApplication.ShowInHelpText"/>
        /// </summary>
        public bool ShowInHelpText { get; set; } = true;

        /// <summary>
        /// Additional text that appears at the bottom of generated help text. <seealso cref="CommandLineApplication.ExtendedHelpText"/>
        /// </summary>
        public string ExtendedHelpText { get; set; }

        /// <summary>
        /// Throw when unexpected arguments are encountered. <seealso cref="CommandLineApplication.ThrowOnUnexpectedArgument"/>
        /// </summary>
        public bool ThrowOnUnexpectedArgument { get; set; } = true;

        /// <summary>
        /// Allow '--' to be used to stop parsing arguments. <seealso cref="CommandLineApplication.AllowArgumentSeparator"/>
        /// </summary>
        public bool AllowArgumentSeparator { get; set; }

        /// <summary>
        /// Treat arguments beginning as '@' as a response file. <seealso cref="CommandLineApplication.ResponseFileHandling"/>
        /// </summary>
        public ResponseFileHandling ResponseFileHandling { get; set; } = ResponseFileHandling.Disabled;

        /// <summary>
        /// The way arguments and options are matched.
        /// </summary>
        public StringComparison OptionsComparison { get; set; } = StringComparison.Ordinal;

        /// <summary>
        /// Specifies the culture used to convert values into types.
        /// </summary>
        public CultureInfo ParseCulture { get; set; } = CultureInfo.CurrentCulture;

        internal void Configure(CommandLineApplication app)
        {
            // this might have been set from SubcommandAttribute
            app.Name = Name ?? app.Name;

            foreach (var name in Names.Skip(1))
            {
                app.AddName(name);
            }

            app.AllowArgumentSeparator = AllowArgumentSeparator;
            app.Description = Description;
            app.ExtendedHelpText = ExtendedHelpText;
            app.FullName = FullName;
            app.ResponseFileHandling = ResponseFileHandling;
            app.ShowInHelpText = ShowInHelpText;
            app.ThrowOnUnexpectedArgument = ThrowOnUnexpectedArgument;
            app.OptionsComparison = OptionsComparison;
            app.ValueParsers.ParseCulture = ParseCulture;
        }
    }
}
