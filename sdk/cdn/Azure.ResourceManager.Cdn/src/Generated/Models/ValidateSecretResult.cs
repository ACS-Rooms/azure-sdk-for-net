// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.ResourceManager.Cdn.Models
{
    /// <summary> Output of the validated secret. </summary>
    internal partial class ValidateSecretResult
    {
        /// <summary> Initializes a new instance of ValidateSecretResult. </summary>
        internal ValidateSecretResult()
        {
        }

        /// <summary> The validation status. </summary>
        public ValidationStatus? Status { get; }
        /// <summary> Detailed error message. </summary>
        public string Message { get; }
    }
}
