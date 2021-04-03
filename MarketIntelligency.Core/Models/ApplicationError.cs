﻿using Microsoft.AspNetCore.Http;
using System;


namespace MarketIntelligency.Core.Models
{
    /// <summary>
    /// A base error class in conformance to RFC 7807 about
    /// Problem Details for HTTP API <see cref="https://tools.ietf.org/html/rfc7807"/>
    /// </summary>
    public class ApplicationError
    {
        public ApplicationError() { }

        public ApplicationError(Uri type, int status, string code, string title, string detail)
        {
            Type = type;
            Status = status;
            Code = code;
            Title = title;
            Detail = detail;
        }

        /// <summary>
        /// A URI reference that identifies the problem type.
        /// </summary>
        public Uri Type { get; set; } = new Uri("https://tools.ietf.org/html/rfc7231#section-6.6.1");

        /// <summary>
        /// A URI reference that identifies the specific occurence of the problem.
        /// </summary>
        public Uri Instance { get; set; }

        /// <summary>
        /// The HTTP status code generated by the origin server for this occurence of the problem.
        /// </summary>
        public int Status { get; set; } = StatusCodes.Status500InternalServerError;

        /// <summary>
        /// An application error code.
        /// </summary>
        public string Code { get; set; } = "InternalServerError";

        /// <summary>
        /// A short, human-readable summary of the problem type.
        /// </summary>
        public string Title { get; set; } = "Internal Server Error.";

        /// <summary>
        /// A human-readable explanation specific to this occurence of the problem.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// A relative URI referencing the specific occurence of the problem.
        /// </summary>
        /// <param name="instance">Represent the URI Path.</param>
        /// <returns>
        /// Returns the self instance of ApplicationError.
        /// </returns>
        /// <remarks>
        /// Example:
        /// '/api/v1/deposits'
        /// </remarks>
        public ApplicationError AddInstance(string instance)
        {
            Instance = new Uri(instance, UriKind.Relative);
            return this;
        }
    }

    public static class ApplicationErrors
    {
        public static readonly ApplicationError InternalServerError = new(
            new Uri("https://tools.ietf.org/html/rfc7231#section-6.6.1"),
            StatusCodes.Status500InternalServerError,
            "InternalServerError",
            "Internal Server Error.",
            "An unexpected error occurred in our server"
            );

        public static readonly ApplicationError BadRequestError = new(
            new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.1"),
            StatusCodes.Status400BadRequest,
            "BadRequestError",
            "Bad Request Error.",
            "Make sure you have passed all the arguments or payloads as valid values."
            );
    }
}
