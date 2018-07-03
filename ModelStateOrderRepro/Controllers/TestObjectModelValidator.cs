// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// The default implementation of <see cref="IObjectModelValidator"/>.
    /// </summary>
    public class ApplicationObjectValidator : ObjectModelValidator
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DefaultObjectValidator"/>.
        /// </summary>
        /// <param name="modelMetadataProvider">The <see cref="IModelMetadataProvider"/>.</param>
        /// <param name="validatorProviders">The list of <see cref="IModelValidatorProvider"/>.</param>
        public ApplicationObjectValidator(
            IModelMetadataProvider modelMetadataProvider,
            IList<IModelValidatorProvider> validatorProviders)
            : base(modelMetadataProvider, validatorProviders)
        {
        }

        public override ValidationVisitor GetValidationVisitor(
            ActionContext actionContext,
            IModelValidatorProvider validatorProvider,
            ValidatorCache validatorCache,
            IModelMetadataProvider metadataProvider,
            ValidationStateDictionary validationState)
        {
            return new FixedValidationVisitor(
                actionContext,
                validatorProvider,
                validatorCache,
                metadataProvider,
                validationState);
        }
    }

    public class FixedValidationVisitor : ValidationVisitor
    {
        public FixedValidationVisitor(
           ActionContext actionContext,
           IModelValidatorProvider validatorProvider,
           ValidatorCache validatorCache,
           IModelMetadataProvider metadataProvider,
           ValidationStateDictionary validationState)
           : base(actionContext, validatorProvider, validatorCache, metadataProvider, validationState)
        {

        }

        protected override void SuppressValidation(string key)
        {
            if (key == null)
            {
                // If the key is null, that means that we shouldn't expect any entries in ModelState for
                // this value, so there's nothing to do.
                return;
            }

            var entries = ModelState.FindKeysWithPrefix(key);
            foreach (var entry in entries)
            {
                if (entry.Value.ValidationState == ModelValidationState.Unvalidated)
                {
                    entry.Value.ValidationState = ModelValidationState.Skipped;
                }
            }
        }
    }
}
