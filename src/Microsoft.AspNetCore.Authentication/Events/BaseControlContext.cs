// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication
{
    public class BaseControlContext : BaseContext
    {
        protected BaseControlContext(HttpContext context) : base(context)
        {
        }

        public EventResultState State { get; set; }

        public bool HandledResponse
        {
            get { return State == EventResultState.HandledResponse; }
        }

        public bool Skipped
        {
            get { return State == EventResultState.Skipped; }
        }

        /// <summary>
        /// Discontinue all processing for this request and return to the client.
        /// The caller is responsible for generating the full response.
        /// Set the <see cref="Ticket"/> to trigger SignIn.
        /// </summary>
        public void HandleResponse()
        {
            State = EventResultState.HandledResponse;
        }

        /// <summary>
        /// Discontinue processing the request in the current middleware and pass control to the next one.
        /// SignIn will not be called.
        /// </summary>
        public void SkipToNextMiddleware()
        {
            State = EventResultState.Skipped;
        }

        /// <summary>
        /// Gets or set the <see cref="Ticket"/> to return if this event signals it handled the event.
        /// </summary>
        public AuthenticationTicket Ticket { get; set; }

        public bool CheckEventResult(out AuthenticateResult result)
        {
            if (HandledResponse)
            {
                result = AuthenticateResult.Success(Ticket);
                return true;
            }
            else if (Skipped)
            {
                result = AuthenticateResult.Skip();
                return true;
            }
            result = null;
            return false;
        }
    }
}